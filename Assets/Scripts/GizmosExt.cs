using UnityEngine;

public class GizmosExt : MonoBehaviour
{

    public static void DrawRectangle(Vector3 center, Vector2 halfDims)
    {
        Vector3 upLeft = new Vector3(center.x - halfDims.x, center.y + halfDims.y, center.z);
        Vector3 upRight = new Vector3(center.x + halfDims.x, center.y + halfDims.y, center.z);
        Vector3 downRight= new Vector3(center.x + halfDims.x, center.y - halfDims.y, center.z);
        Vector3 downLeft= new Vector3(center.x - halfDims.x, center.y - halfDims.y, center.z);

        Gizmos.DrawLine(upLeft, upRight);
        Gizmos.DrawLine(upRight, downRight);
        Gizmos.DrawLine(downRight, downLeft);
        Gizmos.DrawLine(downLeft, upLeft);
    }
    public static void DrawRectangle(Vector2 center, Vector2 halfDims)
    {
        DrawRectangle(new Vector3(center.x, center.y, 0f), halfDims);
    }

    public static void DrawSquare(Vector3 center, float halfDim)
    {
        DrawRectangle(center, new Vector2(halfDim, halfDim));
    }
    public static void DrawSquare(Vector2 center, float halfDim)
    {
        DrawSquare(new Vector3(center.x, center.y, 0f), halfDim);
    }

}
