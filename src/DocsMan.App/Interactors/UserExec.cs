using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class UserExec
	{
		private IRepository<User> _userRepos;
		private IRepository<Profile> _profileRepos;
		private IBindingRepository<User_Role> _userRoles;
		private IUnitWork _unitWork;

		public UserExec
		(
			IRepository<User> repos,
			IBindingRepository<User_Role> userRoles,
			IUnitWork unitWork,
			IRepository<Profile> profileRepos
		)
		{
			_userRepos = repos;
			_profileRepos = profileRepos;
			_userRoles = userRoles;
			_unitWork = unitWork;
		}

		public async Task<Response<IEnumerable<UserDto>?>> GetAll()
		{
			try
			{
				var data = ( await _userRepos.GetAllAsync() )?
					.Select(x => x.ToDto());
				return new(data);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<UserDto?>> GetOne(int id)
		{
			try
			{
				var ent = await _userRepos.GetOneAsync(id);
				return new(ent.ToDto());
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch ( NullReferenceException ex )
			{
				return new("Запись не найдена", ex.Message);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<UserDto?>> GetOne(string email)
		{
			try
			{
				var ent = ( await _userRepos.GetAllAsync() )?
					.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
				if ( ent == null )
					return new("Пользователь не найден", "User not exist");
				else
					return new(ent.ToDto());
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response> Create(UserDto ent)
		{
			try
			{
				if ( ( await _userRepos.GetAllAsync() )?
					.FirstOrDefault(x => x.Email == ent.Email) != null )
					return new("Ошибка создания, пользователь с такой почтой уже существует", "User even exist");

				var user = ent?.ToEntity();
				await _userRepos.CreateAsync(user);
				await _unitWork.Commit();

				await _userRoles.CreateBindAsync(
					new()
					{
						UserId = user.Id,
						RoleId = 1
					});
				await _unitWork.Commit();

				var profile = new Profile()
				{
					UserId = user.Id,
					SurName = "User_Surname",
					Name = "User_Name",
					LastName = "User_Lastname"
				};
				await _profileRepos.CreateAsync(profile);
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch ( Exception ex )
			{
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> Delete(int id)
		{
			try
			{
				await _userRepos.DeleteAsync(id);
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch ( NullReferenceException ex )
			{
				return new("Запись не найдена", ex.Message);
			}
			catch ( Exception ex )
			{
				return new("Ошибка удаления", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<RoleDto>?>> GetRoles(int userId)
		{
			try
			{
				var roles = ( await _userRoles.GetAllBinds() )?
					.Where(x => x.UserId == userId)?
					.Select(x => x.Role.ToDto());
				return new(roles);
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response> AddRole(int userId, int roleId)
		{
			try
			{
				await _userRoles.CreateBindAsync(
					new()
					{
						UserId = userId,
						RoleId = roleId
					});
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch ( Exception ex )
			{
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> DeleteRole(int userId, int roleId)
		{
			try
			{
				await _userRoles.DeleteBindAsync(
					new()
					{
						UserId = userId,
						RoleId = roleId
					});
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch ( Exception ex )
			{
				return new("Ошибка удаления", ex.Message);
			}
		}
	}
}