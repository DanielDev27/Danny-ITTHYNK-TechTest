using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static UnityEvent GameEndEvent = new UnityEvent ();
    public static UnityEvent GameStartEvent = new UnityEvent ();

    [SerializeField] List<GameObject> redFlags = new List<GameObject> ();
    [SerializeField] List<GameObject> greenFlags = new List<GameObject> ();

    int flagsComplete;

    void Awake () {
        instance = this;
    }

    // Start is called before the first frame update
    void Start () { }

    // Update is called once per frame
    void Update () { }

    void OnEnable () {
        FlagTriggerManager.FlagTriggerEvent.AddListener (FlagTriggerUpdate);
    }

    public void RestartGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
    }

    public void EndGame () {
        Debug.Log ($"End game");
        GameEndEvent.Invoke ();
    }

    public void StartGame () {
        GameStartEvent.Invoke ();
        for (int i = 0; i < redFlags.Count; i++) {
            redFlags[i].SetActive (true);
        }

        for (int i = 0; i < greenFlags.Count; i++) {
            greenFlags[i].SetActive (false);
        }
    }

    void FlagTriggerUpdate (int arg0) {
        flagsComplete = 0;
        redFlags[arg0].SetActive (false);
        greenFlags[arg0].SetActive (true);
        for (int i = 0; i < redFlags.Count; i++) {
            if (!redFlags[i].activeInHierarchy) {
                flagsComplete++;
            }
        }

        if (flagsComplete == redFlags.Count) {
            EndGame ();
        }
    }
}