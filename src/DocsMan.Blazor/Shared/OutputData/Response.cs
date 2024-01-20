namespace DocsMan.Blazor.Shared.OutputData
{
	/// <summary>
	/// Ответ на запрос с информацией об успешности,
	/// <br>сообщением ошибки для пользователя и для внутренней обработки.</br>
	/// </summary>
	public class Response
	{
		public bool IsSuccess { get; set; }

		//	for frontView
		public string? ErrorMessage { get; set; } = string.Empty;

		//	for backView
		public string? ErrorInfo { get; set; } = string.Empty;

		public Response() { }

		public Response(bool success)
		{
			IsSuccess = true;
		}

		public Response(string errorMessage, string errorInfo)
		{
			IsSuccess = false;
			ErrorMessage = errorMessage;
			ErrorInfo = errorInfo;
		}
	}

	/// <summary>
	/// Ответ на запрос с информацией об успешности,
	/// <br>сообщением ошибки для пользователя и для внутренней обработки.</br>
	/// <br>Содержит данные типа "T" </br>
	/// </summary>
	public class Response<T> : Response
	{
		public T? Value { get; set; }

		public Response(T? data)
		{
			Value = data;
			IsSuccess = true;
		}

		public Response(string errorMessage, string errorInfo)
		{
			IsSuccess = false;
			ErrorMessage = errorMessage;
			ErrorInfo = errorInfo;
		}
	}
}