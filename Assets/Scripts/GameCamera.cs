using UnityEngine;

public class GameCamera : MonoBehaviour
{

    #region Singleton

    // to make sure it can be accessed without the editor being in play mode
    private static GameCamera cachedInstance;
    private static GameCamera instance
    {
        get
        {
            if (cachedInstance == null) cachedInstance = Camera.main.GetComponent<GameCamera>();
            return cachedInstance;
        }
    }

    #endregion Singleton

    //

    [SerializeField]
    private Camera _Camera = null;

    //

    private Plane[] _planes;
    
    //

    public static bool TestRendererInsideFrustum(Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(instance._planes, renderer.bounds);
    }

    public static Vector2 GetFrustumIntersectionSize()
    {
        float distanceFromCamera = Mathf.Abs(instance.transform.position.z);

        float frustumHeight = 2f * distanceFromCamera * Mathf.Tan(instance._Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * instance._Camera.aspect;

        return new Vector2(frustumWidth, frustumHeight);
    }

    public static void ClampCameraLocalXYPosition()
    {
        instance.transform.localPosition = new Vector3(0f, 0f, instance.transform.position.z);
    }

    //

    private void OnValidate()
    {
        _Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        _planes = GeometryUtility.CalculateFrustumPlanes(_Camera);
    }

}
