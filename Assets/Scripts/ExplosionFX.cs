using UnityEngine;

public class ExplosionFX : MonoBehaviour
{

    [SerializeField]
    private float _Lifetime = 1f;

    //

    private PoolableObject _poolableObjectReference;

    private ParticleSystem _ps;
    private AudioSource _audioSource;

    private float _remainingLifetime;

    //

    private void Awake()
    {
        _poolableObjectReference = GetComponent<PoolableObject>();
        _poolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;

        _ps = GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnPoolableGet()
    {
        _remainingLifetime = _Lifetime;

        if (_ps != null)
        {
            _ps.Clear();
            _ps.Play();
        }
        if (_audioSource != null) _audioSource.Play();
    }

    private void Update()
    {
        _remainingLifetime -= Time.deltaTime;
        if (_remainingLifetime <= 0f) Pool.Release(_poolableObjectReference);
    }

}
