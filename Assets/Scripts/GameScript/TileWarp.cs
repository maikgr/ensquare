using UnityEngine;
using System.Collections;

public class TileWarp : MonoBehaviour {
	private PlayerController playerController;
	private EventManager eventManager;

	void Awake(){
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.tag == "Player"){ 
			//Get the PlayerController script from the player
			playerController = other.GetComponent<PlayerController>();

			//Only warp if the player has no movement left and has not warped yet
			if (playerController.movement == 0 && playerController.warped == false){
				//Set the player's warped state to true
				playerController.warped = true;

				//Warp the player
				StartCoroutine(eventManager.WarpPlayer(other.gameObject));
			}
		}
	}
}