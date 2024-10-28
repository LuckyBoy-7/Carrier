using Lucky.Kits.Collections;
using Lucky.Kits.Extensions;
using Lucky.Kits.Utilities;
using UnityEngine;

namespace DefaultNamespace
{
    public class ChangePlayerFacingDirCommand : ICommand
    {
        private Player player;
        private Vector2 dir;
        private Vector2 origDir;

        public ChangePlayerFacingDirCommand(Player target, Vector2 dir)
        {
            origDir = target.CurrentDir;
            this.dir = dir;
            this.player = target;
        }

        public ICommand Do()
        {
            player.transform.eulerAngles = Vector3.zero.WithZ(MathUtils.SignedAngle(Vector2.right, dir));
            return this;
        }

        public ICommand Undo()
        {
            player.transform.eulerAngles = Vector3.zero.WithZ(MathUtils.SignedAngle(Vector2.right, origDir));
            return this;
        }
    }
}