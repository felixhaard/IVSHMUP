using UnityEngine;
using TMPro;

public class HUDCanvas : MonoBehaviour
{

    #region Singleton

    private static HUDCanvas instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
                Debug.LogWarning("Invalid singleton");
            }
        }
    }

    #endregion

    //

    [SerializeField]
    private Transform _ShieldIconRoot = default;

    [SerializeField]
    private TextMeshProUGUI _ScoreLabel = default;

    //

    public static void SetRemainingHealth(int health)
    {
        int showNumShields = Mathf.Min(health - 1, instance._ShieldIconRoot.childCount);

        for (int i = 0; i < instance._ShieldIconRoot.childCount; i ++)
        {
            instance._ShieldIconRoot.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < showNumShields; i ++)
        {
            instance._ShieldIconRoot.GetChild(i).gameObject.SetActive(true);
        }
    }

    public static void SetScoreLabel(int score)
    {
        instance._ScoreLabel.text = score.ToString();
    }

}
