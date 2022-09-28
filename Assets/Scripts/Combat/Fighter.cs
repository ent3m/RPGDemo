using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] BaseStats baseStats = null;
        [SerializeField] AudioSource audioSource = null;

        Transform target;
        Health targetHealth;
        float timeSinceLastAttack = Mathf.Infinity;
        LazyValue<Weapon> currentWeapon;

        Mover mover;
        Animator animator;
        ActionScheduler actionScheduler;

        void Awake()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            currentWeapon = new LazyValue<Weapon>(CurrentWeaponInit);
        }
        private Weapon CurrentWeaponInit()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }
        void Start()
        {
            currentWeapon.ForceInit();
        }
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (targetHealth.IsDead)
            {
                target = null;
                return;
            }
            if (Vector3.Distance(transform.position, target.position) > currentWeapon.value.WeaponRange)
            {
                mover.MoveTo(target.position);
            }
            else
            {
                mover.Cancel();
                AttackBehavior();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            AttachWeapon(weapon);
            currentWeapon.value = weapon;
        }

        private void AttachWeapon(Weapon weapon)
        {
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        void AttackBehavior()
        {
            transform.LookAt(target);
            if (timeSinceLastAttack >= currentWeapon.value.TimeBetweenAttacks)
            {
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;
            audioSource.PlayOneShot(currentWeapon.value.WeaponImpactSound);
            targetHealth.TakeDamage(CalculateDamage());
        }

        private float CalculateDamage()
        {
            return (baseStats.GetStat(Stat.BaseDamage) + baseStats.GetAdditiveModifier(Stat.BaseDamage)) * (1 + baseStats.GetMultiplicativeModifier(Stat.BaseDamage));
        }

        //Animation Event
        void Shoot()
        {
            if (target == null) return;
            currentWeapon.value.SpawnProjectile(target, rightHandTransform, leftHandTransform);
        }

        public void Attack(GameObject combatTarget)
        {
            target = combatTarget.transform;
            targetHealth = combatTarget.GetComponent<Health>();
            actionScheduler.StartAction(this);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!mover.DestinationReachable(combatTarget.transform.position) && Vector3.Distance(transform.position, target.position) > currentWeapon.value.WeaponRange)
            {
                return false;
            };
            return !combatTarget.GetComponent<Health>().IsDead;
        }

        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            mover.Cancel();
            target = null;
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public int GetEnemyHealthPercentage()
        {
            if (targetHealth == null) return -1;
            return Mathf.RoundToInt(100 * targetHealth.GetPercentage());
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.BaseDamage)
            {
                yield return currentWeapon.value.WeaponDamage;
            }
            else
            {
                yield return 0;
            }
        }

        public IEnumerable<float> GetMultiplicativeModifier(Stat stat)
        {
            if (stat == Stat.BaseDamage)
            {
                yield return 0;
            }
            else
            {
                yield return 0;
            }
        }
    }
}