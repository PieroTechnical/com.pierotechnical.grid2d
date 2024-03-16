using System.Collections.Generic;
using System.Linq;

public class SimplePriorityQueue<T>
{
    private List<(float, T)> elements = new List<(float, T)>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add((priority, item));
        elements = elements.OrderBy(e => e.Item1).ToList();
    }

    public T Dequeue()
    {
        var item = elements[0].Item2;
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Any(e => e.Item2.Equals(item));
    }
}
