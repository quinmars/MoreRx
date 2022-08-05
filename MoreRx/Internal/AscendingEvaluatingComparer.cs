using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx.Internal
{
    internal class AscendingEvaluatingComparer<TSource, TSelect> : EvaluatingComparer<TSource, TSelect>
    {
        private readonly IComparer<TSelect> _comparer;

        public AscendingEvaluatingComparer(Func<TSource, TSelect> selector, IComparer<TSelect> comparer, int size, IEvaluatingComparer<TSource>? child)
            : base(selector, size, child)
        {
            _comparer = comparer;
        }

        public override int Compare(int pos1, int pos2)
        {
            var v = _comparer.Compare(_buffer[pos1], _buffer[pos2]);

            if (v == 0 && _child != null)
            {
                return _child.Compare(pos1, pos2);
            }
            else
            {
                return v;
            }
        }
    }
}
