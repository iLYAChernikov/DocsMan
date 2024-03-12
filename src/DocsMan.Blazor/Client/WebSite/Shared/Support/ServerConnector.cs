using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AltairCA.Blazor.WebAssembly.Cookie;
using DocsMan.Blazor.Shared.DTOs;
using DocsMan.Blazor.Shared.Helpers;
using DocsMan.Blazor.Shared.OutputData;

namespace DocsMan.Blazor.Client.WebSite.Shared.Support
{
	public class ServerConnector
	{
		private IAltairCABlazorCookieUtil _cooker;

		public ServerConnector(IAltairCABlazorCookieUtil cooker)
		{
			_cooker = cooker;
		}

		private static string ServerPath => "https://localhost:7070/";
		protected static HttpClient ServerOk = new() { BaseAddress = new Uri(ServerPath) };

		public async Task<string?> GetCookie(string name) =>
			await _cooker.GetValueAsync(name);

		public async Task SetCookie(string name, string value) =>
			await _cooker.SetValueAsync(name, value);

		public async Task RemoveCookie(string name) =>
			await _cooker.RemoveAsync(name);

		public async Task<bool> IsUserAuthorized()
		{
			try
			{
				var token = await GetCookie("JWT_Token");
				if (token != null)
				{
					ServerOk.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
					using HttpResponseMessage request = await ServerOk.GetAsync("Auth/Check");
					request.EnsureSuccessStatusCode();

					return await request.Content.ReadFromJsonAsync<bool>();
				}
				else
					return false;
			}
			catch
			{
				return false;
			}
		}

		public async Task<RequestResultDto> DoAuthorization(AuthDto auth)
		{
			try
			{
				using HttpResponseMessage request = await ServerOk.PostAsJsonAsync("Auth/GetToken", auth);
				request.EnsureSuccessStatusCode();
				var resp = await request.Content.ReadFromJsonAsync<Response<string>>();
				if (resp != null)
					if (resp.IsSuccess && !string.IsNullOrWhiteSpace(resp.Value))
					{
						await SetCookie("JWT_Token", resp.Value);
						return new(request.StatusCode, new());
					}
					else
						return new(request.StatusCode, resp);
				else
					return new(HttpStatusCode.BadRequest, new("Ошибка авторизации", "Authorization Error"));
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}

		public async Task<RequestResultDto> DoRegistration(UserDto user)
		{
			try
			{
				using HttpResponseMessage request = await ServerOk.PostAsJsonAsync("User/Create", user);
				request.EnsureSuccessStatusCode();
				var resp = await request.Content.ReadFromJsonAsync<Response>();
				if (resp != null)
					return new(request.StatusCode, resp);
				else
					return new(HttpStatusCode.BadRequest, new("Ошибка регистрации", "Reg Error"));
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}

		public async Task LogOut()
		{
			await RemoveCookie("JWT_Token");
			ServerOk.DefaultRequestHeaders.Authorization = null;
		}

		public static string DoDownload(string path) =>
			ServerOk.BaseAddress + path;
	}

	public class ServerGet<Output_T> : ServerConnector
	{
		public ServerGet(IAltairCABlazorCookieUtil cooker) : base(cooker) { }

		public async Task<RequestResultDto<Output_T>> DoRequest_GET(string path)
		{
			try
			{
				if (!await IsUserAuthorized())
					return new(HttpStatusCode.Unauthorized,
						new("Вы не Авторизованы или время Вашей Сессии истекло. \nВойдите в систему еще раз", "Auth Request Error"));

				using var request = await ServerOk.GetAsync(path);
				request.EnsureSuccessStatusCode();

				var resp = await request.Content.ReadFromJsonAsync<Response<Output_T>>();
				return new(request.StatusCode, resp);
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}
	}

	public class ServerPost<Input_T> : ServerConnector
	{
		public ServerPost(IAltairCABlazorCookieUtil cooker) : base(cooker) { }

		public async Task<RequestResultDto> DoRequest_POST(string path, Input_T inputData)
		{
			try
			{
				if (!await IsUserAuthorized())
					return new(HttpStatusCode.Unauthorized,
						new("Вы не Авторизованы или время Вашей Сессии истекло. \nВойдите в систему еще раз", "Auth Request Error"));

				using var request = await ServerOk.PostAsJsonAsync(path, inputData);
				request.EnsureSuccessStatusCode();

				var resp = await request.Content.ReadFromJsonAsync<Response>();
				return new(request.StatusCode, resp);
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}

		public async Task<RequestResultDto<int>> DoRequest_POST_int(string path, Input_T inputData)
		{
			try
			{
				if (!await IsUserAuthorized())
					return new(HttpStatusCode.Unauthorized,
						new("Вы не Авторизованы или время Вашей Сессии истекло. \nВойдите в систему еще раз", "Auth Request Error"));

				using var request = await ServerOk.PostAsJsonAsync(path, inputData);
				request.EnsureSuccessStatusCode();

				var resp = await request.Content.ReadFromJsonAsync<Response<int>>();
				return new(request.StatusCode, resp);
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}
	}

	public class ServerDelete : ServerConnector
	{
		public ServerDelete(IAltairCABlazorCookieUtil cooker) : base(cooker) { }

		public async Task<RequestResultDto> DoRequest_DELETE(string path)
		{
			try
			{
				if (!await IsUserAuthorized())
					return new(HttpStatusCode.Unauthorized,
						new("Вы не Авторизованы или время Вашей Сессии истекло. \nВойдите в систему еще раз", "Auth Request Error"));

				using var request = await ServerOk.DeleteAsync(path);
				request.EnsureSuccessStatusCode();

				var resp = await request.Content.ReadFromJsonAsync<Response>();
				return new(request.StatusCode, resp);
			}
			catch (HttpRequestException ex)
			{
				return new(ex.StatusCode,
					new("Ошибка запроса", ex.Message));
			}
			catch (Exception ex)
			{
				return new(HttpStatusCode.BadRequest,
					new("Ошибка запроса", ex.Message));
			}
		}
	}
}