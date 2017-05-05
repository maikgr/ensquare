using UnityEngine;
using System.Collections;

public class Card_LowerAttack : MonoBehaviour {
    public int attackDebuff = -2;
	private EventManager eventManager;

	void Start() {
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
		eventManager.playerControl.attackBuff += attackDebuff;
		print (eventManager.playerControl.gameObject.name + " attack has reduced by " + attackDebuff);
	}
}