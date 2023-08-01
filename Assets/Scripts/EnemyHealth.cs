using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private float _Health = 10f;

    //

    private Enemy _enemy;

    private float _remainingHealth;
    private bool _dead;

    public event UnityAction Ev_OnDeath;
    public event UnityAction Ev_OnSurviveDamage;

    //

    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.MyPoolableObjectReference.Ev_OnPoolableGet += OnPoolableGet;
    }

    private void OnPoolableGet()
    {
        _remainingHealth = _Health;
        _dead = false;
    }

    public void TakeDamage(float dmg)
    {
        if (_dead || !GameCamera.TestRendererInsideFrustum(_enemy.MyMeshRenderer)) return;

        //

        _remainingHealth -= dmg;
        if (_remainingHealth <= 0f)
        {
            _dead = true;

            if (Ev_OnDeath != null) Ev_OnDeath();

            _enemy.Remove();
        } else
        {
            if (Ev_OnSurviveDamage != null) Ev_OnSurviveDamage();
        }
    }

}
