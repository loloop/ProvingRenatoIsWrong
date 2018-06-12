using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing.Utilities;
using Cinemachine;
using UnityStandardAssets.Utility;

namespace MC{
    [RequireComponent(typeof(PostProcessingController))]
	public class CameraEffects : MonoBehaviour {

        PostProcessingController controller;
        CinemachineBasicMultiChannelPerlin perlin;
        
        [SerializeField] Player player;
        [SerializeField] CinemachineFreeLook freeLook;
        [SerializeField] CinemachineVirtualCamera startCam;
        [SerializeField] CinemachineVirtualCamera bustedCam;

        public Vector3 monsterPosition;

        void Awake(){
            controller = GetComponent<PostProcessingController>();
            perlin = freeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        
        void OnValidate(){
            ComponentValidator.Validate(player, nameof(player));
            ComponentValidator.Validate(freeLook, nameof(freeLook));
            ComponentValidator.Validate(startCam, nameof(startCam));
            ComponentValidator.Validate(bustedCam, nameof(bustedCam));
        }

        void Start(){
            // setting base values
            controller.controlGrain = true;
            controller.enableGrain = true;
            player.OnRun += StartBounce;
            player.OnWalk += StopBounce;
            perlin.m_Definition.OrientationNoise[0] = new NoiseSettings.TransformNoiseParams(){
                X = new NoiseSettings.NoiseParams(){
                    Amplitude = 1.0f,
                    Frequency = 1.0f
                },
                Y = new NoiseSettings.NoiseParams(){
                    Amplitude = 1.0f,
                    Frequency = 1.0f
                },
                Z = new NoiseSettings.NoiseParams(){
                    Amplitude = 1.0f,
                    Frequency = 1.0f
                }
            };
        }

        void Update(){
            controller.grain.intensity = CalculateGrain();
        }

        public enum Position {
            startGame,
            gaming,
            busted
        }

        public void SetCamera(Position position){
            switch(position) {
                case Position.startGame:
                    startCam.enabled = true;
                    freeLook.enabled = false;
                    bustedCam.enabled = false;
                    break;
                case Position.gaming:
                    freeLook.enabled = true;
                    startCam.enabled = false;
                    bustedCam.enabled = false;
                    break;
                case Position.busted:
                    bustedCam.enabled = true;
                    startCam.enabled = false;
                    freeLook.enabled = false;
                    break;
                default:  
                    Debug.LogError("How the fuck did you get here?");
                    break;
            }
        }

        void StartBounce(){
            if(!player.isRunning) StartCoroutine(ChangeBounce(true));
        }

        void StopBounce(){
            if(player.isRunning) StartCoroutine(ChangeBounce(false));
        }

        IEnumerator ChangeBounce(bool running) {
            float time = 0.3f;
            var bounceUpCurve = AnimationCurve.EaseInOut(0, 5, time, 1);
            var bounceDownCurve = AnimationCurve.EaseInOut(0, 1, time, perlin.m_Definition.OrientationNoise[0].X.Amplitude);
            
            var frame = new WaitForEndOfFrame();
            while (time > 0){
                if(running){                    
                    var value = bounceUpCurve.Evaluate(time);
                    perlin.m_Definition.OrientationNoise[0].X.Amplitude = value;
                    perlin.m_Definition.OrientationNoise[0].Y.Amplitude = value;
                    perlin.m_Definition.OrientationNoise[0].Z.Amplitude = value;
                } else {
                    var value = bounceDownCurve.Evaluate(time);
                    perlin.m_Definition.OrientationNoise[0].X.Amplitude = value;
                    perlin.m_Definition.OrientationNoise[0].Y.Amplitude = value;
                    perlin.m_Definition.OrientationNoise[0].Z.Amplitude = value;
                }
                time -= Time.deltaTime;
                yield return frame;
            }
        }

        const float maxGrain = 1.0f;
        float CalculateGrain(){
            float currentGrain = Vector3.SqrMagnitude(player.transform.position - monsterPosition) * -0.01f;
            return maxGrain + currentGrain;
        }

	}
}