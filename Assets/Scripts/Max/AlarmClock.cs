using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    public float damage = 1f;

    public bool isActive = true;
    public bool isRegistered = false;

    private void Start()
    {
        RegisterToWakeMeter();
    }

    public void RegisterToWakeMeter()
    {
        if (!isActive || isRegistered) return;
        if (WakeMeter.Instance == null) return;

        WakeMeter.Instance.IncreaseMultiplier(damage);
        isRegistered = true;
    }

    public void UnregisterFromWakeMeter()
    {
        if (!isRegistered) return;
        if (WakeMeter.Instance == null) return;

        WakeMeter.Instance.DecreaseMultiplier(damage);
        isRegistered = false;
    }

    public void TurnOff()
    {
        if (!isActive) return;

        isActive = false;
        UnregisterFromWakeMeter();

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        UnregisterFromWakeMeter();
    }
}