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
        
        
        [SerializeField] private List<Node> _path;
        [SerializeField] private List<GameObject> _runnerWaveList;
        private List<Node> _blockers;
        private Node _startNode;
        private Node _targetNode;
        private int _waveCount = 0;
        private AStarPathfinder Pathfinder;

        private void Start()
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
        }

        private void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Node hitNode = hit.Value.collider.gameObject.GetComponent<Node>();
                    if (!_blockers.Exists(x => x == hitNode))
                    {
                        hitNode.isWalkable = false;
                        if (FindPath())
                        {
                            hitNode.ShowNode(Color.blue);
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
                }
            }

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

            if (Input.GetKeyDown(KeyCode.S))
            {
                StartWave();
            }
        }

        private void StartWave()
        {
            List<Wave> waveList = WaveCollection.Service.GetWaveByLevel(WaveLevel.Level1);
            StartCoroutine(StartWaveRoutine(waveList[_waveCount]));
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

            yield return new WaitForSeconds(wave.TimeInterval);
            Debug.Log($"#{GetType().Name}# Wave -> Next Wave");
            
            _waveCount++;
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
                        topBorderNode.ShowNode(Color.yellow);
                    }
                    
                    if (_mapManager.map.TryGetValue(rightBorderVector, out Node rightBorderNode) && rightBorderVector != _targetCell)
                    {
                        rightBorderNode.isWalkable = false;
                        rightBorderNode.ShowNode(Color.yellow);
                    }
                }

                for (int y = 0; y >= _bottomBorder; y--)
                {
                    Vector2Int bottomBorderVector = new Vector2Int(x, _bottomBorder);
                    Vector2Int rightBorderVector = new Vector2Int(_rightBorder, y);
                    if (_mapManager.map.TryGetValue(bottomBorderVector, out Node borderNode))
                    {
                        borderNode.isWalkable = false;
                        borderNode.ShowNode(Color.yellow);
                    }
                    
                    if (_mapManager.map.TryGetValue(rightBorderVector, out Node rightBorderNode) && rightBorderVector != _targetCell)
                    {
                        rightBorderNode.isWalkable = false;
                        rightBorderNode.ShowNode(Color.yellow);
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
                        topBorderNode.ShowNode(Color.yellow);
                    }
                    
                    if (_mapManager.map.TryGetValue(leftBorderVector, out Node leftBorderNode) && leftBorderVector != _startCell)
                    {
                        leftBorderNode.isWalkable = false;
                        leftBorderNode.ShowNode(Color.yellow);
                    }
                }

                for (int y = 0; y >= _bottomBorder; y--)
                {
                    Vector2Int bottomBorderVector = new Vector2Int(x, _bottomBorder);
                    Vector2Int leftBorderVector = new Vector2Int(_leftBorder, y);
                    if (_mapManager.map.TryGetValue(bottomBorderVector, out Node borderNode))
                    {
                        borderNode.isWalkable = false;
                        borderNode.ShowNode(Color.yellow);
                    }
                    
                    if (_mapManager.map.TryGetValue(leftBorderVector, out Node leftBorderNode) && leftBorderVector != _startCell)
                    {
                        leftBorderNode.isWalkable = false;
                        leftBorderNode.ShowNode(Color.yellow);
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
    }
}