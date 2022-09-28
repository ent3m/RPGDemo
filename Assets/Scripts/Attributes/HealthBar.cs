using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform bar = null;

        private void Update()
        {
            if (health.IsDead)
            {
                Destroy(gameObject, 0.1f);
                return;
            }
            if (Mathf.Approximately(health.GetPercentage(), 1))
            {
                bar.localScale = new Vector3(0, 1, 1);
                return;
            }
            bar.localScale = new Vector3(health.GetPercentage(), 1, 1);
        }
    }
}