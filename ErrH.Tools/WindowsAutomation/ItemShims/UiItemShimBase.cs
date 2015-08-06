using System;
using ErrH.Tools.ErrorConstructors;
using ErrH.Tools.Extensions;

namespace ErrH.Tools.WindowsAutomation.ItemShims
{
    public abstract class UiItemShimBase
    {
        public virtual event EventHandler Focus = delegate { };

        public string Id { get; set; }
        public string LocalType { get; set; }
        public string Name { get; set; }
        public string AccessKey { get; set; }
        public string Text { get; set; }
        public string PropertyChanges { get; set; }

        public string UsableId
        {
            get
            {
                if (!this.Id.IsBlank()) return this.Id;
                if (this.Name.IsBlank()) Throw.NoMember("Usable ID");
                return this.Name;
            }
        }


        public abstract bool CopyFrom(object sourceElement);


        public override string ToString()
        {
            var typ = this.LocalType ?? this.GetType().Name;
            var id = this.Id ?? "‹null›";
            var nme = this.Name ?? "‹null›";

            return "{0} [id: {1}] {2}".f(
                typ.PadLeft(12), id.PadLeft(2), nme);
        }
    }
}
