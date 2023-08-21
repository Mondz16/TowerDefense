using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class AStarPathfinder
    {
         public  List<Node> FindPath(Node start, Node end)
        {
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            openList.Add(start);

            while (openList.Count > 0)
            {
                Node currentNode = openList.OrderBy(x => x.fCost).First();

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == end)
                    return GetFinishedList(start, end);

                foreach (var tile in GetNeightbourNodes(currentNode))
                {
                    if (!tile.isWalkable || closedList.Contains(tile))
                    {
                        continue;
                    }
                    
                    int tentativeGCost = currentNode.gCost + GetManhattenDistance(currentNode, tile);

                    if (tentativeGCost < tile.gCost || !openList.Contains(tile))
                    {
                        tile.gCost = tentativeGCost;
                        tile.hCost = GetManhattenDistance(tile, end);
                        tile.parent = currentNode;

                        if (!openList.Contains(tile))
                        {
                            openList.Add(tile);
                        }
                    }
                }
            }

            return new List<Node>();
        }

        private  List<Node> GetFinishedList(Node start, Node end)
        {
            List<Node> finishedList = new List<Node>();
            Node currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.parent;
            }

            finishedList.Reverse();

            return finishedList;
        }

        private  int GetManhattenDistance(Node start, Node tile)
        {
            return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
        }

        private List<Node> GetNeightbourNodes(Node currentNode)
        {
            var map = MapManager.Instance.map;

            List<Node> neighbours = new List<Node>();

            //right
            Vector2Int locationToCheck = new Vector2Int(
                currentNode.gridLocation.x + 1,
                currentNode.gridLocation.y
            );

            if (map.ContainsKey(locationToCheck))
            {
                neighbours.Add(map[locationToCheck]);
            }

            //left
            locationToCheck = new Vector2Int(
                currentNode.gridLocation.x - 1,
                currentNode.gridLocation.y
            );

            if (map.ContainsKey(locationToCheck))
            {
                neighbours.Add(map[locationToCheck]);
            }

            //top
            locationToCheck = new Vector2Int(
                currentNode.gridLocation.x,
                currentNode.gridLocation.y + 1
            );

            if (map.ContainsKey(locationToCheck))
            {
                neighbours.Add(map[locationToCheck]);
            }

            //bottom
            locationToCheck = new Vector2Int(
                currentNode.gridLocation.x,
                currentNode.gridLocation.y - 1
            );

            if (map.ContainsKey(locationToCheck))
            {
                neighbours.Add(map[locationToCheck]);
            }

            return neighbours;
        }
    }
}