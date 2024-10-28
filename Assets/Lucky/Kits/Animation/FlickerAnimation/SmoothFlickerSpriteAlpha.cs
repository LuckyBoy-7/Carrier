using System.Collections;
using System.Collections.Generic;
using Lucky.Kits.Extensions;
using UnityEngine;


namespace Lucky.Kits.Animation
{
    public class SmoothFlickerSpriteAlpha : SmoothFlickerValue
    {
        [Header("Flicker Alpha")]
        [SerializeField]
        private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

        protected override void ApplyValue(float value)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = sr.color.WithA(value);
            }
        }
    }
}