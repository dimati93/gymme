using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;

using Gymme.Data.Core;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public abstract class RepositoryBase<T>  where T : Model
    {
        private readonly Table<T> _table;

        protected RepositoryBase()
        {
            _table = DatabaseContext.Instance.GetTable<T>();
        }

        public Table<T> Table
        {
            get { return _table;}
        }

        public virtual IEnumerable<T> FindAll()
        {
            return Table.Select(x => x);
        }

        public abstract T FindById(long id);

        public abstract bool Exists(long id);
        
        public virtual void Save(T entity)
        {
            InsertOnDemand(entity);
            DatabaseContext.Instance.SubmitChanges();
        }

        public virtual void Save(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                InsertOnDemand(entity);
            }

            DatabaseContext.Instance.SubmitChanges();
        }

        private void InsertOnDemand(T entity)
        {
            if (entity.IsNew)
            {
                Table.InsertOnSubmit(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            Table.DeleteOnSubmit(entity);
            DatabaseContext.Instance.SubmitChanges();
        }
    }
}