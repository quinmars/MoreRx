using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx.Internal
{
    internal interface IEvaluatingComparer<T> : IComparer<int>
    {
        void Evaluate(T value, int pos);
        void Move(int pos1, int pos2);
        void Remove(int pos);
    }
}
