using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RollDiceControl : MonoBehaviour
{

    public Image rollDiceButton, diceDisplay;
    public Sprite rollDice, stopDice;
    public List<Sprite> diceImages;
    public float rollingSpeed;

    [HideInInspector]
    public int diceNumber;

    private int number;
    private bool rolling;

    void Start()
    {
        diceDisplay.gameObject.SetActive(false);
    }

    int Roll()
    {
        //Switch State everytime Roll button is pressed
        rolling = !rolling;

        //Roll the dice when "Roll" is displayed
        if (rolling)
        {
            rollDiceButton.sprite = stopDice;
            StartCoroutine(RollTheDice());
            return diceNumber = 0;
        }

        //Stop the dice when the button is pressed again
        else
        {
            //Hide the Roll button
            rollDiceButton.gameObject.SetActive(false);
            return diceNumber = number;
        }
    }

    IEnumerator RollTheDice()
    {
        // Show the dice gameobject
        diceDisplay.gameObject.SetActive(true);

        //Keep rolling the dice until the Stop button is pressed
        while (rolling)
        {
            //Find random number
            int checker = number;
            while (number == checker)
            {
                number = Random.Range(1, 7);
            }

            //Display dice's number based on the random number
            diceDisplay.sprite = diceImages[number - 1];

            //Put the delay of changing between the images
            yield return new WaitForSeconds(rollingSpeed);
        }
    }

    public void PlayerMovement()
    {
        if (Roll() != 0)
        {
            //Move current player's pawn based on dice number
            TurnsController turnsControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>();
            StartCoroutine(turnsControl.thisPlayerTurn.GetComponent<PlayerController>().MovePlayer(diceNumber));
        }
    }

    public void PlayerAttack()
    {
        if (Roll() != 0)
        {
            //Assign dice rolled number to attacker
            EventManager eventScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
            eventScript.attackerAttack = diceNumber;
        }
    }

    public void PlayerDefend()
    {
        if (Roll() != 0)
        {
            //Assign dice rolled number to defender
            EventManager eventScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>();
            eventScript.defenderDefense = diceNumber;
        }
    }
}
