using UnityEngine;
using System.Collections;

public class Card_LowerDefense : MonoBehaviour {
    public int defenseDebuff = -1;
	private EventManager eventManager;

	void Start() {
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.playerControl.defenseBuff += defenseDebuff;
		print (eventManager.playerControl.gameObject.name + " defense has reduced by " + defenseDebuff);
	}
}