using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.Collection;
using TowerDefense.WaveSystem;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class GameManager : MonoBehaviour
    {
        private MapManager _mapManager => MapManager.Service;
        private PoolManager _poolManager => PoolManager.Service;
        private RunnerDataCollection _runnerDataCollection => RunnerDataCollection.Service;
        
        [SerializeField] private Vector2Int _startCell;
        [SerializeField] private Vector2Int _targetCell;
        
        [Header("Border Cell")]
        [SerializeField] private int _topBorder = 5;
        [SerializeField] private int _bottomBorder = -5;
        [SerializeField] private int _rightBorder = 8;
        [SerializeField] private int _leftBorder = -9;
        
        [Header("Player Input")]
        [SerializeField] private LayerMask _defenderLayerMask;
        [SerializeField] private Transform _defenderHolder;
        [SerializeField] private ShooterController _shooterController;

        [Header("Debug")] [SerializeField] [ReadOnly]
        private int _currentLevel = 1;
        [SerializeField][ReadOnly] private List<Wave> _waveList;
        
        [SerializeField] private List<Node> _path;
        [SerializeField] private List<GameObject> _runnerWaveList;
        private List<Node> _blockers;
        private Node _startNode;
        private Node _targetNode;
        private int _waveCount = 0;
        private AStarPathfinder Pathfinder;

        public static Action OnPlayerTouchFieldEvent { get; set; }
        public static Action<float> OnNextWaveEvent { get; set; }

        private void OnEnable()
        {
            UIManager.OnDefenderDropEvent += OnDefenderDrop;
            UIManager.OnPlayButtonClickedEvent += StartWave;
        }

        private void OnDisable()
        {
            UIManager.OnDefenderDropEvent -= OnDefenderDrop;
            UIManager.OnPlayButtonClickedEvent -= StartWave;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            Pathfinder = new AStarPathfinder();
            _blockers = new List<Node>();
            _mapManager.Initialize();
            CreateWall();

            if (_mapManager.map.TryGetValue(_startCell, out Node startNode) && _mapManager.map.TryGetValue(_targetCell, out Node targetNode))
            {
                _startNode = startNode;
                _targetNode = targetNode;
            }
            
            CreatePath();
            _waveList = WaveCollection.Service.GetWaveByDifficulty(Difficulty.Easy);
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _blockers.Count > 0)
            {
                foreach (Node blocker in _blockers)
                {
                    blocker.isWalkable = true;
                    blocker.HideNode();
                }
                
                _blockers.Clear();
                CreatePath();
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnPlayerTouchFieldEvent?.Invoke();
                RaycastHit2D? hit = GetFocusedOnTileByLayer(_defenderLayerMask);
                if (hit.HasValue)
                {
                    Debug.Log($"#{GetType().Name}# OnPlayerTouchFieldEvent: {hit.Value.transform.name}");
                    if (hit.Value.transform.TryGetComponent(out ShooterController shooter))
                        shooter.ShowRadiusIndicator(true);
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                StartWave();
            }
        }

        private void OnDefenderDrop()
        {
            RaycastHit2D? hit = GetFocusedOnTile();
            if (hit.HasValue)
            {
                Node hitNode = hit.Value.collider.gameObject.GetComponent<Node>();
                if (!_blockers.Exists(x => x == hitNode))
                {
                    hitNode.isWalkable = false;
                    if (FindPath())
                    {
                        hitNode.ShowNode(Color.blue);
                        Instantiate(_shooterController, hitNode.transform.position, Quaternion.identity, _defenderHolder);
                        _blockers.Add(hitNode);
                        CreatePath();
                    }
                    else
                    {
                        hitNode.HideNode();
                        hitNode.isWalkable = true;
                        CreatePath();
                        Debug.LogError("No Path Found!");
                    }
                }
                else
                {
                    Debug.LogError("Can't drop defender on that location!");
                }
            }
        }

        public void StartWave()
        {
            StartCoroutine(StartWaveRoutine(_waveList[_waveCount]));
        }

        private IEnumerator StartWaveRoutine(Wave wave)
        {
            foreach (WaveAttribute attribute in wave.GetWaveAttributeList())
            {
                for (int i = 0; i < attribute.EnemyCount; i++)
                {
                    var runnerData = _runnerDataCollection.GetRunnerDataByID(attribute.RunnerID);
                    
                    var obj = _poolManager.UseObject(runnerData.Prefab, new Vector3(_startCell.x, _startCell.y, 0), Quaternion.identity);
                    _runnerWaveList.Add(obj);
                    
                    var runner = obj.GetComponent<Runner>();
                    runner.OnRunnerDisappear += RemoveRunnerFromWaveList;
                    runner.SetRunnerData(runnerData);
                    CreatePath(runner);
                    
                    yield return new WaitForSeconds(1f - (runner.RunnerData.Speed / 100));
                }
            }

            yield return new WaitUntil(() => _runnerWaveList.Count == 0);
            Debug.Log($"#{GetType().Name}# All Runners -> Reached Target");

            OnNextWaveEvent?.Invoke(wave.TimeInterval);
            yield return new WaitForSeconds(wave.TimeInterval);
            Debug.Log($"#{GetType().Name}# Wave -> Next Wave");
            
            _waveCount++;
            if (_waveList.Count == _waveCount)
            {
                Debug.Log($"#{GetType().Name}# Game Finished!");
            }
            else
            {
                StartWave();
            }
        }

        private void RemoveRunnerFromWaveList(GameObject obj)
        {
            if (_runnerWaveList.Exists(x => x == obj))
            {
                Debug.Log($"#{GetType().Name}# RemoveRunnerFromList -> Removed!");
                _runnerWaveList.Remove(obj);
            }
            else
            {
                Debug.Log($"#{GetType().Name}# RemoveRunnerFromList -> Runner doesn't exists!");
            }
        }

        private void CreatePath(Runner runner = null)
        {
            if (FindPath() && runner)
            {
                runner.transform.position = new Vector3(_startCell.x + .5f, _startCell.y+ .5f, 0);
                _startNode.ShowNode(Color.green);

                foreach (Node node in _path)
                {
                    Debug.Log(
                        $"Node: x:{node.x} | y:{node.y} | fcost:{node.fCost} | hcost:{node.hCost} | gcost:{node.gCost}");
                    if (node == _targetNode)
                        node.ShowNode(Color.red);
                    else
                        node.ShowNode(Color.grey);
                }
                
                runner.AddPath(_path);
            }
        }

        public void CreateWall()
        {
            // right side border cell
            for (int x = 0; x < _rightBorder; x++)
            {
                for (int y = 0; y <= _topBorder; y++)
                {
                    Vector2Int topBorderVector = new Vector2Int(x, _topBorder);
                    Vector2Int rightBorderVector = new Vector2Int(_rightBorder, y);
                    if (_mapManager.map.TryGetValue(topBorderVector, out Node topBorderNode))
                    {
                        topBorderNode.isWalkable = false;
                        _blockers.Add(topBorderNode);
                    }
                    
                    if (_mapManager.map.TryGetValue(rightBorderVector, out Node rightBorderNode) && rightBorderVector != _targetCell)
                    {
                        rightBorderNode.isWalkable = false;
                        _blockers.Add(rightBorderNode);
                    }
                }

                for (int y = 0; y >= _bottomBorder; y--)
                {
                    Vector2Int bottomBorderVector = new Vector2Int(x, _bottomBorder);
                    Vector2Int rightBorderVector = new Vector2Int(_rightBorder, y);
                    if (_mapManager.map.TryGetValue(bottomBorderVector, out Node borderNode))
                    {
                        borderNode.isWalkable = false;
                        _blockers.Add(borderNode);
                    }
                    
                    if (_mapManager.map.TryGetValue(rightBorderVector, out Node rightBorderNode) && rightBorderVector != _targetCell)
                    {
                        rightBorderNode.isWalkable = false;
                        _blockers.Add(rightBorderNode);
                    }
                }
            }
            
            // left side border cell
            for (int x = 0; x > _leftBorder; x--)
            {
                for (int y = 0; y <= _topBorder; y++)
                {
                    Vector2Int topBorderVector = new Vector2Int(x, _topBorder);
                    Vector2Int leftBorderVector = new Vector2Int(_leftBorder, y);
                    if (_mapManager.map.TryGetValue(topBorderVector, out Node topBorderNode))
                    {
                        topBorderNode.isWalkable = false;
                        _blockers.Add(topBorderNode);
                    }
                    
                    if (_mapManager.map.TryGetValue(leftBorderVector, out Node leftBorderNode) && leftBorderVector != _startCell)
                    {
                        leftBorderNode.isWalkable = false;
                        _blockers.Add(leftBorderNode);
                    }
                }

                for (int y = 0; y >= _bottomBorder; y--)
                {
                    Vector2Int bottomBorderVector = new Vector2Int(x, _bottomBorder);
                    Vector2Int leftBorderVector = new Vector2Int(_leftBorder, y);
                    if (_mapManager.map.TryGetValue(bottomBorderVector, out Node borderNode))
                    {
                        borderNode.isWalkable = false;
                        _blockers.Add(borderNode);
                    }
                    
                    if (_mapManager.map.TryGetValue(leftBorderVector, out Node leftBorderNode) && leftBorderVector != _startCell)
                    {
                        leftBorderNode.isWalkable = false;
                        _blockers.Add(leftBorderNode);
                    }
                }
            }
        }

        private bool FindPath()
        {
            if (_path.Count > 0)
            {
                foreach (Node node in _path)
                    node.HideNode();

                _path.Clear();
            }

            _path = Pathfinder.FindPath(_startNode, _targetNode);
            return _path.Count > 0 && _path != null;
        }

        private static RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        private RaycastHit2D? GetFocusedOnTileByLayer(LayerMask layer)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hits = Physics2D.Raycast(mousePos2D, Vector2.zero, 100f , layer);

            if (hits.transform != null)
            {
                return hits;
            }

            return null;
        }
    }
}