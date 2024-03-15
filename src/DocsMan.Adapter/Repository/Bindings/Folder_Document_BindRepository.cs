using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository.Bindings
{
	public class Folder_Document_BindRepository : IBindingRepository<Folder_Document>
	{
		private DocsMan_DBContext _context;

		public Folder_Document_BindRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateBindAsync(Folder_Document? bind)
		{
			if (bind == null || bind.FolderId <= 0 || bind.DocumentId <= 0)
				throw new ArgumentNullException("Null input data");
			await _context.Folder_Documents.AddAsync(bind);
		}

		public async Task DeleteBindAsync(Folder_Document? bind)
		{
			if (bind == null || bind.FolderId <= 0 || bind.DocumentId <= 0)
				throw new ArgumentNullException("Null input data");
			_context.Folder_Documents.Remove(bind);
		}

		public async Task<IEnumerable<Folder_Document>?> GetAllBinds()
		{
			return _context.Folder_Documents
				.Include(x => x.Folder)
				.Include(x => x.Document)
				.ThenInclude(x => x.File);
		}

		public async Task<IEnumerable<Folder_Document>?> GetAllBindsNoTracking()
		{
			return _context.Folder_Documents
				.Include(x => x.Folder)
				.Include(x => x.Document)
				.ThenInclude(x => x.File)
				.AsNoTracking();
		}
	}
}