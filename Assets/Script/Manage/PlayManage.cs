using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManage : MonoBehaviour {

    public static PlayManage Instance;
    public float speed;
    public float angle;
    public float score;
    public float distance;
    public bool IsBoost;

    private void Awake()                //싱글톤 오브젝트를 만들자!
    {
        if (Instance == null)           //Static 변수를 지정하고 이것이 없을경우 - PlayManage 스크립트를 저장하고 이것이 전 범위적인 싱글톤 오브젝트가 된다.
        {
            DontDestroyOnLoad(this.gameObject);
            this.speed = 15;
            this.angle = 10;
            this.score = 0;
            this.distance = 5;
            this.IsBoost = false;
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

}
