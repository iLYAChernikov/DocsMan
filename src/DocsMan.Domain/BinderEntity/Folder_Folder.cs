using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Folder_Folder
	{
		public int OwnerFolderId { get; set; }
		public Folder OwnerFolder { get; set; }

		public int ChildFolderId { get; set; }
		public Folder ChildFolder { get; set; }
	}
}