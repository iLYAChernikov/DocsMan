namespace DocsMan.Domain.Entity
{
	public class Document
	{
		public int Id { get; set; }

		public string Name
		{
			get => _name;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения имени");
				_name = value;
			}
		}
		public string FileType
		{
			get => _fileType;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения типа файла");
				_fileType = value;
			}
		}

		public string? Description { get; set; } = string.Empty;

		public int FileId
		{
			get => _fileId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения файла");
				_fileId = value;
			}
		}
		public UploadFile UploadFile { get; set; }

		public bool IsDeleted { get; set; } = false;

		private string _name = string.Empty;
		private string _fileType = string.Empty;
		private int _fileId;
	}
}