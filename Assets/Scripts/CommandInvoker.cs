using System;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType { Spawn, Move }

public class CommandInvoker
{
    public const int MaxHistorySize = 10;

    private readonly GameObject _spawnPrefab;
    private readonly Transform _character;

    private readonly LinkedList<ICommand> _history = new LinkedList<ICommand>();
    private readonly Queue<CommandRequest> _rmbQueue = new Queue<CommandRequest>();

    private struct CommandRequest
    {
        public CommandType Type;
        public Vector2 Position;
    }

    public CommandInvoker(GameObject spawnPrefab, Transform character)
    {
        _spawnPrefab = spawnPrefab;
        _character = character;
    }

    public void Execute(CommandType type, Vector2 position)
    {
        var command = CreateCommand(type);
        command.Invoke(position);
        PushToHistory(command);
    }

    public void Enqueue(CommandType type, Vector2 position)
    {
        _rmbQueue.Enqueue(new CommandRequest { Type = type, Position = position });
    }

    public void ExecuteQueue()
    {
        while (_rmbQueue.Count > 0)
        {
            var request = _rmbQueue.Dequeue();
            var command = CreateCommand(request.Type);
            command.Invoke(request.Position);
            PushToHistory(command);
        }
    }

    public void Undo()
    {
        if (_history.Count == 0) return;

        var last = _history.Last.Value;
        _history.RemoveLast();
        last.Undo();
    }

    private ICommand CreateCommand(CommandType type)
    {
        switch (type)
        {
            case CommandType.Spawn: return new SpawnCommand(_spawnPrefab);
            case CommandType.Move: return new MoveCommand(_character);
            default: throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    private void PushToHistory(ICommand command)
    {
        _history.AddLast(command);
        if (_history.Count > MaxHistorySize)
            _history.RemoveFirst();
    }
}