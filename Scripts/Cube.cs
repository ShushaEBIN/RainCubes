using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private float _minCount = 2.0f;
    [SerializeField] private float _maxCount = 5.0f;

    private bool _isTouched = false;

    public event Action<Cube> Counted;

    public void Reset()
    {
        _isTouched = false;

        GetComponent<Renderer>().material.color = UnityEngine.Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isTouched == false)
        {
            if (collision.gameObject.TryGetComponent<Plane>(out Plane component))
            {
                _isTouched = true;

                GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
                
                StartCoroutine();
            }
        }
    }

    private void StartCoroutine()
    {
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_minCount, _maxCount + 1));
        
        Counted?.Invoke(this);
    }    
}