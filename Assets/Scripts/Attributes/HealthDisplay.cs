using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text text;
        void Awake()
        {
            health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = "HP: " + Mathf.RoundToInt(100 * health.GetPercentage()) + "%";
        }
    }
}