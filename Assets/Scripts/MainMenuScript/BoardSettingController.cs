using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardSettingController : MonoBehaviour {
    public AmountSetting healthSetting, warpSetting, battleSetting, cardSetting;

    [HideInInspector]
    public int maxHealth, maxWarp, maxBattle, maxCard;

    void Start()
    {
        healthSetting = healthSetting.GetComponent<AmountSetting>();
        warpSetting = warpSetting.GetComponent<AmountSetting>();
        battleSetting = battleSetting.GetComponent<AmountSetting>();
        cardSetting = cardSetting.GetComponent<AmountSetting>(); 
    }

    public void ConfirmSetting()
    {
        maxHealth = healthSetting.amount;
        maxWarp = warpSetting.amount;
        maxBattle = battleSetting.amount;
        maxCard = cardSetting.amount;
    }
}
