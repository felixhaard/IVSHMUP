using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float _Speed = 5f;

    [SerializeField]
    private float _Speed_Focus = 2.5f;

    //

    private Rigidbody2D _rb;

    private float _appliedSpeed;

    //

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _appliedSpeed = _Speed;

        PlayerAttack atk = GetComponent<PlayerAttack>();
        atk.Ev_OnFocusStart += OnFocusStart;
        atk.Ev_OnFocusStop += OnFocusStop;
    }

    private void OnFocusStart()
    {
        _appliedSpeed = _Speed_Focus;
    }

    private void OnFocusStop()
    {
        _appliedSpeed = _Speed;
    }

    public void SetMoveDirection(Vector2 normalizedInput)
    {
        _rb.velocity = normalizedInput * _appliedSpeed + Vector2.up * ViewPort.GetSpeed();
    }

    private void LateUpdate()
    {
        ViewPort.ClampRBPosition(_rb);
    }

}
