using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Poly.Collections
{
    public interface IFastArray : IDisposable, IEnumerable
    {
        int Length { get; }
        int Capacity { get; }
        bool IsCreated { get; }
        //object this[int index] { get; set; }
    }
    //public interface IFastArray<T> : IFastArray where T : struct
    //{
    //    new T this[int index] { get; set; }
    //}
    //public struct FastArray<T> : IFastArray<T> where T : struct// : IEnumerable<T>
    public struct FastArray<T> : IFastArray, IEnumerable<T>// where T : struct// : IEnumerable<T>
    {
        private T[] items;
        private int length;

        //public T[] Array => items;
        public int Capacity => items.Length;
        public int Length
        {
            get => length;
            set
            {
                if (value > items.Length)
                {
                    //var capacity = ArrayUtil.NextPowerOf2(value);
                    //System.Array.Resize(ref items, capacity);
                    var array = ArrayPool<T>.Shared.Rent(value);
                    System.Array.Copy(items, array, length);
                    var oldArray = items;
                    items = array;
                    //var oldArray = Interlocked.Exchange(ref items, array);
                    ArrayPool<T>.Shared.Return(oldArray);
                }
                length = value;
            }
        }
        public bool IsCreated => items != null;

        public T this[int index]
        {
            get => index >= items.Length ? default : items[index];
            set
            {
                //if (index >= items.Length)
                //{
                //    var capacity = ArrayUtil.NextPowerOf2(index + 1);
                //    Array.Resize(ref items, capacity);
                //}
                if (index >= length)
                    Length = index + 1;
                items[index] = value;
            }
        }

        public FastArray(int capacity)
        {
            items = ArrayPool<T>.Shared.Rent(capacity);
            this.length = 0;
        }
        public FastArray(FastArray<T> array)
        {
            items = ArrayPool<T>.Shared.Rent(array.items.Length);
            length = array.length;
            System.Array.Copy(array.items, items, length);
        }
        public void Dispose()
        {
            if (items == null) return;
            ArrayPool<T>.Shared.Return(items);
            items = null;
            length = 0;
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private void EnsureCapacity(int capacity)
        //{
        //    var length = items.Length;
        //    while (length < capacity) length <<= 1;
        //    System.Array.Resize(ref items, length);
        //}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T ElementAt(int index) => ref items[index];
        public Enumerator GetEnumerator() => new Enumerator(items, 0, length);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //public static implicit operator T[](FastArray<T> fastArray) => fastArray.items;
        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] array;
            private readonly int length;
            private readonly int offset;
            private volatile int index;
            public T Current => array[index];
            object IEnumerator.Current => array[index];

            public Enumerator(T[] array, int offset, int length)
            {
                this.array = array;
                this.length = length;
                this.offset = offset;
                index = offset - 1;
            }
            public void Dispose() { }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++index < length;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset() => index = offset - 1;
        }
    }
}
