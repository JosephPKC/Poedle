using Poedle.Server.Shared.State;
using Poedle.Server.UniqueByAttr.Results;

namespace Poedle.Server.UniqueByAttr.State
{
    internal class UniqueByAttrState : BaseState
    {
        public LinkedList<UniqueByAttrResult> Results { get; set; } = [];
       
    }
}
