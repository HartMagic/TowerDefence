using System;

namespace Core
{
    public interface ICanSelect
    {
        event Action<ICanSelect> Selected;
        
        void Select();
        void Unselect();
    }
}