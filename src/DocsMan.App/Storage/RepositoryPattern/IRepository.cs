namespace DocsMan.App.Storage.RepositoryPattern
{
	public interface IRepository<TEntity> where TEntity : class
	{
		public Task CreateAsync(TEntity? entity);

		public Task<TEntity> GetOneAsync(object key);
		public Task<TEntity> GetOneAsync(object firstKey, object secondKey);    //	if composite key

		public Task<IEnumerable<TEntity>?> GetAllAsync();

		public Task DeleteAsync(TEntity? entity);
		public Task DeleteAsync(object key);
		public Task DeleteAsync(object firstKey, object secondKey);     //	if composite key
	}
}