using System.Security.Claims;
using DocsMan.App.Interactors;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocsMan.Blazor.Server.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class NotifyController : ControllerBase
	{
		private NotifyExec _master;
		private AuthExec _auth;

		public NotifyController(NotifyExec master, AuthExec auth)
		{
			_master = master;
			_auth = auth;
		}

		[HttpPost("Create")]
		public async Task<Response<int>> Create(NotificationDto dto)
		{
			return await _master.CreateNotify(dto);
		}

		[HttpDelete("Delete/{id}")]
		public async Task<Response> Delete(int id)
		{
			return await _master.DeleteNotify(id);
		}

		[HttpGet("GetAll")]
		public async Task<Response<IEnumerable<NotificationDto>?>> GetAll()
		{
			return await _master.GetAll();
		}

		[HttpDelete("Bind/{profileId}/{notifyId}")]
		public async Task<Response> Add(int profileId, int notifyId)
		{
			return await _master.CreateBindNotify(profileId, notifyId);
		}

		[HttpDelete("DeleteBind/{profileId}/{notifyId}")]
		public async Task<Response> Delete(int profileId, int notifyId)
		{
			return await _master.DeleteBindNotify(profileId, notifyId);
		}

		[HttpDelete("Read/{notifyId}")]
		public async Task<Response> Read(int notifyId)
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if (!ident.IsSuccess)
				return new(ident.ErrorMessage, ident.ErrorInfo);
			else
				return await _master.ReadNotify(ident.Value, notifyId);
		}

		[HttpDelete("Forget/{notifyId}")]
		public async Task<Response> Forget(int notifyId)
		{
			var ident = await _auth.GetProfileId(User.FindFirstValue(ClaimTypes.UserData));
			if (!ident.IsSuccess)
				return new(ident.ErrorMessage, ident.ErrorInfo);
			else
				return await _master.ForgetNotify(ident.Value, notifyId);
		}
	}
}