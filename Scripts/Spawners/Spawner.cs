using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;

    private ObjectPool<T> _pool;

    public int Created { get; private set; } = 0;
    public int Spawned => GetSpawned();
    public int Active => GetActive();

    public event Action<T> ObjectCreated;

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

    protected T CreateObject()
    {
        T obj = Instantiate(_prefab);

        ObjectCreated?.Invoke(obj);

        return obj;
    }    

    private int GetSpawned()
    {
        int minValue = 0;

        if (_pool == null)
            return minValue;

        return _pool.CountAll;
    }

    private int GetActive()
    {
        int minValue = 0;

        if ( _pool == null )
            return minValue;

        return _pool.CountActive;
    }
}