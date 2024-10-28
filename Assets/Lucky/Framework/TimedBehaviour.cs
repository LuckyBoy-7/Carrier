using UnityEngine;
using System.Collections;
using Lucky.Framework;
using Lucky.Kits.Utilities;
using UnityEngine.Serialization;

/// <summary>
/// 每隔一段时间Trigger一次OnTimerReached, 可控制每次时间间隔(调整volatility)
/// </summary>
public class TimedBehaviour : ManagedBehaviour
{
    public float FrequencyMultiplier { get; set; } = 1f;

    public bool Realtime;

    [SerializeField] private float originalFrequency = 1;

    /// <summary>
    /// 频率变化振幅
    /// </summary>
    [SerializeField] private float volatility = 0;

    protected float timer;
    private float frequency;

    /// <summary>
    /// 是否应该暂停
    /// </summary>
    protected virtual bool Paused => false;

    /// <summary>
    /// 多久Trigger一次
    /// </summary>
    private float TriggerTime => 1 / (frequency * FrequencyMultiplier);

    public void AddDelay(float delay) => timer -= delay;

    public void SetFrequency(float newFrequency) => frequency = newFrequency;

    protected override void ManagedFixedUpdate()
    {
        base.ManagedFixedUpdate();
        if (Paused)
            return;
        if (frequency <= 0f)
            frequency = originalFrequency;

        timer -= Timer.FixedDeltaTime(Realtime);
        if (timer <= 0)
        {
            float frequencyAdjust = (-0.5f + Random.value) * volatility; // [-0.5, 0.5] * volatility
            frequency = originalFrequency + frequencyAdjust;
            Reset();
            OnTimerReached();
        }
    }

    protected void SkipToEndOfTimer()
    {
        timer = 0;
    }

    protected void Reset()
    {
        timer = TriggerTime;
    }

    protected virtual void OnTimerReached()
    {
    }
}