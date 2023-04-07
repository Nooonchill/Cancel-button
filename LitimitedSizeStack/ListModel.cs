using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;

namespace LimitedSizeStack;

public class OperationElementInfo<TItem>
{
    public TItem ElementValue { get; }
    public int ElementIndex { get; }
    public bool OperationWasAdding { get; }
    public bool OperationWasMovingUp { get; }

    public OperationElementInfo(TItem value, int index, bool operationWasAdding, bool operationWasMovingUp)
    {
        ElementValue = value;
        ElementIndex = index;
        OperationWasAdding = operationWasAdding;
        OperationWasMovingUp = operationWasMovingUp;
    }
}
public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    private int undoLimit { get; }
    private LimitedSizeStack<OperationElementInfo<TItem>> limitedSizeStackOperationInfo { get; }

    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        this.undoLimit = undoLimit;
        limitedSizeStackOperationInfo = new LimitedSizeStack<OperationElementInfo<TItem>>(undoLimit);
    }

    public void MoveUpItem(int index)
    {
        var movingUpOperationInfo = new OperationElementInfo<TItem>(Items.ElementAt(index),
            index,
            false,
            true);

        limitedSizeStackOperationInfo.Push(movingUpOperationInfo);
        Items.RemoveAt(index);
        Items.Insert(index - 1 < 0 ? 0 : index - 1, movingUpOperationInfo.ElementValue);
    }
    
    public void AddItem(TItem item)
    {
        var addingOperationInfo = new OperationElementInfo<TItem>(item,
            Items.IndexOf(item),
            true,
            false);

        limitedSizeStackOperationInfo.Push(addingOperationInfo);
        Items.Add(item);
    }

    public void RemoveItem(int index)
    {
        var removingOperationInfo = new OperationElementInfo<TItem>(Items.ElementAt(index),
            index,
            false,
            false);
		
        limitedSizeStackOperationInfo.Push(removingOperationInfo);
        Items.RemoveAt(index);
    } 

    public bool CanUndo() => limitedSizeStackOperationInfo.Count > 0;

    public void Undo()
    {
        if (!CanUndo()) return;
        var undoOperationInfo = limitedSizeStackOperationInfo.Pop();
        if (undoOperationInfo.OperationWasMovingUp)
        {
            Items.Remove(undoOperationInfo.ElementValue);
            Items.Insert(undoOperationInfo.ElementIndex, undoOperationInfo.ElementValue);
        }
        else if (undoOperationInfo.OperationWasAdding)
            Items.Remove(undoOperationInfo.ElementValue);
        else
            Items.Insert(undoOperationInfo.ElementIndex, undoOperationInfo.ElementValue);
    }
}