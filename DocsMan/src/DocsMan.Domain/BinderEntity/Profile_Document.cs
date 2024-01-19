using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Document
	{
		public int ProfileId { get; set; }
		public Profile Profile { get; set; }

		public int DocumentId { get; set; }
		public Document Document { get; set; }
	}
}