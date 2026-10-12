using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private Transform character;

    private CommandInvoker _invoker;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _invoker = new CommandInvoker(spawnPrefab, character);
    }

    private void Update()
    {
        Vector2 mouseWorld = _camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
            _invoker.Execute(CommandType.Move, mouseWorld);

        if (Input.GetMouseButtonDown(1))
            _invoker.Enqueue(CommandType.Spawn, mouseWorld);

        if (Input.GetKeyDown(KeyCode.Return))
            _invoker.ExecuteQueue();

        if (Input.GetMouseButtonDown(2))
            _invoker.Undo();
    }
}