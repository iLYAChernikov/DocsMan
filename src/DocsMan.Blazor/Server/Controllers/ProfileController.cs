using DocsMan.App.Interactors;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProfileController : ControllerBase
	{
		private ProfileExec _master;

		public ProfileController(ProfileExec master)
		{
			_master = master;
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
		public async Task<Response<IEnumerable<ProfileDto>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpGet("GetPersonalDocs/{id}")]
		public async Task<Response<IEnumerable<PersonalDocumentDto>?>> GetPersonDocs(int id)
		{
			return await _master.GetAllPersonalDocuments(id);
		}

		[HttpPost("AddPersonalDoc")]
		public async Task<Response> AddDoc(PersonalDocumentDataDto dto)
		{
			if ( dto.FileData == null ) return new("Нет файла", "Empty file");

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
	}
}