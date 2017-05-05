using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmountSetting : MonoBehaviour {
    public Button decrease, increase;
    public int defaultAmount, minAmount, maxAmount, increment;
    private Text amountText;

    [HideInInspector]
    public int amount;

    //Initialization
    void Start()
    {
        amountText = GetComponent<Text>();
        amountText.text = defaultAmount.ToString();
        amount = defaultAmount;

    }

    //Decrease the amount based on increment value
    public void DecreaseAmount()
    {
        if (amount > minAmount)
        {
            amount -= increment;
            amountText.text = amount.ToString();
        }
    }

    //Increase the amount based on increment value
    public void IncreaseAmount()
    {
        if (amount < maxAmount)
        {
            amount += increment;
            amountText.text = amount.ToString();
        }
    }
	
}
