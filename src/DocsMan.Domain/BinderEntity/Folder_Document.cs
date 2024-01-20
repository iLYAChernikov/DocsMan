using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Folder_Document
	{
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
		public UploadFile File { get; set; }

		private int _folderId;
		private int _fileId;
	}
}