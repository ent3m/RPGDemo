using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;
        [SerializeField] BaseStats stats;
        public float ExperiencePoints
        {
            get => experiencePoints;
        }

        public void GiveExp(float amount)
        {
            experiencePoints += amount;
            stats.TryLevelUp(experiencePoints);
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}