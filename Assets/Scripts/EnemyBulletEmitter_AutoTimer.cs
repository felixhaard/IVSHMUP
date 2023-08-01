using UnityEngine;

public class EnemyBulletEmitter_AutoTimer : EnemyBulletEmitter
{

    [SerializeField]
    private Enemy _Enemy = default;

    [Space]

    [SerializeField]
    private float _InitialWait = 3f;

    [SerializeField]
    private float _Frequency = 1.5f;

    [SerializeField]
    private int _Emits = 4;

    //

    private float _timeUntilNextEmit;
    private int _remainingEmits;

    //

    private void Awake()
    {
        _Enemy.MyPoolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;
    }

    private void OnPoolableGet()
    {
        _timeUntilNextEmit = _InitialWait;
        _remainingEmits = _Emits;
    }

    private void Update()
    {
        if (_remainingEmits > 0 && GameCamera.TestRendererInsideFrustum(_Enemy.MyMeshRenderer))
        {
            _timeUntilNextEmit -= Time.deltaTime;
            if (_timeUntilNextEmit <= 0f)
            {
                _timeUntilNextEmit += _Frequency;

                Emit();

                _remainingEmits--;
            }
        }
    }

}
