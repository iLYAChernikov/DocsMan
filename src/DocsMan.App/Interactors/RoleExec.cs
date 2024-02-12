using System.Data;
using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class RoleExec
	{
		private IRepository<Role> _repos;
		private IBindingRepository<User_Role> _userRoles;
		private IUnitWork _unitWork;
		private IRepository<Profile> _profileRepos;

		public RoleExec
		(
			IRepository<Role> repos,
			IUnitWork unitWork,
			IBindingRepository<User_Role> userRoles,
			IRepository<Profile> profileRepos)
		{
			_repos = repos;
			_userRoles = userRoles;
			_unitWork = unitWork;
			_profileRepos = profileRepos;
		}

		public async Task<Response<IEnumerable<RoleDto?>?>> GetAll()
		{
			try
			{
				var data = await _repos.GetAllAsync();
				if (data == null)
					return new("Записи не найдены", "Not found");
				else
					return new(data.Select(x => x.ToDto()));
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<RoleDto?>> GetOne(int id)
		{
			try
			{
				var ent = await _repos.GetOneAsync(id);
				return new(ent.ToDto());
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<RoleDto?>> GetOne(string title)
		{
			try
			{
				var ent = (await _repos.GetAllAsync())?
					.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
				if (ent == null)
					return new("Запись не найдена", "Not found");
				else
					return new(ent.ToDto());
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response> Create(RoleDto ent)
		{
			try
			{
				if ((await _repos.GetAllAsync())?
					.FirstOrDefault(x => x.Title == ent.Title) != null)
					return new("Ошибка создания, такая роль уже существует", "Role even exist");

				await _repos.CreateAsync(ent?.ToEntity());
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch (Exception ex)
			{
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> Delete(int id)
		{
			try
			{
				if (id > 0 && id <= 3)
					return new("Запрещено удалять эту роль", "Forbidden delete this role");

				await _repos.DeleteAsync(id);
				await _unitWork.Commit();

				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка удаления", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<ProfileDto?>?>> GetUsers(int roleId)
		{
			try
			{
				await _repos.GetOneAsync(roleId);

				var users = (await _userRoles.GetAllBinds())?
					.Where(x => x.RoleId == roleId)
					.Select(x => x.User.ToDto());
				if (users == null)
					return new("Записи не найдены", "Not found");
				else
				{
					List<ProfileDto?> profiles = new();
					foreach (var user in users)
					{
						profiles.Add((await _profileRepos
							.GetAllAsync())?
							.FirstOrDefault(x => x.UserId == user.Id)?
							.ToDto());
					}
					return new(profiles);
				}

			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}
	}
}