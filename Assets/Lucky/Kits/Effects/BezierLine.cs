using System;
using System.Collections;
using System.Collections.Generic;
using Lucky.Framework;
using Lucky.Kits.Utilities;
using UnityEditor;
using UnityEngine;

namespace Lucky.Kits.Effects
{

    [RequireComponent(typeof(LineRenderer))]
    public class BezierLine : ManagedBehaviour
    {
        public List<Transform> trackedTransforms;
        public int resolution = 50;
        private LineRenderer line;

        private void Awake()
        {
            line = GetComponent<LineRenderer>();
        }

        protected override void ManagedUpdate()
        {
            base.ManagedUpdate();

            if (trackedTransforms.Count != 3)
                return;
            line.positionCount = resolution + 1;
            BezierCurve curve = new BezierCurve(trackedTransforms[0].position, trackedTransforms[1].position, trackedTransforms[2].position);
            for (var i = 0; i <= resolution; i++)
            {
                line.SetPosition(i, curve.GetPoint((float)i / resolution));
            }
        }
    }
}