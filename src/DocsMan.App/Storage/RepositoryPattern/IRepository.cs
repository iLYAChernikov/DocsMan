namespace DocsMan.App.Storage.RepositoryPattern
{
	public interface IRepository<Entity> where Entity : class
	{
		public Task CreateAsync( Entity entity );

		public Task<Entity?> GetOneAsync( int id );
		public Task<Entity?> GetOneAsync( int firstId, int secondId );    //	если составной ключ

		public Task<IEnumerable<Entity>?> GetAllAsync();

		public Task UpdateAsync( Entity entity );

		public Task DeleteAsync( Entity entity );
	}
}