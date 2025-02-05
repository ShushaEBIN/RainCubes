using UnityEngine;
using UnityEngine.UI;

public class Counter<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string Created = "Created: ";
    private const string Spawned = "Spawned: ";
    private const string Active = "Active: ";

    [SerializeField] protected Spawner<T> _spawner;
    [SerializeField] protected Text _created;
    [SerializeField] protected Text _spawned;
    [SerializeField] protected Text _active;
    
    protected int _createdObjects = 0;
    protected int _spawnedObjects = 0;
    protected int _activeObjects = 0;

    private void OnEnable()
    {
        _spawner.ObjectCreated += UpdateUI;
    }

    private void OnDisable()
    {
        _spawner.ObjectCreated -= UpdateUI;
    }

    protected void UpdateCreated()
    {
        _createdObjects = _spawner.Created;
        _created.text = Created + _createdObjects;
    }

    protected void UpdateSpawned()
    {
        _spawnedObjects = _spawner.Spawned;
        _spawned.text = Spawned + _spawnedObjects;
    }

    protected void UpdateActive()
    {
        _activeObjects = _spawner.Active;
        _active.text = Active + _activeObjects;
    }

    protected void UpdateUI(T obj)
    {
        UpdateCreated();
        UpdateSpawned();
        UpdateActive();
    }
}