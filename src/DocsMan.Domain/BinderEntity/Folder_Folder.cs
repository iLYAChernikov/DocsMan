using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class Folder_Folder
	{
		public int OwnerFolderId
		{
			get => _ownerId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения родительской папки");
				_ownerId = value;
			}
		}
		public Folder OwnerFolder { get; set; }

		public int ChildFolderId
		{
			get => _childId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения вложенной папки");
				_childId = value;
			}
		}
		public Folder ChildFolder { get; set; }

		private int _ownerId;
		private int _childId;
	}
}