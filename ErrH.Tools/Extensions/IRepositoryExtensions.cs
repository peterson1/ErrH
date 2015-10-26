using System.Linq;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Tools.Extensions
{
    public static class IRepositoryExtensions
    {
        public static T ByNid<T>(this IRepository<T> repo, int nid) where T : ID7Node
            => repo.All.FirstOrDefault(x => x.nid == nid);
    }
}
