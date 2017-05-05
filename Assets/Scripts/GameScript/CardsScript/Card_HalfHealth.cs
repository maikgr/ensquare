using UnityEngine;
using System.Collections;

public class Card_HalfHealth : MonoBehaviour {
	private EventManager eventManager;

	void Start() {
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.playerControl.healthPoint = Mathf.Max (1, eventManager.playerControl.healthPoint / 2);
		print (eventManager.playerControl.gameObject.name + " health point reduced by half");
	}
}
