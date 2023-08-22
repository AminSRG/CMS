namespace AS.BaseModels.IRepository
{
    public interface IQueryRepository<T> where T : AS.BaseModels.BaseEntitys.Abstracts.BaseEntity
    {
        Task<T> GetAsync(T model);
        Task<T> GetByIdAsync(string id);
        Task<IList<T>> GetAllAsync();
    }
}
