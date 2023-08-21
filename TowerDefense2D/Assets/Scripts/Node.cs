using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class Node: MonoBehaviour
    {
        public int x;
        public int y;
        public bool isWalkable;
        public Node parent;
        public int gCost = 0;
        public int hCost = 0;
        public SpriteRenderer _sprite;

        public int fCost => gCost + hCost;
        public Vector2Int gridLocation;

        public Node(int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
        }

        public void ShowNode(Color color)
        {
            _sprite.color = color;
            Debug.Log($"#{GetType().Name}# ShowNode Color -> {color}");
        }

        public void HideNode()
        {
            _sprite.color = new Color(1, 1, 1, 0);
        }

        public override string ToString()
        {
             return $"Node: x:{x} | y:{y} | fcost:{fCost} | hcost:{hCost} | gcost:{gCost} | isWalkable: {isWalkable}";
        }
    }
    
}