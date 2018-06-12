using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC{
    [RequireComponent(typeof(BoxCollider))]
    public class Attack : MonoBehaviour, IDamage{

        public enum Type {
            normal
        }

        public float amount => 10.0f;
        float attackDuration = 1f;

        List<ParticleSystem> particles = new List<ParticleSystem>();
        BoxCollider myCollider;

        void Awake(){
            myCollider = GetComponent<BoxCollider>();
            var child = transform.GetChild(0);
            particles.Add(child.GetComponent<ParticleSystem>());
            particles.AddRange(child.GetComponentsInChildren<ParticleSystem>());
            myCollider.isTrigger = true;
            myCollider.enabled = false;
        }

        public void Cast(Type type){
            StartCoroutine(CastAttack());
        }

        IEnumerator CastAttack(){
            particles.ForEach((particle) => particle.Emit(1));
            var time = new WaitForSeconds(attackDuration);
            // enable trigger
            myCollider.enabled = true;
            yield return time;
            // disable trigger
            myCollider.enabled = false;
        }
    }
}