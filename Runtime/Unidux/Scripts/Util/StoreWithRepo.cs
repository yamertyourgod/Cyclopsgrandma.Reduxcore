using System;
using System.Threading.Tasks;

namespace Unidux.Util
{
    public abstract class StoreWithRepo<TState, TRepo> : Unidux.StoreBase<TState> where TState : StateBase where TRepo : RepositoryBase
    {
        protected override RepositoryBase repository { get; set; } = Activator.CreateInstance<TRepo>();

        public static TRepo Repository => GetRepository<TRepo>();

        public static async Task<TRepo> RepositoryAsync()
        {
            return await GetReposytoryAsync<TRepo>();
        }
    }

}
