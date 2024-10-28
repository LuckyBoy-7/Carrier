using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Lucky.Kits.Animation
{
    /// <summary>
    /// 使currentValue向baseValue周围某个位置移动，经过Timer秒更改位置并继续移动
    /// </summary>
    public abstract class SmoothFlickerValue : TimedBehaviour
    {
        [Header("Flicker")] [SerializeField] private float baseValue = 0.5f;

        [SerializeField] private float valueVolatility = 0.8f;

        [SerializeField] private float valueChangeSpeed = 2;

        private float currentValue;
        private float intendedValue;

        private void Awake()
        {
            currentValue = intendedValue = baseValue;
            SetNewIntendedValue();
        }

        protected override void OnTimerReached()
        {
            SetNewIntendedValue();
        }

        protected override void ManagedFixedUpdate()
        {
            currentValue = Mathf.Lerp(currentValue, intendedValue, Time.deltaTime * valueChangeSpeed);
            ApplyValue(currentValue);
        }

        protected abstract void ApplyValue(float value);

        private void SetNewIntendedValue()
        {
            intendedValue = baseValue + (Random.value - 0.5f) * valueVolatility;
        }
    }
}