using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeApplication : MonoBehaviour
{

    [SerializeField]
    private FullScreenMode _FullScreenMode = FullScreenMode.Windowed;

    [SerializeField]
    private Vector2Int _Resolution = new Vector2Int(720, 720);

    private void Start()
    {
        Screen.SetResolution(_Resolution.x, _Resolution.y, _FullScreenMode);

        SceneManager.LoadScene("StartMenu");
    }

}
