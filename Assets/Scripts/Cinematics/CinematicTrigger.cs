using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Saving;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        bool isTriggered = false;

        public object CaptureState()
        {
            return isTriggered;
        }

        public void RestoreState(object state)
        {
            isTriggered = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isTriggered && other.gameObject.tag == "Player")
            {
                GetComponent<PlayableDirector>().Play();
                isTriggered = true;
            }
        }
    }
}
