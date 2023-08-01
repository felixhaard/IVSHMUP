using UnityEngine;
using System.Collections;

public class EnemyDamageFeedback : MonoBehaviour
{

    [SerializeField]
    private PoolableObject _OnDeathVFXPrefab = default;

    [Space]

    [SerializeField]
    private float _OnDamageFlashFrequency = 0.02f;

    [SerializeField]
    private float _OnDamageFlashDuration = 0.2f;

    //

    private MeshRenderer _meshRenderer;

    private float _stopFlashingTime;
    private bool _flashingOngoing;
    private Coroutine _flashCoroutine;

    //

    private void Start()
    {
        Enemy enemy = GetComponent<Enemy>();
        enemy.MyEnemyHealth.Ev_OnSurviveDamage += OnSurviveDamage;
        enemy.MyEnemyHealth.Ev_OnDeath += OnDeath;

        _meshRenderer = enemy.MyMeshRenderer;

        enemy.MyPoolableObjectReference.Ev_OnPoolableRelease += OnPoolableRelease;
    }

    private void OnPoolableRelease()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        _flashingOngoing = false;

        _meshRenderer.enabled = true;
    }

    private void OnSurviveDamage()
    {
        _stopFlashingTime = Time.time + _OnDamageFlashDuration;
        if (!_flashingOngoing)
        {
            _flashCoroutine = StartCoroutine(FlashSequence());
        }
    }

    private void OnDeath()
    {
        if (_OnDeathVFXPrefab != null)
            Pool.Get(_OnDeathVFXPrefab, transform.position, Vector3.up);
    }

    private IEnumerator FlashSequence()
    {
        _flashingOngoing = true;

        while (Time.time < _stopFlashingTime)
        {
            _meshRenderer.enabled = !_meshRenderer.enabled;
            yield return new WaitForSecondsRealtime(_OnDamageFlashFrequency);
        }

        _meshRenderer.enabled = true;

        _flashingOngoing = false;
        _flashCoroutine = null;
    }

}
