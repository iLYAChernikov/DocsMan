using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.App.Storage.Transaction;
using DocsMan.Blazor.Shared.OutputData;
using DocsMan.Domain.Entity;

namespace DocsMan.App.Interactors
{
	public class UploadFileExec
	{
		private IRepository<UploadFile> _fileRepos;
		private IUnitWork _unitWork;

		public UploadFileExec(IRepository<UploadFile> fileRepos, IUnitWork unitWork)
		{
			_fileRepos = fileRepos;
			_unitWork = unitWork;
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

		public async Task<Response<(int FileId, string FileName, string FileType)>> AddFile(string fileName, string storagePath, Stream fileStream)
		{
			try
			{
				UploadFile newFile = new()
				{
					FilePath = GetDateTimeFileName(fileName)
				};
				await _fileRepos.CreateAsync(newFile);
				await _unitWork.Commit();

				using ( var nfs = new FileStream(storagePath + newFile.FilePath, FileMode.Create) )
				{
					await fileStream.CopyToAsync(nfs);
				}

				return new((newFile.Id, GetOnlyFileName(fileName), GetOnlyFileResolution(fileName)));
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

		public async Task<Response> DeleteFile(int fileId, string storagePath)
		{
			var file = await _fileRepos.GetOneAsync(fileId);
			var path = file.FilePath;

			await _fileRepos.DeleteAsync(fileId);
			await _unitWork.Commit();

			if ( File.Exists(storagePath + path) )
				File.Delete(storagePath + path);

			return new();
		}

		public async Task<Response<(string SavedFileType, byte[]? FileData)>> DownloadFile(int fileId, string storagePath)
		{
			try
			{
				var file = await _fileRepos.GetOneAsync(fileId);
				if ( file == null )
					return new("Файл не существует", "File not exist");

				using ( var nfs = new FileStream(storagePath + file.FilePath, FileMode.Open) )
				{
					var ms = new MemoryStream();
					await nfs.CopyToAsync(ms);
					return new((GetOnlyFileResolution(file.FilePath), ms.ToArray()));
				}
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