namespace DocsMan.Blazor.Shared.DTOs
{
	public class FolderDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public bool IsDeleted { get; set; } = false;
		public string? FolderSize { get; set; } = string.Empty;
		public int FilesCount { get; set; }
	}
}