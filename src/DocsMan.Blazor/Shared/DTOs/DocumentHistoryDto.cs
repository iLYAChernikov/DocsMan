namespace DocsMan.Blazor.Shared.DTOs
{
	public class DocumentHistoryDto
	{
		public int DocumentId { get; set; }

		public int FileId { get; set; }

		public DateTime DateTimeOfChanges { get; set; } = DateTime.Now;
		public string? Description { get; set; } = string.Empty;
	}
}