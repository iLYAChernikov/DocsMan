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
				if (value <= 0)
					throw new ArgumentNullException("Ошибка заполнения id папки");
				_folderId = value;
			}
		}
		public Folder Folder { get; set; }

		public int DocumentId
		{
			get => _docId;
			set
			{
				if (value <= 0)
					throw new ArgumentNullException("Ошибка заполнения id файла");
				_docId = value;
			}
		}
		public Document Document { get; set; }

		private int _folderId;
		private int _docId;
	}
}