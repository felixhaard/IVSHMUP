using UnityEngine;

public class Player : MonoBehaviour
{

    #region Singleton

    public static Player Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            if (Instance != this)
            {
                Debug.LogWarning("Invalid singleton");
                Destroy(this);
            }
        }
    }

    #endregion

    public static bool GodMode = false;

    //

    public PlayerHealth MyPlayerHealth = null;
    public PlayerController MyPlayerController = null;

    //

    public static Vector3 GetPosition()
    {
        return Instance.transform.position;
    }

    //

    private void OnValidate()
    {
        MyPlayerHealth = GetComponent<PlayerHealth>();
        MyPlayerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResolveOverlap();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ResolveOverlap();
    }

    private void ResolveOverlap()
    {
        if (!GodMode)
            MyPlayerHealth.TakeDamage();
    }

}
