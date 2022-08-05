//
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx.Internal
{
    internal abstract class EvaluatingComparer<TSource, TSelect> : IEvaluatingComparer<TSource>
    {
        private readonly Func<TSource, TSelect> _selector;
        protected readonly TSelect[] _buffer;
        protected readonly IEvaluatingComparer<TSource>? _child;

        public EvaluatingComparer(Func<TSource, TSelect> selector, int size, IEvaluatingComparer<TSource>? child)
        {
            _selector = selector;
            _buffer = new TSelect[size];
            _child = child;
        }

        public void Evaluate(TSource value, int pos)
        {
            _buffer[pos] = _selector(value);
            _child?.Evaluate(value, pos);
        }

        public void Move(int pos1, int pos2)
        {
            _buffer[pos2] = _buffer[pos1];
            Remove(pos1);
            _child?.Move(pos1, pos2);
        }

        public void Remove(int pos)
        {
            _buffer[pos] = default!;
            _child?.Remove(pos);
        }

        public abstract int Compare(int x, int y);
    }
}
