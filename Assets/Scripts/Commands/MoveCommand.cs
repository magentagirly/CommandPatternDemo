using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly Transform _target;
    private Vector3 _previousPosition;

    public MoveCommand(Transform target)
    {
        _target = target;
    }

    public void Invoke(Vector2 position)
    {
        _previousPosition = _target.position;
        _target.position = new Vector3(position.x, position.y, _previousPosition.z);
    }

    public void Undo()
    {
        _target.position = _previousPosition;
    }
}