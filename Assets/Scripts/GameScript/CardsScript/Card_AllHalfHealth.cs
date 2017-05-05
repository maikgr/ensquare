using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card_AllHalfHealth : MonoBehaviour {

	void Start() {
        //Initialize
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //Set all alive players to half of their current health
		for (int i = 0; i < players.Length; i++){
			int playerHealth = players[i].GetComponent<PlayerController>().healthPoint;
			if (playerHealth > 0){
				players[i].GetComponent<PlayerController>().healthPoint = Mathf.Max(1, playerHealth / 2);
			}
		}
		print ("All player's health has been reduced by half");
	}
}
