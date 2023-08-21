using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class GameManager : MonoBehaviour
    {
        public GridManager gridManager;
        public Vector2Int _startCell;
        private AStarPathfinder Pathfinder;
        [SerializeField]
        private List<Node> _path;
        private MapManager _map;
        private Node _targetNode;

        private void Start()
        {
            Pathfinder = new AStarPathfinder();
            _map = MapManager.Instance;
        }

        private void LateUpdate()
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                if (hit.Value.collider.gameObject.GetComponent<Node>() != _targetNode)
                {
                    _targetNode = hit.Value.collider.gameObject.GetComponent<Node>();
                    if (MapManager.Instance.map.TryGetValue(_startCell, out Node startNode))
                    {
                        _path = Pathfinder.FindPath(startNode, _targetNode);
                        Debug.Log($"Start has value: {startNode.gridLocation} " +
                                  $"--> tile: {_targetNode.gridLocation}  ---- " +
                                  $"PathCount: {_path.Count}");

                        if (_path != null)
                        {
                            foreach (KeyValuePair<Vector2Int,Node> keyValuePair in _map.map)
                                keyValuePair.Value.HideNode();
                            
                            startNode.ShowNode(Color.green);
                            
                            foreach (Node node in _path)
                            {
                                Debug.Log($"Node: x:{node.x} | y:{node.y} | fcost:{node.fCost} | hcost:{node.hCost} | gcost:{node.gCost}");
                                if (node == _targetNode)
                                    node.ShowNode(Color.red);
                                else
                                    node.ShowNode(Color.black);
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && _path.Count > 0)
            {
                foreach (Node node in _path)
                    node.HideNode();
                
                _path.Clear();
            }
        }
        
        private void FindPath()
        {
            Node startCell = new Node(0, 0, true);
            Node targetCell = new Node(4, 4, true);
            List<Node> path = Pathfinder.FindPath(startCell, targetCell);
            Debug.Log($"Start Pathfinding! Path Count: {path.Count}");

            if (path != null)
            {
                foreach (Node node in path)
                {
                    Debug.Log($"Node: x:{node.x} | y:{node.y} | fcost:{node.fCost} | hcost:{node.hCost} | gcost:{node.gCost}");
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(new Vector3(node.x + 0.5f, node.y + 0.5f, 0), Vector3.one);
                }
            }
            else
            {
                // No path found
            }
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