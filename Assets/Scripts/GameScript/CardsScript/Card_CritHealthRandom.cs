using UnityEngine;
using System.Collections;

public class Card_CritHealthRandom : MonoBehaviour {

    void Start()
    {   
        //Initialize
        EventManager eventManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
        GameObject enemy = eventManager.FindRandomEnemy(eventManager.playerControl.gameObject);

        //Set the returned random enemy health to 1
        enemy.GetComponent<PlayerController>().healthPoint = 1;
        
        print(enemy.name + " health has been reduced to 1");
    }
}
