using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class AStarPathfinder
    {
        public static List<Node> FindPath(GridManager gridManager, Vector3Int startCell, Vector3Int targetCell)
        {
            var startNode = gridManager.GetNode(startCell);
            var targetNode = gridManager.GetNode(targetCell);

            var openSet = new List<Node>();
            var closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || 
                        (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode) return RetracePath(startNode, targetNode);

                foreach (var neighbor in GetNeighbors(gridManager, currentNode))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor)) continue;
                    var newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        
                        if(!openSet.Contains(neighbor))openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }

        private static List<Node> RetracePath(Node startNode, Node endNode)
        {
            var path = new List<Node>();
            var currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            return path;
        }

        private static int GetDistance(Node nodeA, Node nodeB)
        {
            var distX = Mathf.Abs(nodeA.x - nodeB.x);
            var distY = Mathf.Abs(nodeA.y - nodeB.y);
            return distX + distY;
        }

        private static List<Node> GetNeighbors(GridManager gridManager, Node node)
        {
            List<Node> neighbors = new List<Node>();
            Vector2Int[] directions = {
                Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
            };

            foreach (Vector2Int dir in directions)
            {
                Vector3Int neighborCell = new Vector3Int(node.x + dir.x, node.y + dir.y, 0);
                Node neighborNode = gridManager.GetNode(neighborCell);

                if (neighborNode != null && neighborNode.isWalkable)
                {
                    neighbors.Add(neighborNode);
                }
            }

            return neighbors;
        }
    }
}