using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository.Bindings
{
	public class Profile_Folder_BindRepository : IBindingRepository<Profile_Folder>
	{
		private DocsMan_DBContext _context;

		public Profile_Folder_BindRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateBindAsync(Profile_Folder? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.FolderId <= 0)
				throw new ArgumentNullException("Null input data");
			await _context.Profile_Folders.AddAsync(bind);
		}

		public async Task DeleteBindAsync(Profile_Folder? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.FolderId <= 0)
				throw new ArgumentNullException("Null input data");
			_context.Profile_Folders.Remove(bind);
		}

		public async Task<IEnumerable<Profile_Folder>?> GetAllBinds()
		{
			return _context.Profile_Folders
				.Include(x => x.Profile)
				.ThenInclude(x => x.User)
				.Include(x => x.Folder);
		}

		public async Task<IEnumerable<Profile_Folder>?> GetAllBindsNoTracking()
		{
			return _context.Profile_Folders
				.Include(x => x.Profile)
				.ThenInclude(x => x.User)
				.Include(x => x.Folder)
				.AsNoTracking();
		}
	}
}