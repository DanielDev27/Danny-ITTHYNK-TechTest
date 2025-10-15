using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header ("UI Visuals")]
    [SerializeField] CanvasGroup gameHudCG;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] CanvasGroup gameEndGC;

    [Header ("Settings")]
    [SerializeField] float timer;

    [SerializeField] float maxTime;

    [SerializeField] bool gameEnd = false;

    void Awake () {
        gameHudCG.alpha = 1;
        gameHudCG.blocksRaycasts = true;
        gameHudCG.interactable = true;
        gameEndGC.alpha = 0;
        gameEndGC.blocksRaycasts = false;
        gameEndGC.interactable = false;

        timer = maxTime;
        timerText.text = $"Time Left: {Math.Round (timer, 2).ToString ()}";

        gameEnd = false;
    }

    void FixedUpdate () {
        timerText.text = $"Time Left: {Math.Round (timer, 2).ToString ()}";
        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
        }

        if (timer < 0 && !gameEnd) {
            gameEnd = true;
            gameEndGC.alpha = 1;
            gameEndGC.blocksRaycasts = true;
            gameEndGC.interactable = true;
        }
    }
}