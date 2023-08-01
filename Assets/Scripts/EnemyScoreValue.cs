using UnityEngine;

public class EnemyScoreValue : MonoBehaviour
{

    [SerializeField]
    private int _ScoreValue = 10;

    //

    private void Awake()
    {
        GetComponent<EnemyHealth>().Ev_OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        GameManager.AddScore(_ScoreValue);
    }

}
