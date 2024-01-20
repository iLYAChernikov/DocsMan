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
					throw new NullReferenceException("Ошибка заполнения документа");
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
					throw new NullReferenceException("Ошибка заполнения описания");
				_fileId = value;
			}
		}
		public UploadFile File { get; set; }

		public DateTime DateTimeOfChanges { get; set; } = DateTime.Now;
		public string? Description { get; set; } = string.Empty;

		private int _documentId;
		private int _fileId;
	}
}