﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MoreRx
{
    internal class Heap<T>
    {
        private struct IndexedItem
        {
            public int _index;
            public T _value;
        }

        readonly List<IndexedItem> _list;
        readonly IComparer<T> _comparer;
        int _count;
        int _nextIndex = 1;

        public Heap(IComparer<T> comparer, int capacity)
        {
            _list = new List<IndexedItem>(capacity);
            _comparer = comparer;
        }

        public void Push(T value)
        {
            var item = new IndexedItem { _index = _nextIndex, _value = value };
            if (_count == _list.Count)
            {
                _list.Add(item);
            }
            else
            {
                _list[_count] = item;
            }

            _nextIndex++;
            _count++;

            Decrease(_count - 1);
        }

        public int Count => _count;

        public T Pop()
        {
            var v = _list[0];
            _list[0] = _list[_count - 1];
            _list[_count - 1] = default;
            _count--;

            if (_count > 0)
            {
                Heapify(0);
            }

            return v._value;
        }

        public T Peak()
        {
            return _list[0]._value;
        }

        void Heapify(int i)
        {
            while (true)
            {
                var min = i;
                if (Left(i) < _count && IsLesser(Left(i), min))
                {
                    min = Left(i);
                }

                if (Right(i) < _count && IsLesser(Right(i), min))
                {
                    min = Right(i);
                }

                if (min == i)
                {
                    break;
                }

                Swap(i, min);
                i = min;
            }
        }

        void Decrease(int i)
        {
            while (i > 0 && IsLesser(i, Parent(i)))
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        static int Left(int i) => 2 * i + 1;
        static int Right(int i) => 2 * i + 2;
        static int Parent(int i) => (i - 1) / 2;

        private bool IsLesser(int a, int b)
        {
            var v = _comparer.Compare(_list[a]._value, _list[b]._value);
            if (v == 0)
            {
                return _list[a]._index < _list[b]._index;
            }
            else
            {
                return v < 0;
            }
        }

        private void Swap(int a, int b)
        {
            (_list[b], _list[a]) = (_list[a], _list[b]);
        }
    }
}
