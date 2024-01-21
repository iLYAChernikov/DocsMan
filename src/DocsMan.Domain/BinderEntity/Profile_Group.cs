using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Group
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

		public int GroupId
		{
			get => _groupId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id группы");
				_groupId = value;
			}
		}
		public Group Group { get; set; }

		public bool IsAdmin { get; set; } = false;

		private int _profileId;
		private int _groupId;
	}
}