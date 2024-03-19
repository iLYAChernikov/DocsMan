namespace DocsMan.Blazor.Shared.DTOs
{
	public class DocumentDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public string FileType { get; set; } = null!;

		public string? Description { get; set; } = string.Empty;

		public int FileId { get; set; }

		public bool IsDeleted { get; set; } = false;
		public string? FileSize { get; set; } = string.Empty;
	}
}