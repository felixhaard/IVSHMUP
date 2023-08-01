using UnityEngine;
using System.Collections.Generic;

public class Pool : MonoBehaviour
{

    public static Pool GetInst()
    {
        return instance;
    }

    #region Singleton

    private static Pool instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            if (instance != this)
            {
                Debug.LogWarning("Invalid singleton");
                Destroy(this);
            }
        }
    }

    #endregion Singleton

    //

    private Dictionary<PoolableObject, Stack<PoolableObject>> _prefabToPool = new Dictionary<PoolableObject, Stack<PoolableObject>>();

    //

    private void InitializePool(PoolableObject prefab, int defaultCapacity)
    {
        _prefabToPool.Add(prefab, new Stack<PoolableObject>());
        for (int i = 0; i < defaultCapacity; i ++)
        {
            AddPoolableInstance(prefab, false, Vector3.zero, Vector3.right);
        }
    }

    private PoolableObject AddPoolableInstance(PoolableObject prefab, bool useRightAway, Vector3 position, Vector3 facingDirection)
    {
        PoolableObject inst = Instantiate(prefab, position, Quaternion.identity);
        inst.transform.right = facingDirection;
        inst.Prefab = prefab;

        if (!useRightAway)
        {
            inst.gameObject.SetActive(false);
            _prefabToPool[prefab].Push(inst);
        }

        return inst;
    }

    //
    
    public static PoolableObject Get(PoolableObject prefab, Vector3 position, Vector3 facingDir)
    {
        if (!instance._prefabToPool.ContainsKey(prefab))
        {
            instance.InitializePool(prefab, 1);

            Debug.Log("Auto-initializing pool for prefab " + prefab.gameObject.name);
        }

        if (instance._prefabToPool[prefab].Count == 0)
        {
            PoolableObject inst = instance.AddPoolableInstance(prefab, true, position, facingDir);
            inst.TriggerOnGetCallbacks();
            return inst;
        } else {
            PoolableObject inst = instance._prefabToPool[prefab].Pop();
#if UNITY_EDITOR
            if (inst.gameObject.activeSelf)
            {
                Debug.LogError("Trying to grab an already active instance from a pool of prefab " + prefab.name);
            }
#endif
            inst.transform.position = position;
            inst.transform.right = facingDir;
            inst.gameObject.SetActive(true);
            inst.TriggerOnGetCallbacks();
            return inst;
        }
    }
    public static PoolableObject Get(PoolableObject prefab)
    { return Get(prefab, Vector3.zero, Vector3.right); }
    public static PoolableObject Get(PoolableObject prefab, Vector3 pos)
    { return Get(prefab, pos, Vector3.right); }

    public static void Release(PoolableObject instToReturnToPool)
    {
        instToReturnToPool.TriggerOnReleaseCallbacks();
        Debug.Assert(instToReturnToPool.gameObject.activeSelf);
        instToReturnToPool.gameObject.SetActive(false);
        instance._prefabToPool[instToReturnToPool.Prefab].Push(instToReturnToPool);
    }

}
