using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class GameManager : MonoBehaviour
    {
        public Vector2Int _startCell;
        public Vector2Int _targetCell;
        private AStarPathfinder Pathfinder;
        [SerializeField]
        private Runner _runner;
        
        [SerializeField] private List<Node> _path;
        private List<Node> _blockers;
        private MapManager _map;
        private Node _startNode;
        private Node _targetNode;
        private Runner _localRunner;

        private void Start()
        {
            Pathfinder = new AStarPathfinder();
            _map = MapManager.Instance;
            _blockers = new List<Node>();

            if (MapManager.Instance.map.TryGetValue(_startCell, out Node startNode) && MapManager.Instance.map.TryGetValue(_targetCell, out Node targetNode))
            {
                _startNode = startNode;
                _targetNode = targetNode;
                _localRunner = Instantiate(_runner, new Vector3(_startCell.x, _startCell.y, 0), Quaternion.identity);
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
        }

        private void CreatePath()
        {
            if (FindPath())
            {
                _localRunner.transform.position = new Vector3(_startCell.x, _startCell.y, 0);
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
                
                _localRunner.AddPath(_path);
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