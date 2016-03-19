using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class InventoryTabWindowScript : MonoBehaviour
{
    private GameObject goldAmountTextObject;
    private GameObject shardsAmountTextObject;

	// Use this for initialization
	void Start () 
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                StaticScripts.CreateGameObj(string.Format("InventoryGrid_{0}_{1}", j, i),
                    "Borders/InGame/Inventory/InventoryGrid_Slot", new Vector3(0.95f, 0.95f),
                    new Vector3(1f + (0.61f * i), 2.22f - (0.595f * j), 10f), child: true, parentName: name, sortingOrder:3);
            }
        }

	    var currencyBackground = StaticScripts.CreateGameObj("CurrencyBackgroundObject",
	        "Borders/InGame/Inventory/BattleNetAuctionHouse_AccountBalanceBg", new Vector3(1.1f, 1.1f),
	        new Vector3(2.5f, -3.8f, 0f), child: true, parentName: name);

	    StaticScripts.CreateGameObj("CoinIcon", "Borders/InGame/Inventory/gold", new Vector3(0.9f, 0.9f),
	        new Vector3(0.25f, 0.08f, 0f), child: true, parentName: currencyBackground.name);

        StaticScripts.CreateGameObj("ShardsIcon", "Borders/InGame/Inventory/x1_shard", new Vector3(0.9f, 0.9f),
            new Vector3(2.5f, 0.08f, 0f), child: true, parentName: currencyBackground.name);

	    goldAmountTextObject = StaticScripts.CreateTextObj("GoldAmountTextObject",
	        Account.CurrentAccount.GetCurrentHero().Heroic
	            ? Account.CurrentAccount.GoldHardcore.ToString()
	            : Account.CurrentAccount.Gold.ToString(),
	        new Vector3(0.02f, 0.02f), new Vector3(0.65f, 0.35f), FontType.StandartFont, 110, Color.white,
	        TextAlignment.Center,
	        true, currencyBackground.name);

	    shardsAmountTextObject = StaticScripts.CreateTextObj("ShardsAmountTextObject",
	        Account.CurrentAccount.GetCurrentHero().Heroic
	            ? Account.CurrentAccount.BloodShardsHardcore.ToString()
	            : Account.CurrentAccount.BloodShards.ToString(),
	        new Vector3(0.02f, 0.02f), new Vector3(2.9f, 0.35f), FontType.StandartFont, 110, Color.white,
	        TextAlignment.Center,
	        true, currencyBackground.name);
    }


    void FixedUpdate()
    {
        goldAmountTextObject.GetComponent<TextMesh>().text = Account.CurrentAccount.GetCurrentHero().Heroic
            ? Account.CurrentAccount.GoldHardcore.ToString()
            : Account.CurrentAccount.Gold.ToString();

        shardsAmountTextObject.GetComponent<TextMesh>().text = Account.CurrentAccount.GetCurrentHero().Heroic
            ? Account.CurrentAccount.BloodShardsHardcore.ToString()
            : Account.CurrentAccount.BloodShards.ToString();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
