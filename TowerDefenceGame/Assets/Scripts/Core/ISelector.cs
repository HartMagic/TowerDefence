using System.Collections.Generic;

namespace Core
{
    public interface ISelector
    {
        ICanSelect Select(IEnumerable<ICanSelect> selectableItems);
    }
}

