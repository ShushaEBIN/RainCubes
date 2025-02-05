using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    private Vector3 _cubePosition;

    public void CreateBomb(Cube cube)
    {
        _cubePosition = cube.transform.position;

        GetObject();
    }

    protected override void ActionOnGet(Bomb bomb)
    {
        bomb.transform.position = _cubePosition;
        bomb.gameObject.SetActive(true);

        bomb.Exploded += ReleaseObject;
    }

    protected override void DeactivateObject(Bomb bomb)
    {
        base.DeactivateObject(bomb);

        bomb.Exploded -= ReleaseObject;
    } 
}