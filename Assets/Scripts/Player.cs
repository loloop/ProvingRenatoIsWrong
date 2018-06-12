using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

namespace MC{
	public class Player : ThirdPersonUserControl {

        [SerializeField] Attack attack;

        public bool isRunning { private set; get; }        

        void OnValidate(){
            ComponentValidator.Validate(attack, nameof(attack));
        }

        void Awake(){
            
        }

        void Update(){
            if(GameManager.state != GameManager.State.running) {
                return;
            }

            if(CrossPlatformInputManager.GetButtonDown("Jump")){
                CastAttack();
            }
        }

        void CastAttack(){
            attack.Cast(Attack.Type.normal);
        }

        // chupinhado do thirdpersoncontrol porque ele é tosco
        void FixedUpdate(){
            if(GameManager.state != GameManager.State.running) {
                m_Character.Move(Vector3.zero, false, false);
                return;
            }

            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = (v*m_CamForward + h*m_Cam.right) * 0.6f;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }
#if !MOBILE_INPUT
			// run speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)){
                if(OnRun != null) OnRun();
                isRunning = true;
                m_Move *= 2f;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift)){
                if(OnWalk != null) OnWalk();
                isRunning = false;
            };
#endif

            // pass all parameters to the character control script
            m_Character.Move(move: m_Move, crouch: false, jump: false);
        }

        public event OnRunAction OnRun;
        public delegate void OnRunAction();

        public event OnWalkAction OnWalk;
        public delegate void OnWalkAction();
        
	}
}