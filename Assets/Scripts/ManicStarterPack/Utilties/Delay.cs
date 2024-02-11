using System;
using System.Collections;
using UnityEngine;

public static class Delay
{
    public static void RunLater(MonoBehaviour monoBehaviour, float delay, Action OnDelayCompleted)
    {
        monoBehaviour.StartCoroutine(DelayRoutine(delay, OnDelayCompleted));
        
        IEnumerator DelayRoutine(float delay, Action onDelayComplete)
        {
            yield return new WaitForSeconds(delay);
            onDelayComplete?.Invoke();
        }
    }
}
