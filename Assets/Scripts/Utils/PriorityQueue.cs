using System;
using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> Heap { get; set; } = new List<T>();

    public int GetCount() { return Heap.Count; } 

    public void Push(T data)
    {
        Heap.Add(data);

        int now = Heap.Count - 1;
        while (now > 0)
        {
            int next = (now - 1) / 2;
            if (Heap[now].CompareTo(Heap[next]) < 0)
                break;

            (Heap[now], Heap[next]) = (Heap[next], Heap[now]);
            
            now = next;
        }
    }

    public T Pop()
    {
        T ret = Heap[0];

        int lastIndex = Heap.Count - 1;
        Heap[0] = Heap[lastIndex];
        Heap.RemoveAt(lastIndex);
        lastIndex--;

        Heapify(lastIndex);

        return ret;
    }

    private void Heapify(int lastIndex)
    {
        int now = 0;
        while (true)
        {
            int left = 2 * now + 1;
            int right = 2 * now + 2;

            int next = now;
            if (left <= lastIndex && Heap[next].CompareTo(Heap[left]) < 0)
                next = left;
            if (right <= lastIndex && Heap[next].CompareTo(Heap[right]) < 0)
                next = right;

            if (next == now)
                break;

            (Heap[now], Heap[next]) = (Heap[next], Heap[now]);

            now = next;
        }
    }

    public T Peek()
    {
        if (Heap.Count == 0)
            return default;
        return Heap[0];
    }

    public T Find(Func<T, bool> predicate)
    {
        return Heap.FirstOrDefault(predicate);
    }

    public void Delete(Func<T, bool> predicate)
    {
        T value = Find(predicate);
        Heap.Remove(value);

        int lastIndex = Heap.Count - 2;
        Heapify(lastIndex);
    }

    public T FindPop(Func<T, bool> predicate)
    {
        T value = Find(predicate);
        Heap.Remove(value);

        int lastIndex = Heap.Count - 2;
        Heapify(lastIndex);

        return value;
    }
}
