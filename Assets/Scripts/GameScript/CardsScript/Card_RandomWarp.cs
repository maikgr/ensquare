using UnityEngine;
using System.Collections;

public class Card_RandomWarp : MonoBehaviour {

	void Start () {
        EventManager eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        GameObject player = eventManager.playerControl.gameObject;
        eventManager.playerControl.warped = true;
        eventManager.delayEnd = true;
        StartCoroutine(eventManager.WarpPlayer(player));
        print("Warp " + eventManager.playerControl.gameObject.name + " to a random warp point");
	}
}
