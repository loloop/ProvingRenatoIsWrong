using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.SceneManagement;

namespace MC {
	public class GameManager : MonoBehaviour {

		// todo: game must have an end
		// todo: game must be restartable

		[SerializeField] CameraEffects effects;
		[SerializeField] MonsterSpawner spawner;
		[SerializeField] GameUI ui;

		HiddenMonster monster;

		public enum State {
			starting,
			running,
			ended
		}

        void Awake(){
            ui.restartButton.onClick.AddListener(RestartGame);
            ui.startButton.onClick.AddListener(StartGame);
        }

        void RestartGame(){
            SceneManager.LoadScene(0);
        }

		void StartGame(){
			Cursor.visible = false;
			state = State.running;
			effects.SetCamera(CameraEffects.Position.gaming);
		}

		public static State state { private set; get; }
		
		void OnValidate(){ 
			ComponentValidator.Validate(effects, nameof(effects));
			ComponentValidator.Validate(spawner, nameof(spawner));
			ComponentValidator.Validate(ui, nameof(ui));
		}

		void Start(){
			// Cursor.lockState = CursorLockMode.Confined;
			state = State.starting;
			// TODO: generate play area
			// TODO: set generated spawnable areas in monster spawner
			// tell monster spawner to spawn
			monster = spawner.SpawnMonster();
			// set spawned monster into camerafx
			if(monster!=null) effects.monsterPosition = monster.transform.position;
		}

		void Update(){
			if(state == State.running) {
				// check if monster is alive
				if (monster == null) {
					// display end ui
					ui.ShowEndgameCanvas();
					effects.SetCamera(CameraEffects.Position.busted);
					Cursor.visible = true;
					state = State.ended;
				}
			}
		}

	}
}