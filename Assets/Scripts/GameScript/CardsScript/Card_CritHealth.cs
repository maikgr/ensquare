using UnityEngine;
using System.Collections;

public class Card_CritHealth : MonoBehaviour {
	private EventManager eventManager;
	
	void Start() {
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.playerControl.healthPoint = 1;
		print (eventManager.playerControl.gameObject.name + " health is at critical point");
	}
}
