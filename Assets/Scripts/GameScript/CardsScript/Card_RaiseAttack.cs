using UnityEngine;
using System.Collections;

public class Card_RaiseAttack : MonoBehaviour {

    public int attackBuff = 1;
    private EventManager eventManager;

    void Start()
    {
        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        eventManager.playerControl.attackBuff += attackBuff;
        print(eventManager.playerControl.gameObject.name + " attack has raised by " + attackBuff);
    }
}
