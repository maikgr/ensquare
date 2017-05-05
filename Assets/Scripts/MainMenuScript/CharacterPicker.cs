using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterPicker : MonoBehaviour {
    public int spriteIndex;
    private GameSettings gameSettings;

	//Initialization
	void Start () {
        gameSettings = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<GameSettings>();
        GetComponent<Image>().sprite = gameSettings.spritesList[spriteIndex];
	}

    public void PickCharacter()
    {
        gameSettings.playerSprites.Add(spriteIndex);
        gameSettings.playerAvatar.Add(gameSettings.avatarList[spriteIndex]);
    }
}
