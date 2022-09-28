using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats baseStats;
        Text text;
        void Awake()
        {
            baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            text.text = "Level: " + baseStats.Level;
        }
    }
}