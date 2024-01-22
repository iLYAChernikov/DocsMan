using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class ProfileExec
	{
		private IRepository<Profile> _profileRepos;
		private IRepository<PersonalDocument> _persDocRepos;
		private IRepository<PersonalDocumentType> _docTypeRepos;
		private UploadFileExec _fileExec;
		private IUnitWork _unitWork;

		public ProfileExec
		(
			IRepository<Profile> profileRepos,
			IUnitWork unitWork,
			IRepository<PersonalDocument> persDocRepos,
			IRepository<PersonalDocumentType> docTypeRepos,
			UploadFileExec fileExec)
		{
			_profileRepos = profileRepos;
			_unitWork = unitWork;
			_persDocRepos = persDocRepos;
			_docTypeRepos = docTypeRepos;
			_fileExec = fileExec;
		}

		public async Task<Response> ChangeInfo(ProfileDto dto)
		{
			try
			{
				var ent = dto.ToEntity();

				var old = await _profileRepos.GetOneAsync(ent.Id);

				old.SurName = ent.SurName;
				old.Name = ent.Name;
				old.LastName = ent.LastName;
				old.Birthdate = ent.Birthdate;
				old.Gender = ent.Gender;
				old.PhoneNumber = ent.PhoneNumber;

				await _unitWork.Commit();
				return new(true);
			}
			catch ( ArgumentNullException ex )
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch ( NullReferenceException ex )
			{
				return new("Запись не найдена", ex.Message);
			}
			catch ( Exception ex )
			{
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response<ProfileDto?>> GetOne(int id)
		{
			try
			{
				var ent = await _profileRepos.GetOneAsync(id);

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

		public async Task<Response<ProfileDto?>> GetOne(string email)
		{
			try
			{
				var ent = ( await _profileRepos.GetAllAsync() )?
					.FirstOrDefault(x => x.Email.ToLower() == email.ToLower())?
					.ToDto();
				if ( ent == null )
					return new("Запись не найдена", "Profile not exist");
				else
					return new(ent);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<ProfileDto>?>> GetAll()
		{
			try
			{
				var data = ( await _profileRepos.GetAllAsync() )?
					.Select(x => x.ToDto());
				return new(data);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<PersonalDocumentDto>?>> GetAllPersonalDocuments(int profileId)
		{
			try
			{
				var docs = ( await _persDocRepos.GetAllAsync() )?
					.Where(x => x.ProfileId == profileId)
					.Select(x => x.ToDto());

				return new(docs);
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

		public async Task<Response> AddPersonDoc
			(
				int profileId, int typeId,
				string fileName, string storagePath, Stream fileStream,
				string textDoc
			)
		{
			try
			{
				var respNewFile = await _fileExec.AddFile(fileName, storagePath, fileStream);
				if ( !respNewFile.IsSuccess )
					return new(respNewFile.ErrorMessage, respNewFile.ErrorInfo);

				PersonalDocument doc = new()
				{
					ProfileId = profileId,
					TypeId = typeId,
					FileId = respNewFile.Value,
					Text = textDoc
				};
				await _persDocRepos.CreateAsync(doc);
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

		public async Task<Response> DeletePersonDoc(int profileId, int typeId, string storagePath)
		{
			try
			{
				var doc = await _persDocRepos.GetOneAsync(profileId, typeId);

				await _fileExec.DeleteFile(doc.FileId, storagePath);
				await _persDocRepos.DeleteAsync(doc.ProfileId, doc.TypeId);

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
	}
}