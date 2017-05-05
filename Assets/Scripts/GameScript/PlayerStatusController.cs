using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerStatusController : MonoBehaviour {
    public Image[] playerAvatar = new Image[4];
    public Text[] playerHealth = new Text[4];

    [HideInInspector]
    public GameObject[] playerList;

    private GameSettings gameScript;
    private bool assignStatus;

	//Initialization
	void Start () {
        gameScript = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<GameSettings>();

        //Assign player's avatar
        for (int i = 0; i < playerAvatar.Length; i++)
        {
            playerAvatar[i].sprite = gameScript.playerAvatar[i];
        }
	}

    void Update()
    {
        //Keep updating all player's health throughout the game
        for (int i = 0; i < playerList.Length; i++)
        {
            PlayerController playerControl = playerList[i].GetComponent<PlayerController>();
            playerHealth[i].text = Mathf.Clamp(playerControl.healthPoint, 0, 99).ToString() + "/" + playerControl.maxHealthPoint.ToString();
        }
    }
}
