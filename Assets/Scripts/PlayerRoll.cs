using UnityEngine;

public class PlayerRoll : MonoBehaviour
{

    [SerializeField]
    private Transform _RollAnchor = default;

    [Space]

    [SerializeField]
    private float _MaxDegrees = 36f;

    [SerializeField]
    private float _OutwardRollSpeed = 225f;
    [SerializeField]
    private float _ReturnRollSpeed = 135f;

    //

    private Rigidbody2D _rb;
    private float _rollAngleDegrees = 0f;

    //

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        float xVel = _rb.velocity.x;

        float targetRollDegrees;
        float spd;

        if (Mathf.Approximately(xVel, 0f))
        {
            targetRollDegrees = 0f;
            spd = _ReturnRollSpeed;
        }
        else
        {
            targetRollDegrees = xVel > 0f ? -_MaxDegrees : _MaxDegrees;
            spd = _OutwardRollSpeed;
        }

        _rollAngleDegrees = Mathf.MoveTowards(_rollAngleDegrees, targetRollDegrees, spd * Time.deltaTime);
        _RollAnchor.localRotation = Quaternion.Euler(0f, _rollAngleDegrees, 0f);
    }

}
