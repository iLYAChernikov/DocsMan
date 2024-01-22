using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository
{
	public class ProfileRepository : IRepository<Profile>
	{
		private DocsMan_DBContext _context;

		public ProfileRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(Profile? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			await _context.Profiles.AddAsync(entity);
		}

		public async Task DeleteAsync(Profile? entity)
		{
			if ( entity == null ) throw new NullReferenceException("Not found");
			_context.Profiles.Remove(entity);
		}

		public async Task DeleteAsync(object key)
		{
			int id = (int) key;
			if ( id <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Profiles
				.FirstOrDefaultAsync(x => x.Id == id);
			if ( ent == null ) throw new NullReferenceException("Not found");
			_context.Profiles.Remove(ent);
		}

		public async Task DeleteAsync(object firstKey, object secondKey)
		{
			throw new NotSupportedException("Not a composite key");
		}

		public async Task<IEnumerable<Profile>?> GetAllAsync()
		{
			return _context.Profiles
				.Include(x => x.User);
		}

		public async Task<Profile> GetOneAsync(object key)
		{
			int id = (int) key;
			if ( id <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Profiles
				.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.Id == id);
			if ( ent == null ) throw new NullReferenceException("Not found");
			return ent;
		}

		public async Task<Profile> GetOneAsync(object firstKey, object secondKey)
		{
			throw new NotSupportedException("Not a composite key");
		}
	}
}