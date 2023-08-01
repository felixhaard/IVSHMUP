using UnityEngine;

[ExecuteInEditMode]
public class ViewPort : MonoBehaviour
{

    #region Singleton

    // to make sure it can be accessed without the editor being in play mode
    private static ViewPort cachedInstance;
    private static ViewPort instance
    {
        get
        {
            if (cachedInstance == null) cachedInstance = GameObject.FindObjectOfType<ViewPort>();
            return cachedInstance;
        }
    }

    #endregion Singleton

    //

    public static float GetSpeed() { return instance.Speed; }

    public static float SweepTimePosition
    {
        get
        {
            float distTraveled = instance.transform.position.y + instance.SweepOffset;
            return distTraveled / instance.Speed;
        }
    }

    public static float SweepPosition
    {
        get 
        {
            return instance.transform.position.y + instance.SweepOffset;
        }
    }

    public static Vector2 CenterPosition
    {
        get
        {
            return instance.transform.position;
        }
    }

    public static Vector2 Size
    {
        get
        {
            return new Vector2(instance.Width, instance.Height);
        }
    }

    public static void ClampRBPosition(Rigidbody2D rb)
    {
        Vector3 viewPortPos = instance.transform.position;
        float halfWidth = instance.Width / 2f;
        float halfHeight = instance.Height / 2f;

        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(rb.position.x, viewPortPos.x - halfWidth, viewPortPos.x + halfWidth),
            Mathf.Clamp(rb.position.y, viewPortPos.y - halfHeight, viewPortPos.y + halfHeight));

        rb.position = clampedPos;
    }

    public static void ClampTransformPosition(Transform t)
    {
        Vector3 viewPortPos = instance.transform.position;
        float halfWidth = instance.Width / 2f;
        float halfHeight = instance.Height / 2f;

        Vector2 clampedPos = new Vector2(
            Mathf.Clamp(t.position.x, viewPortPos.x - halfWidth, viewPortPos.x + halfWidth),
            Mathf.Clamp(t.position.y, viewPortPos.y - halfHeight, viewPortPos.y + halfHeight));

        t.position = new Vector3(clampedPos.x, clampedPos.y, t.position.z);
    }

    public static void ResetPosition()
    {
        instance.transform.position = Vector3.zero;
    }

    public static bool Progress(float dt)
    {
        float newY = instance.transform.position.y + dt * instance.Speed;

        if (newY >= instance.TravelDistance)
        {
            instance.transform.position = new Vector3(0f, instance.TravelDistance, 0f);
            return true; // returns true if reached end of level
        } else
        {
            instance.transform.position = new Vector3(0f, newY, 0f);
            return false;
        }
    }

    //

    public float TravelDistance = 50f;
    public float Speed = 1f;

    [Space]

    [Tooltip("Assigned automatically based on camera position")] public float Width = 12f;
    [Tooltip("Assigned automatically based on camera position")] public float Height = 12f;

    [Space]

    public float BorderPadding = 0.5f;
    public float SweepOffset = 0f;

    //

    private void LateUpdate()
    {
        transform.position = new Vector3(0f, Mathf.Clamp(transform.position.y, 0f, TravelDistance), 0f);

        GameCamera.ClampCameraLocalXYPosition();

        Vector2 frustumIntersectionSize = GameCamera.GetFrustumIntersectionSize();
        Width = frustumIntersectionSize.x - BorderPadding;
        Height = frustumIntersectionSize.y - BorderPadding;
    }

    private void OnDrawGizmos()
    {
        float halfWidth = Width / 2f;
        float halfHeight = Height / 2f;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        Vector3 viewUpLeftCorner = new Vector3(x - halfWidth, y + halfHeight, z);
        Vector3 viewUpRightCorner = new Vector3(x + halfWidth, y + halfHeight, z);
        Vector3 viewDownRightCorner = new Vector3(x + halfWidth, y - halfHeight, z);
        Vector3 viewDownLeftCorner = new Vector3(x - halfWidth, y - halfHeight, z);

        Vector3 levelUpLeftCorner = new Vector3(viewUpLeftCorner.x, TravelDistance + halfHeight, viewUpLeftCorner.z);
        Vector3 levelUpRightCorner = new Vector3(viewUpRightCorner.x, TravelDistance + halfHeight, viewUpRightCorner.z);
        Vector3 levelDownRightCorner = new Vector3(viewDownRightCorner.x, -halfHeight, viewDownRightCorner.z);
        Vector3 levelDownLeftCorner = new Vector3(viewDownLeftCorner.x, -halfHeight, viewDownLeftCorner.z);

        Gizmos.color = new Color(0f, 0.75f, 1f, 0.25f);
        Gizmos.DrawLine(levelUpLeftCorner, levelUpRightCorner);
        Gizmos.DrawLine(levelUpRightCorner, levelDownRightCorner);
        Gizmos.DrawLine(levelDownRightCorner, levelDownLeftCorner);
        Gizmos.DrawLine(levelDownLeftCorner, levelUpLeftCorner);

        Gizmos.color = new Color(0f, 0.85f, 1f, 1f);
        Gizmos.DrawLine(viewUpLeftCorner, viewUpRightCorner);
        Gizmos.DrawLine(viewUpRightCorner, viewDownRightCorner);
        Gizmos.DrawLine(viewDownRightCorner, viewDownLeftCorner);
        Gizmos.DrawLine(viewDownLeftCorner, viewUpLeftCorner);

        Gizmos.color = new Color(0f, 1f, 1f, 1f);
        float sweepHalfWidth = 10f;
        Gizmos.DrawLine(
            new Vector3(-halfWidth - sweepHalfWidth, y + SweepOffset, z),
            new Vector3(halfWidth + sweepHalfWidth, y + SweepOffset, z));

    }

}
