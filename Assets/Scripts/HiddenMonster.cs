using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC{
	[RequireComponent(typeof(BoxCollider))]
	public class HiddenMonster : MonoBehaviour {

		public GameObject model;

		public bool hidden {private set; get;}

		void Awake(){
			GetComponent<BoxCollider>().isTrigger = true;
			hidden = true;
		}

		void OnValidate(){
			ComponentValidator.Validate(model, nameof(model));
		}

		void OnTriggerEnter(Collider col) {
			var obj = col.gameObject.GetComponent<Player>();
			if(obj != null) {
				Unhide();
			}
			var damage = col.gameObject.GetComponent<IDamage>();
			if(damage != null && !hidden) {
				Die();
			}
		}

		void Unhide(){
			Debug.Log("oh no player found me");
			model.SetActive(true);
			hidden = false;
		}

		void Die(){
			Debug.Log("oh no player kill me");
			if(OnDeath != null) OnDeath();
			GameObject.Destroy(this.gameObject);
		}

		public delegate void OnDeathAction();
		public event OnDeathAction OnDeath;

	}
}