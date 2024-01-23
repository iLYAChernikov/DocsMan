using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class PersonalDocumentTypeExec
	{
		private IRepository<PersonalDocumentType> _repos;
		private IUnitWork _unitWork;

		public PersonalDocumentTypeExec(IRepository<PersonalDocumentType> repos, IUnitWork unitWork)
		{
			_repos = repos;
			_unitWork = unitWork;
		}

		public async Task<Response<IEnumerable<PersonalDocumentTypeDto?>?>> GetAll()
		{
			try
			{
				var data = ( await _repos.GetAllAsync() )?
					.Select(x => x.ToDto());
				if ( data == null )
					return new("Записи не найдены", "Not found");
				else
					return new(data);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<PersonalDocumentTypeDto?>> GetOne(int id)
		{
			try
			{
				return new(( await _repos.GetOneAsync(id) ).ToDto());
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

		public async Task<Response<PersonalDocumentTypeDto?>> GetOne(string title)
		{
			try
			{
				var ent = ( await _repos.GetAllAsync() )?
					.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
				if ( ent == null )
					return new("Запись не найдена", "Personal Doc Type not exist");
				else
					return new(ent.ToDto());
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response> Create(PersonalDocumentTypeDto dto)
		{
			try
			{
				await _repos.CreateAsync(dto.ToEntity());
				await _unitWork.Commit();

				return new();
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
				if ( id == 1 )
					return new("Запрещено удалять этот тип", "Forbidden delete this type");

				await _repos.DeleteAsync(id);
				await _unitWork.Commit();

				return new();
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
	}
}