using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lucky.Kits.Animation
{
    public class RotatedSpriteDisplayer : FrameByFrameAnimation

    {
        [SerializeField] private List<Sprite> rotationFrames = new List<Sprite>();

        [SerializeField] private SpriteRenderer spriteRenderer = default;

        [SerializeField] private float rotationRange = 360f;

        protected override int FrameCount => rotationFrames.Count;
        private float origDegree;

        protected override void FindRendererComponent()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            origDegree = transform.eulerAngles.z;
        }

        protected override void DisplayFrame(int frameIndex)
        {
            spriteRenderer.sprite = rotationFrames[frameIndex];

            float rotationPerFrame = rotationRange / FrameCount; // 表示每一帧对应多少degree
            transform.localEulerAngles = new Vector3(0f, 0f, origDegree + frameIndex * rotationPerFrame);
        }

        protected override void Clear()
        {
        }
    }
}