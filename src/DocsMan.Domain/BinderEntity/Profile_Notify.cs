using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Notify
	{
		public int ProfileId
		{
			get => _profileId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id профиля");
				_profileId = value;
			}
		}
		public Profile Profile { get; set; }

		public int NotificationId
		{
			get => _notificationId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id уведомления");
				_notificationId = value;
			}
		}
		public Notification Notification { get; set; }

		public bool IsRead { get; set; } = false;

		private int _profileId;
		private int _notificationId;
	}
}