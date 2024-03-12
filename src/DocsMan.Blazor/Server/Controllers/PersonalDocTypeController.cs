using DocsMan.App.Interactors;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class PersonalDocTypeController : ControllerBase
	{
		private PersonalDocumentTypeExec _master;

		public PersonalDocTypeController(PersonalDocumentTypeExec master)
		{
			_master = master;
		}

		[HttpGet("GetAll")]
		public async Task<Response<IEnumerable<PersonalDocumentTypeDto>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpGet("GetOneById/{typeId}")]
		public async Task<Response<PersonalDocumentTypeDto?>> GetOne(int typeId)
		{
			return await _master.GetOne(typeId);
		}

		[HttpGet("GetOneByTitle/{title}")]
		public async Task<Response<PersonalDocumentTypeDto?>> GetOne(string title)
		{
			return await _master.GetOne(title);
		}

		[HttpPost("Create")]
		public async Task<Response> Create(PersonalDocumentTypeDto dto)
		{
			return await _master.Create(dto);
		}

		[HttpDelete("Delete/{typeId}")]
		public async Task<Response> Delete(int typeId)
		{
			return await _master.Delete(typeId);
		}
	}
}