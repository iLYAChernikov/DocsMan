namespace DocsMan.Domain.Entity
{
	public class Document
	{
		public int Id
		{
			get => _id;
			set
			{
				if (value < 0)
					throw new ArgumentNullException("Ошибка заполнения id документа");
				_id = value;
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("Ошибка заполнения имени документа");
				_name = value;
			}
		}
		public string FileType
		{
			get => _fileType;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("Ошибка заполнения типа файла");
				_fileType = value;
			}
		}

		public string? Description { get; set; } = string.Empty;

		public int FileId
		{
			get => _fileId;
			set
			{
				if (value <= 0)
					throw new ArgumentNullException("Ошибка заполнения id файла");
				_fileId = value;
			}
		}
		public UploadFile File { get; set; }

		public bool IsDeleted { get; set; } = false;

		private int _id;
		private string _name = string.Empty;
		private string _fileType = string.Empty;
		private int _fileId;
	}
}