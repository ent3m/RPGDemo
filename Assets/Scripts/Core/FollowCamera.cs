using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        Transform target;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;   
        }
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
