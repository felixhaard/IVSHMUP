using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    [SerializeField]
    private InputAction _Input_StartGame = default;
    [SerializeField]
    private InputAction _Input_Exit = default;

    [Space]

    [SerializeField]
    private float _BlockInputFor = 0.25f; // hacky solution to prevent keypress from previous scene to carry over

    //

    private bool _canInteract = true;
    private float _startTime;

    //

    private void Awake()
    {
        _Input_StartGame.Enable();
        _Input_Exit.Enable();

        _startTime = Time.time;
    }

    private void Update()
    {
        if (!_canInteract || Time.time < _startTime + _BlockInputFor) return;

        //

        if (_Input_StartGame.ReadValue<float>() > 0f && _Input_StartGame.triggered)
        {
            _canInteract = false;

            Player.GodMode = false;
            SceneManager.LoadScene("GameScene");
        } else if (_Input_Exit.ReadValue<float>() > 0f && _Input_Exit.triggered)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Unable to exit application from Editor Play Mode");
#else
            _canInteract = false;
#endif
            Application.Quit();
        }
    }

}
