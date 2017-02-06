using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {

    public string targetScene;
    public GameObject clientObject;

    public bool IsSave;
    public Button B;

    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        B.onClick.AddListener(LoadScene);
        B.onClick.AddListener(SavePref);
        B.onClick.AddListener(Matching);
    }
    void Matching()
    {
        clientObject.GetComponent<KingGodClient>().Matching();
    }
    void LoadScene()
    {
        StartCoroutine(PlayManage.Instance.LoadScene(targetScene));
    }

    void SavePref()
    {
        if (IsSave == true)
        {
            PlayManage.Instance.SaveData();
            PlayerPrefs.Save();
        }
    }
}
