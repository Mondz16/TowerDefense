using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.PathFinding;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bulletSprite;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private LayerMask _runnerLayerMask;
    [SerializeField] private GameObject _bulletHitEffectPrefab;
    private int _damage = 0;

    private PoolManager _poolManager => PoolManager.Service;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _bulletSpeed * Time.deltaTime);
    }

    public void SetBulletVisual(Sprite bullet)
    {
        _bulletSprite.sprite = bullet;
    }

    public void SetBulletDamage(int damage)
    {
        _damage = damage;
        
        Delay.RunLater(this, 1f, () =>
        {
            if(gameObject.activeInHierarchy)
                _poolManager.ReturnObject(gameObject);
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"#{GetType().Name}# Runner Hit!");
        if (other.TryGetComponent(out Runner runner))
        {
            runner.TakeDamage(_damage);
            _poolManager.UseObject(_bulletHitEffectPrefab, transform.position, _bulletHitEffectPrefab.transform.rotation);
            _poolManager.ReturnObject(gameObject);
        }
    }
}