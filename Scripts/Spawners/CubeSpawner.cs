using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] protected float _repeatRate = 0.4f;
    [SerializeField] private Vector3 _spawnPositionMin = new Vector3(-24, 35, -24);
    [SerializeField] private Vector3 _spawnPositionMax = new Vector3(24, 40, 24);
    [SerializeField] private BombSpawner _bombSpawner;

    private void Start()
    {
        StartCoroutine(Count());
    }

    private IEnumerator Count() 
    {
        while (enabled)
        {
            GetObject();

            yield return new WaitForSeconds(_repeatRate);
        }
    }

    protected override void ActionOnGet(Cube cube)
    {       
        cube.transform.position = GetRandomPosition();
        cube.Reset();
        cube.gameObject.SetActive(true);

        cube.Counted += ReleaseObject;
        cube.Counted += _bombSpawner.CreateBomb;
    }

    protected override void DeactivateObject(Cube cube)
    {
        base.DeactivateObject(cube);

        cube.Counted -= ReleaseObject;
        cube.Counted -= _bombSpawner.CreateBomb;
    }

    private Vector3 GetRandomPosition()
    {
        float positionX = Random.Range(_spawnPositionMin.x, _spawnPositionMax.x);
        float positionY = Random.Range(_spawnPositionMin.y, _spawnPositionMax.y);
        float positionZ = Random.Range(_spawnPositionMin.z, _spawnPositionMax.z);

        return new Vector3(positionX, positionY, positionZ);
    }
}