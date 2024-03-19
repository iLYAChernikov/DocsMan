using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DocsMan.App.Interactors;
using DocsMan.Blazor.Server.DataStorage;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DocsMan.Blazor.Server.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AuthController : Controller
	{
		private DocsMan_AuthenticationOptions _authOptions;
		private UserExec _user;
		private ProfileExec _profile;

		public AuthController(UserExec master, DocsMan_AuthenticationOptions authOptions, ProfileExec profile)
		{
			_authOptions = authOptions;
			_user = master;
			_profile = profile;
		}

		[HttpGet("Check")]
		public async Task<bool> CheckAuth()
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				var check = await GetProfileId();
				return check.IsSuccess;
			}
			else
				return false;
		}

		[HttpGet("GetUserId")]
		public async Task<Response<int>> GetUserId()
		{
			int userId;
			int.TryParse(User.FindFirstValue(ClaimTypes.UserData), out userId);
			if (userId <= 0 || !(await _user.GetOne(userId)).IsSuccess)
				return new("Ошибка авторизации", "Id not found");
			else
				return new(userId);
		}

		[HttpGet("GetProfileId")]
		public async Task<Response<int>> GetProfileId()
		{
			var id_resp = await GetUserId();
			if (!id_resp.IsSuccess)
				return new("Ошибка авторизации", "Id not found");
			else
			{
				var profile = (await _profile.GetAll())?.Value?.FirstOrDefault(x => x.UserId == id_resp.Value);
				if (profile != null)
					return new(profile.Id);
				else
					return new("Ошибка авторизации", "Id not found");
			}
		}

		[HttpGet("GetEmail")]
		public async Task<Response<string>> GetEmail()
		{
			string? email = User.FindFirstValue(ClaimTypes.Email);
			if (email == null || string.IsNullOrWhiteSpace(email) || !(await _user.GetOne(email)).IsSuccess)
				return new("Ошибка авторизации", "Email not found");
			else
				return new(email);
		}

		[HttpGet("IsAdmin")]
		public async Task<Response<bool>> IsSuperAdmin()
		{
			var resp = await GetUserId();
			if (resp.IsSuccess)
				return await _user.IsUserSuperAdmin(resp.Value);
			else
				return new(resp.ErrorMessage, resp.ErrorInfo);
		}

		[AllowAnonymous]
		[HttpPost("GetToken")]
		public async Task<Response<string>> GetToken(AuthDto auth)
		{
			try
			{
				var resp_claims = await GetClaims(auth.Email, auth.Password);
				if (resp_claims.IsSuccess)
				{
					var result = resp_claims.Value;
					var nowTime = DateTime.UtcNow;

					var jwt = new JwtSecurityToken
						(
							issuer: _authOptions.Issuer,
							audience: _authOptions.Audience,
							notBefore: nowTime,
							claims: result,
							expires: nowTime.Add(_authOptions.LifeTime),
							signingCredentials: new SigningCredentials
								(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
						);
					var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

					return new(encodedJwt);
				}
				else
					return new(resp_claims.ErrorMessage, resp_claims.ErrorInfo);
			}
			catch (Exception ex)
			{
				return new("Ошибка авторизации", ex.Message);
			}
		}

		private async Task<Response<Claim[]?>> GetClaims(string email, string password)
		{
			var responseUser = await FindUser(email);
			if (responseUser.IsSuccess && responseUser.Value != null)
			{
				var user = responseUser.Value;
				if (user.Password == password)
				{
					var claims = new Claim[]
					{
						new Claim(ClaimTypes.NameIdentifier, $"user_{user.Email}"),
						new Claim(ClaimTypes.UserData, user.Id.ToString()),
						new Claim(ClaimTypes.Email, user.Email)
					};
					return new(claims);
				}
				else
					return new("Неверный пароль", "Invalid password");
			}
			else
				return new(responseUser.ErrorMessage, responseUser.ErrorInfo);
		}

		private async Task<Response<UserDto?>> FindUser(string email)
		{
			try
			{
				var respUser = await _user.GetOne(email);
				if (respUser.IsSuccess)
					return respUser;
				else
					return new("Пользователь не найден или не существует", respUser.ErrorInfo);
			}
			catch (Exception ex)
			{
				return new("Ошибка поиска пользователей", ex.Message);
			}
		}
	}
}