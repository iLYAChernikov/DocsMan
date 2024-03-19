using System.Net;
using DocsMan.Blazor.Shared.OutputData;

namespace DocsMan.Blazor.Shared.Helpers
{
	public class RequestResultDto
	{
		public HttpStatusCode? ResponseStatusCode { get; set; } = null;
		public Response Response { get; set; } = new();

		public RequestResultDto() { }
		public RequestResultDto(HttpStatusCode? statusCode, Response? response)
		{
			ResponseStatusCode = statusCode;
			Response = response == null ? new("Ошибка запроса", "Request Error") : response;
		}
	}

	public class RequestResultDto<T> : RequestResultDto
	{
		public T? Value { get; set; }

		public RequestResultDto() { }
		public RequestResultDto(HttpStatusCode? statusCode, Response<T>? response)
			: base(statusCode, response)
		{
			Value = response == null ? default : response.Value;
		}
	}
}