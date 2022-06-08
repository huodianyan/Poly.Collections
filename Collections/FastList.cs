using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Poly.Collections
{
    public interface IFastList : IDisposable, IEnumerable
    {
        int Capacity { get; }
        bool IsCreated { get; }
        bool IsEmpty { get; }
        int Length { get; set; }
        //object this[int index] { get; set; }

        void Add();
        //void Add(object item);
        void Clear();
        //bool Contains(object item);
        //int IndexOf(object item);
        //bool Remove(object item);
        //object RemoveAt(int index);
        void RemoveAtSwapBack(int index);
    }
    public interface IFastList<T> : IFastList, IEnumerable<T> where T : struct
    {
        T this[int index] { get; set; }

        void Add(T item);
        //bool Contains(T item);
        //int IndexOf(T item);
        //bool Remove(T item);
        void RemoveAt(int index);
        ref T ElementAt(int index);
    }
    public struct FastList<T> : IFastList<T> where T : struct// : IEnumerable<T>
    {
        private T[] items;
        private int length;

        public int Capacity => items.Length;
        public bool IsCreated => items != null;
        public bool IsEmpty => length == 0;
        //public int Count => length;
        public int Length
        {
            get => length;
            set
            {
                if (value > items.Length)
                {
                    EnsureCapacity(value);
                    //var array = ArrayPool<T>.Shared.Rent(value);
                    //System.Array.Copy(items, array, length);
                    //var oldArray = items;
                    //items = array;
                    //ArrayPool<T>.Shared.Return(oldArray);
                }
                length = value;
            }
        }
        public T this[int index]
        {
            get => index >= items.Length ? default : items[index];
            set
            {
                //if (index > items.Length) EnsureCapacity(index);
                if (index >= length)
                    Length = index + 1;
                items[index] = value;
            }
        }

        public FastList(int capacity)
        {
            items = ArrayPool<T>.Shared.Rent(capacity);
            //items = new T[capacity];
            length = 0;
        }
        public void Dispose()
        {
            if (items == null) return;
            ArrayPool<T>.Shared.Return(items);
            items = null;
            length = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add() => Add(default);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (length == items.Length) EnsureCapacity(length + 1);//Array.Resize(ref items, length << 1);
            items[length++] = item;
            //Console.WriteLine($"FastList.Add: {count}, {item.ToString()}");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddRange(FastList<T> range)
        {
            var newCount = range.length + length;
            if (newCount > items.Length) EnsureCapacity(newCount);
            for (int i = 0, j = range.length; i < j; i++)
                items[length++] = range[i];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (length > 0) Array.Clear(items, 0, length);
            length = 0;
        }
        public ref T ElementAt(int index) => ref items[index];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int capacity)
        {
            if (capacity <= items.Length) return;
            var array = ArrayPool<T>.Shared.Rent(capacity);
            System.Array.Copy(items, array, length);
            var oldArray = items;
            items = array;
            ArrayPool<T>.Shared.Return(oldArray);
            //var length = items.Length;
            //while (length < capacity) length <<= 1;
            //Array.Resize(ref items, length);
        }
        //public Enumerator GetEnumerator() => new Enumerator(this);
        public FastArray<T>.Enumerator GetEnumerator() => new FastArray<T>.Enumerator(items, 0, length);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public bool Contains(T item)
        //{
        //    for (var index = length - 1; index >= 0; --index)
        //        if (item.Equals(items[index]))
        //            return true;
        //    return false;
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public int IndexOf(T item) => Array.IndexOf(items, item);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            if (length == items.Length) EnsureCapacity(length + 1);
            for (int i = length - 1; i >= index; i--)
                items[i + 1] = items[i];
            items[index] = item;
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public bool Remove(T item)
        //{
        //    var index = IndexOf(item);
        //    if (index < 0) return false;
        //    RemoveAt(index);
        //    return true;
        //}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            items[index] = default;
            if (--length > index)
                for (int i = index; i < length; i++)
                    items[i] = items[i + 1];
            //var result = items[index];
            //if (--length > index)
            //    items[index] = items[length];
            //items[length] = default;
            //return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAtSwapBack(int index)
        {
            if (--length > index)
                items[index] = items[length];
            //Console.WriteLine($"FastList.RemoveAtSwap: {count}, {index}");
            items[length] = default;
        }

        //public struct Enumerator : IDisposable //IEnumerator<T>
        //{
        //    private readonly FastList<T> bag;
        //    private volatile int index;
        //    public Enumerator(FastList<T> bag)
        //    {
        //        this.bag = bag;
        //        index = -1;
        //    }
        //    public T Current => bag[index];
        //    public bool MoveNext() => ++index < bag.Count;
        //    public void Dispose() { }
        //}
    }
}
