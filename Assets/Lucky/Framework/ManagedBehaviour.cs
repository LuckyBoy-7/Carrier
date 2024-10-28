using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Lucky.Framework
{

    public class ManagedBehaviour : ManagedBehaviourBase
    {

        #region Components

        public LuckyComponentList Components;

        public void Add(LuckyComponent component)
        {
            Components ??= new(this);
            Components.Add(component);
        }

        public void Remove(LuckyComponent component)
        {
            Components ??= new(this);
            Components.Remove(component);
        }

        #endregion

        public virtual bool AlwaysUpdate { get; set; } = false;
        public virtual bool UpdateCondition { get; set; } = true;

        private bool CanUpdate
        {
            get
            {
                if (AlwaysUpdate)
                    return true;
                if (!UpdateCondition)
                    return false;
                switch (GameManager.Instance.GameState)
                {
                    case GameManager.GameStateType.Play:
                        return true;
                    case GameManager.GameStateType.Pause:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool visible = true;

        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value)
                    return;
                visible = value;
                GetComponent<Renderer>().enabled = visible;
            }
        }

        #region Update

        protected virtual void ManagedUpdate()
        {
            Components?.Update();
        }

        protected virtual void ManagedFixedUpdate()
        {
            Components?.FixedUpdate();
        }

        /// <summary>
        /// 把Update Ban了(后来想了下可以用在偏视觉的方面, 这样不影响逻辑, 也更流畅)
        /// </summary>
        public override sealed void Update()
        {
            if (CanUpdate)
            {
                ManagedUpdate();
            }
        }

        public override sealed void FixedUpdate()
        {
            if (CanUpdate)
            {
                ManagedFixedUpdate();
                Render(); // 卡爆了(
            }
            // DebugRender();
        }

        public virtual void Render() // 虽然不是真正意义上的render, 但还是在这儿做个逻辑上的分离
        {
            Components?.Render();
        }

        #endregion

        protected new Coroutine StartCoroutine(IEnumerator enumerator)
        {
            Coroutine coroutine = new Coroutine(enumerator, true);
            Add(coroutine);
            return coroutine;
        }

        protected Coroutine StartCoroutine(Action callback, float delay)
        {
            IEnumerator Enumerator()
            {
                yield return delay;
                callback?.Invoke();
            }

            Coroutine coroutine = new Coroutine(Enumerator());
            Add(coroutine);
            return coroutine;
        }


    }

}