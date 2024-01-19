using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Notify
	{
		public int ProfileId { get; set; }
		public Profile Profile { get; set; }

		public int NotificationId { get; set; }
		public Notification Notification { get; set; }

		public bool IsRead { get; set; } = false;
	}
}