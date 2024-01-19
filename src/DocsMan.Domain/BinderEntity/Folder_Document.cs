using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Folder_Document
	{
		public int FolderId { get; set; }
		public Folder Folder { get; set; }

		public int FileId { get; set; }
		public UploadFile File { get; set; }
	}
}