using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository.Bindings
{
	public class Profile_Document_BindRepository : IBindingRepository<Profile_Document>
	{
		private DocsMan_DBContext _context;

		public Profile_Document_BindRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateBindAsync(Profile_Document? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.DocumentId <= 0)
				throw new ArgumentNullException("Null input data");
			await _context.Profile_Documents.AddAsync(bind);
		}

		public async Task DeleteBindAsync(Profile_Document? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.DocumentId <= 0)
				throw new ArgumentNullException("Null input data");
			_context.Profile_Documents.Remove(bind);
		}

		public async Task<IEnumerable<Profile_Document>?> GetAllBinds()
		{
			return _context.Profile_Documents
				.Include(x => x.Profile)
				.ThenInclude(x => x.User)
				.Include(x => x.Document);
		}

		public async Task<IEnumerable<Profile_Document>?> GetAllBindsNoTracking()
		{
			return _context.Profile_Documents
				.Include(x => x.Profile)
				.ThenInclude(x => x.User)
				.Include(x => x.Document)
				.AsNoTracking();
		}
	}
}