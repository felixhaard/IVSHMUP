using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private InputAction _Input_Movement = default;
    [SerializeField]
    private InputAction _Input_Attack = default;

    //

    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;

    private bool _previousAttackInput = false;

    //

    private void Awake()
    {
        _Input_Movement.Enable();
        _Input_Attack.Enable();

        //

        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        _playerMovement.SetMoveDirection(_Input_Movement.ReadValue<Vector2>().normalized);

        //

        bool attackInput = _Input_Attack.ReadValue<float>() > 0f;
        if (attackInput != _previousAttackInput)
        {
            if (attackInput)
            {
                _playerAttack.OnPress();
            } else
            {
                _playerAttack.OnRelease();
            }
        }
        _previousAttackInput = attackInput;
    }

}
