namespace DocsMan.Blazor.Shared.Helpers
{
	public class DataFile
	{
		public int OwnerId { get; set; }
		public string FileName { get; set; } = string.Empty;
		public byte[]? FileData { get; set; }
	}

	public class PersonalDocumentDataDto : DataFile
	{
		public int PersonalDocumentTypeId { get; set; }
		public string TextData { get; set; } = string.Empty;
	}
}