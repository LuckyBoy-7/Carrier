using System;
using Lucky.Kits.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Lucky.Kits.Animation
{
    /// <summary>
    /// 控制帧随时间变化的行为，之后可运用到动画或是其他方面
    /// </summary>
    public abstract class FrameByFrameAnimation : TimedBehaviour
    {
        public bool Reversed { get; set; } // 表示方向是否逆转
        public bool Animating => enabled;

        public Action<int> FrameChanged; // 当帧为value时调用的函数

        protected abstract int FrameCount { get; } // 总共几帧，对动画来说一般就是有几张图片


        [Header("Animation"), SerializeField] private bool startAtRandomFrame; // 开始时是否从随机帧开始

        [HideIf("startAtRandomFrame"), SerializeField]
        private int startFrameOffset; // 开始时帧的偏移

        [HideIf("randomizeFrames"), SerializeField]
        private bool stopAfterSingleIteration; // 一次迭代后是否停止

        [HideIf("stopAfterSingleIteration"), SerializeField]
        private bool pingpong; // 是否pingpong

        [HideIf("stopAfterSingleIteration"), SerializeField]
        private float waitBetweenLoopsMin; // 两帧间的最小等待时间

        [HideIf("stopAfterSingleIteration"), SerializeField]
        private float waitBetweenLoopsMax; // 两帧间的最大等待事件

        [Header("Frames"), SerializeField] private bool randomizeFrames; // 下一帧是随机的还是按固定步数的

        [ShowIf("randomizeFrames"), SerializeField]
        private bool noRepeat; // 随机帧的时候是否可以连续随机到一样的

        private int frameIndex;
        private bool stopOnNextFrame;

        protected abstract void FindRendererComponent(); // 一般是spriteRenderer
        protected abstract void DisplayFrame(int frameIndex); // 和前面配合，前面get，这里set，为子类提供接口
        protected abstract void Clear();

        private void Awake()
        {
            FindRendererComponent();

            if (startAtRandomFrame) // 如果从随机位置开始，就roll一个位置
            {
                frameIndex = Random.Range(0, FrameCount);
            }
            else if (startFrameOffset > 0)
            {
                frameIndex = startFrameOffset % FrameCount;
            }

            SetFrame(frameIndex);

            AddDelay(RandomUtils.Range(waitBetweenLoopsMin, waitBetweenLoopsMax));
        }

        protected override void OnTimerReached()
        {
            IterateFrame();
        }

        public void StartFromBeginning()
        {
            enabled = true;
            frameIndex = 0;
            SetFrame(0);
            timer = 0f;
        }

        public void Resume()
        {
            enabled = true;
            stopOnNextFrame = false;
        }

        public void StopAnimating()
        {
            stopOnNextFrame = true;
        }

        public void Stop()
        {
            stopOnNextFrame = false;
            enabled = false;
            SetFrame(0);
        }

        public void SkipToEnd()
        {
            if (Reversed)
            {
                frameIndex = 0;
            }
            else
            {
                frameIndex = FrameCount - 1;
            }

            SetFrame(frameIndex);
        }

        private void IterateFrame()
        {
            if (stopOnNextFrame)
            {
                Stop();
                return;
            }

            timer = 0f;

            if (randomizeFrames)
            {
                int randomFrame = Random.Range(0, FrameCount);
                while (randomFrame == frameIndex && noRepeat)
                {
                    randomFrame = Random.Range(0, FrameCount);
                }

                frameIndex = randomFrame;
                SetFrame(randomFrame);
            }
            else // 按固定步数改变帧
            {
                int dir = Reversed ? -1 : 1;
                frameIndex += dir;
                if ((!Reversed && frameIndex >= FrameCount) || (Reversed && frameIndex < 0)) // 超过一个循环了
                {
                    if (stopAfterSingleIteration) // 如果只搞一个循环的话就撤销
                    {
                        enabled = false;
                        frameIndex -= dir;
                    }
                    else // 如果loop的话
                    {
                        if (pingpong)
                        {
                            frameIndex -= dir; // 先撤销
                            frameIndex -= dir; // 再反向走，加不加的区别就是在边界的帧上是否会有多一帧的停顿
                            Reversed = !Reversed; // 再pingpong
                        }
                        else
                        {
                            frameIndex = Reversed ? FrameCount - 1 : 0; // 继续走
                        }

                        AddDelay(RandomUtils.Range(waitBetweenLoopsMin, waitBetweenLoopsMax)); // 添加帧之间的延迟
                    }
                }

                SetFrame(frameIndex);
            }
        }

        private void SetFrame(int index)
        {
            FrameChanged?.Invoke(index);
            DisplayFrame(index);
        }
    }
}