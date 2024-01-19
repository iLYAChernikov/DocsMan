namespace DocsMan.Domain.Entity
{
	public class DocumentHistory
	{
		public int DocumentId { get; set; }
		public Document Document { get; set; }

		public int FileId { get; set; }
		public UploadFile File { get; set; }

		public DateTime DateTimeOfChanges { get; set; } = DateTime.Now;
		public string? Description { get; set; } = string.Empty;
	}
}