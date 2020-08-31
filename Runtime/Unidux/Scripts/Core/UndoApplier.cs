using System;
using System.Collections.Generic;
using System.Linq;
using Unidux;

public class UndoApplier<TState> where TState : StateBase
{
    private List<UniduxAction<TState>> _story;
    private List<UniduxAction<TState>> _undoList;
    private int _storyLength;
    private Func<UniduxAction<TState>, bool> _contextMatched;

    public UndoApplier(int storyLength, Func<UniduxAction<TState>, bool> contextMatched)
    {
        _story = new List<UniduxAction<TState>>(storyLength);
        _undoList = new List<UniduxAction<TState>>();
        _storyLength = storyLength;
        _contextMatched = contextMatched;
    }

    public void Write(UniduxAction<TState> action)
    {
        if (_story.Count >= _storyLength)
        {
            _story.RemoveAt(0);
        }

        if (!_story.Contains(action))
        {
            _story.Add(action);
            _undoList.ForEach(u => u.Storage.Dispose());
            _undoList.Clear();
        }
    }

    public void Clear()
    {
        _story.Clear();
    }

    public void Undo(TState state)
    {
        var targetAction = _story.LastOrDefault(action => _contextMatched(action));
        if (targetAction != null)
        {
            if (targetAction.Undo != null)
            {
                targetAction.Undo(state, targetAction.Storage);
                _story.Remove(targetAction);
                _undoList.Add(targetAction);
            }
        }
    }

    public void Redo(TState state)
    {
        var targetAction = _undoList.LastOrDefault(action => _contextMatched(action));
        if (targetAction != null)
        {
            if (targetAction.Redo != null)
            {
                targetAction.Redo(state, targetAction.Storage);
                _story.Add(targetAction);
                _undoList.Remove(targetAction);
            }
        }
    }
}