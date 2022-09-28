using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        Text text;
        int enemyHealth = 0;
        void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            text = GetComponent<Text>();
        }

        void Update()
        {
            enemyHealth = fighter.GetEnemyHealthPercentage();
            if (enemyHealth == -1)
            {
                text.text = "HP: N/A";
                return;
            }
            text.text = "HP: " + enemyHealth + "%";
        }
    }
}