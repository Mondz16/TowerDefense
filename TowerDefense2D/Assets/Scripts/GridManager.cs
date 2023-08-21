using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.PathFinding;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap walkableTilemap;
    public Transform Indicator;

    // private void Start()
    // {
    //     BoundsInt bounds = walkableTilemap.cellBounds;
    //     Debug.Log($"Bounds: Max: {bounds.max.x}, {bounds.max.y}, {bounds.max.z} Min: {bounds.min.x}, {bounds.min.y}, {bounds.min.z}");
    //     
    //     for (int y = bounds.min.y; y < bounds.max.y; y++)
    //     {
    //         for (int x = bounds.min.x; x < bounds.max.x; x++)
    //         {
    //             var tileLocation = new Vector3Int(x, y, 0);
    //             Debug.Log($"Tilelocation: {tileLocation.x} , {tileLocation.y} , {tileLocation.z}");
    //             if (walkableTilemap.HasTile(tileLocation))
    //             {
    //                 var cellWorldPosition = walkableTilemap.GetCellCenterWorld(tileLocation);
    //                 Instantiate(Indicator, cellWorldPosition, Quaternion.identity);
    //             }
    //         }
    //     }
    // }

    public Node GetNode(Vector2Int cellPosition)
    {
        Vector3Int cellPos = new Vector3Int(cellPosition.x, cellPosition.y, 0);
        bool isWalkable = walkableTilemap.GetTile(cellPos) != null;
        return new Node(cellPosition.x, cellPosition.y, isWalkable);
    }
}
