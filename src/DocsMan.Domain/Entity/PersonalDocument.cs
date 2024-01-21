namespace DocsMan.Domain.Entity
{
	public class PersonalDocument
	{
		public int TypeId
		{
			get => _typeId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id типа личного документа");
				_typeId = value;
			}
		}
		public PersonalDocumentType PersonalDocumentType { get; set; }

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

		public string Text
		{
			get => _text;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения текста личного документа");
				_text = value;
			}
		}

		public int FileId
		{
			get => _fileId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id файла");
				_fileId = value;
			}
		}
		public UploadFile File { get; set; }

		private int _typeId;
		private int _profileId;
		private string _text = string.Empty;
		private int _fileId;
	}
}