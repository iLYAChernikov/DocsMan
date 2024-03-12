using System.Security.Claims;
using DocsMan.App.Interactors;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ProfileController : ControllerBase
	{
		private ProfileExec _master;
		private AuthExec _auth;

		public ProfileController(ProfileExec master, AuthExec auth)
		{
			_master = master;
			_auth = auth;
		}

		[HttpPost("ChangeInfo")]
		public async Task<Response> Change(ProfileDto dto)
		{
			return await _master.ChangeInfo(dto);
		}

		[HttpGet("GetOneById/{id}")]
		public async Task<Response<ProfileDto?>> GetOne(int id)
		{
			return await _master.GetOne(id);
		}

		[HttpGet("GetOneByEmail/{email}")]
		public async Task<Response<ProfileDto?>> GetOne(string email)
		{
			return await _master.GetOne(email);
		}

		[HttpGet("GetAll")]
		public async Task<Response<IEnumerable<ProfileDto?>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpGet("GetFind/{line}")]
		public async Task<Response<IEnumerable<ProfileDto?>?>> GetAll(string line)
		{
			return await _master.GetFind(line);
		}

		[HttpGet("GetPersonalDocs/{id}")]
		public async Task<Response<IEnumerable<PersonalDocumentDto?>?>> GetPersonDocs(int id)
		{
			return await _master.GetAllPersonalDocuments(id);
		}

		[HttpPost("AddPersonalDoc")]
		public async Task<Response> AddDoc(PersonalDocumentDataDto dto)
		{
			if (dto.FileData == null)
				return new("Нет файла", "Empty file");

			return await _master.AddPersonDoc
				(
					profileId: dto.OwnerId,
					typeId: dto.PersonalDocumentTypeId,
					fileName: dto.FileName,
					storagePath: PathStorage.PersonalDocs_Dir,
					fileStream: new MemoryStream(dto.FileData),
					textDoc: dto.TextData
				);
		}

		[HttpDelete("DeletePersonalDoc/{profileId}/{typeId}")]
		public async Task<Response> DeleteDoc(int profileId, int typeId)
		{
			return await _master.DeletePersonDoc(profileId, typeId, PathStorage.PersonalDocs_Dir);
		}

		[HttpGet("DownloadPersonalDoc/{profileId}/{typeId}")]
		public async Task<ActionResult> Download(int profileId, int typeId)
		{
			var resp = await _master.DownloadPersonDoc(profileId, typeId, PathStorage.PersonalDocs_Dir);
			if (resp.IsSuccess && resp.Value?.FileData != null)
				return File(resp.Value.FileData, "application/x-rar-compressed", resp.Value.FileName);
			else
				return NotFound($"{resp.ErrorInfo} {resp.ErrorMessage}");
		}

		[HttpGet("GetPersonalPhoto/{profileId}")]
		public async Task<Response<byte[]>> DownloadPhoto(int profileId)
		{
			var resp = await _master.DownloadPersonDoc(profileId, 1, PathStorage.PersonalDocs_Dir);
			if (resp.IsSuccess && resp.Value?.FileData != null)
				return new(resp.Value.FileData);
			else
				return new(resp.ErrorMessage, resp.ErrorInfo);
		}

		[HttpGet("IsAnyNotifyNotRead/{profileId}")]
		public async Task<Response<bool>> IsReadNotify(int profileId)
		{
			return await _master.IsAnyNotifyNotRead(profileId);
		}

		[HttpGet("GetNotify")]
		public async Task<Response<IEnumerable<(NotificationDto? Notify, bool IsRead)>?>> GetNotifications()
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if (!ident.IsSuccess)
				return new(ident.ErrorMessage, ident.ErrorInfo);
			else
				return await _master.GetNotifications(ident.Value);
		}
	}
}