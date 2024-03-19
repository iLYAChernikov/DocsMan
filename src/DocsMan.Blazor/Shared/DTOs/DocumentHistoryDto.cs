namespace DocsMan.Blazor.Shared.DTOs
{
	public class DocumentHistoryDto
	{
		public int DocumentId { get; set; }

		public int FileId { get; set; }

		public string DateTimeOfChanges { get; set; } = string.Empty;
		public string? Description { get; set; } = string.Empty;
	}
}