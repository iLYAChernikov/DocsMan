using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.BinderEntity;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class ProfileExec
	{
		private IRepository<Profile> _profileRepos;
		private IRepository<PersonalDocument> _persDocRepos;
		private IRepository<PersonalDocumentType> _docTypeRepos;
		private UploadFileExec _fileExec;
		private IBindingRepository<Profile_Notify> _profileNotifiesRepos;
		private IUnitWork _unitWork;

		public ProfileExec
		(
			IRepository<Profile> profileRepos,
			IUnitWork unitWork,
			IRepository<PersonalDocument> persDocRepos,
			IRepository<PersonalDocumentType> docTypeRepos,
			UploadFileExec fileExec,
			IBindingRepository<Profile_Notify> profileNotifiesRepos)
		{
			_profileRepos = profileRepos;
			_unitWork = unitWork;
			_persDocRepos = persDocRepos;
			_docTypeRepos = docTypeRepos;
			_fileExec = fileExec;
			_profileNotifiesRepos = profileNotifiesRepos;
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
				return new();
			}
			catch (ArgumentNullException ex)
			{
				return new($"Пустые входные данные: {ex.ParamName}", "Internal error of entity null props");
			}
			catch (NullReferenceException ex)
			{
				return new("Запись не найдена", ex.Message);
			}
			catch (Exception ex)
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

		public async Task<Response<ProfileDto?>> GetOne(string email)
		{
			try
			{
				var ent = (await _profileRepos.GetAllAsync())?
					.FirstOrDefault(x => x.Email.ToLower() == email.ToLower())?
					.ToDto();
				if (ent == null)
					return new("Запись не найдена", "Profile not exist");
				else
					return new(ent);
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<ProfileDto?>?>> GetAll()
		{
			try
			{
				var data = (await _profileRepos.GetAllAsync())?
					.Select(x => x.ToDto());
				if (data == null)
					return new("Записи не найдены", "Not found");
				else
					return new(data);
			}
			catch (Exception ex)
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<PersonalDocumentDto?>?>> GetAllPersonalDocuments(int profileId)
		{
			try
			{
				var docs = (await _persDocRepos.GetAllAsync())?
					.Where(x => x.ProfileId == profileId)
					.Select(x => x.ToDto());
				if (docs == null)
					return new("Записи не найдены", "Not found");
				else
					return new(docs);
			}
			catch (ArgumentNullException ex)
			{
				return new("Пустые входные данные", ex.ParamName);
			}
			catch (Exception ex)
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
				if (!respNewFile.IsSuccess)
					return new(respNewFile.ErrorMessage, respNewFile.ErrorInfo);

				PersonalDocument doc = new()
				{
					ProfileId = profileId,
					TypeId = typeId,
					FileId = respNewFile.Value.FileId,
					Text = textDoc
				};
				await _persDocRepos.CreateAsync(doc);
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

		public async Task<Response> DeletePersonDoc(int profileId, int typeId, string storagePath)
		{
			try
			{
				var doc = await _persDocRepos.GetOneAsync(profileId, typeId);

				await _persDocRepos.DeleteAsync(doc.ProfileId, doc.TypeId);
				await _fileExec.DeleteFile(doc.FileId, storagePath);

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

		public async Task<Response<DataFile>> DownloadPersonDoc(int profileId, int typeId, string storagePath)
		{
			try
			{
				var profile = await _profileRepos.GetOneAsync(profileId);
				var doc = await _persDocRepos.GetOneAsync(profileId, typeId);
				var resp = await _fileExec.DownloadFile(doc.FileId, storagePath);
				if (!resp.IsSuccess)
					return new(resp.ErrorMessage, resp.ErrorInfo);

				return new
					(
						new()
						{
							OwnerId = profileId,
							FileName = $"{doc.PersonalDocumentType.Title} {profile.Email}{resp.Value.SavedFileType}",
							FileData = resp.Value.FileData
						}
					);
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

		public async Task<Response<bool>> IsAnyNotifyNotRead(int profileId)
		{
			try
			{
				var profile = await _profileRepos.GetOneAsync(profileId);
				var binds = await _profileNotifiesRepos.GetAllBinds();
				if (binds != null)
				{
					var user_notifies = binds.Where(x => x.ProfileId == profileId);
					if (user_notifies != null)
					{
						bool result = user_notifies.Any(x => x.IsRead == false);
						return new(result);
					}
					else
						return new(false);
				}
				else
					return new(false);
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