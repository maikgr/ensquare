using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	//Change to scene name when clicked
	public void ToScene (string sceneName){
		Application.LoadLevel(sceneName);
	}

    //Change to a specified screen by activating it
    public void ToScreen(GameObject screen)
    {
        //Set target and all its childs to active
        screen.SetActive(true);
        for (int i = 0; i < screen.transform.childCount; i++)
            screen.transform.GetChild(i).gameObject.SetActive(true);
    }

    //And deactivate the current screen
    public void FromScreen (GameObject currentScreen)
    {
        currentScreen.SetActive(false);
    }

    //Quit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
