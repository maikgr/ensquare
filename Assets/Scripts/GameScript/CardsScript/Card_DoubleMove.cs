using UnityEngine;
using System.Collections;

public class Card_DoubleMove : MonoBehaviour {

    void Start()
    {
        EventManager eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>().DiceExtra();
        StartCoroutine(eventManager.playerControl.MovePlayer(eventManager.playerControl.lastMove));
        eventManager.delayEnd = true;
        print(eventManager.playerControl.gameObject.name + " got " + eventManager.playerControl.lastMove + " more movement");
    }
}