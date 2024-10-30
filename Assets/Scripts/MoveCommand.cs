using DG.Tweening;
using Lucky.Kits.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class MoveCommand : ICommand
    {
        private Transform target;
        private Vector2 origPos;
        private Vector2 targetPos;

        public MoveCommand(Transform target, Vector2 dir)
        {
            origPos = target.position;
            targetPos = origPos + dir;
            this.target = target;
        }

        public ICommand Do()
        {
            Player.instance.canOperate = false;
            target.DOMove(targetPos, 0.1f).onComplete += () => Player.instance.canOperate = true;
            return this;
        }

        public ICommand Undo()
        {
            target.position = origPos;
            return this;
        }
    }
}