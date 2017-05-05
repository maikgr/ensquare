using UnityEngine;
using System.Collections;

public class Card_HalfMaxHealth : MonoBehaviour {
    private EventManager eventManager;

    void Start()
    {
        eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        int newHealth = Mathf.Max(1, eventManager.playerControl.maxHealthPoint / 2);
        eventManager.playerControl.healthPoint = newHealth;
        print(eventManager.playerControl.gameObject.name + " health has been halved");
    }
}
