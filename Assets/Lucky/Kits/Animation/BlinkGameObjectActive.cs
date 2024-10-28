using System.Collections;
using System.Collections.Generic;
using Lucky.Kits.Audio;
using UnityEngine;

namespace Lucky.Kits.Animation
{
    /// <summary>
    /// 开关对象并在打开时发出声音
    /// </summary>
    public class BlinkGameObjectActive : TimedBehaviour
    {
        [Header("Blink")] [SerializeField] private GameObject objectToBlink = default;

        [SerializeField] private string blinkSound = default;

        protected override void OnTimerReached()
        {
            objectToBlink.SetActive(!objectToBlink.activeSelf);

            if (objectToBlink.activeSelf && !string.IsNullOrEmpty(blinkSound))
            {
                AudioController.Instance.PlaySound2D(blinkSound, volume: 0.25f);
            }
        }
    }
}