using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoadIndex = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeTime = 1.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (sceneToLoadIndex == -1)
                {
                    Debug.LogError("Scene to load not set.");
                    return;
                }
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savewrap = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(fadeTime);
            //savewrap.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoadIndex);

            //savewrap.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            //savewrap.Save();

            yield return new WaitForSeconds(fadeTime);
            yield return fader.FadeIn(fadeTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal.destination == this.destination && portal != this)
                {
                    return portal;
                }
            }
            return null;
        }

        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
    }
}
