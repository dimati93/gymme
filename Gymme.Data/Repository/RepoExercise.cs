using System.Collections.Generic;
using System.Linq;
using Gymme.Data.Models;

namespace Gymme.Data.Repository
{
    public class RepoExercise : RepositoryBase<Exercise>
    {
        private static RepoExercise _instance;

        public static RepoExercise Instance { get { return _instance ?? (_instance = new RepoExercise()); } }

        private RepoExercise()
        {
        }

        public override Exercise FindById(long id)
        {
            return Table.SingleOrDefault(x => x.Id == id);
        }

        public override bool Exists(long id)
        {
            return Table.Any(x => x.Id == id);
        }

        public override void Save(Exercise entity)
        {
            if (entity.Order == null)
            {
                entity.Order = GetMaxOrder(entity) + 1;
            }

            base.Save(entity);
        }

        public override void Save(IEnumerable<Exercise> entities)
        {
            var entitiesArray = entities as Exercise[] ?? entities.ToArray();
            var emptyOrder = entitiesArray.Where(x => x.Order == null).ToArray();

            if (emptyOrder.Length != 0)
            {
                double currentOrder = GetMaxOrder(emptyOrder[0]) + 1;
                
                foreach (var entity in emptyOrder)
                {
                    entity.Order = currentOrder;
                    currentOrder++;
                }
            }

            base.Save(entitiesArray);
        }

        private double GetMaxOrder(Exercise entity)
        {
            return Table.Where(x => x.IdWorkout == entity.IdWorkout).Max(x => x.Order) ?? 0;
        }
    }
}
