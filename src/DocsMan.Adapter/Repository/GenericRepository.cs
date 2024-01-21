using DocsMan.App.Storage.RepositoryPattern;

namespace DocsMan.Adapter.Repository
{
	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private DocsMan_DBContext _context;

		public GenericRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(TEntity? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			await _context.Set<TEntity>().AddAsync(entity);
		}

		public async Task DeleteAsync(TEntity? entity)
		{
			if ( entity == null ) throw new NullReferenceException("Not found");
			_context.Remove(entity);
		}

		public async Task DeleteAsync(int id)
		{
			if ( id <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Set<TEntity>().FindAsync(id);
			if ( ent == null ) throw new NullReferenceException("Not found");
			_context.Remove(ent);
		}

		public async Task DeleteAsync(int firstId, int secondId)
		{
			if ( firstId <= 0 || secondId <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Set<TEntity>().FindAsync(firstId, secondId);
			if ( ent == null ) throw new NullReferenceException("Not found");
			_context.Remove(ent);
		}

		public async Task<IEnumerable<TEntity>?> GetAllAsync()
		{
			return _context.Set<TEntity>();
		}

		public async Task<TEntity> GetOneAsync(int id)
		{
			if ( id <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Set<TEntity>().FindAsync(id);
			if ( ent == null ) throw new NullReferenceException("Not found");
			return ent;
		}

		public async Task<TEntity> GetOneAsync(int firstId, int secondId)
		{
			if ( firstId <= 0 || secondId <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Set<TEntity>().FindAsync(firstId, secondId);
			if ( ent == null ) throw new NullReferenceException("Not found");
			return ent;
		}
	}
}