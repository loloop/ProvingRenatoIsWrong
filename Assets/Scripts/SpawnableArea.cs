using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC {
	[RequireComponent(typeof(BoxCollider))]
	public class SpawnableArea : MonoBehaviour {

		BoxCollider m_collider;

		void Awake(){
			m_collider = GetComponent<BoxCollider>();
			m_collider.isTrigger = true;
		}

		public Vector3 GetRandomPositionInside(){
			var x = transform.position.x + Random.Range(-m_collider.size.x / 2, m_collider.size.x /2);
			var z = transform.position.z + Random.Range(-m_collider.size.z / 2, m_collider.size.z /2);
			return new Vector3(x, 1, z);
		}
	}
}