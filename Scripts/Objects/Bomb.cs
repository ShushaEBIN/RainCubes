using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Object
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    private float _startAlpha = 1f;
    private float _targetAlpha = 0f;
    private Color _originalColor;
    private MeshRenderer _renderer;

    public event Action<Bomb> Exploded;

    private void OnEnable()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalColor = new Color(_renderer.material.color.r, _renderer.material.color.g, _renderer.material.color.b, _startAlpha);

        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        float count = UnityEngine.Random.Range(MinCount, MaxCount);
        float elapsedTime = 0f;
        
        while (elapsedTime < count)
        {
            elapsedTime += Time.deltaTime;
            float interpolator = elapsedTime / count;

            float newAlpha = Mathf.Lerp(_originalColor.a, _targetAlpha, interpolator);
            Color newColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, newAlpha);
            _renderer.material.color = newColor;

            yield return null;
        }

        Explode();
        
        Exploded?.Invoke(this);
    }

    private void Explode()
    {     
        foreach (Rigidbody explodableObject in GetExplodableObjects())
        {
            explodableObject.AddExplosionForce(_explosionForce, this.transform.position, _explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects()
    {
        Collider[] hits = Physics.OverlapSphere(this.transform.position, _explosionRadius);

        List<Rigidbody> objects = new List<Rigidbody>();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                objects.Add(hit.attachedRigidbody);
            }
        }

        return objects;
    }
}