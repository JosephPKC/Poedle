using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeWikiData.Utils
{
    internal class PoeDbCache
    {
        // Static Data that will likely never change.
        public Dictionary<string, string> UniqueItemDropSources { get; private set; } = [];
        public Dictionary<string, string> UniqueItemDropSourcesR { get; private set; } = [];
        public Dictionary<ushort, string> UniqueItemDropTypes { get; private set; } = [];
        public Dictionary<ushort, string> UniqueItemItemAspects { get; private set; } = [];
        public Dictionary<string, string>? UniqueItemItemClasses { get; set; } = null;
        public Dictionary<string, string>? UniqueItemItemClassesR { get; set; } = null;



    }
}
