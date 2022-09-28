using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Attributes;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Fighter fighter;
        Health health;

        [SerializeField] CursorType defaultCursor;
        [SerializeField] CursorType movementCursor;
        public CursorType combatCursor;
        [SerializeField] CursorType uiCursor;
        public CursorType weaponPickupCursor;

        [SerializeField] float maxNavMeshProjectionDistance = 0.2f;

        void Awake()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }
            if (health.IsDead)
            {
                defaultCursor.SetCursor();
                return;
            }
            if (InteractWithComponent())
            {
                return;
            }
            if (InteractWithMovement())
            {
                return;
            }
            defaultCursor.SetCursor();
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                uiCursor.SetCursor();
                return true;
            }
            else
            {
                return false;
            }
        }
        
        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in RaycastSorted())
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        raycastable.HandleCursor(this);
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), 0.5f);
            var keys = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                keys[i] = hits[i].distance;
            }
            Array.Sort(keys, hits);
            Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            return hits;
            //return Physics.RaycastAll(GetMouseRay()).OrderBy(hit => hit.distance).ToArray();//use more memory and generate more garbage. need more profiling
        }

        private bool InteractWithMovement()
        {
            Vector3 target;

            if (RaycastNavmesh(out target))
            {
                if (Input.GetMouseButton(1))
                {
                    mover.StartMoveAction(target);
                }
                movementCursor.SetCursor();
                return true;
            }
            return false;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            RaycastHit rayHit;
            NavMeshHit navHit;
            target = Vector3.zero;
            if (Physics.Raycast(GetMouseRay(), out rayHit))
            {
                if (NavMesh.SamplePosition(rayHit.point, out navHit, maxNavMeshProjectionDistance, NavMesh.AllAreas))
                {
                    target = navHit.position;
                    if (mover.DestinationReachable(target))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}