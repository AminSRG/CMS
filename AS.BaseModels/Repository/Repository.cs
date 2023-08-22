using AS.BaseModels.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AS.BaseModels.Repository
{
    public abstract class Repository<T> : object, IRepository<T> where T : AS.BaseModels.BaseEntitys.Abstracts.EntityID
    {
        protected internal Repository(DbContext databaseContext) : base()
        {
            DatabaseContext =
                databaseContext ??
                throw new ArgumentNullException(paramName: nameof(databaseContext));

            DbSet = DatabaseContext.Set<T>();
        }


        protected DbSet<T> DbSet { get; }
        protected DbContext DatabaseContext { get; }

        public virtual async Task<bool> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName: nameof(entity));
            }

            var response = await DbSet.AddAsync(entity);

            SaveAsync();

            if (response == null) return false;

            return true;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName: nameof(entity));
            }

            EntityEntry<T>? response = null;

            await RemoveById(entity.ID);

            await Task.Run(() =>
            {
                response = DbSet.Update(entity);
            });

            SaveAsync();

            if (response == null) return false;

            return true;
        }

        private async Task RemoveById(string Id)
        {
            var entityForRemove = await DbSet.Where(current => current.ID == Id).FirstAsync();
            DbSet.Remove(entityForRemove);
            SaveAsync();
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(paramName: nameof(entity));
            }

            EntityEntry<T>? response = null;
            await Task.Run(() =>
            {
                response = DbSet.Remove(entity);
            });

            SaveAsync();

            if (response == null) return false;

            return true;
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await DbSet.FindAsync(keyValues: id);
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            var result =
                await
                DbSet.ToListAsync();

            return result;
        }


        private async void SaveAsync()
        {
            await DatabaseContext.SaveChangesAsync();
        }
    }
}
