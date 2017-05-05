using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSettings : MonoBehaviour {
    //Sprite list and avatar list should use the same index
	public List<Sprite> spritesList;
    public List<Sprite> avatarList;
    public AmountSetting healthSetting, warpSetting, battleSetting, cardSetting;

    public static GameSettings instance;

    [HideInInspector]
    public int maxHealth, maxWarp, maxBattle, maxCard;
    [HideInInspector]
    public List<int> playerSprites;
    [HideInInspector]
    public List<Sprite> playerAvatar;

    void Awake()
    {
        // Keep only one Game Setting
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Keep this Game Object even after scene change
        DontDestroyOnLoad(this);
    }

	void Start () {
        SettingInitialization();
	}

    void OnLevelWasLoaded(int level)
    {
        if (level == 0)
            SettingInitialization();
    }

    public void SettingInitialization()
    {
        // Initialization
        playerSprites = new List<int>();
        playerAvatar = new List<Sprite>();
        healthSetting = healthSetting.GetComponent<AmountSetting>();
        warpSetting = warpSetting.GetComponent<AmountSetting>();
        battleSetting = battleSetting.GetComponent<AmountSetting>();
        cardSetting = cardSetting.GetComponent<AmountSetting>();
    }

    public void ConfirmSetting()
    {
        //Set the value to be used in setting up the game
        maxHealth = healthSetting.amount;
        maxWarp = warpSetting.amount;
        maxBattle = battleSetting.amount;
        maxCard = cardSetting.amount;
    }
}