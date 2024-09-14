using System;
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
        public Vector2 nodeLocation;
        public Vector2Int gridLocation;

        private bool _isShowingIndicator = false;
        private float _timeBeforeNormalColor = 0f;
        private Color _normalColor;

        public Node(int x, int y, bool isWalkable)
        {
            this.x = x;
            this.y = y;
            this.isWalkable = isWalkable;
            nodeLocation = new Vector2(x, y);
        }

        private void Start()
        {
            _normalColor = _sprite.color;
        }

        private void Update()
        {
            if (_isShowingIndicator)
            {
                if (_timeBeforeNormalColor > 0)
                {
                    Color highlightColor = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, .2f);
                    _sprite.color = highlightColor;
                    _timeBeforeNormalColor -= Time.deltaTime;
                }
                else
                {
                    _sprite.color = _normalColor;
                    _timeBeforeNormalColor = 0;
                    _isShowingIndicator = false;
                }
            }
        }

        public void ShowHighlightIndicator()
        {
            _timeBeforeNormalColor = .1f;
            _isShowingIndicator = true;
        }

        public void ShowNode(Color color)
        {
            _sprite.color = color;
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