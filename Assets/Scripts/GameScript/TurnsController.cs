using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TurnsController: MonoBehaviour {

	public GameObject diceRoll;
	public float diceOffset = 1.5f;
	public float turnChangeDelay = 0.01f;
    public GameObject victoryScreen, endScreen;

    public GameObject statusMonitor;

	[HideInInspector] public GameObject thisPlayerTurn = null, winner;
	[HideInInspector] public bool cardPhase, movePhase, battlePhase, endPhase, gameEnd;
    [HideInInspector] public List<GameObject> deadPlayers = new List<GameObject>();

	private GameObject[] players = new GameObject[4];
	private GameObject diceObject = null;
	private Vector2 diceOnTop;
    private int turnNumber;
    private bool victoryPhase, ending;
    private float screenFadeTime = 200;

	void Start () {
		//Find all players
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Player");

		//Assign player's turn based on index
		for (int i = 0; i < temp.Length; i++){
			Vector3 tempPos = temp[i].transform.position;
			if (tempPos == new Vector3(0f, 0f, 0f)) {
				players[0] = temp[i];
			}
			else if (tempPos == new Vector3 (11f,11f,0f)){
				players[1] = temp[i];
			}
			else if (tempPos == new Vector3 (11f, 0, 0f)) {
				players[2] = temp[i];
			}
			else if (tempPos == new Vector3 (0, 11f, 0f)){
				players[3] = temp[i];
			}
		}

        //Send arranged list to status control
        PlayerStatusController statusControl = statusMonitor.GetComponent<PlayerStatusController>();
        statusControl.playerList = players;

		//Start moving the first player
		StartCoroutine(RollDicePhase());
	}

	void Update () {
		if (!gameEnd){
			if (endPhase)
            {
				//Set current player's turn to inactive
				thisPlayerTurn.GetComponent<PlayerController>().animate.SetBool("playerActive", false);
				thisPlayerTurn.GetComponent<PlayerController>().enabled = false;
				endPhase = false;
				
				//Move to next player's index, reset if it has reached player 4
				turnNumber++;
				if (turnNumber > (players.Length-1)) turnNumber = 0;

                //Skip a player with 0 health point's turn
                while (players[turnNumber].GetComponent<PlayerController>().healthPoint <= 0)
                {
                    bool registered = false;
                    for (int i = 0; i < deadPlayers.Count; i++)
                    {
                        if (players[turnNumber] == deadPlayers[i])
                        {
                            registered = true;
                        }
                    }
                    if (!registered)
                    {
                        deadPlayers.Add(players[turnNumber]);
                        StartCoroutine(PlayerDeath(players[turnNumber]));
                    }
                    turnNumber++;
                    if (turnNumber > (players.Length - 1)) turnNumber = 0;
                }

                //Activate the next player
				StartCoroutine(RollDicePhase());
			}

            //End the game if there are 3 dead players
            else if (deadPlayers.Count == 3)
            {
                gameEnd = true;

                //Find the winner
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<PlayerController>().healthPoint > 0)
                        winner = players[i];
                }
            }
		}

        else if (gameEnd)
        {
            if (!victoryPhase)
            {
                //Set victory screen to active
                victoryScreen.SetActive(true);

                //Set victory phase to true
                victoryPhase = true;
            }

            if (Input.GetMouseButtonDown(0) && !ending)
            {
                ending = true;
                StartCoroutine(BlackScreen());
            }
        }
	}

	void LateUpdate(){
        if (diceObject != null)
        {
            diceOnTop = new Vector2(thisPlayerTurn.transform.position.x, thisPlayerTurn.transform.position.y + diceOffset);
            diceObject.transform.position = diceOnTop;
        }
	}

    public void DiceExtra()
    {
        //Instantiate dice for extra movement
        diceOnTop = new Vector2(thisPlayerTurn.transform.position.x, thisPlayerTurn.transform.position.y + diceOffset);
        diceObject = Instantiate(diceRoll, diceOnTop, Quaternion.identity) as GameObject;
        diceObject.transform.SetParent(GameObject.Find("BoardCanvas").transform, false);
        diceObject.transform.FindChild("Roll Button").gameObject.SetActive(false);
    }

    public void DiceNumberDisplay(int diceNumber)
    {
        diceObject.GetComponent<RollDiceControl>().diceDisplay.sprite =
            diceObject.GetComponent<RollDiceControl>().diceImages[Mathf.Clamp((diceNumber - 1), 0, 5)];
    }

    public void DiceEnd()
    {
        Destroy(diceObject);
        diceObject = null;
    }

    IEnumerator RollDicePhase()
    {
        //Wait for a delay before changing the player
        yield return new WaitForSeconds(turnChangeDelay);

        //Activate current player
        thisPlayerTurn = players[turnNumber];
        thisPlayerTurn.GetComponent<PlayerController>().enabled = true;
        thisPlayerTurn.GetComponent<PlayerController>().animate.SetBool("playerActive", true);

        //Instantiate dice interface
        diceOnTop = new Vector2(thisPlayerTurn.transform.position.x, thisPlayerTurn.transform.position.y + diceOffset);
        diceObject = Instantiate(diceRoll, diceOnTop, Quaternion.identity) as GameObject;
        diceObject.transform.SetParent(GameObject.Find("BoardCanvas").transform, false);
    }

    public IEnumerator PlayerDeath(GameObject player)
    {
        //Initialization
        SpriteRenderer spriteColor = player.GetComponentInChildren<SpriteRenderer>();
        float lerpTime = 0;

        //make the player's sprite covered with grey shade over time
        while (spriteColor.color != new Color(0.098f, 0.098f, 0.098f))
        {
            Color startColor = spriteColor.color;
            Color endColor = new Color(0.098f, 0.098f, 0.098f);
            if (lerpTime == 0)
                lerpTime = Time.time + 2f; //Set the color gradual change to 2 seconds
            spriteColor.color = Color.Lerp(startColor, endColor, Time.time / lerpTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator BlackScreen()
    {
        //Activate end screen
        endScreen.SetActive(true);

        //Get and set the Image component
        Image blackLayer = endScreen.GetComponent<Image>();
        blackLayer.color = Color.clear;

        //Start to fade in the screen to black for 200 frames
        int i = 0;
        while (blackLayer.color != Color.black && i < screenFadeTime)
        {
            blackLayer.color = Color.Lerp(blackLayer.color, Color.black, i / screenFadeTime);
            i++;
            yield return new WaitForEndOfFrame();
        }

        //End the game
        while (blackLayer.color != Color.black)
            yield return null;

        Application.Quit();
    }
}