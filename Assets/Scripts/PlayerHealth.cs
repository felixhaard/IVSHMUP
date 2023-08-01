using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private MeshRenderer _ShipRenderer = default;

    [Space]

    [SerializeField]
    private int _Health = 3;

    [Space]

    [SerializeField]
    private float _OnHitSlowdownTimescale = 0.1f;
    [SerializeField]
    private float _OnHitSlowdownDuration = 0.2f;

    [Space]

    [SerializeField]
    private float _IFrameDuration = 1.75f;

    [SerializeField]
    private float _IFrameFlashFrequency = 0.04f;

    [Space]

    [SerializeField]
    private PoolableObject _OnSurviveDamageFXPrefab = default;

    [SerializeField]
    private PoolableObject _OnDeathFXPrefab = default;

    //

    private int _currentHealth;

    private bool _dead = false;

    private bool _iFrames = false;

    //

    private void Awake()
    {
        _currentHealth = _Health;
    }

    private void Start()
    {
        HUDCanvas.SetRemainingHealth(_currentHealth);
    }

    public void TakeDamage()
    {
        if (_dead || _iFrames) return;

        //

        TimeManager.AddSlowdownInstance(_OnHitSlowdownTimescale, _OnHitSlowdownDuration);

        _currentHealth--;
        HUDCanvas.SetRemainingHealth(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnDeath();
        }
        else
        {
            OnSurviveDamage();
        }
    }

    private void OnDeath()
    {
        _dead = true;

        Pool.Get(_OnDeathFXPrefab, transform.position, Vector3.up);

        GameManager.EndGame(false);
    }

    private void OnSurviveDamage()
    {
        Pool.Get(_OnSurviveDamageFXPrefab, transform.position, Vector3.up);

        StartCoroutine(SurviveDamageSequence());
    }

    private IEnumerator SurviveDamageSequence()
    {
        _iFrames = true;

        float stopIFrameTime = Time.time + _IFrameDuration;
        while (Time.time <= stopIFrameTime)
        {
            _ShipRenderer.enabled = !_ShipRenderer.enabled;

            yield return new WaitForSecondsRealtime(_IFrameFlashFrequency);
        }

        _ShipRenderer.enabled = true;

        _iFrames = false;
    }

}
