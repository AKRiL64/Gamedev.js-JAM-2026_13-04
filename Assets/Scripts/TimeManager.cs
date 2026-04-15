using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    private bool _isWaiting;
    private float currentTimescale = 1;

    void Awake() => Instance = this;

    public void HitStop(float duration)
    {
        if (_isWaiting) return;
        StartCoroutine(DoHitStop(duration));
    }

    private IEnumerator DoHitStop(float duration)
    {
        _isWaiting = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = currentTimescale;
        _isWaiting = false;
    }
}