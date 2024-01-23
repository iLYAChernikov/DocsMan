using DocsMan.App.Interactors;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RoleController : ControllerBase
	{
		private RoleExec _master;

		public RoleController(RoleExec master)
		{
			_master = master;
		}

		[HttpPost("Create")]
		public async Task<Response> Create(RoleDto role)
		{
			return await _master.Create(role);
		}

		[HttpDelete("Delete/{roleId}")]
		public async Task<Response> Delete(int roleId)
		{
			return await _master.Delete(roleId);
		}

		[HttpGet("GetAll")]
		public async Task<Response<IEnumerable<RoleDto?>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpGet("GetOneById/{roleId}")]
		public async Task<Response<RoleDto?>> GetOne(int roleId)
		{
			return await _master.GetOne(roleId);
		}

		[HttpGet("GetOneByTitle/{title}")]
		public async Task<Response<RoleDto?>> GetOne(string title)
		{
			return await _master.GetOne(title);
		}

		[HttpGet("GetUsers/{roleId}")]
		public async Task<Response<IEnumerable<UserDto?>?>> GetUsers(int roleId)
		{
			return await _master.GetUsers(roleId);
		}
	}
}