using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ManicStarterPack.Utilties
{
    public static class Helper
    {
        public static Camera Camera
        {
            get
            {
                if (_camera == null)
                    _camera = Camera.main;
                return _camera;
            }
        }

        private static Camera _camera;
        
        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _results;
        
        /// <summary>
        /// Check if mouse pointer is over ui
        /// </summary>
        /// <returns></returns>
        public static bool isOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
            return _results.Count > 0;
        }

        /// <summary>
        /// Gets the world position of a rect transform
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Vector3 GetWorldPositionOfRectTransform(RectTransform rect)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, rect.position, Camera, out var result);
            return result;
        }

        /// <summary>
        /// This method delete all the children of a Transform
        /// </summary>
        /// <param name="transform"></param>
        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform) Object.Destroy(child.gameObject);
        }
        
        /// <summary>
        /// Returns random enum values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
    }
}