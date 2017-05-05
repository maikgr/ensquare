using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileBattle : MonoBehaviour {

	private bool active;
	private EventManager eventManager;

	void Awake(){
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag("Player")){
			//Get the player's script
			PlayerController playerControl = other.GetComponent<PlayerController>();

			//If this tile is the player's last movement, and the player hasn't battled yet
			if (playerControl.movement == 0 && playerControl.battled == false) {
				playerControl.battled = true;

				//Find other random player to battle with
                GameObject enemy = eventManager.FindRandomEnemy(other.gameObject);

				//Initiate battle
				eventManager.SetBattleground(other.gameObject, enemy);
			}
		}
	}
}