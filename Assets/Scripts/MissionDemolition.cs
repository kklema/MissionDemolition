using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void Update()
    {
        UpdateGUI();

        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelend;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    private void UpdateGUI()
    { 
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    private void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile"); // ”ничтожить прежние снар€ды, если они существуют
        foreach (GameObject projectile in gos)
        {
            Destroy(projectile);
        }

        // —оздать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // ѕереустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // —бросить цель
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    private void NextLevel()
    {
        level++;
        if (level == levelMax)        
            level = 0;
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCamera.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCamera.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCamera.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}

public enum GameMode
{
    idle,
    playing,
    levelend
}
