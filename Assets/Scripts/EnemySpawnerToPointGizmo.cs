using UnityEngine;

// This is handled from a separate script instead of from EnemySpawner so that clicking the endpoint sphere selects the endpoint transform directly

public class EnemySpawnerToPointGizmo : MonoBehaviour
{

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

}
