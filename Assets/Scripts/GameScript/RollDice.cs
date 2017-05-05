using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RollDice : MonoBehaviour {
	public Image rollDiceButton, diceDisplay;
	public Sprite rollDice, stopDice;
	public List<Sprite> diceImages;
	public float rollingSpeed;

	private int number;
	private bool rolling = false;
	private TurnsController player;

	private int cheat = 1;


	void Start() {
		//Initialization
		player = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>();
		diceDisplay.gameObject.SetActive(false);
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.Alpha1)) cheat = 1;
		else if (Input.GetKeyDown(KeyCode.Alpha2)) cheat = 2;
		else if (Input.GetKeyDown(KeyCode.Alpha3)) cheat = 3;
		else if (Input.GetKeyDown(KeyCode.Alpha4)) cheat = 4;
		else if (Input.GetKeyDown(KeyCode.Alpha5)) cheat = 5;
		else if (Input.GetKeyDown(KeyCode.Alpha6)) cheat = 6;
		else if (Input.GetKeyDown(KeyCode.Alpha7)) cheat = 12;
	}

	public void Roll (){
		//Switch State everytime Roll button is pressed
		rolling = !rolling;

		//Roll the dice when "Roll" is displayed
		if (rolling){
			rollDiceButton.sprite = stopDice;
			StartCoroutine(RollTheDice());
		}

		//Stop the dice and send the
		else if (!rolling){
            PlayerController thisPlayer = player.thisPlayerTurn.GetComponent<PlayerController>();
			diceDisplay.sprite = diceImages[Mathf.Clamp(cheat - 1, 0, 5)];

			//Hide the Roll button
			rollDiceButton.gameObject.SetActive(false);

			//Give the player's movement value based on the dice number
            thisPlayer.lastMove = cheat;
			thisPlayer.movement = cheat;

			//Reset player's states
			thisPlayer.warped = false;
			thisPlayer.battled = false;
		}
	}

	IEnumerator RollTheDice(){
		// Show the dice gameobject
		diceDisplay.gameObject.SetActive(true);

		//Keep rolling the dice until the Stop button is pressed
		while (rolling) {
			//Find random number

			int checker = number;
			while (number == checker){
				number = Random.Range(1,7);
			}


			//Display dice's number based on the random number
			diceDisplay.sprite = diceImages[number - 1];

			//Put the delay of changing between the images
			yield return new WaitForSeconds(rollingSpeed);
		}
	}
}
