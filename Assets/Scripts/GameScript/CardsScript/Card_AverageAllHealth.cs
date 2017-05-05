using UnityEngine;
using System.Collections;

public class Card_AverageAllHealth : MonoBehaviour {
    private GameObject[] players;
    int totalPlayer, totalHealth = 0;

    void Start()
    {
        //Initialize
        players = GameObject.FindGameObjectsWithTag("Player");

        //Get the total health of all alive players
        for (int i = 0; i < players.Length; i++)
        {
            int playerHealth = players[i].GetComponent<PlayerController>().healthPoint;
            if (playerHealth > 0){
                totalHealth += playerHealth;
                totalPlayer ++;
            }
        }

        //Set each players health to the average of totalHealth
        for (int j = 0; j < players.Length; j++)
        {
            if (players[j].GetComponent<PlayerController>().healthPoint > 0)
            {
                players[j].GetComponent<PlayerController>().healthPoint = Mathf.Max(1, totalHealth / totalPlayer);
            }
        }
        print("All player's health has been set to average");
    }
}
