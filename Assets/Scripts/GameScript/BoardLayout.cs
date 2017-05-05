using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardLayout : MonoBehaviour {

	public GameObject player, cardTile, warpTile, battleTile;
	public List<Sprite> characterSprites;
	public GameObject mainCamera;

	[HideInInspector] public List<Vector2> playerPosition;

	private List<Vector2> gridPosition = new List<Vector2>();
	private Transform boardLayout;
	private int errorTries = 5; //Adjacent tile error check amount
	private GameSettings gameScript;

	void Awake () {
        //Deactivate TurnsController before placing players
        GetComponent<TurnsController>().enabled = false;

		//Initialization
		playerPosition = new List<Vector2> {
			new Vector2(0,0),
			new Vector2(11f,11f),
			new Vector2(11f,0),
			new Vector2(0,11f)
		};
		boardLayout = new GameObject("Board Tiles Layout").transform;
		gameScript = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<GameSettings>();
        
		//Setting up the board
        SetGrids();
        LayoutTileAtRandom(cardTile, gameScript.maxCard);
        LayoutTileAtRandom(warpTile, gameScript.maxWarp);
        LayoutTileAtRandom(battleTile, gameScript.maxBattle);

		PlacePlayers();

		//Activate TurnsController
        GetComponent<TurnsController>().enabled = true;
		if (mainCamera.activeSelf == false)
            mainCamera.gameObject.SetActive(true);
	}

	//Initialize grid coordinates
	void SetGrids (){
		GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor Tile");
		for (int i = 0; i < floors.Length; i++){
			bool clash = false;
			Vector2 floorPos = new Vector2 (floors[i].transform.position.x, floors[i].transform.position.y);
			for (int j = 0; j < playerPosition.Count; j++){
				if (floorPos == playerPosition[j]) clash = true;
			}
			if (clash == false)	gridPosition.Add(floorPos);
		}
	}

	//Pick a random position from the grid
	Vector2 RandomPosition(GameObject tiles){
		int i = 0;
		int randomIndex = Random.Range (0, gridPosition.Count);
		Vector2 randomPos = gridPosition[randomIndex];
        
		//Pick another random position until there's no clash or error tries limit reached
		while (!AdjacentTileClash(tiles, randomPos)) {
			randomIndex = Random.Range (0, gridPosition.Count);
			randomPos = gridPosition[randomIndex];
			i++;
            if (i >= errorTries)
                break;
		}
        
		//Remove the picked position so it won't be picked positon
		gridPosition.RemoveAt(randomIndex);

		///Return the picked position
		return randomPos;
	}

	//Check if there's any adjacent tiles with the same tag
	bool AdjacentTileClash (GameObject tiles, Vector2 randomPos){

		//Raycast on the randomPos
        RaycastHit2D[] hits = Physics2D.CircleCastAll(randomPos, 1f, Vector2.zero, 0f, LayerMask.NameToLayer("Event Layer"));

        //If there's any nearby objects with the same tag, return false
        for (int i = 0; i < hits.Length; i++)
            if (hits[i].collider.CompareTag(tiles.tag))
                return false;

        //Return true if there's no gameobject with the same tag
        return true;
	}

	//Layout the tiles at random position on the grid
	void LayoutTileAtRandom(GameObject tiles, int tileCount){
		for (int i = 0; i < tileCount; i++){
			GameObject placeTile = Instantiate (tiles, RandomPosition(tiles), Quaternion.identity) as GameObject;
			placeTile.transform.SetParent(boardLayout);
		}
	}

	//Instantiate the players
	void PlacePlayers(){
		//Set a gameobject to hold players
		GameObject players = new GameObject("Players");

		//Instantiate players character based on total players
		for (int i = 0; i < gameScript.playerSprites.Count; i++){
			GameObject createPlayer = Instantiate(player, playerPosition[i], Quaternion.identity) as GameObject;

			//Set the instantiated object to a parent
			createPlayer.transform.SetParent(players.transform);

			//Find the SpriteRenderer component of the children of the instantiated object
			SpriteRenderer setPlayerSprite = createPlayer.gameObject.GetComponentInChildren<SpriteRenderer>();

			//Add the selected sprite to the gamobject
			setPlayerSprite.sprite = characterSprites[gameScript.playerSprites[i]];
		}
	}
}