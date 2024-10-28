using System;
using System.Collections;
using System.Collections.Generic;
using Lucky.Framework;
using UnityEngine;

namespace Lucky.Kits.Animation
{
    public class Pulsater : LuckyComponent
    {
        public float amount = 0.1f;
        public float speed = 1f;

        private float k = -1f;

        private Vector3 origSize;

        public override void Added(ManagedBehaviour entity)
        {
            base.Added(entity);
            origSize = entity.transform.localScale;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (k >= 0f)
            {
                k = Mathf.MoveTowards(k, 1f, Time.deltaTime * speed);
                var stepped = Mathf.SmoothStep(0f, 1f, k);
                // [1, 2]
                var size = Mathf.Sin(Mathf.PI * stepped) * amount + 1f;
                Entity.transform.localScale = size * origSize;
            }
        }

        public void Pulsate()
        {
            k = 0f;
        }
    }
}