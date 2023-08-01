using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    [SerializeField]
    private float _Lifetime = 5f;

    //

    private PoolableObject _poolableObjectReference;

    private MaterialPropertyBlock _propertyBlock;
    private Rigidbody2D _rb;

    private float _remainingLifetime;

    //

    private void Awake()
    {
        _poolableObjectReference = GetComponent<PoolableObject>();
        _poolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;

        _propertyBlock = new MaterialPropertyBlock();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnPoolableGet()
    {
        _propertyBlock.SetFloat("_StartTime", Time.time);

        _remainingLifetime = _Lifetime;
    }

    public void Launch(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }

    private void Update()
    {
        _remainingLifetime -= Time.deltaTime;
        if (_remainingLifetime <= 0f)
        {
            Pool.Release(_poolableObjectReference);
        }
    }

}
