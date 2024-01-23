using DocsMan.App.Interactors;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private UserExec _master;

		public UserController(UserExec master)
		{
			_master = master;
		}

		[HttpPost("Create")]
		public async Task<Response> Create(UserDto user)
		{
			return await _master.Create(user);
		}

		[HttpGet("GetAll")]
		public async Task<Response<IEnumerable<UserDto?>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpGet("GetOneById/{userId}")]
		public async Task<Response<UserDto?>> GetOne(int userId)
		{
			return await _master.GetOne(userId);
		}

		[HttpGet("GetOneByEmail/{email}")]
		public async Task<Response<UserDto?>> GetOne(string email)
		{
			return await _master.GetOne(email);
		}

		[HttpGet("GetRoles/{userId}")]
		public async Task<Response<IEnumerable<RoleDto?>?>> GetRoles(int userId)
		{
			return await _master.GetRoles(userId);
		}

		[HttpPost("AddRole/{userId}/{roleId}")]
		public async Task<Response> AddRole(int userId, int roleId)
		{
			return await _master.AddRole(userId, roleId);
		}

		[HttpDelete("DeleteRole/{userId}/{roleId}")]
		public async Task<Response> DeleteRole(int userId, int roleId)
		{
			return await _master.DeleteRole(userId, roleId);
		}
	}
}