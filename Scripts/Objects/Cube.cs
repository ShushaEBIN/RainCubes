using System;
using System.Collections;
using UnityEngine;

public class Cube : Object
{
    private bool _isTouched = false;
    private Renderer _renderer;

    public event Action<Cube> Counted;   

    public void Reset()
    {
        _isTouched = false;        
        _renderer.material.color = UnityEngine.Color.white;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.gameObject.TryGetComponent<Plane>(out Plane component))
            {
                _isTouched = true;
                _renderer.material.color = UnityEngine.Random.ColorHSV();
                
                StartCoroutine(Count());
            }
        }
    }

    private IEnumerator Count()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(MinCount, MaxCount));

        Counted?.Invoke(this);
    }
}