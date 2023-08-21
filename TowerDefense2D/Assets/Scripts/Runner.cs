using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.PathFinding
{
    public class Runner : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        
        [SerializeField]
        private List<Node> _path = new List<Node>();
        
        private void LateUpdate()
        {
            if (_path.Count > 0)
            {
                MoveAlongPath();
            }
        }
        
        private void MoveAlongPath()
        {
            var step = _speed * Time.deltaTime;
            
            transform.position = Vector2.MoveTowards(transform.position, _path[0].transform.position, step);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            if(Vector2.Distance(transform.position, _path[0].transform.position) < 0.00001f)
                _path.RemoveAt(0);
        }

        public void AddPath(List<Node> path)
        {
            _path = new List<Node>(path);
        }
    }
}