namespace DocsMan.Domain.Entity
{
	public class DocumentHistory
	{
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

		public string DateTimeOfChanges
		{
			get => _datetime;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения даты-времени истории");
				_datetime = value;
			}
		}

		public string? Description { get; set; } = string.Empty;

		private int _documentId;
		private int _fileId;
		private string _datetime = string.Empty;
	}
}