using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Lucky.Framework;
using Lucky.Kits.Collections;
using Lucky.Kits.Extensions;
using Lucky.Kits.Utilities;
using UnityEngine;
using Input = Lucky.Kits.Inputs.Input;

public class Player : ManagedBehaviour
{
    public GameObject idleHand;
    public GameObject holdHand;
    public Vector2 lastDir = Vector2.right;

    protected override void ManagedFixedUpdate()
    {
        base.ManagedFixedUpdate();

        if (Input.GetKey(KeyCode.Space) && HasRock(lastDir)) // 按空格就是搬运
        {
            idleHand.gameObject.SetActive(false);
            holdHand.gameObject.SetActive(true);

            Vector2 dir = Vector2.zero;
            if (Input.Up.Pressed)
                dir = Vector2.up;
            else if (Input.Down.Pressed)
                dir = Vector2.down;
            else if (Input.Left.Pressed)
                dir = Vector2.left;
            else if (Input.Right.Pressed)
                dir = Vector2.right;
            else
                return;
            // 只能朝相邻方向搬运
            if (-dir == lastDir)
                return;
            // 就是搬运转弯时的那个对角位置
            Vector2 cornerDir = dir + lastDir;
            // 没路或者挡着就不行
            if (!HasPath(cornerDir) || HasRock(cornerDir) || !HasPath(dir) || HasRock(dir))
                return;

            CommandManager.Instance.CreateNewCommandSequence();
            CommandManager.Instance.AddCommand(new MoveCommand(GetRock(lastDir), dir - lastDir).Do());
            CommandManager.Instance.AddCommand(new ChangePlayerFacingDirCommand(this, dir).Do());
        }
        else // 不按空格就是移动
        {
            idleHand.gameObject.SetActive(true);
            holdHand.gameObject.SetActive(false);

            Vector2 dir = Vector2.zero;
            if (Input.Up.Pressed)
                dir = Vector2.up;
            else if (Input.Down.Pressed)
                dir = Vector2.down;
            else if (Input.Left.Pressed)
                dir = Vector2.left;
            else if (Input.Right.Pressed)
                dir = Vector2.right;
            else
                return;
            // 判断是否能走
            if (!HasPath(dir))
                return;
            if (HasPath(dir) && HasRock(dir) && (!HasPath(dir * 2) || HasRock(dir * 2)))
                return;

            CommandManager.Instance.CreateNewCommandSequence();
            if (HasRock(dir))
            {
                CommandManager.Instance.AddCommand(new MoveCommand(GetRock(dir), dir).Do());
            }

            CommandManager.Instance.AddCommand(new MoveCommand(transform, dir).Do());
            CommandManager.Instance.AddCommand(new ChangePlayerFacingDirCommand(this, dir).Do());
        }
    }

    public override void Render()
    {
        base.Render();
        transform.eulerAngles = Vector3.zero.WithZ(MathUtils.SignedAngle(Vector2.right, lastDir));
    }

    public bool HasPath(Vector2 dir) => Physics2D.OverlapPointAll(transform.position + (Vector3)dir).Any(box => box.gameObject.CompareTag("Path"));
    public bool HasRock(Vector2 dir) => Physics2D.OverlapPointAll(transform.position + (Vector3)dir).Any(box => box.gameObject.CompareTag("Rock"));

    public Transform GetRock(Vector2 dir) =>
        Physics2D.OverlapPointAll(transform.position + (Vector3)dir).First(box => box.gameObject.CompareTag("Rock")).transform;


}