using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {
    public List<GameObject> pickArrow;
	public List<Button> selectChars;
    public Button goButton;
    public Sprite goButtonEnabled, goButtonDisabled;

    private GameObject pointer;
    private int turn = 0;
    private bool disabled, instantiated;
    private Vector3 pointerPos, pointerAngle;

    void Start()
    {
        goButton.enabled = false;
        goButton.GetComponent<Image>().sprite = goButtonDisabled;
    }

    public void OnCharacterHover(GameObject selectedChar)
    {
        if (!instantiated && turn < 4)
        {
            pointerTransform();
            pointer = Instantiate(pickArrow[turn], selectedChar.transform.position + pointerPos, Quaternion.Euler(pointerAngle)) as GameObject;
            pointer.transform.SetParent(selectedChar.transform);
            instantiated = true;
        }
    }

    public void OnCharacterExit()
    {
        if (pointer != null && turn < 4)
        {
            Destroy(pointer);
            instantiated = false;
        }
    }

	//If the character button is clicked, place the arrow on that button
	public void ClickCharacter (Button selectedChar) {
        pointerTransform();
        GameObject placeArrow = Instantiate(pickArrow[turn], selectedChar.transform.position + pointerPos, Quaternion.Euler(pointerAngle)) as GameObject;
        placeArrow.transform.SetParent(selectedChar.transform);
        turn++;
        if (turn == 4)
            PlayersReady();
	}

    void PlayersReady()
    {
        //Disable all button
        for (int i = 0; i < selectChars.Count; i++)
        {
            selectChars[i].enabled = false;
        }

        //Enable go button
        goButton.enabled = true;
        goButton.GetComponent<Image>().sprite = goButtonEnabled;
    }

    void pointerTransform()
    {
        //Assign new position and angle based on player's turn
        switch (turn)
        {
            default:
                pointerPos = new Vector3(-45f, 80f);
                pointerAngle = new Vector3(0f, 0f, 25f);
                break;
            case 1:
                pointerPos = new Vector3(-15f, 90f);
                pointerAngle = new Vector3(0f, 0f, 15f);
                break;
            case 2:
                pointerPos = new Vector3(15f, 90f);
                pointerAngle = new Vector3(0f, 0f, -15f);
                break;
            case 3:
                pointerPos = new Vector3(45f, 80f);
                pointerAngle = new Vector3(0f, 0f, -25f);
                break;
        }

    }
}