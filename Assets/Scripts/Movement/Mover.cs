using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        float defaultSpeed;

        NavMeshAgent navAgent;
        Animator animator;
        ActionScheduler actionScheduler;
        Health health;

        [SerializeField] float maxNavMeshPathLength = 30f;

        void Awake()
        {
            navAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();

            defaultSpeed = navAgent.speed;
        }

        void Update()
        {
            navAgent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            MoveTo(destination);
            actionScheduler.StartAction(this);
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            navAgent.speed = defaultSpeed * Mathf.Clamp01(speedFraction);
            navAgent.isStopped = false;
            navAgent.SetDestination(destination);
            actionScheduler.StartAction(this);
        }

        public void MoveTo(Vector3 destination)
        {
            navAgent.speed = defaultSpeed;
            navAgent.isStopped = false;
            navAgent.SetDestination(destination);
        }

        public void Cancel()
        {
            navAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }

        public bool DestinationReachable(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
            {
                if (CalculatePathLength(path) < maxNavMeshPathLength)
                {
                    return true;
                }
            }
            return false;
        }

        private float CalculatePathLength(NavMeshPath path)
        {
            float length = 0;
            Vector3[] points = path.corners;
            for (int i = 1; i < points.Length; i++)
            {
                length += Vector3.Distance(points[i], points[i - 1]);
            }
            return length;
        }

        public object CaptureState()
        {
            var data = new Dictionary<string, SerializableVector3>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            var data = (Dictionary<string, SerializableVector3>)state;

            navAgent.Warp(data["position"].ToVector());
            transform.eulerAngles = data["rotation"].ToVector();
        }
    }
}
