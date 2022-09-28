using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System.IO;
using System.Text;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 0.2f;

        SavingSystem savingSystem;

        void Awake()
        {
            //StartCoroutine(LoadSceneOnStart());
        }

        IEnumerator LoadSceneOnStart()
        {
            savingSystem = GetComponent<SavingSystem>();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
        }

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    Save();
            //}
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    Load();
            //}
            //if (Input.GetKeyDown(KeyCode.Delete))
            //{
            //    Delete();
            //}
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        void Delete()
        {
            savingSystem.Delete(defaultSaveFile);
        }
    }
}