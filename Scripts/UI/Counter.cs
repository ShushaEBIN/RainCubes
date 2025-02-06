using UnityEngine;
using UnityEngine.UI;

public class Counter<T> : MonoBehaviour where T : MonoBehaviour
{
    private const string Created = "Created: ";
    private const string Spawned = "Spawned: ";
    private const string Active = "Active: ";

    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private Text _created;
    [SerializeField] private Text _spawned;
    [SerializeField] private Text _active;
    
    private int _createdObjects = 0;
    private int _spawnedObjects = 0;
    private int _activeObjects = 0;

    private void OnEnable()
    {
        _spawner.ObjectCreated += UpdateUI;
    }

    private void OnDisable()
    {
        _spawner.ObjectCreated -= UpdateUI;
    }

    private void UpdateCreated()
    {
        _createdObjects = _spawner.Created;
        _created.text = Created + _createdObjects;
    }

    private void UpdateSpawned()
    {
        _spawnedObjects = _spawner.Spawned;
        _spawned.text = Spawned + _spawnedObjects;
    }

    private void UpdateActive()
    {
        _activeObjects = _spawner.Active;
        _active.text = Active + _activeObjects;
    }

    private void UpdateUI(T obj)
    {
        UpdateCreated();
        UpdateSpawned();
        UpdateActive();
    }
}