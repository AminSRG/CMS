using AS.BaseModels.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AS.BaseModels.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : AS.BaseModels.BaseEntitys.Abstracts.EntityID
    {
        protected internal Repository(DbContext databaseContext)
        {
            DatabaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            DbSet = DatabaseContext.Set<T>();
        }

        protected DbSet<T> DbSet { get; }
        protected DbContext DatabaseContext { get; }

        protected async Task SaveAsync()
        {
            try
            {
                await DatabaseContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exceptions here
                throw new Exception("Error saving changes to the database.", ex);
            }
        }

        public virtual async Task<bool> InsertAsync(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                await DbSet.AddAsync(entity);
                await SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                throw new Exception("Error inserting entity.", ex);
            }
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                await RemoveById(entity.ID);
                DbSet.Update(entity);
                await SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                throw new Exception("Error updating entity.", ex);
            }
        }

        private async Task RemoveById(string Id)
        {
            try
            {
                var entityForRemove = await DbSet.FirstOrDefaultAsync(current => current.ID == Id);
                if (entityForRemove != null)
                {
                    DbSet.Remove(entityForRemove);
                    await SaveAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                throw new Exception("Error removing entity by ID.", ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null) throw new ArgumentNullException(nameof(entity));

                DbSet.Remove(entity);
                await SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                throw new Exception("Error deleting entity.", ex);
            }
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            try
            {
                return await DbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                throw new Exception("Error getting entity by ID.", ex);
            }
        }

        public virtual async Task<IList<T>> GetAllAsync()
        {
            try
            {
                return await DbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle other exceptions here
                throw new Exception("Error getting all entities.", ex);
            }
        }
    }
}
