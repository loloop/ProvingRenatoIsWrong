using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace MC{
    public class GameUI: MonoBehaviour {

        public Button restartButton;
        public Button startButton;
        
        [SerializeField] Canvas endgameCanvas;

        void OnValidate(){
            ComponentValidator.Validate(restartButton, nameof(restartButton));
            ComponentValidator.Validate(startButton, nameof(startButton));
        }

        public void ShowEndgameCanvas(){
            var cg = endgameCanvas.GetComponent<CanvasGroup>();
            cg.interactable = true;
            cg.blocksRaycasts = true;
            cg.DOFade(1, 0.3f);
        }

    }
}