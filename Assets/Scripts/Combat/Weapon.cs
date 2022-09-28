using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Create Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponRange = 3;
        [SerializeField] float timeBetweenAttacks = 1;
        [SerializeField] float weaponDamage = 10;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectilePrefab = null;
        [SerializeField] AudioClip weaponImpactSound = null;

        const string weaponName = "Weapon";

        public float WeaponRange
        {
            get { return weaponRange; }
        }

        public float TimeBetweenAttacks
        {
            get { return timeBetweenAttacks; }
        }

        public float WeaponDamage
        {
            get { return weaponDamage; }
        }

        public AudioClip WeaponImpactSound
        {
            get => weaponImpactSound;
        }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                if (isRightHanded)
                {
                    var weapon = Instantiate(equippedPrefab, rightHand);
                    weapon.name = weaponName;
                }
                else
                {
                    var weapon = Instantiate(equippedPrefab, leftHand);
                    weapon.name = weaponName;
                }
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;
            oldWeapon.name = "Destroying";      //safety measure to prevent a bug from destroying new weapon with the same name 
            Destroy(oldWeapon.gameObject);
        }

        public void SpawnProjectile(Transform target, Transform rightHand, Transform leftHand)
        {
            var projectile = Instantiate(projectilePrefab, leftHand.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(target, weaponDamage);
        }
    }
}