using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using Assets.Resources.Scripts;


//This class is used to show a tooltip for inventory or equipped items
public class ShowTooltipScript : MonoBehaviour
{
    private GameObject tooltipObject;
    public InventoryItem Item;
    private bool isHovered = false;
    protected void OnMouseEnter()
    {
        isHovered = true;

        if(!tooltipObject)
            tooltipObject = new GameObject("TooltipGameObject");
        tooltipObject.transform.parent = transform.parent;
        tooltipObject.transform.position = new Vector3(transform.position.x + GetComponent<Renderer>().bounds.size.x,
            Item.Size == 1 ? transform.position.y + GetComponent<Renderer>().bounds.size.y / 2f : transform.position.y + (GetComponent<Renderer>().bounds.size.y/4f)*3f);
        StaticScripts.CreateGameObj("TitleTopGameObject", "Borders/InGame/Inventory/Tooltip_Frame_Title_Top", new Vector3(0.9f, 0.9f),
            new Vector3(0f, 0f), child: true, parentName: tooltipObject.name, sortingOrder:6);
        StaticScripts.CreateGameObj("TitleMiddleGameObject", "Borders/InGame/Inventory/Tooltip_Frame_Cost", new Vector3(0.9f, 0.9f),
            new Vector3(0f, -0.3f), child: true, parentName: tooltipObject.name, sortingOrder:5);
        StaticScripts.CreateGameObj("TitleBottomGameObject", "Borders/InGame/Inventory/Tooltip_Frame_Title_Bottom", new Vector3(0.9f, 0.9f),
            new Vector3(0f, -0.3f), child: true, parentName: tooltipObject.name, sortingOrder: 6);

        var pathToTitleQualityBackground = "";
        var textColor = new Color32();
        switch (Item.Quality)
        {
            case ItemQuality.Rare:
                pathToTitleQualityBackground = "Borders/InGame/Inventory/Tooltip_Frame_QualityColor_Rare";
                textColor = new Color32(195, 201, 101, 255);
                break;
            case ItemQuality.Legendary:
                pathToTitleQualityBackground = "Borders/InGame/Inventory/Tooltip_Frame_QualityColor_Legendary";
                textColor = new Color32(129, 84, 61, 255);
                break;
            case ItemQuality.Set:
                pathToTitleQualityBackground = "Borders/InGame/Inventory/Tooltip_Frame_QualityColor_Set";
                textColor = new Color32(73, 202, 75, 255);
                break;
        }
        StaticScripts.CreateGameObj("TitleQualityBackgroundGameObject", pathToTitleQualityBackground,
            new Vector3(0.9f, 0.4f), new Vector3(0.3f, -0.21f, 0f), child: true, parentName: tooltipObject.name, sortingOrder: 7);
        
        var Description = ((DescriptionAttribute[])Item.Type.GetType()
            .GetField(Item.Type.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false)); //the easy way to get names with spaces

        var ItemTypeText = StaticScripts.CreateTextObj("ItemTypeTextObject", string.Format("{0} {1}", Item.Quality, Description[0].Description), new Vector3(0.02f, 0.02f),
            new Vector3(0.95f, -0.4f), FontType.StandartFont, Item.Type == ItemType.MightyWeapon2H ? 82 : 90, textColor, TextAlignment.Left, true,
            tooltipObject.name);
        ItemTypeText.GetComponent<Renderer>().sortingOrder = 6;

        StaticScripts.CreateGameObj("StatsTopGameObject", "Borders/InGame/Inventory/Tooltip_Frame_Tile_Top", new Vector3(0.9f, 0.9f),
            new Vector3(0f, -0.4f), child: true, parentName: tooltipObject.name, sortingOrder: 5);
        var firstMiddleGameObject = StaticScripts.CreateGameObj("StatsMiddleGameObject1", "Borders/InGame/Inventory/Tooltip_Frame_Tile_Middle", new Vector3(0.9f, 0.9f),
            new Vector3(0f, -0.9f), child: true, parentName: tooltipObject.name, sortingOrder: 5);

        var lastMiddleTileGameObject = StaticScripts.CreateGameObj("StatsMiddleGameObject2",
            "Borders/InGame/Inventory/Tooltip_Frame_Tile_Middle", new Vector3(0.9f, 0.9f),
            new Vector3(0f, -1.4f), child: true, parentName: tooltipObject.name, sortingOrder: 5);


        #region ItemIcon

        if (Item.Size == 1)
        {
            StaticScripts.CreateGameObj("tooltipIconGameObject",
                "Borders/InGame/Inventory/Tooltip_IconFrame_Small", new Vector3(0.9f, 0.9f), new Vector3(0.2f, -1f),
                child: true, parentName: tooltipObject.name, sortingOrder: 6);

            string pathToQualityBackground = "";
            switch (Item.Quality)
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

            StaticScripts.CreateGameObj("tooltipQualityGameObject", pathToQualityBackground, new Vector3(0.9f, 0.9f),
                new Vector3(0.24f, -0.95f), child: true, parentName: tooltipObject.name, sortingOrder: 6);

            if (Item.Name != null)
            {
                if (
                    Resources.Load<Texture2D>(string.Format(@"Items/{0}/{0}_{1}", Item.Type, Item.Name.Replace(" ", "_"))))
                {
                    StaticScripts.CreateGameObj("tooltipItemIconGameObject",
                        string.Format(@"Items/{0}/{0}_{1}", Item.Type, Item.Name.Replace(" ", "_")),
                        new Vector3(0.75f, 0.75f), new Vector3(0.28f, -0.93f),
                        child: true, parentName: tooltipObject.name, sortingOrder: 7);
                }
            }
        }
        else
        {
            lastMiddleTileGameObject = StaticScripts.CreateGameObj("StatsMiddleGameObject3",
                "Borders/InGame/Inventory/Tooltip_Frame_Tile_Middle", new Vector3(0.9f, 0.9f),
                new Vector3(0f, -1.9f), child: true, parentName: tooltipObject.name, sortingOrder: 5);

            var armorAmountTextObject = StaticScripts.CreateTextObj("ArmorAmountTextObject", Item.Armor.ToString(), new Vector3(0.02f, 0.02f),
            new Vector3(0.95f, -0.65f), FontType.StandartFont, 260, Color.white, TextAlignment.Left, true,
            tooltipObject.name);
            armorAmountTextObject.GetComponent<Renderer>().sortingOrder = 6;

            var armorTextObject = StaticScripts.CreateTextObj("ArmorTextObject", "Armor", new Vector3(0.02f, 0.02f),
            new Vector3(0.95f, -1.25f), FontType.StandartFont, 80, new Color32(110, 110, 110, 255), TextAlignment.Left, true,
            tooltipObject.name);
            armorTextObject.GetComponent<Renderer>().sortingOrder = 6;

            StaticScripts.CreateGameObj("tooltipIconGameObject",
                "Borders/InGame/Inventory/Tooltip_IconFrame_Large", new Vector3(0.9f, 0.9f), new Vector3(0.2f, -1.6f),
                child: true, parentName: tooltipObject.name, sortingOrder: 6);

            string pathToQualityBackground = "";
            switch (Item.Quality)
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

            StaticScripts.CreateGameObj("tooltipQualityGameObject", pathToQualityBackground, new Vector3(0.9f, 0.9f),
                new Vector3(0.24f, -1.55f), child: true, parentName: tooltipObject.name, sortingOrder: 6);

            if (Item.Name != null)
            {
                if (
                    Resources.Load<Texture2D>(string.Format(@"Items/{0}/{0}_{1}", Item.Type, Item.Name.Replace(" ", "_"))))
                {
                    StaticScripts.CreateGameObj("tooltipItemIconGameObject",
                        string.Format(@"Items/{0}/{0}_{1}", Item.Type, Item.Name.Replace(" ", "_")),
                        new Vector3(0.75f, 0.75f), new Vector3(0.28f, -1.53f),
                        child: true, parentName: tooltipObject.name, sortingOrder: 7);
                }
            }
        }

        #endregion

        var amountOfTiles = Math.Ceiling((double)Item.itemStats.Length / 2) ; // this helps to round number to the nearest NEXT integer

        for (int i = 0; i < amountOfTiles; i++) // calculate the amount of middle tiles. Depends on the amount of rolled stats
        {
            lastMiddleTileGameObject = StaticScripts.CreateGameObj(string.Format("StatsMiddleGameObject{0}", 4 + i),
                "Borders/InGame/Inventory/Tooltip_Frame_Tile_Middle", new Vector3(0.9f, 0.9f),
                new Vector3(0f, lastMiddleTileGameObject.transform.localPosition.y - 0.5f), child: true, parentName: tooltipObject.name, sortingOrder: 5);
        }

        GameObject lastItemStatObject = null;

        foreach (var stat in Item.itemStats)
        {
            if (lastItemStatObject == null)
            {
                lastItemStatObject = StaticScripts.CreateTextObj("ItemStatTextObject", string.Format("{0} {1}", stat.StatAmount, stat.stat),
                    new Vector3(0.02f, 0.02f), new Vector3(0.45f, Item.Size == 2? -1.8f : -1.2f, 0f), FontType.StandartFont, 84, new Color32(75, 75, 147, 255),
                    TextAlignment.Left, true, tooltipObject.name);
            }
            else
            {
                lastItemStatObject = StaticScripts.CreateTextObj("ItemStatTextObject", string.Format("{0} {1}", stat.StatAmount, stat.stat),
                    new Vector3(0.02f, 0.02f), new Vector3(0.45f, lastItemStatObject.transform.localPosition.y - 0.25f, 0f), FontType.StandartFont,
                    84, new Color32(75, 75, 147, 255), TextAlignment.Left, true, tooltipObject.name);
            }
            lastItemStatObject.GetComponent<Renderer>().sortingOrder = 6;

            StaticScripts.CreateGameObj("ItemStatIconGameObject", "Borders/InGame/Inventory/bullet", new Vector3(0.5f, 0.5f),
                new Vector3(0.25f, lastItemStatObject.transform.localPosition.y - 0.184f), child: true,
                parentName: tooltipObject.name, sortingOrder: 6);
        }

        var bottomTileGameObject = StaticScripts.CreateGameObj("StatsBottomGameObject",
            "Borders/InGame/Inventory/Tooltip_Frame_Tile_Bottom", new Vector3(0.9f, 0.9f),
            new Vector3(0f, lastMiddleTileGameObject.transform.localPosition.y - 0.1f), child: true,
            parentName: tooltipObject.name, sortingOrder: 5);
        var costFrameGameObject = StaticScripts.CreateGameObj("CostGameObject",
            "Borders/InGame/Inventory/Tooltip_Frame_Cost", new Vector3(0.9f, 0.9f),
            new Vector3(0f, lastMiddleTileGameObject.transform.localPosition.y - 0.475f), child: true,
            parentName: tooltipObject.name, sortingOrder: 5);

        StaticScripts.CreateTextObj("SellValueTextObject", "Sell Value:", new Vector3(0.02f, 0.02f), new Vector3(0.25f, 0.33f),
            FontType.StandartFont, 88, new Color32(186, 178, 155, 255), TextAlignment.Left, true,
            costFrameGameObject.name);

        var sellValueTextObject = StaticScripts.CreateTextObj("SellValuePriceTextObject", Item.SellValue.ToString(), new Vector3(0.02f, 0.02f),
            new Vector3(1.2f, 0.33f), FontType.StandartFont, 88, Color.white, TextAlignment.Left, true, costFrameGameObject.name, FontStyle.Bold);
        StaticScripts.CreateGameObj("SellValueIconGameObject", "Borders/InGame/Inventory/gold", new Vector3(0.55f, 0.55f),
            new Vector3( 1.2f + sellValueTextObject.GetComponent<Renderer>().bounds.size.x + 0.15f, 0.135f), child: true, parentName: costFrameGameObject.name);

        var endOfTooltipGameObject = StaticScripts.CreateGameObj("EndOfTooltipGameObject",
            "Borders/InGame/Inventory/Tooltip_Frame_Bottom", new Vector3(0.9f, 0.9f),
            new Vector3(0f, lastMiddleTileGameObject.transform.localPosition.y - 0.53f), child: true,
            parentName: tooltipObject.name, sortingOrder: 5);

        //Check if there is enough space to show tooltip on the right bottom side of the screen. If there is no space then tooltip will be moved to fill.

        var screenBorders = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        if(tooltipObject.transform.position.x + endOfTooltipGameObject.GetComponent<Renderer>().bounds.size.x > screenBorders.x)
            tooltipObject.transform.position = new Vector3(transform.position.x - endOfTooltipGameObject.GetComponent<Renderer>().bounds.size.x, tooltipObject.transform.position.y);

        if(endOfTooltipGameObject.transform.position.y - endOfTooltipGameObject.GetComponent<Renderer>().bounds.size.y < screenBorders.y * -1)
            tooltipObject.transform.position = new Vector3(tooltipObject.transform.position.x, screenBorders.y * -1 + Math.Abs(endOfTooltipGameObject.transform.localPosition.y - endOfTooltipGameObject.GetComponent<Renderer>().bounds.size.y) + 0.2f);

    }

    protected void OnMouseExit()
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
            Destroy(tooltipObject);
        }
        isHovered = false;
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(1) && isHovered && (Item.LeftTopCornerCoordinate.x != -1 && Item.LeftTopCornerCoordinate.y != -1))
        {
           StartCoroutine(DrawRMBMenu());
        }
    }

    /*
     * This method draws a menu which can be called with the right mouse button click.
     * That menu have three buttons:
     * - Trash (removing item from the inventory)
     * - Equip (equipping that item if it's possible)
     * - Cancel (hides menu)
     * Right-clicking on the another item will close previous menu
     */
    protected IEnumerator DrawRMBMenu()
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
            Destroy(tooltipObject);
        }
        if (GameObject.Find("RMBMenuGameObject"))
        {
            var prevObject = GameObject.Find("RMBMenuGameObject");
            prevObject.name += "_old";
            prevObject.SetActive(false);
            Destroy(prevObject);
        }

        yield return 1; //wait for the next frame to be sure that objects are killed

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var menuBorder = StaticScripts.CreateGameObj("RMBMenuGameObject", "Borders/InGame/Inventory/QuestReward_Base2",
            new Vector3(0.7f, 0.8f, -5f), new Vector3(0f, 0f, 0f), true, 1, child: true, parentName: transform.parent.parent.parent.name, sortingOrder:10);
        menuBorder.transform.position = new Vector3(mousePosition.x + 0.1f, mousePosition.y, -1f);
        menuBorder.GetComponent<Collider2D>().isTrigger = true;

        var cancelButton = StaticScripts.CreateGameObj("CancelButtonGameObject", "Buttons/D3_Btn_Reward_Up",
            new Vector3(1.35f, 0.95f), new Vector3(0.25f, 0.2f, 1f), true, 1, true, typeof(ButtonBaseMouseEvents), true,
            menuBorder.name);

        var cancelButtonTextObject = StaticScripts.CreateTextObj("CancelButtonTextObject", "Cancel",
            new Vector3(0.02f, 0.02f), new Vector3(1.165f, 0.36f), FontType.DiabloFont, 110, new Color32(243, 170, 85, 255),
            TextAlignment.Left, true, cancelButton.name);
        cancelButtonTextObject.transform.localScale = new Vector3(1f, 1.2f);

        var buttonEquip = StaticScripts.CreateGameObj("EquipItemButtonGameObject", "Buttons/D3_Btn_Reward_Up",
            new Vector3(1.35f, 0.95f), new Vector3(0.25f, 0.725f, 1f), true, 1, true, typeof(ButtonBaseMouseEvents), true,
            menuBorder.name);

        var buttonEquipTextObject = StaticScripts.CreateTextObj("EquipButtonTextObject", "Equip",
            new Vector3(0.02f, 0.02f), new Vector3(1.25f, 0.36f), FontType.DiabloFont, 110, new Color32(243, 170, 85, 255),
            TextAlignment.Left, true, buttonEquip.name);
        buttonEquipTextObject.transform.localScale = new Vector3(1f, 1.2f);

        var buttonTrash = StaticScripts.CreateGameObj("TrashItemButtonGameObject", "Buttons/D3_Btn_Reward_Up",
            new Vector3(1.35f, 0.95f), new Vector3(0.25f, 1.25f, 1f), true, 1, true, typeof(ButtonBaseMouseEvents), true,
            menuBorder.name);

        var buttonTrashTextObject = StaticScripts.CreateTextObj("TrashButtonTextObject", "Trash",
            new Vector3(0.02f, 0.02f), new Vector3(1.25f, 0.36f), FontType.DiabloFont, 110, new Color32(243, 170, 85, 255),
            TextAlignment.Left, true, buttonTrash.name);
        buttonTrashTextObject.transform.localScale = new Vector3(1f, 1.2f);

        buttonTrash.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            Account.CurrentAccount.GetCurrentHero().InventoryClass.RemoveFromInventory(Item.LeftTopCornerCoordinate);
            menuBorder.SetActive(false);
            Destroy(menuBorder);
        };

        cancelButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick+= delegate
        {
            menuBorder.SetActive(false);
            Destroy(menuBorder);
        };

        buttonEquip.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            Account.CurrentAccount.GetCurrentHero().InventoryClass.EquipItem(Item.LeftTopCornerCoordinate);
            menuBorder.SetActive(false);
            Destroy(menuBorder);
        };


    }
}
