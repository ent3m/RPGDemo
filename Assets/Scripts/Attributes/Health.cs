using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] BaseStats baseStats;
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDeath;

        float maxHealth;
        LazyValue<float> health;

        bool isDead = false;
        public bool IsDead
        {
            get { return isDead; }
        }

        public object CaptureState()
        {
            return health;
        }

        private void Awake()
        {
            health = new LazyValue<float>(HealthInit);
        }
        private void Start()
        {
            health.ForceInit();
        }
        private float HealthInit()
        {
            maxHealth = baseStats.GetStat(Stat.Health);
            return maxHealth;
        }

        private void OnEnable()
        {
            baseStats.LeveledUp += OnLeveledUp;
        }

        private void OnDisable()
        {
            baseStats.LeveledUp -= OnLeveledUp;
        }

        private void OnLeveledUp(object sender, LeveledUpEventArgs e)
        {
            maxHealth = baseStats.GetStat(Stat.Health);
            health.value += e.healthGain;
        }

        public void RestoreState(object state)
        {
            health.value = (float)state;
            if (isDead == false && health.value == 0)
            {
                Die();
            }
        }
        public float GetPercentage()
        {
            return (health.value / maxHealth);
        }
        public void TakeDamage(float damage)
        {
            takeDamage.Invoke(damage);
            health.value = Mathf.Max(health.value - damage, 0);
            if (isDead == false && health.value == 0)
            {
                onDeath.Invoke();
                Die();
                if (GameObject.FindGameObjectWithTag("Player") is GameObject player)
                {
                    player.GetComponent<Experience>().GiveExp(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
                }
            }
        }
        void Die()
        {
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            gameObject.layer = 6;
            isDead = true;
        }
    }
}