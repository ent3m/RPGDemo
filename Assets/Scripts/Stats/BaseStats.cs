using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour, ISaveable
    {
        [Range(1, 10)]
        [SerializeField] int level = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] ParticleSystem levelupEffect;

        public int Level
        {
            get => level;
        }

        public event EventHandler<LeveledUpEventArgs> LeveledUp;

        private void OnLeveledUp()
        {
            LeveledUp?.Invoke(this, new LeveledUpEventArgs { newLevel = level, healthGain = GetStat(Stat.Health) - progression.GetStat(characterClass, Stat.Health, level - 1) });
            Instantiate(levelupEffect, this.transform.position, Quaternion.identity);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(characterClass, stat, level);
        }

        public float GetAdditiveModifier(Stat stat)
        {
            float sum = 0;
            foreach (var source in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in source.GetAdditiveModifier(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        public float GetMultiplicativeModifier(Stat stat)
        {
            float sum = 0;
            foreach (var source in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in source.GetMultiplicativeModifier(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        public void TryLevelUp(float exp)
        {
            if (level >= progression.GetMaxLevel(characterClass)) return;
            if (exp >= progression.GetStat(characterClass, Stat.ExperienceToLevelUp, level + 1))
            {
                level++;
                OnLeveledUp();
                TryLevelUp(exp);
            }
        }

        public object CaptureState()
        {
            return level;
        }

        public void RestoreState(object state)
        {
            level = (int)state;
        }
    }
}