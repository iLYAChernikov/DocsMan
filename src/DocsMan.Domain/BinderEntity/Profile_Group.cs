using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Group
	{
		public int ProfileId { get; set; }
		public Profile Profile { get; set; }

		public int GroupId { get; set; }
		public Group Group { get; set; }

		public bool IsAdmin { get; set; } = false;
	}
}