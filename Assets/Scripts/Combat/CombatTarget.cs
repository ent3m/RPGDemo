using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        [SerializeField] CursorType cursor;
        public CursorType Cursor { get => cursor; }
        public bool HandleRaycast(PlayerController player)
        {
            if (!player.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                player.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }
        public void HandleCursor(PlayerController player)
        {
            player.combatCursor.SetCursor();
        }
    }
}