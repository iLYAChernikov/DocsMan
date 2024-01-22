using DocsMan.App.Mappers;
using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class DocumentHistoryExec
	{
		private IRepository<DocumentHistory> _historyRepos;
		private IRepository<Document> _docRepos;
		private IUnitWork _unitWork;
		private UploadFileExec _fileExec;

		public DocumentHistoryExec(IRepository<DocumentHistory> historyRepos, IUnitWork unitWork, IRepository<Document> docRepos, UploadFileExec fileExec)
		{
			_historyRepos = historyRepos;
			_unitWork = unitWork;
			_docRepos = docRepos;
			_fileExec = fileExec;
		}

		public async Task<Response> AddHistory(int documentId, int fileId, string description)
		{
			try
			{
				DocumentHistory history = new()
				{
					DocumentId = documentId,
					FileId = fileId,
					Description = description,
					DateTimeOfChanges = DateTime.Now
				};

				await _historyRepos.CreateAsync(history);
				await _unitWork.Commit();

				return new();
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

		public async Task<Response<IEnumerable<DocumentHistoryDto>?>> GetDocumentHistory(int documentId)
		{
			try
			{
				var data = ( await _historyRepos.GetAllAsync() )?
					.Where(x => x.DocumentId == documentId)?
					.Select(x => x.ToDto());
				if ( data == null )
					return new("Записи не найдены", "History not exist");
				else
					return new(data);
			}
			catch ( Exception ex )
			{
				return new("Ошибка получения", ex.Message);
			}
		}

		public async Task<Response<DataFile>> DownloadFile(int documentId, DateTime dateTime, string storagePath)
		{
			try
			{
				var doc = await _docRepos.GetOneAsync(documentId);
				var history = await _historyRepos.GetOneAsync(documentId, dateTime);
				var resp = await _fileExec.DownloadFile(history.FileId, storagePath);
				if ( !resp.IsSuccess )
					return new(resp.ErrorMessage, resp.ErrorInfo);

				DataFile dataFile = new()
				{
					OwnerId = doc.Id,
					FileName = doc.Name + resp.Value.SavedFileType,
					FileData = resp.Value.FileData
				};

				return new(dataFile);
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
	}
}