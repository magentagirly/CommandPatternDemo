using UnityEngine;

public class SpawnCommand : ICommand
{
    private readonly GameObject _prefab;
    private GameObject _spawned;

    public SpawnCommand(GameObject prefab)
    {
        _prefab = prefab;
    }

    public void Invoke(Vector2 position)
    {
        _spawned = Object.Instantiate(_prefab, position, Quaternion.identity);
    }

    public void Undo()
    {
        if (_spawned != null)
            Object.Destroy(_spawned);
    }
}