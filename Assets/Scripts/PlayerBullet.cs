using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    [SerializeField]
    private float _Damage = 2f;

    [SerializeField]
    private float _Lifetime = 1.25f;

    [SerializeField, Tooltip("To allow for trail renderers etc to play out before the instance is recycled")]
    private float _PostHitRemovalDelay = 0.2f;

    [Space]

    [SerializeField]
    private PoolableObject _OnHitFXPrefab = default;

    //

    private PoolableObject _poolableObjectReference;
    private Rigidbody2D _rb;
    private TrailRenderer _trailRenderer;
    private Collider2D _collider;

    private float _remainingLifetime;
    private bool _hitSomething; // used to prevent hitting multiple enemies at once

    private float _remainingPostHitRemovalDelay;

    //

    private void Awake()
    {
        _poolableObjectReference = GetComponent<PoolableObject>();
        _rb = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _collider = GetComponent<Collider2D>();

        _poolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;
    }

    private void OnPoolableGet()
    {
        _trailRenderer.Clear();
        _collider.enabled = true;

        _remainingLifetime = _Lifetime;
        _hitSomething = false;
    }

    public void Launch(Vector2 velocity)
    {
        _rb.velocity = velocity;
    }

    private void LateUpdate()
    {
        if (_hitSomething)
        {
            _remainingPostHitRemovalDelay -= Time.deltaTime;
            if (_remainingPostHitRemovalDelay <= 0f) Pool.Release(_poolableObjectReference);
        } else
        {
            _remainingLifetime -= Time.deltaTime;
            if (_remainingLifetime <= 0f) Pool.Release(_poolableObjectReference);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hitSomething)
        {
            _hitSomething = true;
            _remainingPostHitRemovalDelay = _PostHitRemovalDelay;

            collision.GetComponent<Enemy>().MyEnemyHealth.TakeDamage(_Damage);

            _collider.enabled = false;
            _rb.velocity = Vector2.zero;

            Pool.Get(_OnHitFXPrefab, transform.position, Vector3.up);
        }
    }

}
