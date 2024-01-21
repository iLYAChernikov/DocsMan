using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository
{
	public class PersonalDocumentRepository : IRepository<PersonalDocument>
	{
		private DocsMan_DBContext _context;

		public PersonalDocumentRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(PersonalDocument? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			await _context.PersonalDocuments.AddAsync(entity);
		}

		public async Task DeleteAsync(PersonalDocument? entity)
		{
			if ( entity == null ) throw new NullReferenceException("Not found");
			_context.PersonalDocuments.Remove(entity);
		}

		public async Task DeleteAsync(int id)
		{
			throw new NotSupportedException("Has a composite key");
		}

		public async Task DeleteAsync(int firstId, int secondId)
		{
			if ( firstId <= 0 || secondId <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.PersonalDocuments
				.FirstOrDefaultAsync(x => x.ProfileId == firstId && x.TypeId == secondId);
			if ( ent == null ) throw new NullReferenceException("Not found");
			_context.Remove(ent);
		}

		public async Task<IEnumerable<PersonalDocument>?> GetAllAsync()
		{
			return _context.PersonalDocuments
				.Include(x => x.PersonalDocumentType)
				.Include(x => x.File);
		}

		public async Task<PersonalDocument> GetOneAsync(int id)
		{
			throw new NotSupportedException("Has a composite key");
		}

		public async Task<PersonalDocument> GetOneAsync(int firstId, int secondId)
		{
			if ( firstId <= 0 || secondId <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.PersonalDocuments
				.Include(x => x.PersonalDocumentType)
				.Include(x => x.File)
				.FirstOrDefaultAsync(x => x.ProfileId == firstId && x.TypeId == secondId);
			if ( ent == null ) throw new NullReferenceException("Not found");
			return ent;
		}
	}
}