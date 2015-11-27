using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.Drupal7Models.Entities;

namespace ErrH.Drupal7Client.TaxonomyTerms
{
    internal class TaxoTermsLoader
    {
        internal bool                 IsLoaded { get; private set; }
        internal IEnumerable<D7Term>  Terms    { get; private set; }


        internal async Task<bool> Load(ID7Client client, CancellationToken token)
        {
            if (IsLoaded) return true;
            Terms = await client.Get<List<D7Term>>(URL.Api_EntityTaxonomyTerm, token);
            return IsLoaded = true;
        }


    }
}
