using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrH.Tools.CollectionShims;
using ErrH.Tools.Drupal7Models;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Drupal7Client
{
    public abstract class D7NodesRepoBase<TNodeDto, TClass> : ListRepoBase<TClass>
    {
        protected ID7Client _client;

        private event EventHandler AfterPreload;

        //private string _resource;
        //private List<TNodeDto> _dtos;

        //public event EventHandler<UserEventArg> LoggedIn;




        public D7NodesRepoBase(ID7Client d7Client)
        {
            _client = d7Client;
        }


        public override bool Load(params object[] args)
        {
            AfterPreload += async (s, e) 
                => { await DoActualLoad(args); };

            _list = new List<TClass>();
            Fire_Loaded();

            AfterPreload?.Invoke(this, EventArgs.Empty);
            return true;
        }


        private async Task<bool> DoActualLoad(object[] args)
        {
            var rsrc = args[0].ToString().Slash(args[1]);

            Debug_n("Loading repository data from source...", rsrc);

            if (!_client.IsLoggedIn) return Error_n(
                "Currently disconnected from data source.",
                    "Call Connect() before Load().");

            var dtos = await _client.Get<List<TNodeDto>>(rsrc, null,
                        "Successfully loaded repository data ({0} fetched).",
                            x => "{0:record}".f(x.Count));

            _list = dtos.Select(x => FromDto(x)).ToList();
            Fire_Loaded();
            return true;
        }


        protected override List<TClass> LoadList(object[] args)
        {
            throw Error.BadAct("In an implementation of ISlowRepository<T>, method LoadList() should not be called.");
        }


        protected abstract TClass FromDto(TNodeDto dto);
    }
}
