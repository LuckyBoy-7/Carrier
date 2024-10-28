using DG.Tweening;
using Lucky.Kits.Collections;
using Lucky.Kits.Extensions;
using Lucky.Kits.Utilities;
using UnityEngine;

namespace DefaultNamespace
{
    public class CarryCommand : ICommand
    {
        private Player player;
        private Vector2 lastDir;
        private Vector2 dir;
        private Vector2 rockOrigPos;
        private Transform rock;

        public CarryCommand(Player player, Vector2 dir)
        {
            this.player = player;
            lastDir = player.CurrentDir;
            this.dir = dir;
        }

        public ICommand Do()
        {
            player.canOperate = false;
            player.dontChangeHand = true;
            rock = player.GetRock(lastDir);
            rockOrigPos = rock.position;
            var tween = player.transform.DORotate(Vector3.zero.WithZ(MathUtils.SignedAngle(Vector2.right, dir)), 0.12f);
            tween.onComplete += () =>
            {
                player.dontChangeHand = false;
                player.canOperate = true;
            };
            tween.onUpdate += () => { rock.position = player.transform.position + (Vector3)MathUtils.AngleToVector(player.transform.eulerAngles.z, 1); };
            return this;
        }

        public ICommand Undo()
        {
            rock.position = rockOrigPos;
            player.transform.eulerAngles = Vector3.zero.WithZ(MathUtils.SignedAngle(Vector2.right, lastDir));
            return this;
        }
    }
}