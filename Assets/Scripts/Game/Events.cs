using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    [Header("Game Raised Events")]
    public Action OnGameStart;
    public Action OnLevelCompleted;
    public Action<bool> ChangeDeselectVisibility;

    [Header("UI Raised Events")]
    public Action OnUIDeselectRequest;

    public static Events singleton { get; set; }

    private void Awake()
    {
        if (Events.singleton != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        Events.singleton = this;
    }
}
