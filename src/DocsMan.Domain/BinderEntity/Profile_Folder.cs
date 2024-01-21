using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Folder
	{
		public int ProfileId
		{
			get => _profileId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения профиля");
				_profileId = value;
			}
		}
		public Profile Profile { get; set; }

		public int FolderId
		{
			get => _folderId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения папки");
				_folderId = value;
			}
		}
		public Folder Folder { get; set; }

		private int _profileId;
		private int _folderId;
	}
}