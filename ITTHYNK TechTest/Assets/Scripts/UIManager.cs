using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [Header ("UI Visuals")]
    [SerializeField] CanvasGroup gameHudCG;

    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] List<Image> flagChecks = new List<Image> ();

    [SerializeField] CanvasGroup gameEndGC;

    [Header ("Settings")]
    [SerializeField] float timer;

    [SerializeField] float maxTime;

    [SerializeField] bool gameEnd = false;

    /// <summary>
    /// Awake function for assigning UI starting values
    /// </summary>
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
        for (int i = 0; i < flagChecks.Count; i++) {
            flagChecks[i].GetComponent<Image> ().color = Color.white;
        }
    }

    void OnEnable () {
        GameManager.GameEndEvent.AddListener (GameEndResponse);
        FlagTriggerManager.FlagTriggerEvent.AddListener (FlagTriggerResponse);
    }


    /// <summary>
    /// Fixed update for timers
    /// </summary>
    void FixedUpdate () {
        timerText.text = $"Time Left: {Math.Round (timer, 2).ToString ()}";
        if (timer > 0 && !gameEnd) {
            timer -= Time.fixedDeltaTime;
        }

        if (timer < 0 && !gameEnd) {
            gameEnd = true;
            CanvasEnable ();
        }
    }

    void GameEndResponse () {
        gameEnd = true;
        CanvasEnable ();
    }

    void CanvasEnable () {
        gameEndGC.alpha = 1;
        gameEndGC.blocksRaycasts = true;
        gameEndGC.interactable = true;
    }


    void FlagTriggerResponse (int arg0) {
        flagChecks[arg0].GetComponent<Image> ().color = Color.green;
    }
}