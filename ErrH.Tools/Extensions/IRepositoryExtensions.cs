using System.Linq;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Tools.Extensions
{
    public static class IRepositoryExtensions
    {
        public static T ByNid<T>(this IRepository<T> repo, int nid) 
            where T : ID7Node
        {
            var ret = repo.All.FirstOrDefault(x => x.nid == nid);

            if (ret == null)
                repo.Warn_n($"No nid matching [{nid}].", 
                    $"‹{repo.GetType().Name}› : repo for ‹{typeof(T).Name}›.");

            return ret;
        }
    }
}
