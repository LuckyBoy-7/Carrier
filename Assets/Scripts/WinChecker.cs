using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lucky.Framework;
using Lucky.Kits.Extensions;
using UnityEngine;

public class WinChecker : ManagedBehaviour
{
    private List<Transform> holes = new();
    private List<Transform> rocks = new();

    private void Awake()
    {
        holes.Extend(GameObject.FindGameObjectsWithTag("Hole").Select(go => go.transform).ToList());
        rocks.Extend(GameObject.FindGameObjectsWithTag("Rock").Select(go => go.transform).ToList());
    }

    protected override void ManagedFixedUpdate()
    {
        base.ManagedFixedUpdate();
        foreach (var hole in holes)
        {
            if (rocks.All(rock => Vector2.Distance(rock.position, hole.position) > 0.01f))
                return;
        }

        print("Win");
    }
}