using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

//This class draws inventory - inventory grid, items you have etc. 
public class InventoryTabWindowScript : MonoBehaviour
{
    private GameObject goldAmountTextObject;
    private GameObject shardsAmountTextObject;


	void Start () 
    {
	    Account.CurrentAccount.GetCurrentHero().InventoryClass.InventoryChangedEvent += DrawInventory;

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


    void OnEnable()
    {
        DrawInventory();
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


    void DrawInventory()
    {
        if (!isActiveAndEnabled) return;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("InventoryGrid_"))
            {
                child.gameObject.name = name + "_old";
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        //Drawing free slots
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                if (!Account.CurrentAccount.GetCurrentHero().InventoryClass.InventorySpace[i, j])
                    StaticScripts.CreateGameObj(string.Format("InventoryGrid_{0}_{1}", j, i),
                        "Borders/InGame/Inventory/InventoryGrid_Slot", new Vector3(0.95f, 0.95f),
                            new Vector3(1f + (0.61f * i), 2.22f - (0.595f * j), 10f), child: true, parentName: name, sortingOrder: 3);
            }
        }

        foreach (var item in Account.CurrentAccount.GetCurrentHero().InventoryClass.Inventory)
        {

            if (item.Size == 1)
            {
                var background =
                    StaticScripts.CreateGameObj(
                        string.Format("InventoryGrid_{0}_{1}", item.LeftTopCornerCoordinate.y,
                            item.LeftTopCornerCoordinate.x), "Borders/InGame/Inventory/InventoryGrid_Slot",
                        new Vector3(0.95f, 0.95f),
                        new Vector3(1f + (0.61f * item.LeftTopCornerCoordinate.x),
                            2.22f - (0.595f * item.LeftTopCornerCoordinate.y), 10f), true, 1, true, typeof(ShowTooltipScript), true, name,
                        sortingOrder: 3);

                background.GetComponent<ShowTooltipScript>().Item = item;

                string pathToQualityBackground = "";
                switch (item.Quality)
                {
                    case ItemQuality.Rare:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Small_Equip_Rare";
                        break;
                    case ItemQuality.Legendary:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Small_Inventory_Legendary";
                        break;
                    case ItemQuality.Set:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Small_Inventory_Set";
                        break;
                }
                StaticScripts.CreateGameObj(
                    string.Format("InventoryGrid_{0}_{1}_quality", item.LeftTopCornerCoordinate.y,
                        item.LeftTopCornerCoordinate.x), pathToQualityBackground, item.Quality == ItemQuality.Rare ? new Vector3(0.94f, 0.94f) : new Vector3(1f, 1f),
                    item.Quality == ItemQuality.Rare ? new Vector3(0.03f, 0.03f) : new Vector3(0.02f, 0.02f), child: true, parentName: background.name);


                if (item.Name == null) continue;
                if (Resources.Load<Texture2D>(string.Format(@"Items/{0}/{0}_{1}", item.Type, item.Name.Replace(" ", "_"))))
                {
                    StaticScripts.CreateGameObj("InventoryGrid_{0}_{1}_icon",
                        string.Format(@"Items/{0}/{0}_{1}", item.Type, item.Name.Replace(" ", "_")), new Vector3(0.9f, 0.9f), new Vector3(0.03f, 0.03f),
                        child: true, parentName: background.name, sortingOrder:5);
                }
            }
            else
            {


                var background = StaticScripts.CreateGameObj(string.Format("InventoryGrid_{0}_{1}", item.LeftTopCornerCoordinate.y,
                            item.LeftTopCornerCoordinate.x), "Borders/InGame/Inventory/Quality_Large_Blank_Inventory", new Vector3(0.95f, 0.95f),
                            new Vector3(1f + (0.61f * item.LeftTopCornerCoordinate.x), 2.22f - (0.595f * (item.LeftTopCornerCoordinate.y + 1)), 10f), true, 1, true, typeof(ShowTooltipScript), child: true,
                            parentName: name, sortingOrder: 3);
                background.GetComponent<ShowTooltipScript>().Item = item;
                string pathToQualityBackground = "";
                switch (item.Quality)
                {
                    case ItemQuality.Rare:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Large_Equip_Rare";
                        break;
                    case ItemQuality.Legendary:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Large_Equip_Legendary";
                        break;
                    case ItemQuality.Set:
                        pathToQualityBackground = "Borders/InGame/Inventory/Quality_Large_Equip_Set";
                        break;
                }
                StaticScripts.CreateGameObj(string.Format("InventoryGrid_{0}_{1}_quality", item.LeftTopCornerCoordinate.y,
                        item.LeftTopCornerCoordinate.x), pathToQualityBackground, new Vector3(1f, 1f),
                        new Vector3(0.02f, 0.02f), child: true, parentName: background.name);

                if(item.Name == null) continue;
                if (Resources.Load<Texture2D>(string.Format(@"Items/{0}/{0}_{1}", item.Type, item.Name.Replace(" ", "_"))))
                {
                    StaticScripts.CreateGameObj("InventoryGrid_{0}_{1}_icon",
                        string.Format(@"Items/{0}/{0}_{1}", item.Type, item.Name.Replace(" ", "_")), new Vector3(0.9f, 0.9f), new Vector3(0.03f, 0.03f),
                        child: true, parentName: background.name, sortingOrder: 5);
                }

            }
        }
    }
}
