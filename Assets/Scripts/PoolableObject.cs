using UnityEngine;
using UnityEngine.Events;

public class PoolableObject : MonoBehaviour
{

    [HideInInspector]
    public PoolableObject Prefab;

    public event UnityAction Ev_OnPoolableGet;
    public event UnityAction Ev_OnPoolableRelease;

    // this should ONLY be called from Pool.cs Get fn
    public void TriggerOnGetCallbacks()
    {
        if (Ev_OnPoolableGet != null)
        {
            Ev_OnPoolableGet();
        }
    }

    // this should ONLY be called from Pool.cs Release fn
    public void TriggerOnReleaseCallbacks()
    {
        if (Ev_OnPoolableRelease != null)
        {
            Ev_OnPoolableRelease();
        }
    }

}
