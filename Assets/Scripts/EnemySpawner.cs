using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField]
    private PoolableObject _EnemyPrefab = default;
    [SerializeField]
    private Transform _ToPosition = default;

    [Space]

    public float ActivationPositionOffset = 5f;
    public float ActiveDuration = 5f;

    //

    public float GetActivationYPos()
    {
        return transform.position.y + ActivationPositionOffset;
    }

    public float GetStartTime()
    {
        return GetActivationYPos() / ViewPort.GetSpeed();
    }

    public void SpawnEnemy()
    {
        Pool.Get(_EnemyPrefab).GetComponent<Enemy>().MyEnemyMovement
            .BeginMovement(transform.position, _ToPosition.position, GetStartTime(), ActiveDuration);
    }

    //

    private void OnDrawGizmos()
    {
        Vector3 fromPos = transform.position;
        Vector3 toPos = _ToPosition.position;

        float sweepPos = ViewPort.SweepPosition;
        float sweepTimePos = ViewPort.SweepTimePosition;

        float activationYPos = GetActivationYPos();
        float startTime = GetStartTime();

        const float fromPosSphereRadius = 0.3f;

        if (sweepPos < activationYPos)
        {
            // Activation sweep has not reached yet

            Gizmos.color = Color.red;
            DrawShipMeshGizmo(fromPos);
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawLine(fromPos, toPos);
            Gizmos.DrawSphere(fromPos, fromPosSphereRadius);

            Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
            DrawActivationPointGizmo(fromPos, activationYPos);
        } else if (sweepTimePos <= startTime + ActiveDuration)
        {
            // Movement ongoing

            float previewAge = sweepTimePos - startTime;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(fromPos, toPos);
            Gizmos.DrawSphere(fromPos, fromPosSphereRadius);
            DrawShipMeshGizmo(Vector3.Lerp(fromPos, toPos, previewAge / ActiveDuration));

            Gizmos.color = new Color(1f, 1f, 0f, 0.6f);
            DrawActivationPointGizmo(fromPos, activationYPos);
        } else
        {
            // Movement finished

            Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
            Gizmos.DrawLine(fromPos, toPos);
            Gizmos.DrawSphere(fromPos, fromPosSphereRadius);
            DrawShipMeshGizmo(toPos);
        }
    }

    private void DrawActivationPointGizmo(Vector3 fromPos, float activationYPos)
    {
        Vector3 anglePt = new Vector3(fromPos.x, activationYPos, fromPos.z);
        Gizmos.DrawLine(fromPos, anglePt);
        Vector3 timelinePos = new Vector3(fromPos.x >= 0f ? 10f : -10f, activationYPos, fromPos.z);
        Gizmos.DrawLine(anglePt, timelinePos);
        Gizmos.DrawSphere(timelinePos, 0.25f);
    }

    private void DrawShipMeshGizmo(Vector3 pos)
    {
        MeshFilter filter = _EnemyPrefab.GetComponent<Enemy>().MyMeshFilter;
        Gizmos.DrawWireMesh(filter.sharedMesh, pos, Quaternion.identity, filter.transform.lossyScale);
    }

}
