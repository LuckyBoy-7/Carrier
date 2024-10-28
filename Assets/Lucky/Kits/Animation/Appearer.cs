using System;
using DG.Tweening;
using Lucky.Framework;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lucky.Kits.Animation
{
    /// <summary>
    /// 弹出, 缩回动画
    /// </summary>
    public class Appearer : ManagedBehaviour
    {
        [SerializeField] private Vector3 hiddenSize;

        /// <summary>
        /// 出现的延迟, 如果值 小于 0, 那么就是等待外部调用
        /// </summary>
        public float appearDelay = -1f;

        public float hideDelay;
        public GameObject visuals;
        public bool inScreenSpace;
        public Camera cam;

        public TMP_Text text;
        private Vector3 origSize;

        public bool IsShown { get; private set; }

        private const float ShowDuration = 0.3f;
        private const float HideDuration = 0.2f;

        private void Awake()
        {
            // 记录原始缩放并隐藏
            origSize = transform.localScale;
            transform.localScale = hiddenSize;
            visuals?.SetActive(false);

            if (appearDelay >= 0)
                ShowAfter(appearDelay);
        }

        public void ShowAfter(float delay) => StartCoroutine(Show, delay);

        public void HideWithDelay() => Invoke(nameof(Hide), hideDelay);

        public void HideWithDelay(float delay) => Invoke(nameof(Hide), delay);

        public void Show()
        {
            IsShown = true;
            visuals?.SetActive(true);

            DOTween.Complete("Hide" + gameObject.GetInstanceID());
            transform.DOScale(origSize, ShowDuration).SetEase(Ease.OutBounce).SetId("Show" + gameObject.GetInstanceID());
        }

        public void Hide()
        {
            IsShown = false;

            DOTween.Complete("Show" + gameObject.GetInstanceID());
            transform.DOScale(hiddenSize, HideDuration).SetEase(Ease.OutQuad).SetId("Hide" + gameObject.GetInstanceID())
                .onComplete += () => visuals?.SetActive(false);
        }

        public void ShowWithText(string t, float delay)
        {
            if (!text)
            {
                Debug.LogWarning("No text component is attached!!!");
                return;
            }

            text.text = t;
            ShowAfter(delay);
        }
    }
}