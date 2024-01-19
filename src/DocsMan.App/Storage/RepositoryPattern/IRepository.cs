namespace DocsMan.App.Storage.RepositoryPattern
{
	public interface IRepository<TEntity> where TEntity : class
	{
		public Task CreateAsync(TEntity entity);

		public Task<TEntity?> GetOneAsync(int id);
		public Task<TEntity?> GetOneAsync(int firstId, int secondId);    //	если составной ключ

		public Task<IEnumerable<TEntity>?> GetAllAsync();

		public Task UpdateAsync(TEntity entity);

		public Task DeleteAsync(TEntity entity);
	}
}