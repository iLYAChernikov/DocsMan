using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class RoleExec
	{
		private IRepository<Role> _repos;
		private IUnitWork _unitWork;

		public RoleExec(IRepository<Role> repos, IUnitWork unitWork)
		{
			_repos = repos;
			_unitWork = unitWork;
		}

		public async Task<Response<IEnumerable<RoleDto>?>> GetAll()
		{
			try
			{
				var data = await _repos.GetAllAsync();
				return new(data?.Select(x => x?.ToDto()));
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<RoleDto?>> GetOne(int id)
		{
			try
			{
				var ent = await _repos.GetOneAsync(id);
				return new(ent?.ToDto() ?? throw new NullReferenceException("Not found"));
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.Message);
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

		public async Task<Response<RoleDto?>> GetOne(string title)
		{
			try
			{
				var ent = ( await _repos.GetAllAsync() )?
					.FirstOrDefault(x => x.Title == title);
				return new(ent?.ToDto() ?? throw new NullReferenceException("Not found"));
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.Message);
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

		public async Task<Response> Create(RoleDto ent)
		{
			try
			{
				if ( ( await _repos.GetAllAsync() )?
					.FirstOrDefault(x => x.Title == ent.Title) != null )
					return new("Ошибка создания, такая роль уже существует", "Role even exist");

				await _repos.CreateAsync(ent?.ToEntity());
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new($"Пустые входные данные\n{ex.Message}", "Internal error of entity null props");
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
				if ( id <= 3 )
					return new("Запрещено удалять эту роль", "Forbidden delete this role");

				await _repos.DeleteAsync(( await _repos.GetOneAsync(id) )
					?? throw new NullReferenceException("Not found"));
				await _unitWork.Commit();

				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new("Пустые входные данные", ex.Message);
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
	}
}