using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card_CopyLastCard : MonoBehaviour {
    private EventManager eventManager;

	void Start () {
        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        StartCoroutine(CopyCard());
	}

    IEnumerator CopyCard()
    {
        GameObject lastCard = Instantiate(eventManager.cardList[eventManager.lastCardIndex], Vector2.zero, Quaternion.identity) as GameObject;
        lastCard.transform.localScale = Vector3.zero;
        lastCard.transform.SetParent(transform);
        print ("Last card event executed: " + eventManager.cardList[eventManager.lastCardIndex].name);
        yield return null;
    }
}
