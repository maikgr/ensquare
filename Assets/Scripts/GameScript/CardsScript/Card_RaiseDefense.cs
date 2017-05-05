using UnityEngine;
using System.Collections;

public class Card_RaiseDefense : MonoBehaviour {

    public int defenseBuff = 3;
    private EventManager eventManager;

    void Start()
    {
        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        eventManager.playerControl.defenseBuff += defenseBuff;
        print(eventManager.playerControl.gameObject.name + " defense has raised by " + defenseBuff);
    }
}
