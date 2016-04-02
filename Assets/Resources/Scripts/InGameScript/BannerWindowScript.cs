using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class BannerWindowScript : MonoBehaviour
{
    public GameObject _currentTab; 

	void Start ()
	{
	    var tab = CurrentTab.Core;
        for (int i = 0; i < 7; i++)
        {
            var banner = StaticScripts.CreateGameObj(string.Format("Banner_{0}", i), "Borders/InGame/Paragon_BannerInactive", new Vector3(0.7f, 1f),
                 new Vector3(-1.3f, 2.2f - (i * 0.95f)), true, 1, true, typeof(BannerTabScript), true, name);

            banner.transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, 270f));
            banner.GetComponent<BannerTabScript>().SetRepresentedTab(tab);
            if(i == 0)
                banner.GetComponent<BannerTabScript>().SetTabSelected();
            tab = (CurrentTab) ((int) tab + 1);
        }	
	}
}
