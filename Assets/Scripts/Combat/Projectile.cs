using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileVelocity = 20f;
        [SerializeField] bool isHoming;
        [SerializeField] ParticleSystem impactEffect = null;
        [SerializeField] float projectileLifetime = 10f;

        [SerializeField] UnityEvent onImpact;

        float damage;
        Transform target;
        private void OnTriggerEnter(Collider other)
        {
            onImpact.Invoke();
            if (other.GetComponent<Health>() != null)
            {
                other.GetComponent<Health>().TakeDamage(damage);
            }
            if (impactEffect != null)
            {
                Instantiate(impactEffect, other.bounds.center, transform.rotation);
            }
            Destroy(gameObject);
        }

        public void SetTarget(Transform target, float damage)
        {
            Destroy(gameObject, projectileLifetime);
            this.damage = damage;
            this.target = target;
            if (!isHoming)
            {
                transform.LookAt(GetTargetCenter(target));
                GetComponent<Rigidbody>().AddForce(transform.forward * projectileVelocity, ForceMode.VelocityChange);
            }
        }

        private void Update()
        {
            if (!isHoming || target == null) return;
            transform.LookAt(GetTargetCenter(target));
            transform.Translate(transform.forward * projectileVelocity * Time.deltaTime);
        }

        private Vector3 GetTargetCenter(Transform target)
        {
            return target.GetComponent<Collider>().bounds.center;
        }
    }
}