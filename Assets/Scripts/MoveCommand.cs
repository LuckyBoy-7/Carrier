using Lucky.Kits.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveCommand : ICommand
    {
        private Transform target;
        private Vector2 dir;

        public MoveCommand(Transform target, Vector2 dir)
        {
            this.dir = dir;
            this.target = target;
        }
        public ICommand Do()
        {
            target.position += (Vector3)dir;
            return this;
        }

        public ICommand Undo()
        {
            target.position -= (Vector3)dir;
            return this;
        }
    }
}