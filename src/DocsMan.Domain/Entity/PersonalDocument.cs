namespace DocsMan.Domain.Entity
{
	public class PersonalDocument
	{
		public int TypeId { get; set; }
		public PersonalDocumentType PersonalDocumentType { get; set; }

		public int ProfileId { get; set; }
		public Profile Profile { get; set; }

		public string Text { get; set; } = null!;

		public int FileId { get; set; }
		public UploadFile File { get; set; }
	}
}