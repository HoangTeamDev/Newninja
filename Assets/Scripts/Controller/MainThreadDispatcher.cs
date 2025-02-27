using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly ConcurrentQueue<Action> _executionQueue = new ConcurrentQueue<Action>();

    protected  void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                if (_executionQueue.TryDequeue(out Action action))
                {
                    action.Invoke();
                }


            }
        }

    }


    public static void Enqueue(Action action)
    {
        if (action == null) return;
        _executionQueue.Enqueue(action);
    }
    public static void ClearMessage()
    {
        _executionQueue.Clear();
    }
}