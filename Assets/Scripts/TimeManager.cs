using UnityEngine;
using System.Collections.Generic;

public class TimeManager : MonoBehaviour
{

    #region Singleton

    private static TimeManager instance;

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
                Destroy(this);
            }
        }
    }

    #endregion Singleton

    //

    private List<Vector2> _activeSlowdownInstances = new List<Vector2>();

    //

    public static void AddSlowdownInstance(float timeScale, float unscaledDuration)
    {
        Debug.Assert(timeScale < 1f);
        Debug.Assert(unscaledDuration > 0f);

        instance._activeSlowdownInstances.Add(new Vector2(timeScale, Time.unscaledTime + unscaledDuration));
    }

    //

    private void LateUpdate()
    {
        float timeScale = 1f;

        for (int i = _activeSlowdownInstances.Count - 1; i >= 0; i --)
        {
            if (Time.unscaledTime > _activeSlowdownInstances[i].y)
            {
                _activeSlowdownInstances.RemoveAt(i);
            } else
            {
                timeScale = Mathf.Min(timeScale, _activeSlowdownInstances[i].x);
            }
        }

        Time.timeScale = timeScale;
    }

}
