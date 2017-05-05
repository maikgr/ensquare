using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TileFloor : MonoBehaviour {
    private Stack<GameObject> battleCandidates = new Stack<GameObject>();
	private EventManager eventManager;

    //Initialization
	void Awake(){
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Player")){
			//Get the player's script
			PlayerController playerControl = other.GetComponent<PlayerController>();
			
			//If this tile is the player's last movement, and the player hasn't battled yet
			if (playerControl.movement <= 1 && playerControl.moved) {

				//Check for event tile, if there's any event tile, no need to find player
                //As the event tile will check the player after executing the event
                bool findPlayer = true;
				RaycastHit2D[] checkEvent = Physics2D.RaycastAll (transform.position, Vector2.zero);
				for (int i = 0; i < checkEvent.Length; i++){
					if (checkEvent[i].collider.gameObject.layer == LayerMask.NameToLayer("Event Layer")) {
                        findPlayer = false;
					}
				}

                //If there's no event tile found
                if (findPlayer)
                {
                    //Try to find all other players and add them to the stack
                    RaycastHit2D[] enemies = Physics2D.RaycastAll(transform.position, Vector2.zero);
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        if (enemies[i].collider.CompareTag("Player") && enemies[i].collider != other)
                        {
                            battleCandidates.Push(enemies[i].collider.gameObject);
                        }
                    }

                    //Start invasion battle if there's enemy
                    if (battleCandidates.Count > 0)
                        StartCoroutine(eventManager.InvasionBattle(other.gameObject, battleCandidates));

                    //End the player turn if there's none
                    else
                    {
                        playerControl.EndPlayerTurn();
                    }
                }
			}
		}
	}
}