namespace DocsMan.Domain.Entity
{
	public class Document
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;
		public string FileType { get; set; } = null!;

		public string? Description { get; set; } = string.Empty;

		public int FileId { get; set; }
		public UploadFile UploadFile { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}