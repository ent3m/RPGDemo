using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "ScriptableObjects/Create Progression", order = 1)]
    public class Progression : ScriptableObject
    {
        [SerializeField] CharacterProgression[] progressions;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        [System.Serializable]
        class CharacterProgression
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] ProgressionStat[] stats;

            public CharacterClass CharacterClass
            {
                get => characterClass;
            }
            public ProgressionStat[] Stats
            {
                get => stats;
            }
        }
        [System.Serializable]
        class ProgressionStat
        {
            [SerializeField] Stat stat;
            [SerializeField] float[] levels;
            
            public Stat Stat
            {
                get => stat;
            }
            public float[] Levels
            {
                get => levels;
            }
        }

        public int GetMaxLevel(CharacterClass characterClass)
        {
            BuildLookupTableLinQ();
            return lookupTable[characterClass][Stat.ExperienceToLevelUp].Length;
        }

        public float GetStat(CharacterClass characterClass, Stat stat, int level)
        {
            BuildLookupTableLinQ();
            return lookupTable[characterClass][stat].ElementAtOrDefault(level - 1);
        }

        void BuildLookupTableLinQ()
        {
            if (lookupTable != null) return;
            lookupTable = progressions.ToDictionary(p => p.CharacterClass, p => p.Stats.ToDictionary(s => s.Stat, s => s.Levels));
        }

        //this method is equivalent to the LinQ implementation above, but is more complicated to write
        void BuildDictionaryTable()
        {
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (CharacterProgression progression in progressions)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat st in progression.Stats)
                {
                    statLookupTable[st.Stat] = st.Levels;
                }
                lookupTable[progression.CharacterClass] = statLookupTable;
            }
        }

        //attempt to substitute Dictionary<> with Lookup<>. not tested
        float BuildLookUp()
        {
            //building the lookup
            var lookup = progressions.ToLookup(p => p.CharacterClass, p => p.Stats.ToLookup(s => s.Stat, s => s.Levels));
            //retrieving the value
            foreach (var l in lookup[CharacterClass.Player])
            {
                foreach (var m in l[Stat.Health])
                {
                    return m[0];
                }
            }
            return 0;
        }
    }
}