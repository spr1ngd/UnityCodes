
using System.Collections.Generic;

namespace SpringGUI
{
    public interface IInject
    {
        void Inject<T>(IList<T> data);
        void Inject<T>(IList<T>[] datas );
    }
}