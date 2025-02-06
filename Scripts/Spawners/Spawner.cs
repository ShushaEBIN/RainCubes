using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;

    private ObjectPool<T> _pool;

    public int Spawned => GetSpawned();
    public int Active => GetActive();

    public event Action<T> ObjectCreated;

    public int Created { get; private set; } = 0;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
        createFunc: () => CreateObject(),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => DeactivateObject(obj));
    }

    protected abstract void ActionOnGet(T obj);

    protected virtual void DeactivateObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected void ReleaseObject(T obj)
    {
        _pool.Release(obj);
    }

    protected void GetObject()
    {
        _pool.Get();

        Created++;
    }

    private T CreateObject()
    {
        T obj = Instantiate(_prefab);

        ObjectCreated?.Invoke(obj);

        return obj;
    }    

    private int GetSpawned()
    {
        return _pool?.CountAll??0;
    }

    private int GetActive()
    {
        return _pool?.CountActive??0;
    }
}