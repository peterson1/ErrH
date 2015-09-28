using System.Collections.Generic;
using System.Linq;
using ErrH.Tools.FileSystemShims;

namespace ErrH.BinUpdater.Core
{
    public static class LocalFilesFilter
    {
        public static List<FileShim> Declutter(this List<FileShim> files)
        {
            var withXmls = files.Select(x => x.LessExtension)
                                .GroupBy(x => x)
                                .Where(x => x.Count() > 1)
                                .Select(x => x.First() + ".xml");

            return files.Where(x => !x.Hidden)
                        .Where(x => x.Extension != ".pdb")
                        .Where(x => !x.Name.Contains(".vshost."))
                        .Where(x => !withXmls.Contains(x.Name))
                        .ToList();
        }
    }
}
