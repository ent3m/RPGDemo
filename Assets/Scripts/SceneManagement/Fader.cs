using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentCoroutine = null;
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            return FadeRoutine(1, time);
        }
        public IEnumerator FadeIn(float time)
        {
            return FadeRoutine(0, time);
        }

        public void FadeOutImmmediate()
        {
            canvasGroup.alpha = 1;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(Fade(target, time));
            yield return currentCoroutine;
        }

        private IEnumerator Fade(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}