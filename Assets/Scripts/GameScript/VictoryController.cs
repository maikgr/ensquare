using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VictoryController : MonoBehaviour {
    public GameObject winnerSprite, victoryBox, spotlight;
    private TurnsController turnsControl;

    void Start()
    {
        victoryBox.SetActive(false);
        spotlight.SetActive(false);
        turnsControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<TurnsController>();
        StartCoroutine(WinLayer());
    }

    IEnumerator WinLayer()
    {
        yield return new WaitForSeconds(1f);

        winnerSprite.GetComponent<Image>().sprite = turnsControl.winner.GetComponentInChildren<SpriteRenderer>().sprite;
        winnerSprite.GetComponent<Image>().color = Color.white;

        yield return new WaitForSeconds(0.5f);

        victoryBox.SetActive(true);

        yield return new WaitForSeconds(1f);
        spotlight.SetActive(true);
    }
}
