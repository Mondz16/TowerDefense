using UnityEngine;

namespace Manic.Services
{
    public sealed class UnityComponentServiceProvider<T> : IServiceProvider where T : Object
    {
        private T _instance;
        public object GetService()
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<T>();
            return _instance;
        }
    }
}