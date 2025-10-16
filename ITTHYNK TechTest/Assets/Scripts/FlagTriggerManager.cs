using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagTriggerManager : MonoBehaviour {
    public static UnityEvent<int> FlagTriggerEvent = new UnityEvent<int> ();
    [SerializeField] int flagNumber;

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.GetComponent<PlayerController> () != null) {
            FlagTriggerEvent.Invoke (flagNumber);
            this.GetComponent<Collider> ().enabled = false;
        }
    }
}