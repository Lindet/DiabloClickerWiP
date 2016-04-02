using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;


/*
 * Class which is responsible for all that enemy-things.
 * Contains methods for 'spawning' new hero, applying damage, adding experience, 
 * asking for generating new loot etc. 
 */
public class EnemyScript : MonoBehaviour
{
    private double maxHP;
    private double currentHP;

    private string enemyName = "Enemy";
    private GameObject hpTextObject;
    private GameObject hpMeterDepthMask;

	void Start ()
	{
        maxHP = Convert.ToInt32(Math.Pow(10, Math.Sqrt(Account.CurrentAccount.GetCurrentHero().Level))); 
        currentHP = maxHP;

        var textObj = StaticScripts.CreateTextObj("EnemyNameText", enemyName, new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 0f),
            FontType.DiabloFont, 120, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "enemyBorder", FontStyle.Bold);
        
        textObj.transform.localPosition = new Vector3(2.5f - (textObj.GetComponent<Renderer>().bounds.size.x/4f), 5.3f);

	    StaticScripts.CreateGameObj("EnemyHpBar", "Borders/InGame/EnemyHPbar01_Base",
            new Vector3(2.1f, 2f), new Vector3(0.2f, 0.25f, 10f), child: true, parentName: "enemyBorder");

        var hpMeterObject = StaticScripts.CreateGameObj("EnemyHpMeter", "Borders/InGame/EnemyHPbar01_Meter_Full",
            new Vector3(2.1f, 2f), new Vector3(-6.7f, -2.165f, 160f), child: true, parentName: "InGameSceneObject");
        hpMeterObject.GetComponent<SpriteRenderer>().sortingOrder = 3;

        hpTextObject = StaticScripts.CreateTextObj("enemyHpText", currentHP.ToString(), new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 10f),
            FontType.DiabloFont, 70, Color.white, TextAlignment.Center, true, "EnemyHpMeter", FontStyle.Bold);
        hpTextObject.transform.localPosition = new Vector3(0.95f - (hpTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 0.165f);
        hpTextObject.transform.localScale = new Vector3(1f, 1.2f);

	    var shader = Shader.Find("Masked/Mask"); //this shader is used to hide that part of the HP which was 'damaged' 
        hpMeterDepthMask = new GameObject("HpMeterMaskObject");
        hpMeterDepthMask.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
        hpMeterDepthMask.GetComponent<SpriteRenderer>().material = new Material(shader);
        hpMeterDepthMask.transform.position = new Vector3(-2.73f, -2.2f, 140f);
	    hpMeterDepthMask.transform.parent = GameObject.Find("InGameSceneObject").transform;
        hpMeterDepthMask.transform.localScale = new Vector3(400f, 35f, 1f);

        //Start a timer which will call for a method every 1 second
        InvokeRepeating("ApplyBotDamage", 1f, 1f);
	}


    void OnMouseUpAsButton()
    {
        ApplyDamage(Account.CurrentAccount.GetCurrentHero().ReturnStat(CharacterStat.Damage));
    }

    //Applies damage to the enemy's health. Also checks if enemy is already died or still breaths.
    void ApplyDamage(double damage)
    {
        currentHP -= Convert.ToInt32(damage);
        hpTextObject.GetComponent<TextMesh>().text = currentHP.ToString();
        hpTextObject.transform.localPosition = new Vector3(0.95f - (hpTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 0.165f);

        float diff = Convert.ToSingle((maxHP - currentHP) / maxHP);
        hpMeterDepthMask.transform.position = new Vector3(Mathf.Lerp(-2.73f, -6.7f, diff), -2.2f, 140f);


        if (currentHP <= 0)
            EnemyKilled();
        
    }
    void GenerateNewEnemy()
    {
        maxHP = Convert.ToInt32(Math.Pow(10, Math.Sqrt(Account.CurrentAccount.GetCurrentHero().Level)));
        currentHP = maxHP;

        hpTextObject.GetComponent<TextMesh>().text = currentHP.ToString();
        hpTextObject.transform.localPosition = new Vector3(0.95f - (hpTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 0.165f);

        hpMeterDepthMask.transform.position = new Vector3(Mathf.Lerp(-2.73f, -6.7f, 0f), -2.2f, 140f);
    }


    //If enemy is killed, you should be rewarded - gives experience for the current hero, also gives gold to the whole account and generates loot. Check for achievements will be added soon.
    void EnemyKilled()
    {
        Account.CurrentAccount.GetCurrentHero().AddExperience(20*Math.Pow(1.05, Account.CurrentAccount.GetCurrentHero().Level));
        if (Account.CurrentAccount.GetCurrentHero().Heroic)
            Account.CurrentAccount.GoldHardcore += (uint)maxHP/10;
        else
            Account.CurrentAccount.Gold += (uint)maxHP / 10;

        var item = LootGenerator.CurrentGenerator.GenerateNewItem();
        if (item != null) Account.CurrentAccount.GetCurrentHero().InventoryClass.AddItemToInventory(item);
        GenerateNewEnemy();
    }

    void ApplyBotDamage()
    {
        ApplyDamage(Account.CurrentAccount.GetCurrentHero().ReturnStat(CharacterStat.BotDamage));
    }
}
