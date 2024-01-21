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
		private IRepository<UploadFile> _fileRepos;
		private IUnitWork _unitWork;

		public ProfileExec
		(
			IRepository<Profile> profileRepos,
			IUnitWork unitWork,
			IRepository<PersonalDocument> persDocRepos,
			IRepository<PersonalDocumentType> docTypeRepos,
			IRepository<UploadFile> fileRepos
		)
		{
			_profileRepos = profileRepos;
			_unitWork = unitWork;
			_persDocRepos = persDocRepos;
			_docTypeRepos = docTypeRepos;
			_fileRepos = fileRepos;
		}

		private string GetOnlyFileResolution(string fileName) =>
			fileName.Substring(fileName.LastIndexOf('.'));
		private string GetOnlyFileName(string fileName) =>
			fileName.Substring(0, fileName.Length - GetOnlyFileResolution(fileName).Length);
		private string GetDateTimeFileName(string fileName)
		{
			var howNow = DateTime.Now;
			string name = GetOnlyFileName(fileName);
			string tempName = $"{name}_{howNow.Day}.{howNow.Month}.{howNow.Year}_{howNow.Hour}.{howNow.Minute}.{howNow.Second}";
			return tempName + GetOnlyFileResolution(fileName);
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
					.FirstOrDefault(x => x.Email == email)?
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
				UploadFile newFile = new() { FilePath = GetDateTimeFileName(fileName) };
				await _fileRepos.CreateAsync(newFile);
				await _unitWork.Commit();

				using ( var nfs = new FileStream(storagePath + newFile.FilePath, FileMode.Create) )
				{
					await fileStream.CopyToAsync(nfs);
				}

				PersonalDocument doc = new()
				{
					ProfileId = profileId,
					TypeId = typeId,
					FileId = newFile.Id,
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

				string filePath = doc.File.FilePath;

				await _persDocRepos.DeleteAsync(doc.ProfileId, doc.TypeId);
				await _fileRepos.DeleteAsync(doc.FileId);

				await _unitWork.Commit();

				if ( File.Exists(storagePath + filePath) )
					File.Delete(storagePath + filePath);

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
				return new("Ошибка создания", ex.Message);
			}
		}
	}
}