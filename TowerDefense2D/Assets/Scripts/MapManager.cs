﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TowerDefense.PathFinding
{
    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance { get { return _instance; } }


        public GameObject overlayPrefab;
        public GameObject overlayContainer;

        public Dictionary<Vector2Int, Node> map;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else
            {
                _instance = this;
            }
        }

        void Start()
        {
            var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
            map = new Dictionary<Vector2Int, Node>();

            foreach (var tm in tileMaps)
            {
                BoundsInt bounds = tm.cellBounds;

                for (int z = bounds.max.z; z >= bounds.min.z; z--)
                {
                    for (int y = bounds.min.y; y < bounds.max.y; y++)
                    {
                        for (int x = bounds.min.x; x < bounds.max.x; x++)
                        {
                            if (tm.HasTile(new Vector3Int(x, y, z)))
                            {
                                if (!map.ContainsKey(new Vector2Int(x, y)))
                                {
                                    var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                    var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y));
                                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                    overlayTile.gameObject.GetComponent<Node>().gridLocation = new Vector2Int(x, y);
                                    
                                    map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<Node>());
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}