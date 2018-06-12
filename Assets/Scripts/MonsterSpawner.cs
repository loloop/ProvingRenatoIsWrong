using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MC{
	public class MonsterSpawner : MonoBehaviour {

		[SerializeField] HiddenMonster monsterPrefab;
		[SerializeField] List<SpawnableArea> spawnableAreas;

		void OnValidate(){
			ComponentValidator.Validate(monsterPrefab, nameof(monsterPrefab));
		}

		public void AddSpawnableArea(SpawnableArea newArea){
			spawnableAreas.Add(newArea);
		}

		public HiddenMonster SpawnMonster(){
			// check if there are any available spawnableAreas
			foreach(SpawnableArea area in spawnableAreas){
				if(area == null) {
					Debug.LogError("[MC.MonsterSpawner] No SpawnableArea found");
					return null;
				}
			}

			var monsterGo = GameObject.Instantiate(monsterPrefab);
			// adjust scale
			monsterGo.transform.localScale = Vector3.one;
			// set position inside a random spawnable area
			monsterGo.transform.position = spawnableAreas[Random.Range(0,spawnableAreas.Count)].GetRandomPositionInside();
			var monster = monsterGo.GetComponent<HiddenMonster>();
			return monster;
		}




	}
}