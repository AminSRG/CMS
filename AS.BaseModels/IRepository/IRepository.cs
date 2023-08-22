namespace AS.BaseModels.IRepository
{
    public interface IRepository<T> where T : BaseEntitys.Abstracts.BaseEntity
    {
        Task<bool> InsertAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<IList<T>> GetAllAsync();
    }
}
