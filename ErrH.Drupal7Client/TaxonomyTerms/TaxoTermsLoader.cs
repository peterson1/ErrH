using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.Drupal7Client.TaxonomyTerms
{
    internal class TaxoTermsLoader
    {
        internal bool                 IsLoaded { get; private set; }
        internal IEnumerable<D7Term>  Terms    { get; private set; }


        internal async Task<bool> Load(ID7Client client, CancellationToken token)
        {
            if (IsLoaded) return true;
            Terms = await client.Get<List<D7Term>>(URL.Json_TaxoTerms, token);
            if (Terms == null) return false;
            return IsLoaded = true;
        }


        internal D7Term Term(int tid, bool errorIfMissing)
        {
            if (!IsLoaded)     throw Error.BadAct("D7Terms not loaded.");
            if (Terms == null) throw Error.BadAct("D7Terms not loaded or empty.");

            var trm = Terms.FirstOrDefault(x => x.tid == tid);

            if (trm == null && errorIfMissing)
                throw Error.NoMember($"where tid = {tid}");

            return trm;
        }
    }
}
