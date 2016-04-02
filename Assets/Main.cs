using System;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Assets.Resources.Scripts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using FileMode = System.IO.FileMode;
using Random = System.Random;

public class Main : MonoBehaviour
{
    public Camera mainCamera;
    private GameState _currentGameState;
    private GameObject currentStateObject;
    private GameObject SceneGameObject;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            _currentGameState = value;

            switch (value)
            {
                case GameState.Login:
                    if(SceneGameObject != null)
                        ReadWriteAccountDateScript.WriteAccountData(Account.CurrentAccount.AccountName);
                    LoginMenu();
                    break;
                case GameState.MainMenu:
                    if (Account.CurrentAccount.ListOfHeroes.Count == 0)
                    {
                        CurrentGameState = GameState.CharacterCreation;
                        break;
                    }
                    MainMenu();     
                    break;
                case GameState.CharacterCreation:
                    CharacterCreationStaff();
                    break;
                case GameState.InGame:
                    InGame();
                    break;
                case GameState.InGamePause:
                    break;
                case GameState.CharacterSelection:
                     SelectHeroMenu();
                    break;
            }
        }
    }


    void Start()
    {
        SceneGameObject = new GameObject("SceneGameObject");
        SceneGameObject.transform.parent = transform;
       // ReadDDS(@"F:\Users\Oksana\Desktop\DiabloModels\textures\2DUIQuestReward.dds", false);
        MakeCameraDarker();
        ShowTopAndBottomBorders();
        LoginMenu();
    }

    void Update()
    {
    }

    void CharacterCreationStaff()
    {
        try
        {
            if (currentStateObject != null)
                currentStateObject.SetActive(false);


            var CharacterCreationGameObject = SceneGameObject.transform.FindChild("CharacterCreationSceneObject") != null ? SceneGameObject.transform.FindChild("CharacterCreationSceneObject").gameObject : null;

            if (CharacterCreationGameObject != null)
                CharacterCreationGameObject.SetActive(true);
            else
            {
                CharacterCreationGameObject = new GameObject("CharacterCreationSceneObject");
                CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
                CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
                CharacterCreationGameObject.transform.parent = SceneGameObject.transform;
                CharacterCreationGameObject.AddComponent<CreateCharacterObjectScript>();

                Act1Background();

            }

            currentStateObject = CharacterCreationGameObject;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
    }

    void MainMenu()
    {
        if (currentStateObject != null)
            currentStateObject.SetActive(false);

        var MainMenuGameObject = SceneGameObject.transform.FindChild("MainMenuSceneObject") != null ? SceneGameObject.transform.FindChild("MainMenuSceneObject").gameObject : null;

        if (MainMenuGameObject != null)
            MainMenuGameObject.SetActive(true);
        else
        {
            MainMenuGameObject = new GameObject("MainMenuSceneObject");
            MainMenuGameObject.transform.position = new Vector3(0, 0, 10);
            MainMenuGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
            MainMenuGameObject.transform.parent = SceneGameObject.transform;
            MainMenuGameObject.AddComponent<MainMenuObjectScript>();

            Act5Background();
        }
        currentStateObject = MainMenuGameObject;
    }

    void InGame()
    {
        if (currentStateObject != null)
            currentStateObject.SetActive(false);

        var InGameObject = SceneGameObject.transform.FindChild("InGameSceneObject") != null ? SceneGameObject.transform.FindChild("InGameSceneObject").gameObject : null;

        if(InGameObject != null)
            InGameObject.SetActive(true);
        else
        {
            InGameObject = new GameObject("InGameSceneObject");
            InGameObject.transform.position = new Vector3(0,0,10);
            InGameObject.transform.parent = SceneGameObject.transform;

            var enemyBorder = StaticScripts.CreateGameObj("enemyBorder",
                "Borders/InGame/BattleNetModalNotifications_MediumDialog", new Vector3(1f, 1f), new Vector3(-7.5f, -2.75f, 10f), true, 1, true, typeof(EnemyScript), true, "InGameSceneObject");

            StaticScripts.CreateGameObj("paragonBorder", "Borders/InGame/SkillsPane_Paragon_Base", new Vector3(0.75f, 0.9f), new Vector3(0.15f, -4.5f, 10f), child: true, parentName: "InGameSceneObject");
            
            var bannerWindowObject = new GameObject("BannerWindowGameObject");
            bannerWindowObject.transform.parent = InGameObject.transform;
            bannerWindowObject.AddComponent<BannerWindowScript>();

        }
        currentStateObject = InGameObject;

        Act1Background();
    }

    void SelectHeroMenu()
    {
        if (currentStateObject != null)
            currentStateObject.SetActive(false);

        GameObject CharacterSelectionSceneObject = SceneGameObject.transform.FindChild("CharacterSelectionSceneObject") != null ? SceneGameObject.transform.FindChild("CharacterSelectionSceneObject").gameObject : null;
        if (CharacterSelectionSceneObject != null)
            CharacterSelectionSceneObject.SetActive(true);
        else
        {
            CharacterSelectionSceneObject = new GameObject("CharacterSelectionSceneObject");
            CharacterSelectionSceneObject.transform.position = new Vector3(0, 0, 10);
            CharacterSelectionSceneObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
            CharacterSelectionSceneObject.transform.parent = SceneGameObject.transform;
            CharacterSelectionSceneObject.AddComponent<CharacterSelectionObjectScript>();

            Act5Background();


            var HeroListObject = new GameObject("HeroListGameObject");
            //CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
            HeroListObject.AddComponent<HeroScrollingList>();
            HeroListObject.transform.parent = CharacterSelectionSceneObject.transform;


            var shader = Shader.Find("Masked/Mask");

            var DepthMaskTop = new GameObject("DepthMaskTopGameObject");
            DepthMaskTop.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
            DepthMaskTop.GetComponent<SpriteRenderer>().material = new Material(shader);
            DepthMaskTop.transform.localScale = new Vector3(500, 200);
            DepthMaskTop.transform.position = new Vector3(-9f, 3.35f, 140f);
            DepthMaskTop.transform.parent = CharacterSelectionSceneObject.transform;

            var DepthMaskBottom = new GameObject("DepthMaskBottomGameObject");
            DepthMaskBottom.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
            DepthMaskBottom.GetComponent<SpriteRenderer>().material = new Material(shader);
            DepthMaskBottom.transform.localScale = new Vector3(500, 300);
            DepthMaskBottom.transform.position = new Vector3(-9f, -5.31f, 140f);
            DepthMaskBottom.transform.parent = CharacterSelectionSceneObject.transform;
        }

        currentStateObject = CharacterSelectionSceneObject;
    }

    void Act5Background()
    {
        var gameObj = GameObject.Find("BackgroundObject");
        if (gameObj != null)
        {
            DestroyImmediate(gameObj);
        }

        var CharacterCreationGameObject = new GameObject("BackgroundObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        CharacterCreationGameObject.transform.parent = transform;
        CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());

        StaticScripts.CreateGameObj("StairsObject", "Backgrounds/Act5/Act5_foreground_0005_Layer-6", new Vector3(0.95f, 0.95f), new Vector3(-9.9f, -7.7f, 20f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("FloorObject", "Backgrounds/Act5/Act5_foreground_0004_Layer-5", new Vector3(0.87f, 0.87f), new Vector3(-9f, -2.35f, 30f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("GatesObject", "Backgrounds/Act5/Act5_bridge_0000_Layer-1", new Vector3(0.76f, 0.76f), new Vector3(-8f, -1.65f, 35f), child: true, parentName: "BackgroundObject");

        StaticScripts.CreateGameObj("House1", "Backgrounds/Act5/Act5_foreground_0002_Layer-3", new Vector3(0.6f, 0.6f), new Vector3(-5.7f, 1.4f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House2", "Backgrounds/Act5/Act5_buildings_0000_Layer-1", new Vector3(0.65f, 0.65f), new Vector3(-2.3f, -1f, 40f), child: true, parentName: "BackgroundObject");

        StaticScripts.CreateGameObj("House3", "Backgrounds/Act5/Act5_buildings_0004_Layer-5", new Vector3(0.75f, 0.75f), new Vector3(2.35f, 0.73f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House4", "Backgrounds/Act5/Act5_buildings_0002_Layer-3", new Vector3(0.6f, 0.6f), new Vector3(2.3f, 1f, 50f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House5", "Backgrounds/Act5/Act5_buildings_0005_Layer-6", new Vector3(0.75f, 0.75f), new Vector3(-3.25f, -0.05f, 40f), child: true, parentName: "BackgroundObject");

        StaticScripts.CreateGameObj("Statue1", "Backgrounds/Act5/Act5_foreground_0000_Layer-1", new Vector3(0.8f, 0.8f), new Vector3(-9.8f, -2f, 25f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("Statue2", "Backgrounds/Act5/Act5_foreground_0001_Layer-2", new Vector3(0.75f, 0.75f), new Vector3(6.15f, -1.2f, 25f), child: true, parentName: "BackgroundObject");
    }

    void Act1Background()
    {
        var gameObj = GameObject.Find("BackgroundObject");
        if (gameObj != null)
        {
            DestroyImmediate(gameObj);
        }

        var CharacterCreationGameObject = new GameObject("BackgroundObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        CharacterCreationGameObject.transform.parent = transform;
        CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());

        StaticScripts.CreateGameObj("CharacterCreationBackground", @"Backgrounds/CharacterCreation/Battlenet_MainScreenBackground_flipped", new Vector3(1.1f, 1.1f), new Vector3(-12.1f, -5.5f, 100f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("CharacterCreationTree", @"Backgrounds/CharacterCreation/Battlenet_MainScreenBackground_Alpha_flipped", new Vector3(1.3f, 1.3f), new Vector3(-11.65f, -6.5f, 95f), child: true, parentName: "BackgroundObject");

        #region CharacterPlatform

        StaticScripts.CreateGameObj("platform2", @"Backgrounds/CharacterCreation/CharacterPlatform_0000_Layer-1", new Vector3(1.1f, 0.6f), new Vector3(-2.3f, -5.8f, 60f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform1", @"Backgrounds/CharacterCreation/CharacterPlatform_0001_Layer-2", new Vector3(1.15f, 0.9f), new Vector3(-4.88f, -5.45f, 50f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform3", @"Backgrounds/CharacterCreation/CharacterPlatform_0004_Layer-5", new Vector3(1.2f, 1.3f), new Vector3(-12.30f, -8.55f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform4", @"Backgrounds/CharacterCreation/CharacterPlatform_0003_Layer-4", new Vector3(0.7f, 0.7f), new Vector3(-4f, -4f, 55f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform5", @"Backgrounds/CharacterCreation/CharacterPlatform_0002_Layer-3", new Vector3(1f, 1f), new Vector3(6.57f, -1.65f, 40f), child: true, parentName: "BackgroundObject");

        #endregion
    }

    void LoginMenu()
    {
        if (currentStateObject != null)
            Destroy(currentStateObject);

        Act5Background();

        var LoginGameObject = SceneGameObject.transform.FindChild("LoginSceneObject") != null ? SceneGameObject.transform.FindChild("LoginSceneObject").gameObject : null;

        if (LoginGameObject != null)
            LoginGameObject.SetActive(true);
        else
        {
            LoginGameObject = new GameObject("LoginSceneObject");
            LoginGameObject.transform.position = new Vector3(0, 0, 10);
            LoginGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
            LoginGameObject.transform.parent = SceneGameObject.transform;

            StaticScripts.CreateGameObj("LoginAndPasswordTextBoxes", @"UI/Login/BattleNetLogin_InputContainer", new Vector3(0.85f, 0.85f), new Vector3(-1.96f, -2.54f, 10f), true, 1, true, typeof (LoginPasswordTextBoxesScript), true, "LoginSceneObject");
            StaticScripts.CreateGameObj("Diablo3Logo", @"Logo/BattleNetLogo_Diablo3_x1", new Vector3(0.82f, 0.85f), new Vector3(-4.69f, -1.13f, 5f), child: true, parentName: "LoginSceneObject");
            StaticScripts.CreateGameObj("RememberMe", @"UI/BattleNetLogin_CheckboxUp", new Vector3(0.85f, 0.85f), new Vector3(-0.98f, -2.195f, 0f), true, 1, true, typeof (CheckBoxMouseEvents), child: true, parentName: "LoginSceneObject");
            StaticScripts.CreateTextObj("AccountNameText", "Account Name", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.972f, 0.17f, 0f), FontType.DiabloFont, 110, new Color32(255, 246, 222, 255), child: true, parentName: "LoginSceneObject");
            StaticScripts.CreateTextObj("PasswordText", "Password", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.635f, -0.92f, 0f), FontType.DiabloFont, 110, new Color32(255, 246, 222, 255), child: true, parentName: "LoginSceneObject");


            StaticScripts.CreateGameObj("ManageAccountsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -1.83f, 10f), true, 1, true, typeof (ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
            GameObject.Find("ManageAccountsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
            StaticScripts.CreateTextObj("ManageAccountsButtonText", "Manage Accounts", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.8f, -1.55f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

            StaticScripts.CreateGameObj("OptionsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -2.28f, 10f), true, 1, true, typeof (ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
            GameObject.Find("OptionsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
            StaticScripts.CreateTextObj("OptionsButtonText", "Options", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.37f, -2f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

            StaticScripts.CreateGameObj("CreditsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -2.73f, 10f), true, 1, true, typeof (ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
            GameObject.Find("CreditsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
            StaticScripts.CreateTextObj("CreditsButtonText", "Credits", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.35f, -2.45f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

            StaticScripts.CreateGameObj("ExitButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -3.95f, 10f), true, 1, true, typeof (ExitButtonScript), child: true, parentName: "LoginSceneObject");
            StaticScripts.CreateTextObj("ExitButtonText", "Exit", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.25f, -3.67f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

            StaticScripts.CreateGameObj("LoginButton", @"Buttons/BattleNetButton_RedDisabled_397x66", new Vector3(0.845f, 0.85f), new Vector3(-1.66f, -3.2f, 10f), true, 1, true, typeof (LoginButtonScript), child: true, parentName: "LoginSceneObject");
            GameObject.Find("LoginButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
            StaticScripts.CreateTextObj("LoginButtonText", "Login", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.3f, -2.84f, 0f), FontType.DiabloFont, 96, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

            StaticScripts.CreateGameObj("BreakingNews", @"UI/Login/BattleNetLogin_BreakingNewsBg", new Vector3(0.85f, 0.85f), new Vector3(5.3f, -4f), child: true, parentName: "LoginSceneObject");
            StaticScripts.CreateTextObj("BreakingNewsHeaderText", "Login Help", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 4.15f, 0f), FontType.DiabloFont, 150, new Color32(255, 246, 222, 255), child: true, parentName: "BreakingNews");
            StaticScripts.CreateTextObj("BreakingNewsBodyText", "\tYou can create new account by entering \ndata in \"Account Name\" and \"Password\" fields. \nBe sure that you have at least 6 characters at \n\"Account Name\" field and at least 1 character \nat \"Password\" field.", new Vector3(0.02f, 0.02f), new Vector3(0.2f, 3.2f, 0f), FontType.StandartFont, 80, new Color32(255, 246, 222, 255), child: true, parentName: "BreakingNews");
        }
    }

    void ShowTopAndBottomBorders()
    {
        StaticScripts.CreateGameObj("footer", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_4x3", new Vector3(1f, 0.5f), new Vector3(-10.24f, -5f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);
        StaticScripts.CreateGameObj("footerRight", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_EndCapRight", new Vector3(1f, 0.5f), new Vector3(10.24f, -5f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);
        StaticScripts.CreateGameObj("footerLeft", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_EndCapLeft", new Vector3(1f, 0.5f), new Vector3(-12.8f, -5f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);

        StaticScripts.CreateGameObj("header", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_4x3", new Vector3(0.7f, 1f), new Vector3(-7.168f, 4.36f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);
        StaticScripts.CreateGameObj("headerLeft", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_EndCapLeft", new Vector3(0.7f, 1f), new Vector3(-8.96f, 4.36f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);
        StaticScripts.CreateGameObj("headerRight", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_EndCapRight", new Vector3(0.7f, 1f), new Vector3(7.168f, 4.36f, 10f), child: true, parentName: "MainLogic", sortingOrder: 7);
    }

    void OnDestroy()
    {
        ReadWriteAccountDateScript.WriteAccountData(Account.CurrentAccount.AccountName);
    }

    void MakeCameraDarker()
    {
        var texture = new Texture2D(1, 1);
        var obj = new GameObject("CameraDarkFilter");
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(1920, 1080);
        obj.AddComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2());
        obj.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 100);
        obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 5f));
    }

    void ReadDDS(string path, bool alpha)
    {
        /*
         * Метод для чтения изначально "выдранных" из Diablo 3 dds файлов, которые Unity отказывается считывать правильно - появляется сдвиг на 32 пикселя вправо, а сама картинка перевернута. 
         * Кроме того, файлы, в которых объедененны несколько спрайтов, имеют при себе txt файл с набором "Имя" - "Координаты", благодаря которым можно порезать файл на отдельные спрайты.
         */
        int height, width;
        using (var br = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            br.ReadBytes(12);
            height = BitConverter.ToInt32(br.ReadBytes(4), 0);
            width = BitConverter.ToInt32(br.ReadBytes(4), 0);
            br.ReadBytes(64);

            var type = Encoding.UTF8.GetString(br.ReadBytes(4));

            if (type != "DXT5" && type != "DXT3") return;
        }

        var bytes = File.ReadAllBytes(path);
        var texture = new Texture2D((int) width, (int) height, TextureFormat.DXT5, false);
        var nonflippedtexture = new Texture2D(texture.width, texture.height);
        texture.LoadRawTextureData(bytes.ToArray());
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                var pixel = texture.GetPixel(i, j);
                nonflippedtexture.SetPixel(i, j, pixel);
            }
        }
        nonflippedtexture.Apply();
        var flippedtexture = new Texture2D(texture.width, texture.height);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                var pixel = texture.GetPixel(i, j);
                if (alpha)
                    pixel.a = 1;
                if (i >= 31)
                    flippedtexture.SetPixel(i - 31, texture.height - 1 - j, pixel);
                else
                {
                    flippedtexture.SetPixel(width - 1 - (31 - i), texture.height - j, pixel);
                }
            }
        }

        for (int i = flippedtexture.width - 31; i < flippedtexture.width; i++)
        {
            var arr = flippedtexture.GetPixels(i, 0, 1, flippedtexture.height);
            var flippedArray = new Texture2D(1, flippedtexture.height).GetPixels();
            Array.Copy(arr, 0, flippedArray, 3, arr.Length - 3);
            Array.Copy(arr, arr.Length - 3, flippedArray, 0, 3);
            flippedtexture.SetPixels(i, 0, 1, flippedtexture.height, flippedArray);
        }

        flippedtexture.Apply();

        using (var br = new BinaryWriter(new FileStream(path.Replace(".dds", "_flipped.png"), FileMode.OpenOrCreate)))
        {
            br.Write(flippedtexture.EncodeToPNG());
        }

        if (File.Exists(path.Replace(".dds", "_atlas.txt")))
        {
            using (var sr = new StreamReader(path.Replace(".dds", "_atlas.txt")))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var FirstLine = sr.ReadLine().Split('\t');
                    var tex = new Texture2D(int.Parse(FirstLine[7]) - (int.Parse(FirstLine[5]) + 1), int.Parse(FirstLine[8]) - (int.Parse(FirstLine[6]) + 2));
                    int x = 0;
                    for (int i = int.Parse(FirstLine[5]) + 1; i <= int.Parse(FirstLine[7]); i++)
                    {
                        int y = 0;
                        for (int j = int.Parse(FirstLine[6]) + 1; j < int.Parse(FirstLine[8]); j++)
                        {
                            var color = flippedtexture.GetPixel(i, flippedtexture.height - j);
                            tex.SetPixel(x, tex.height - y, color);
                            y++;
                        }
                        x++;
                    }
                    tex.Apply();
                    using (var br = new BinaryWriter(new FileStream(FirstLine[0] + ".png", FileMode.OpenOrCreate)))
                    {
                        br.Write(tex.EncodeToPNG());
                    }
                }
            }
        }
    }
}




