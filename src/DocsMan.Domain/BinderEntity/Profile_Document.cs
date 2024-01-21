using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Profile_Document
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

		public int DocumentId
		{
			get => _documentId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id документа");
				_documentId = value;
			}
		}
		public Document Document { get; set; }

		private int _profileId;
		private int _documentId;
	}
}