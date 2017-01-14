using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManage : MonoBehaviour {

    public static PlayManage Instance;

    public string playerID;
    public int playerlevel;
    public float speed;
    public float angle;
    public float score;
    public float distance;
    public bool IsBoost;
    public float EXP;
    public float sound;
    public int Quality;

    public float PlayerScore;
    public float EnemyScore;

    private void Awake()                //싱글톤 오브젝트를 만들자!
    {
        if (Instance == null)           //Static 변수를 지정하고 이것이 없을경우 - PlayManage 스크립트를 저장하고 이것이 전 범위적인 싱글톤 오브젝트가 된다.
        {
            DontDestroyOnLoad(this.gameObject);
            LoadData();
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);   //싱글톤 오브젝트가 있을경우 다른 오브젝트를 제거.
        }
        
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void SaveData()
    {
        PlayerPrefs.SetString("PLAYERID", playerID);
        PlayerPrefs.SetInt("PLAYERLEVEL", playerlevel);
        PlayerPrefs.SetFloat("INITIALSPEED",speed);
        PlayerPrefs.SetFloat("INITIALANGLE", angle);
        PlayerPrefs.SetFloat("INITIALSCORE",score);
        PlayerPrefs.SetFloat("INITIALDISTANCE",distance);
        if (IsBoost == false)
            PlayerPrefs.SetInt("INITIALBOOST", 0);
        else
            PlayerPrefs.SetInt("INITIALBOOST", 1);
        PlayerPrefs.SetFloat("SOUND", sound);
        PlayerPrefs.SetInt("QUALITY", Quality);
        PlayerPrefs.SetFloat("EXP", EXP);
    }

    public void LoadData()
    {
        this.playerID = PlayerPrefs.GetString("PLAYERID", "Beginner");
        this.playerlevel = PlayerPrefs.GetInt("PLAYERLEVEL", 1);
        this.speed = PlayerPrefs.GetFloat("INITIALSPEED",15);
        this.angle = PlayerPrefs.GetFloat("INITIALANGLE", 10);
        this.score = PlayerPrefs.GetFloat("INITIALSCORE", 0);
        this.distance = PlayerPrefs.GetFloat("INITIALDISTANCE", 5);
        int boostcheck = PlayerPrefs.GetInt("INITIALBOOST", 0);
        if (boostcheck == 0)
            this.IsBoost = false;
        else
            this.IsBoost = true;
        this.sound = PlayerPrefs.GetFloat("SOUND", 50);
        this.Quality = PlayerPrefs.GetInt("QUALITY", 2);
        this.EXP = PlayerPrefs.GetFloat("EXP", 0);
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }
}
