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
		private NotifyExec _notifyExec;
		private IRepository<Folder> _folderRepos;
		private IBindingRepository<Profile_Folder> _bindProfileFolder;
		private IBindingRepository<Folder_Document> _bindFolderDoc;

		public FileManagerExec(
			IRepository<Document> docRepos, UploadFileExec fileExec,
			IUnitWork unitWork, IRepository<Profile> profileRepos,
			IBindingRepository<Profile_Document> bindProfileDoc, DocumentHistoryExec historyExec,
			NotifyExec notifyExec, IRepository<Folder> folderRepos,
			IBindingRepository<Profile_Folder> bindProfileFolder, IBindingRepository<Folder_Document> bindFolderDoc)
		{
			_docRepos = docRepos;
			_fileExec = fileExec;
			_unitWork = unitWork;
			_profileRepos = profileRepos;
			_bindProfileDoc = bindProfileDoc;
			_historyExec = historyExec;
			_notifyExec = notifyExec;
			_folderRepos = folderRepos;
			_bindProfileFolder = bindProfileFolder;
			_bindFolderDoc = bindFolderDoc;
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

		public async Task<Response> AddFolderDocument(int profileId, int folderId, string fileName, string storagePath, Stream fileStream, string description)
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
				await _bindFolderDoc.CreateBindAsync(
					new()
					{
						DocumentId = doc.Id,
						FolderId = folderId
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
				var docs = (await _bindProfileDoc.GetAllBinds())?
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

		public async Task<Response<IEnumerable<FolderDto>?>> GetFolders(int profileId, string storagePath)
		{
			try
			{
				List<FolderDto> folders = new();
				var files = (await _bindProfileFolder.GetAllBinds())?
					.Where(x => x.ProfileId == profileId && !x.Folder.IsDeleted)
					.Select(x => x.Folder);
				foreach (var item in files)
				{
					var dto = item.ToDto();
					var resp = await _fileExec.GetSizeFolder(item.Id, storagePath);
					if (!resp.IsSuccess)
					{
						dto.FolderSize = "0?";
						dto.FilesCount = 0;
					}
					else
					{
						dto.FolderSize = resp.Value.Size;
						dto.FilesCount = resp.Value.Count;
					}
					folders.Add(dto);
				}
				return new(folders);
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

		public async Task<Response<IEnumerable<DocumentDto>?>> GetFolderDocs(int folderId, string storagePath)
		{
			try
			{
				List<DocumentDto> documents = new();
				var docs = (await _bindFolderDoc.GetAllBinds())?
					.Where(x => x.FolderId == folderId && !x.Document.IsDeleted)
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

		public async Task<Response<IEnumerable<FolderDto>?>> GetFolderTrash(int profileId, string storagePath)
		{
			try
			{
				List<FolderDto> folders = new();
				var folds = (await _bindProfileFolder.GetAllBinds())?
					.Where(x => x.ProfileId == profileId && x.Folder.IsDeleted)
					.Select(x => x.Folder);
				foreach (var item in folds)
				{
					var dto = item.ToDto();
					var resp = await _fileExec.GetSizeFolder(item.Id, storagePath);
					if (!resp.IsSuccess)
					{
						dto.FolderSize = "0?";
						dto.FilesCount = 0;
					}
					else
					{
						dto.FolderSize = resp.Value.Size;
						dto.FilesCount = resp.Value.Count;
					}
					folders.Add(dto);
				}
				return new(folders);
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
				var doc = await _docRepos.GetOneAsync(documentId);
				string docName = doc.Name + doc.FileType;
				var profiles = (await _bindProfileDoc.GetAllBinds())?
						.Where(x => x.DocumentId == documentId)
						.Select(x => x.ProfileId).ToList();

				var history = await GetHistory(documentId);
				if (!history.IsSuccess)
					return history;

				foreach (var item in history.Value)
				{
					await _fileExec.DeleteFile(item.FileId, storagePath);
				}

				Notification alert = new()
				{
					Title = "Уничтожение файла",
					Description = $"Файл \"{docName}\" навсегда удалён",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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

		public async Task<Response> DeleteFolder(int folderId, string storagePath)
		{
			try
			{
				var folder = await _folderRepos.GetOneAsync(folderId);
				string docName = folder.Name;
				var profiles = (await _bindProfileFolder.GetAllBinds())?
						.Where(x => x.FolderId == folderId)
						.Select(x => x.ProfileId).ToList();

				var docs = (await _bindFolderDoc.GetAllBindsNoTracking())?
					.Where(x => x.FolderId == folderId)
					.Select(x => x.Document);
				foreach (var doc in docs)
				{
					var history = await GetHistory(doc.Id);
					foreach (var item in history.Value)
					{
						await _fileExec.DeleteFile(item.FileId, storagePath);
					}
				}

				await _folderRepos.DeleteAsync(folderId);

				Notification alert = new()
				{
					Title = "Уничтожение папки",
					Description = $"Папка \"{docName}\" навсегда удалена",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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

		public async Task<Response> HideDocument(int profileId, int folderId)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(folderId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				doc.IsDeleted = true;
				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Удаление файла - by \"{profile.Email}\"");

				Notification alert = new()
				{
					Title = "Удаление файла",
					Description = $"Файл \"{doc?.Name + doc?.FileType}\" перемещён в Корзину",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					var profiles = (await _bindProfileDoc.GetAllBinds())?
						.Where(x => x.DocumentId == folderId)
						.Select(x => x.ProfileId);

					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response> HideFolder(int profileId, int folderId)
		{
			try
			{
				var folder = await _folderRepos.GetOneAsync(folderId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				folder.IsDeleted = true;
				await _unitWork.Commit();

				Notification alert = new()
				{
					Title = "Удаление папки",
					Description = $"Папка \"{folder?.Name}\" перемещена в Корзину",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					var profiles = (await _bindProfileFolder.GetAllBinds())?
						.Where(x => x.FolderId == folderId)
						.Select(x => x.ProfileId);

					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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

				Notification alert = new()
				{
					Title = "Восстановление файла",
					Description = $"Файл \"{doc?.Name + doc?.FileType}\" возвращён в Файловый менеджер",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					var profiles = (await _bindProfileDoc.GetAllBinds())?
						.Where(x => x.DocumentId == documentId)
						.Select(x => x.ProfileId);

					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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
				return new("Ошибка изменения", ex.Message);
			}
		}

		public async Task<Response> ReturnFolder(int profileId, int folderId)
		{
			try
			{
				var folder = await _folderRepos.GetOneAsync(folderId);
				var profile = await _profileRepos.GetOneAsync(profileId);
				folder.IsDeleted = false;
				await _unitWork.Commit();

				Notification alert = new()
				{
					Title = "Восстановление папки",
					Description = $"Папка \"{folder?.Name}\" возвращена в Файловый менеджер",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
				{
					var profiles = (await _bindProfileFolder.GetAllBinds())?
						.Where(x => x.FolderId == folderId)
						.Select(x => x.ProfileId);

					foreach (var id in profiles)
					{
						await _notifyExec.CreateBindNotify(id, req.Value);
					}
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

		public async Task<Response<IEnumerable<ProfileDto>?>> GetSharedProfilesFolder(int folderId)
		{
			try
			{
				return new(((await _bindProfileFolder.GetAllBinds())?
					.Where(x => x.FolderId == folderId))?
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

				var doc = (await _bindProfileDoc.GetAllBinds())?
					.FirstOrDefault(x => x.ProfileId == profileId && x.DocumentId == documentId)?
					.Document;

				Notification alert = new()
				{
					Title = "Получение доступа к файлу",
					Description = $"Вам был дан доступ к файлу \"{doc?.Name + doc?.FileType}\"",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
					await _notifyExec.CreateBindNotify(profileId, req.Value);

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

		public async Task<Response> ShareFolder(int profileId, int folderId)
		{
			try
			{
				await _bindProfileFolder.CreateBindAsync(
					new()
					{
						ProfileId = profileId,
						FolderId = folderId
					});
				await _unitWork.Commit();

				var folder = (await _bindProfileFolder.GetAllBinds())?
					.FirstOrDefault(x => x.ProfileId == profileId && x.FolderId == folderId)?
					.Folder;

				Notification alert = new()
				{
					Title = "Получение доступа к папке",
					Description = $"Вам был дан доступ к папке \"{folder?.Name}\"",
					DateTime = DateTime.Now
				};
				var req = await _notifyExec.CreateNotify(alert.ToDto());
				if (req.IsSuccess)
					await _notifyExec.CreateBindNotify(profileId, req.Value);

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

		public async Task<Response> DeleteShareFolder(int profileId, int folderId)
		{
			try
			{
				await _bindProfileFolder.DeleteBindAsync(
					new()
					{
						ProfileId = profileId,
						FolderId = folderId
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
				string oldName = doc.Name;
				var profile = await _profileRepos.GetOneAsync(profileId);
				doc.Name = name;
				doc.Description = description;

				await _unitWork.Commit();

				await _historyExec.AddHistory(doc.Id, doc.FileId, $"Изменение документа - by \"{profile.Email}\"");

				if (oldName != name)
				{
					Notification alert = new()
					{
						Title = "Переименование файла",
						Description = $"Файл \"{oldName + doc?.FileType}\" переименован в \"{name + doc?.FileType}\"",
						DateTime = DateTime.Now
					};
					var req = await _notifyExec.CreateNotify(alert.ToDto());
					if (req.IsSuccess)
					{
						var profiles = (await _bindProfileDoc.GetAllBinds())?
							.Where(x => x.DocumentId == documentId)
							.Select(x => x.ProfileId);

						foreach (var id in profiles)
						{
							await _notifyExec.CreateBindNotify(id, req.Value);
						}
					}
				}

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

		public async Task<Response> RenameFolder(int profileId, int folderId, string name, string? description)
		{
			try
			{
				var folder = await _folderRepos.GetOneAsync(folderId);
				string oldName = folder.Name;
				var profile = await _profileRepos.GetOneAsync(profileId);
				folder.Name = name;
				folder.Description = description;

				await _unitWork.Commit();

				if (oldName != name)
				{
					Notification alert = new()
					{
						Title = "Переименование папки",
						Description = $"Папка \"{oldName}\" переименована в  \"{name}\"",
						DateTime = DateTime.Now
					};
					var req = await _notifyExec.CreateNotify(alert.ToDto());
					if (req.IsSuccess)
					{
						var profiles = (await _bindProfileFolder.GetAllBinds())?
							.Where(x => x.FolderId == folderId)
							.Select(x => x.ProfileId);

						foreach (var id in profiles)
						{
							await _notifyExec.CreateBindNotify(id, req.Value);
						}
					}
				}

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

		public async Task<Response> CreateFolder(int profileId, FolderDto dto)
		{
			try
			{
				var profile = await _profileRepos.GetOneAsync(profileId);

				Folder newFolder = new()
				{
					Name = dto.Name,
					Description = dto.Description,
					IsDeleted = false
				};
				await _folderRepos.CreateAsync(newFolder);
				await _unitWork.Commit();

				await _bindProfileFolder.CreateBindAsync(
					new()
					{
						FolderId = newFolder.Id,
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
	}
}