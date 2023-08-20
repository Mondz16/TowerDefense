using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class GameManager : MonoBehaviour
    {
        public GridManager gridManager;

        private void Start()
        {
            Vector3Int startCell = new Vector3Int(0, 0, 0);
            Vector3Int targetCell = new Vector3Int(4, 4, 0);

            List<Node> path = AStarPathfinder.FindPath(gridManager, startCell, targetCell);
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
    }
}