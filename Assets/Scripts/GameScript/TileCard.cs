using UnityEngine;
using System.Collections;

public class TileCard : MonoBehaviour {
	private PlayerController playerControl;
	private EventManager eventManager;

	void Awake(){
		eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerControl = other.GetComponent<PlayerController>();
            if (playerControl.movement <= 0 && playerControl.moved && !playerControl.cardEvent)
            {
                playerControl.cardEvent = true;
                StartCoroutine(eventManager.DrawCard(other.gameObject));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerControl.cardEvent = false;
        }
    }
}
