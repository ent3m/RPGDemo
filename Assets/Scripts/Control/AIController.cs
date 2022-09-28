using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5;
        [SerializeField] float suspicionTime = 3;
        [SerializeField] float aggrevateTime = 5;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] [Range(0, 1)] float patrolSpeedMultiplier = 0.3f;
        [SerializeField] float waypointTolerance = 1;
        [SerializeField] float waypointDwellTime = 3;
        [SerializeField] float alertDistance = 5;

        LazyValue<Vector3> guardPosition;
        int currentWaypointIndex = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrive = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        GameObject player;
        LazyValue<Transform> playerTransform;
        Fighter fighter;
        Health health;
        Mover mover;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            playerTransform = new LazyValue<Transform>(PlayerTransformInit);
            guardPosition = new LazyValue<Vector3>(GuardPositionInit);
        }

        private Transform PlayerTransformInit() => player.transform;
        private Vector3 GuardPositionInit() => transform.position;

        private void Start()
        {
            playerTransform.ForceInit();
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead)
            {
                return;
            }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            aggrevateTime += Time.deltaTime;
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private bool IsAggrevated()
        {
            return (Vector3.Distance(transform.position, playerTransform.value.position) < chaseDistance) || (timeSinceAggrevated < aggrevateTime);
        }

        private void AttackBehavior()
        {
            fighter.Attack(player.gameObject);
            timeSinceLastSawPlayer = 0;
            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, alertDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController enemy = null;
                if (hit.transform.TryGetComponent<AIController>(out enemy))
                {
                    enemy.Aggrevate();
                }
            }
        }

        private void SuspicionBehavior()
        {
            mover.StartMoveAction(transform.position);
        }
        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrive += Time.deltaTime;
                    if (timeSinceArrive > waypointDwellTime)
                    {
                        CycleWaypoint();
                        timeSinceArrive = 0;
                    }
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition, patrolSpeedMultiplier);
        }

        private bool AtWayPoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        //Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
