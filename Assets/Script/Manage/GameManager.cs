using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    GameObject Planet;
    GameObject Sheephorde;
    
    int SheepCount;

    Text UItext;
    Text UIsheep;
    GameObject EndScreen;
    GameObject Player1;
    GameObject Player2;

    int Player1Score;
    int Player2Score;

    public GameObject sheepprefab;
    public float PlanetScale;
    public float initialtime;
    public int initialSheep;

    void Start()
    {
        Planet = GameObject.Find("Planet");
        Sheephorde = GameObject.Find("Sheephorde");

        UItext = GameObject.Find("TimeText").GetComponent<Text>();
        UIsheep = GameObject.Find("SheepText").GetComponent<Text>();

        EndScreen = GameObject.Find("EndScreen");
        EndScreen.SetActive(false);
        Player1 = GameObject.Find("PlayerOne");
        Player2 = GameObject.Find("PlayerTwo");

        SheepCount = 0;
        for (int i = 0; i < initialSheep; i++)
            SheepSpawn();
    }

    void Showremainingtime()
    {
        string timetext;
        if (initialtime - Time.fixedTime >= 0)
        {
            timetext = "Left Time : " + (initialtime - Time.fixedTime).ToString("N0");       //Tostring뒤에 붙은 N0는 소수점 표기를 안한다는거.
        }
        else
        {
            timetext = "Left Time : " + 0;
            finishgame();
        }
        UItext.text = timetext;
    }

    void ShowMySheep()
    {
        string sheeptext;
        SheepCount = Player1.GetComponent<PlayerControltwo>().SheepList.Count;
        if (SheepCount >= 10)
        {
            sheeptext = "My Sheep : " + SheepCount;
        }
        else
        {
            sheeptext = "My Sheep : 0" + SheepCount;
        }
        UIsheep.text = sheeptext;
    }

    /*void IncreaseSheepCount()
    {
        SheepCount++;
    }

    void DecreaseSheepCount()
    {
        if (SheepCount > 0)
            SheepCount--;
        else
            SheepCount = 0;
    }*/

    void finishgame()
    {
        EndScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void SheepSpawn()   //양을 임의의 위치에 소환하는 메서드.
    {
        Vector3 newposition = Random.onUnitSphere * PlanetScale;
        GameObject tempSheep = Instantiate(sheepprefab, newposition, Quaternion.Euler(0,0,0), Sheephorde.transform);
        tempSheep.transform.rotation = Quaternion.FromToRotation(tempSheep.transform.up, newposition) * tempSheep.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Showremainingtime();
        ShowMySheep();
    }
}
