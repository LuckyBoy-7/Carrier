using System.Collections.Generic;
using Lucky.Framework;
using Lucky.Kits.Extensions;
using UnityEngine;
using Input = Lucky.Kits.Inputs.Input;

namespace Lucky.Kits.Collections.Test
{
    public class AddIntCommand : ICommand
    {
        private CommandTest parent;
        public AddIntCommand(CommandTest parent)
        {
            this.parent = parent;
        }
        public ICommand Do()
        {
            parent.Lst.Add(1);
            return this;
        }

        public ICommand Undo()
        {
            parent.Lst.Pop();
            return this;
        }
    }

    public class CommandTest : ManagedBehaviour
    {
        public List<int> Lst = new();

        protected override void ManagedFixedUpdate()
        {
            base.ManagedFixedUpdate();
            if (Input.GetKeyDown(KeyCode.J))
            {
                CommandManager.Instance.CreateNewCommandSequence();
                ICommand command = new AddIntCommand(this);
                command.Do();
                CommandManager.Instance.AddCommand(command);
            }
        }


    }
}