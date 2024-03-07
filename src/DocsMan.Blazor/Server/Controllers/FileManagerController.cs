using DocsMan.App.Interactors;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;

using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace DocsMan.Blazor.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FileManagerController : ControllerBase
	{
		private FileManagerExec _master;
		private AuthExec _auth;

		public FileManagerController(FileManagerExec master, AuthExec auth)
		{
			_master = master;
			_auth = auth;
		}

		[HttpPost("AddDocument")]
		public async Task<Response> AddDoc(DataFile file)
		{
			if ( file.FileData == null )
				return new("Нет файла", "Empty file");

			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if ( !ident.IsSuccess )
				return ident;

			return await _master.AddDocument
				(
					ident.Value, file.FileName,
					PathStorage.Files_Dir, new MemoryStream(file.FileData),
					"NewFile"
				);
		}

		[HttpGet("GetDocs")]
		public async Task<Response<IEnumerable<DocumentDto>?>> GetAllDocs()
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if ( !ident.IsSuccess )
				return new(ident.ErrorMessage, ident.ErrorInfo);
			else
				return await _master.GetDocs(ident.Value, PathStorage.Files_Dir);
		}

		[HttpGet("GetTrash")]
		public async Task<Response<IEnumerable<DocumentDto>?>> GetTrash()
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if (!ident.IsSuccess)
				return new(ident.ErrorMessage, ident.ErrorInfo);
			else
				return await _master.GetTrash(ident.Value, PathStorage.Files_Dir);
		}

		[HttpGet("GetHistory/{docId}")]
		public async Task<Response<IEnumerable<DocumentHistoryDto>?>> GetDocHistory(int docId)
		{
			return await _master.GetHistory(docId);
		}

		[HttpDelete("HideDocument/{docId}")]
		public async Task<Response> HideDoc(int docId)
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if ( !ident.IsSuccess )
				return ident;

			return await _master.HideDocument(ident.Value, docId);
		}

		[HttpDelete("ReturnDocument/{docId}")]
		public async Task<Response> ReturnDoc(int docId)
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if (!ident.IsSuccess)
				return ident;

			return await _master.ReturnDocument(ident.Value, docId);
		}

		[HttpDelete("DeleteDocument/{docId}")]
		public async Task<Response> DeleteDoc(int docId)
		{
			return await _master.DeleteDocument(docId, PathStorage.Files_Dir);
		}

		[HttpPost("RenameDocument")]
		public async Task<Response> RenameDoc(DocumentDto dto)
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if ( !ident.IsSuccess )
				return ident;

			return await _master.RenameDocument(ident.Value, dto.Id, dto.Name, dto.Description);
		}

		[HttpPost("ChangeFile")]
		public async Task<Response> ChangeFile(DataFile file)
		{
			if ( file.FileData == null )
				return new("Нет файла", "Empty file");

			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if ( !ident.IsSuccess )
				return ident;

			return await _master.ChangeFile
				(
					ident.Value, file.OwnerId,
					file.FileName, PathStorage.Files_Dir,
					new MemoryStream(file.FileData)
				);
		}

		[HttpGet("DownloadDocument/{docId}")]
		public async Task<ActionResult> DownloadDoc(int docId)
		{
			var resp = await _master.DownloadFile(docId, PathStorage.Files_Dir);
			if ( resp.IsSuccess && resp.Value?.FileData != null )
				return File(resp.Value.FileData, "application/x-rar-compressed", resp.Value.FileName);
			else
				return NotFound($"{resp.ErrorInfo} {resp.ErrorMessage}");
		}

		[HttpGet("DownloadHistoryFile/{docId}/{time}")]
		public async Task<ActionResult> DownloadHistory(int docId, string time)
		{
			var resp = await _master.DownloadHistoryFile(docId, time, PathStorage.Files_Dir);
			if ( resp.IsSuccess && resp.Value?.FileData != null )
				return File(resp.Value.FileData, "application/x-rar-compressed", resp.Value.FileName);
			else
				return NotFound($"{resp.ErrorInfo} {resp.ErrorMessage}");
		}
	}
}