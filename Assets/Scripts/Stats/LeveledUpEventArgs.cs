using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class LeveledUpEventArgs : EventArgs
    {
        public int newLevel;
        public float healthGain;
    }
}