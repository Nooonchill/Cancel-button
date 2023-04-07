using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    private LinkedList<T> limitedSizeStack = new();
    private int limitOfStackSize;

    public LimitedSizeStack(int undoLimit)
    {
        limitOfStackSize = undoLimit;
    }

    public void Push(T item)
    {
        if (limitOfStackSize <= 0) return;
        
        if (limitedSizeStack.Count == limitOfStackSize)
            limitedSizeStack.RemoveFirst();
        
        limitedSizeStack.AddLast(item);
    }

    public T Pop()
    {
        var lastStackItemBeforePop = limitedSizeStack.Last.Value;
        limitedSizeStack.RemoveLast();
        return lastStackItemBeforePop;
    }

    public int Count => limitedSizeStack.Count;
}