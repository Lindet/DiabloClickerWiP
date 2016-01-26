using UnityEngine;
using System.Collections;

public class HeroScrollingList : MonoBehaviour {

	void Start () {
	    for (int i = 0; i < 19; i ++)
	    {
            StaticScripts.CreateGameObj(string.Format("ScrollBackgroundPiece_{0}", i), @"Controls/ScrollSmallMiddle", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, 2.8f - (0.27f*i), 10f), child: true, parentName: name, sortingOrder: 2);
	    }
        StaticScripts.CreateGameObj("HeroListFrame", @"Borders/MainMenu/BattlenetHeroSelect_ListFrame", new Vector3(0.83f, 0.83f), new Vector3(-8.27f, -2.35f, 10f), child: true, parentName: name);
        StaticScripts.CreateGameObj("ScrollUp", @"Controls/ScrollSmallUp_Up", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, 3.07f, 10f), true, 1, true, typeof(ButtonScrollUp), parentName: name, sortingOrder: 3);
        StaticScripts.CreateGameObj("ScrollDown", @"Controls/ScrollSmallDown_Up", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, -2.3f, 10f), true, 1, true, typeof(ButtonScrollDown), child: true, parentName: name, sortingOrder: 3);
        StaticScripts.CreateGameObj("Scrollbar", @"Controls/ScrollbarThumb_Up", new Vector3(0.9f, 0.9f), new Vector3(-5f, 2.75f, 10f), true, 1, true, typeof(ScrollBarScript), child: true, parentName: name, sortingOrder: 3);


	    for (int i = 0; i < 12; i++)
	    {
	        if (i < Account.CurrentAccount.ListOfHeroes.Count)
	        {
	            StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}", i), @"Buttons/BattleNetButton_MainMenu_ClearUp", new Vector3(0.83f, 0.83f), new Vector3(-8.24f,2.6f -(0.8f * i),150f), child:true, parentName:name, sortingOrder: 3);
                StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}_background", i), @"Controls/BattleNetHeroSelect_BaseSlot", new Vector3(0.83f, 0.83f), new Vector3(-8.23f, 2.6f - (0.8f * i), 150f), child: true, parentName: name, sortingOrder: 3);
	            continue;
	        }
	        StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}", i), @"Controls/BattleNetHeroSelect_EmptySlot", new Vector3(0.83f, 0.83f), new Vector3(-8.24f,2.6f -(0.8f * i),150f), child:true, parentName:name, sortingOrder: 3);
	    }
	}
}
