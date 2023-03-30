using System.Collections;
using UnityEngine;

namespace NoGround.UI
{
    public class Screen : MonoBehaviour
    {
        public delegate void ShowComplete();
        public ShowComplete OnShowComplete;
        public delegate void HideComplete();
        public HideComplete OnHideComplete;

        protected virtual void Awake()
        {
        }

        public void Show()
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowScreenCoroutine());
        }

        private IEnumerator ShowScreenCoroutine()
        {
            yield return StartCoroutine(ShowCoroutine());
            OnShowComplete?.Invoke();
        }

        protected virtual IEnumerator ShowCoroutine()
        {
            yield return null;
        }

        public void Hide()
        {
            StartCoroutine(HideScreenCoroutine());
        }

        private IEnumerator HideScreenCoroutine()
        {
            yield return StartCoroutine(HideCoroutine());
            OnHideComplete?.Invoke();
            gameObject.SetActive(false);
        }

        protected virtual IEnumerator HideCoroutine()
        {
            yield return null;
        }
    }
}