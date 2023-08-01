using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    [Header("Burst")]

    [SerializeField]
    private PoolableObject _BurstBulletPrefab = default;

    [SerializeField]
    private Transform _BurstFirePointsParent = default;

    [Space]

    [SerializeField]
    private int _BurstVolleyCount = 4;
    [SerializeField]
    private float _BurstVolleyDelay = 0.1f;

    [Space]

    [SerializeField]
    private float _BurstBulletSpeed = 30f;

    [Header("Focus")]

    [SerializeField]
    private GameObject _FocusBeamCylinder = default;
    [SerializeField]
    private ParticleSystem _FocusPS = default;

    [Space]

    [SerializeField]
    private float _FocusInputHoldTimeRequired = 1f;

    [Space]

    [SerializeField]
    private float _FocusBeamTimeUntilMaxScale = 0.25f;

    [SerializeField]
    private Vector2 _FocusBeamSineWaveScaleMinMaxMultiplier = new Vector2(0.9f, 1.1f);
    [SerializeField]
    private float _FocusBeamSineWaveScaleTimeMultiplier = 25f;

    [Space]

    [SerializeField]
    private LayerMask _BeamDamageLayerMask = default;

    [SerializeField]
    private float _FocusBeamDamagePerSecond = 2.5f;

    [SerializeField]
    private float _FocusBeamEffectiveWidth = 0.5f;

    [Header("Audio")]

    [SerializeField]
    private AudioSource _Audio_OnFireBurstVolley = default;

    [SerializeField]
    private AudioSource _Audio_FocusBeam = default;


    //

    private Coroutine _burstCoroutine = null;

    private bool _bufferedPressInput = false;
    private bool _inputHeld = false;
    private float _inputHeldTime = 0f;

    private Transform[] _burstFirePoints;
    private ParticleSystem[] _burstFirePointParticleSystems;

    private bool _focusEngaged = false;
    private float _focusEngagedTime = 0f;

    private Vector2 _focusBeamBaseXZscale;
    private Transform _focusBeamEndPoint;

    private RaycastHit2D[] _beamOverlaps = new RaycastHit2D[999];

    //

    public event UnityAction Ev_OnFocusStart;
    public event UnityAction Ev_OnFocusStop;

    //

    private void Awake()
    {
        Debug.Assert(_BurstFirePointsParent.childCount > 0);

        _burstFirePoints = new Transform[_BurstFirePointsParent.childCount];
        _burstFirePointParticleSystems = new ParticleSystem[_burstFirePoints.Length];
        for (int i = 0; i < _burstFirePoints.Length; i ++)
        {
            _burstFirePoints[i] = _BurstFirePointsParent.GetChild(i);
            _burstFirePointParticleSystems[i] = _burstFirePoints[i].GetChild(0).GetComponent<ParticleSystem>();
        }

        _focusBeamEndPoint = _FocusBeamCylinder.transform.GetChild(0);

        _focusBeamBaseXZscale = new Vector2(_FocusBeamCylinder.transform.localScale.x, _FocusBeamCylinder.transform.localScale.z);
        _FocusBeamCylinder.SetActive(false);
    }

    public void OnPress()
    {
        if (_burstCoroutine == null)
        {
            _burstCoroutine = StartCoroutine(BurstSequence());
        } else
        {
            _bufferedPressInput = true;
        }

        _inputHeld = true;
    }

    public void OnRelease()
    {
        _inputHeld = false;
        _inputHeldTime = 0f;

        if (_focusEngaged)
        {
            StopFocus();
        }
    }

    private void Update()
    {
        if (_inputHeld)
        {
            _inputHeldTime += Time.deltaTime;

            if (!_focusEngaged)
            {
                if (_inputHeldTime >= _FocusInputHoldTimeRequired)
                {
                    StartFocus();
                }
            } else
            {
                _focusEngagedTime += Time.deltaTime;

                float sinMult = Mathf.Lerp(_FocusBeamSineWaveScaleMinMaxMultiplier.x, _FocusBeamSineWaveScaleMinMaxMultiplier.y,
                    (Mathf.Sin(Time.time * _FocusBeamSineWaveScaleTimeMultiplier) + 1f) / 2f);

                float beamXZScaleCoefficient = Mathf.Clamp(_focusEngagedTime / _FocusBeamTimeUntilMaxScale, 0f, 1f);
                Vector2 beamXZScale = _focusBeamBaseXZscale * beamXZScaleCoefficient;
                _FocusBeamCylinder.transform.localScale = new Vector3(beamXZScale.x * sinMult, _FocusBeamCylinder.transform.localScale.y, beamXZScale.y * sinMult);

                int numOverlaps = Physics2D.CircleCastNonAlloc(_FocusBeamCylinder.transform.position, _FocusBeamEffectiveWidth, Vector2.up, _beamOverlaps,
                    _focusBeamEndPoint.position.y - _FocusBeamCylinder.transform.position.y, _BeamDamageLayerMask);
                for (int i = 0; i < numOverlaps; i ++)
                {
                    _beamOverlaps[i].collider.GetComponent<Enemy>()
                        .MyEnemyHealth.TakeDamage(_FocusBeamDamagePerSecond * Time.deltaTime);
                }
            }
        }
    }

    private IEnumerator BurstSequence()
    {
        do
        {
            _bufferedPressInput = false;

            int burstVolleysRemaining = _BurstVolleyCount;

            while (burstVolleysRemaining > 0)
            {
                burstVolleysRemaining--;
                FireBurstVolley();

                if (burstVolleysRemaining > 0)
                    yield return new WaitForSeconds(_BurstVolleyDelay);

            }
        } while (_bufferedPressInput);

        _burstCoroutine = null;
    }

    private void FireBurstVolley()
    {
        Transform firePoint;
        for (int i = 0; i < _burstFirePoints.Length; i ++)
        {
            firePoint = _burstFirePoints[i];
            Pool.Get(_BurstBulletPrefab, firePoint.position, firePoint.up)
                .GetComponent<PlayerBullet>().Launch(firePoint.up * _BurstBulletSpeed);

            _burstFirePointParticleSystems[i].Play();
        }

        _Audio_OnFireBurstVolley.Play();
    }

    private void StartFocus()
    {
        _focusEngaged = true;
        _focusEngagedTime = 0f;

        _FocusBeamCylinder.SetActive(true);
        _FocusBeamCylinder.transform.localScale = new Vector3(0f, _FocusBeamCylinder.transform.localScale.y, 0f);

        _FocusPS.Play();

        _Audio_FocusBeam.Play();

        if (Ev_OnFocusStart != null) Ev_OnFocusStart();
    }

    private void StopFocus()
    {
        _focusEngaged = false;

        _FocusBeamCylinder.SetActive(false);

        _FocusPS.Stop();

        _Audio_FocusBeam.Stop();

        if (Ev_OnFocusStop != null) Ev_OnFocusStop();
    }

}
