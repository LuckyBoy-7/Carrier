using Lucky.Kits.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class ChangePlayerFacingDirCommand : ICommand
    {
        private Player target;
        private Vector2 dir;
        private Vector2 origDir;

        public ChangePlayerFacingDirCommand(Player target, Vector2 dir)
        {
            origDir = target.lastDir;
            this.dir = dir;
            this.target = target;
        }

        public ICommand Do()
        {
            target.lastDir = dir;
            return this;
        }

        public ICommand Undo()
        {
            target.lastDir = origDir;
            return this;
        }
    }
}