using UnityEngine;
using System.Collections;

public class Card_RecoverHealth : MonoBehaviour {
    public int healthBonus = 5;
    private EventManager eventManager;

    void Start()
    {
        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        eventManager.playerControl.healthPoint += healthBonus;
        print(eventManager.playerControl.gameObject.name + " health has been recovered by " + healthBonus);
    }
}
