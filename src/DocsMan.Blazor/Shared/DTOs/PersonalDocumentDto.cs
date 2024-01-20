namespace DocsMan.Blazor.Shared.DTOs
{
	public class PersonalDocumentDto
	{
		public int TypeId { get; set; }
		public PersonalDocumentTypeDto PersonalDocumentType { get; set; }

		public int ProfileId { get; set; }

		public string Text { get; set; } = null!;

		public int FileId { get; set; }
	}
}