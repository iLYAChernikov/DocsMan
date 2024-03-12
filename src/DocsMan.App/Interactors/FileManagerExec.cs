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
	public class FileManagerExec
	{
		private IRepository<Document> _docRepos;
		private IUnitWork _unitWork;
		private IRepository<Profile> _profileRepos;
		private IBindingRepository<Profile_Document> _bindProfileDoc;
		private DocumentHistoryExec _historyExec;
		private UploadFileExec _fileExec;

		public FileManagerExec(
			IRepository<Document> docRepos, UploadFileExec fileExec,
			IUnitWork unitWork, IRepository<Profile> profileRepos,
			IBindingRepository<Profile_Document> bindProfileDoc, DocumentHistoryExec historyExec)
		{
			_docRepos = docRepos;
			_fileExec = fileExec;
			_unitWork = unitWork;
			_profileRepos = profileRepos;
			_bindProfileDoc = bindProfileDoc;
			_historyExec = historyExec;
		}

		public async Task<Response> AddDocument(int profileId, string fileName, string storagePath, Stream fileStream, string description)
		{
			try
			{
				var profile = await _profileRepos.GetOneAsync(profileId);
				var respFile = await _fileExec.AddFile(fileName, storagePath, fileStream);
				if (!respFile.IsSuccess)
					return new(respFile.ErrorMessage, respFile.ErrorInfo);
				var file = respFile.Value;
				Document doc = new()
				{
					FileId = file.FileId,
					Name = file.FileName,
					FileType = file.FileType,
					Description = description,
					IsDeleted = false
				};
				await _docRepos.CreateAsync(doc);
				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, file.FileId, $"Загрузка файла - by \"{profile.Email}\"");
				await _bindProfileDoc.CreateBindAsync(
					new()
					{
						DocumentId = doc.Id,
						ProfileId = profile.Id
					});
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
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<DocumentDto>?>> GetDocs(int profileId, string storagePath)
		{
			try
			{
				List<DocumentDto> documents = new();
				var docs = (await _bindProfileDoc.GetAllBinds())
					.Where(x => x.ProfileId == profileId && !x.Document.IsDeleted)
					.Select(x => x.Document);
				foreach (var item in docs)
				{
					var dto = item.ToDto();
					var resp = await _fileExec.GetSizeFile(item.FileId, storagePath);
					if (!resp.IsSuccess)
						dto.FileSize = "0?";
					else
						dto.FileSize = resp.Value;
					documents.Add(dto);
				}
				return new(documents);
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

		public async Task<Response<IEnumerable<DocumentDto>?>> GetTrash(int profileId, string storagePath)
		{
			try
			{
				List<DocumentDto> documents = new();
				var docs = (await _bindProfileDoc.GetAllBinds())
					.Where(x => x.ProfileId == profileId && x.Document.IsDeleted)
					.Select(x => x.Document);
				foreach (var item in docs)
				{
					var dto = item.ToDto();
					var resp = await _fileExec.GetSizeFile(item.FileId, storagePath);
					if (!resp.IsSuccess)
						dto.FileSize = "0?";
					else
						dto.FileSize = resp.Value;
					documents.Add(dto);
				}
				return new(documents);
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

		public async Task<Response> DeleteDocument(int documentId, string storagePath)
		{
			try
			{
				var history = await GetHistory(documentId);
				if (!history.IsSuccess)
					return history;

				foreach (var item in history.Value)
				{
					await _fileExec.DeleteFile(item.FileId, storagePath);
				}

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

		public async Task<Response> HideDocument(int profileId, int documentId)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				doc.IsDeleted = true;
				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Удаление файла - by \"{profile.Email}\"");

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
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response> ReturnDocument(int profileId, int documentId)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				doc.IsDeleted = false;
				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Восстановление файла - by \"{profile.Email}\"");

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
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<ProfileDto>?>> GetSharedProfiles(int documentId)
		{
			try
			{
				return new(((await _bindProfileDoc.GetAllBinds())?
					.Where(x => x.DocumentId == documentId))?
					.Select(x => x.Profile?.ToDto()));
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

		public async Task<Response> ShareDocument(int profileId, int documentId)
		{
			try
			{
				await _bindProfileDoc.CreateBindAsync(
					new()
					{
						ProfileId = profileId,
						DocumentId = documentId
					});
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
				return new("Ошибка создания", ex.Message);
			}
		}

		public async Task<Response> DeleteShareDocument(int profileId, int documentId)
		{
			try
			{
				await _bindProfileDoc.DeleteBindAsync(
					new()
					{
						ProfileId = profileId,
						DocumentId = documentId
					});
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

		public async Task<Response> RenameDocument(int profileId, int documentId, string name, string? description)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				doc.Name = name;
				doc.Description = description;

				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Изменение файла - by \"{profile.Email}\"");

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

		public async Task<Response> ChangeFile(int profileId, int documentId, string fileName, string storagePath, Stream fileStream)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				var respFile = await _fileExec.AddFile(fileName, storagePath, fileStream);
				if (!respFile.IsSuccess)
					return new(respFile.ErrorMessage, respFile.ErrorInfo);

				var file = respFile.Value;
				doc.FileId = file.FileId;
				doc.FileType = file.FileType;

				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Замена файла - by \"{profile.Email}\"");

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
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<DataFile>> DownloadFile(int documentId, string storagePath)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var resp = await _fileExec.DownloadFile(doc.FileId, storagePath);
				if (!resp.IsSuccess)
					return new(resp.ErrorMessage, resp.ErrorInfo);

				DataFile dataFile = new()
				{
					OwnerId = doc.Id,
					FileName = doc.Name + resp.Value.SavedFileType,
					FileData = resp.Value.FileData
				};

				return new(dataFile);
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

		public async Task<Response<DataFile>> DownloadHistoryFile(int documentId, string timeChange, string storagePath)
		{
			try
			{
				return await _historyExec.DownloadFile(documentId, timeChange, storagePath);
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
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<IEnumerable<DocumentHistoryDto>?>> GetHistory(int documentId)
		{
			return await _historyExec.GetDocumentHistory(documentId);
		}
	}
}