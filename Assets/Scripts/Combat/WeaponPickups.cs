using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Movement;

namespace RPG.Combat
{
    public class WeaponPickups : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButton(1))
            {
                player.GetComponent<Mover>().StartMoveAction(transform.position);
            }
            return true;
        }
        public void HandleCursor(PlayerController player)
        {
            player.weaponPickupCursor.SetCursor();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}