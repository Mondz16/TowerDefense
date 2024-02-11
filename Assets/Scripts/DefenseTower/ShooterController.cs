using System;
using System.Collections;
using TowerDefense.Collection;
using TowerDefense.PathFinding;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField]private DefenderID _defenderID;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private SpriteRenderer _radiusIndicatorRenderer;
    [SerializeField] private LayerMask _runnerLayerMask;
    [SerializeField][ReadOnly] private DefenderData _defenderData;

    private float _timeBeforeShoot = 0;
    
    private DefenderDataCollection _defenderDataCollection => DefenderDataCollection.Service;
    private PoolManager _poolManager => PoolManager.Service;

    private void OnEnable()
    {
        GameManager.OnPlayerTouchFieldEvent += () => ShowRadiusIndicator(false);
    }

    private void OnDisable()
    {
        GameManager.OnPlayerTouchFieldEvent -= () => ShowRadiusIndicator(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // sets the data of the defender
        _defenderData = _defenderDataCollection.GetDefenderDataByID(_defenderID);
        
        _radiusIndicatorRenderer.transform.localScale = transform.localScale + ((Vector3.one * _defenderData.Range) + (Vector3.one * .7f));
    }

    // Update is called once per frame
    void Update()
    {
        if (OnDefenderAim())
        {
            StartShooting();
        }
    }

    public void ShowRadiusIndicator(bool show)
    {
        if(!_radiusIndicatorRenderer.gameObject.activeInHierarchy)
            _radiusIndicatorRenderer.gameObject.SetActive(show);
        
        if(_radiusIndicatorRenderer.gameObject.activeInHierarchy)
            _radiusIndicatorRenderer.gameObject.SetActive(show);
    }

    private bool OnDefenderAim()
    {
        if (Physics2D.OverlapCircle(transform.position , _defenderData.Range, _runnerLayerMask))
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, _defenderData.Range, _runnerLayerMask);

            if (collider.Length > 0)
            {
                Vector2 difference = collider[0].transform.position - transform.position;
                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ - 90);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 1f);
                return true;
            }
        }

        return false;
    }

    private void StartShooting()
    {
        if (_timeBeforeShoot <= 0)
        {
            SpawnBullet();
            _timeBeforeShoot = _defenderData.AttackSpeed;
        }
        else
            _timeBeforeShoot -= Time.deltaTime;
    }

    private void SpawnBullet()
    {
        GameObject obj = _poolManager.UseObject(_defenderData.BulletPrefab.gameObject, _firePosition.position, transform.rotation);
        obj.GetComponent<BulletController>().SetBulletDamage(_defenderData.Damage);
    }

    private void OnDrawGizmos()
    {
        
    }
}
