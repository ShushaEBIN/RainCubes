using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Vector3 _spawnPositionMin = new Vector3(-24, 35, -24);
    [SerializeField] private Vector3 _spawnPositionMax = new Vector3(24, 40, 24);
    [SerializeField] private float _repeatRate = 0.4f;
    [SerializeField] private int _poolCapacity = 50;
    [SerializeField] private int _poolMaxCapacity = 50;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => cube.gameObject.SetActive(false),
        actionOnDestroy: (cube) => Delete(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxCapacity);
    }

    private void Start()
    {
        StartCoroutine(GetCube());
    }

    private IEnumerator GetCube()
    {
        bool isWork = true;

        while (isWork)
        {
            _pool.Get();

            yield return new WaitForSeconds(_repeatRate);
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.Counted += SendToPool;

        cube.transform.position = GetRandomPosition();
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.gameObject.SetActive(true);
    }

    private Vector3 GetRandomPosition()
    {
        float positionX = Random.Range(_spawnPositionMin.x, _spawnPositionMax.x);
        float positionY = Random.Range(_spawnPositionMin.y, _spawnPositionMax.y);
        float positionZ = Random.Range(_spawnPositionMin.z, _spawnPositionMax.z);

        return new Vector3(positionX, positionY, positionZ);
    }

    private void SendToPool(Cube cube)
    {
        cube.Counted -= SendToPool;

        _pool.Release(cube);
    }

    private void Delete(Cube cube)
    {
        cube.Counted -= SendToPool;

        Destroy(cube);
    }
}
