using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;
using UnityEditor;

public class BannerTabScript : MonoBehaviour
{
    private string text;
    public bool isSelected = false;
    public CurrentTab _representedTab;

    public void SetRepresentedTab(CurrentTab tab)
    {
        _representedTab = tab;
        text = tab.ToString();
        var textObj = StaticScripts.CreateTextObj(name + "_text", text, new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 0f),
            FontType.DiabloFont, 96, new Color32(131, 176, 209, 255), TextAlignment.Center, true, name, FontStyle.Bold);
        textObj.transform.localPosition = text.Length > 8 ? new Vector3(0.9f, 0.5f, 0f) : new Vector3(0.9f, 1.1f - (textObj.GetComponent<Renderer>().bounds.size.x / 2f), 0f);
        textObj.transform.localScale = new Vector2(1f, 1.5f);
    }

    void OnMouseUpAsButton()
    {
       SetTabSelected();
    }

    public void Disable()
    {
        if (!isSelected) return;
        var bannerTex = Resources.Load<Texture2D>(@"Borders/InGame/Paragon_BannerInactive");
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(bannerTex, new Rect(0, 0, bannerTex.width, bannerTex.height), new Vector2(0f, 0f));
        isSelected = false;
    }


    /*
     * This method is looking for previously selected tab, disabling all its components and,
     * after that, it finds currently selected tab. If currently selected tab have been selected before then it just 
     * 'awake' it. In the other case, it'll create new object for selected tab. 
     */
    public void SetTabSelected()
    {
        if (isSelected) return;

        foreach (Transform child in transform.parent)
        {
            if (child.name.Contains("Banner_"))
            {
                if (child.gameObject.GetComponent<BannerTabScript>().isSelected)
                    child.gameObject.GetComponent<BannerTabScript>().Disable();
            }

        }

        var bannerTex = Resources.Load<Texture2D>(@"Borders/InGame/Paragon_BannerActive");
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(bannerTex, new Rect(0, 0, bannerTex.width, bannerTex.height), new Vector2(0f, 0f));
        isSelected = true;

        var bannerWindow = GameObject.Find("BannerWindowGameObject");
        if (bannerWindow && bannerWindow.GetComponent<BannerWindowScript>()._currentTab)
        {
            bannerWindow.GetComponent<BannerWindowScript>()._currentTab.SetActive(false);
        }
        if (bannerWindow.transform.FindChild(string.Format("Tab_{0}", _representedTab)))
        {
            bannerWindow.transform.FindChild(string.Format("Tab_{0}", _representedTab)).gameObject.SetActive(true);
            bannerWindow.GetComponent<BannerWindowScript>()._currentTab =
                bannerWindow.transform.FindChild(string.Format("Tab_{0}", _representedTab)).gameObject;
        }
        else
        {
            var tabWindow = new GameObject(string.Format("Tab_{0}", _representedTab));
            tabWindow.transform.parent = bannerWindow.transform;
            bannerWindow.GetComponent<BannerWindowScript>()._currentTab = tabWindow;

            switch (_representedTab)
            {
                case CurrentTab.Core:
                    tabWindow.AddComponent<CoreTabWindowScript>();
                    break;
                case CurrentTab.Offense:
                    tabWindow.AddComponent<OffenseTabWindowScript>();
                    break;
                case CurrentTab.Defense:
                    tabWindow.AddComponent<DefenseTabWindowScript>();
                    break;
                case CurrentTab.Utility:
                    tabWindow.AddComponent<UtilityTabWindowScript>();
                    break;
                case CurrentTab.Bot:
                    tabWindow.AddComponent<BotTabWindowScript>();
                    break;
                case CurrentTab.Character:
                    tabWindow.AddComponent<CharacterTabWindowScript>();
                    break;
                case CurrentTab.Inventory:
                    tabWindow.AddComponent<InventoryTabWindowScript>();
                    break;
            }
        }
    }
}
