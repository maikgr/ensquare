    using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	public Vector2 attackerPos = new Vector2 (4f, 6f);
	public Vector2 defenderPos = new Vector2 (7f, 6f);
	public GameObject attackerDice, defenderDice;

	public List<GameObject> cardList;

    public GameObject getCard;

	[HideInInspector] public bool battleDone, checkFloor, fromFloor, delayEnd;
	[HideInInspector] public int lastCardIndex, attackerAttack, defenderDefense;
	[HideInInspector] public PlayerController playerControl;

	private GameObject attackerSprite, defenderSprite, attackerDiceTemp, defenderDiceTemp;
	private bool battleMode, invasionMode;

    private GameObject initiator;
    private CameraController cameraControl;

    void Start()
    {
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

	void Update(){
		if (battleMode){
			attackerAttack = attackerDiceTemp.GetComponent<RollDiceControl>().diceNumber;
			defenderDefense = defenderDiceTemp.GetComponent<RollDiceControl>().diceNumber;
			if (attackerAttack != 0 && defenderDefense != 0){
				battleMode = false;
                int bonusAttack = initiator.GetComponent<PlayerController>().attackBuff;
                int bonusDefense = playerControl.defenseBuff;
                int damage = Mathf.Max((attackerAttack + bonusAttack) - (defenderDefense + bonusDefense), 0);
				StartCoroutine(BattlePlayer(damage));
			}
		}
	}

	public void CheckOtherPlayer (GameObject player){
        //Initialize
        initiator = player;
        Stack<GameObject> otherPlayers = new Stack<GameObject>();

		//Raycast to check if there's other player on the same tile
		RaycastHit2D[] nearbyPlayers = Physics2D.RaycastAll (initiator.transform.position, Vector2.zero);
		
		//Add other players to enemy stack if there's any
		for (int i = 0; i < nearbyPlayers.Length; i++) {
            if (nearbyPlayers[i].collider.CompareTag("Player")
                && nearbyPlayers[i].collider.gameObject != initiator
                && nearbyPlayers[i].collider.GetComponent<PlayerController>().healthPoint > 0)

                otherPlayers.Push(nearbyPlayers[i].collider.gameObject);
		}

		//End turn if there's no player
        if (otherPlayers.Count == 0)
        {
            initiator.GetComponent<PlayerController>().EndPlayerTurn();
        }

        //Start invasion battle if there's any
        else if (otherPlayers.Count > 0)
        {
            StartCoroutine(InvasionBattle(initiator, otherPlayers));
        }
	}

	public IEnumerator WarpPlayer (GameObject player){
		//Define the initiator
		initiator = player;
		//Set playerControl to the warping player's
		playerControl = player.GetComponent<PlayerController>();

		//Warp in the player
		playerControl.animate.SetTrigger("warpingIn");
		yield return new WaitForSeconds(1f); //Duration of warp animation

		//Choose a random warp tile as destination
		GameObject[] warps = GameObject.FindGameObjectsWithTag("Warp Tile");
		int randIndex = Random.Range (0, warps.Length);
		while (warps[randIndex].transform.position == player.transform.position){
			randIndex = Random.Range (0, warps.Length);
		}
		
		//Warp the player to other warp tile
		player.transform.position = warps[randIndex].transform.position;
		playerControl.startPos = player.transform.position;
		
		//Warp out the player
		playerControl.animate.SetTrigger("warpingOut");
		yield return new WaitForSeconds(1f); //Duration of warp animation
		
		CheckOtherPlayer(player);
	}

	public IEnumerator BattlePlayer (int damage){
		playerControl.healthPoint -= damage;
        StartCoroutine(cameraControl.ShakeScreen());
		yield return new WaitForSeconds (1f); //Attack animation

        GameObject damageNumber = new GameObject("Damage Number");
        damageNumber.transform.SetParent(GameObject.Find("UICanvas").transform);
        damageNumber.transform.localPosition = new Vector2(100f, 100f);
        damageNumber.AddComponent<Text>().text = damage.ToString();
        damageNumber.GetComponent<Text>().fontSize = 40;
        damageNumber.GetComponent<Text>().font = Resources.Load<Font>("Fonts/Juice Bold");
        damageNumber.GetComponent<Text>().color = Color.red;

        yield return new WaitForSeconds(0.5f); //Damage number animation
        Destroy(damageNumber);
        yield return new WaitForSeconds(0.5f);

        //Destroy battle event UI
		Destroy(attackerSprite);
		Destroy(defenderSprite);
		Destroy(attackerDiceTemp);
		Destroy(defenderDiceTemp);     

        //Reset players bonus status
        initiator.GetComponent<PlayerController>().attackBuff = 0;
        playerControl.defenseBuff = 0;

        //Reset camera view
        cameraControl.battleCamera = false;

		//Will only be used for invasion battle
        battleDone = true;

        //Check for other player only if it's not invasion battle
        if (!invasionMode)
            CheckOtherPlayer(initiator);
	}
	
	public void SetBattleground(GameObject attacker, GameObject defender) {
		//Define initiator
		initiator = attacker;

		//Set playerControl to defender's
		playerControl = defender.GetComponent<PlayerController>();

		//Make attacker's sprite
		attackerSprite = new GameObject ("Attacker");
		attackerSprite.transform.SetParent(GameObject.Find("BoardCanvas").transform);
		attackerSprite.transform.position = attackerPos;
		attackerSprite.transform.eulerAngles = new Vector3 (0f, 0f);
		attackerSprite.AddComponent<Image>().sprite = attacker.GetComponentInChildren<SpriteRenderer>().sprite;
		attackerSprite.GetComponent<RectTransform>().sizeDelta = new Vector2 (2.4f, 2.4f);
		
		//Make defender's sprite
		defenderSprite = new GameObject ("Defender");
		defenderSprite.transform.SetParent(GameObject.Find("BoardCanvas").transform);
		defenderSprite.transform.position = defenderPos;
		defenderSprite.transform.eulerAngles = new Vector3 (0f, 180f);
		defenderSprite.AddComponent<Image>().sprite = defender.GetComponentInChildren<SpriteRenderer>().sprite;
		defenderSprite.GetComponent<RectTransform>().sizeDelta = new Vector2 (2.4f, 2.4f);
		
		//Instantiate battle UI
		attackerDiceTemp = Instantiate(attackerDice, new Vector2 (3f, 3f), Quaternion.identity) as GameObject;
		defenderDiceTemp = Instantiate(defenderDice, new Vector2 (7f, 3f), Quaternion.identity) as GameObject;
		
		//Set the dice to world space canvas
		attackerDiceTemp.transform.SetParent(GameObject.Find("BoardCanvas").transform);
		defenderDiceTemp.transform.SetParent(GameObject.Find("BoardCanvas").transform);

        //Set the camera to the battleground
        StartCoroutine(cameraControl.BattleView());

		battleMode = true;
	}

	public IEnumerator DrawCard (GameObject player) {
		//Initialization
		initiator = player;
        playerControl = player.GetComponent<PlayerController>();
        delayEnd = false;

		//Instantiate the card on top of the player
        yield return new WaitForSeconds(0.5f);
		GameObject cardIcon = Instantiate(getCard, Vector2.zero, Quaternion.identity) as GameObject;
        cardIcon.transform.SetParent(GameObject.Find("BoardCanvas").transform);
        cardIcon.transform.position = new Vector2 (player.transform.position.x, player.transform.position.y + 1.25f);
		Destroy(cardIcon, 1f);
		yield return new WaitForSeconds(0.5f);

        //Draw a random card from card list
        int randomIndex = Random.Range(0, cardList.Count);
        while (randomIndex == lastCardIndex)
             randomIndex = Random.Range(0, cardList.Count);
        GameObject chosenCard = Instantiate(cardList[randomIndex], Vector2.zero, Quaternion.identity) as GameObject;

        //Set the card to UI canvas
        chosenCard.transform.SetParent(GameObject.Find("UICanvas").transform);
        chosenCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.5f, 0.5f);

        //Destroy the card after 3 seconds
        Destroy(chosenCard, 3f);
		yield return new WaitForSeconds(3f);

        //Put the card index as the last card used
        lastCardIndex = randomIndex;

        //End turn if allowed
        if (!delayEnd)
            CheckOtherPlayer(player);
	}

    public IEnumerator InvasionBattle (GameObject player, Stack<GameObject> enemies)
    {
        while (enemies.Count > 0)
        {
            invasionMode = true;
            battleDone = false;
            SetBattleground(player, enemies.Pop());
            while (!battleDone)
            {
                yield return null;
            }
        }

        player.GetComponent<PlayerController>().EndPlayerTurn();
        invasionMode = false;
    }

    public GameObject FindRandomEnemy(GameObject player)
    {
        //Find all players
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Player");

        //Find a random living enemy
        int randomIndex = Random.Range(0, enemies.Length);
        while (enemies[randomIndex].GetComponent<PlayerController>().healthPoint <= 0 || enemies[randomIndex] == player)
        {
            randomIndex = Random.Range(0, enemies.Length);
        }
        //Return the random enemy
        return enemies[randomIndex];
    }

    public List<GameObject> FindAllEnemies(GameObject player)
    {
        //Find all players
        GameObject[] allplayers = GameObject.FindGameObjectsWithTag("Player");

        //Add all enemy to list
        List<GameObject> enemies = new List<GameObject>();
        foreach (GameObject anotherplayer in allplayers){
            if (anotherplayer != player || anotherplayer.GetComponent<PlayerController>().healthPoint > 0)
                enemies.Add(anotherplayer);
        }

        //Return enemy list
        return enemies;
    }
}