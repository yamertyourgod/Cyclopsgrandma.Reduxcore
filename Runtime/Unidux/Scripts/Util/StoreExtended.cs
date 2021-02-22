using System;
using System.Threading.Tasks;

namespace Unidux.Util
{
    public abstract class StoreExtended<TState, TRepo, TServices, TEntities> : StoreWithRepo<TState, TRepo> where TState : StateBase where TRepo : RepositoryBase where TServices: ServicesBase where TEntities: EntitiesBase
    {
        protected override ServicesBase services { get; set; } = Activator.CreateInstance<TServices>();
        protected override EntitiesBase entities { get; set; } = Activator.CreateInstance<TEntities>();

        public static TServices Services => GetServices<TServices>();
        public static TEntities Entities => GetEntities<TEntities>();

    }

}
