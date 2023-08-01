using UnityEngine;

public class Enemy : MonoBehaviour
{

    public PoolableObject MyPoolableObjectReference = null;

    [Space]

    public Rigidbody2D MyRigidbody2D = null;
    public EnemyMovement MyEnemyMovement = null;
    public EnemyHealth MyEnemyHealth = null;

    [Space]

    public MeshFilter MyMeshFilter = null;
    public MeshRenderer MyMeshRenderer = null;

    //

    private void OnValidate()
    {
        MyPoolableObjectReference = GetComponent<PoolableObject>();

        MyRigidbody2D = GetComponent<Rigidbody2D>();
        MyEnemyMovement = GetComponent<EnemyMovement>();
        MyEnemyHealth = GetComponent<EnemyHealth>();

        MyMeshRenderer = GetComponentInChildren<MeshRenderer>();
        MyMeshFilter = MyMeshRenderer.GetComponent<MeshFilter>();
    }

    //

    public void Remove()
    {
        Pool.Release(MyPoolableObjectReference);
    }

}
