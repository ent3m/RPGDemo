using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience exp;
        Text text;
        void Awake()
        {
            exp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = "XP: " + exp.ExperiencePoints;
        }
    }
}