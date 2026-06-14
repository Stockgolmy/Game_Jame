using UnityEngine;

public class Cactus : MonoBehaviour
{
    public float damage = 10f;
    public float delay = 1f;
    public float lastContactTime = -1f;

    private void Start()
    {
        lastContactTime = -delay;
    }

    public void TryDealDamage()
    {
        if (WakeMeter.Instance == null) return;
        if (Time.time < lastContactTime + delay) return;

        WakeMeter.Instance.AddValue(damage);
        lastContactTime = Time.time;
    }
}