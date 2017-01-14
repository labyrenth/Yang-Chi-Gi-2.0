using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {

    public Image FADE;
    public string targetScene;
    public bool IsSave;
    public Button B;

    private void Start()
    {
        FADE = GameObject.FindGameObjectWithTag("Fadescreen").GetComponent<Image>();
        StartCoroutine("FadeIn");
        B = this.gameObject.GetComponent<Button>();
        B.onClick.AddListener(LoadScene);
        B.onClick.AddListener(SavePref);
    }

    void LoadScene()
    {
        StartCoroutine("FadeOutAndLoadScene");
    }

    void SavePref()
    {
        if (IsSave == true)
        {
            PlayManage.Instance.SaveData();
            PlayerPrefs.Save();
        }
    }

    IEnumerator FadeIn()
    {
        FADE.gameObject.SetActive(true);
        for (float i = 1f; i >= 0; i -= 0.025f)
        {
            Color color = new Vector4(0, 0, 0, i);
            FADE.color = color;
            yield return 0;
        }
        FADE.gameObject.SetActive(false);
    }

    IEnumerator FadeOutAndLoadScene()
    {
        FADE.gameObject.SetActive(true);
        for (float i = 0f; i <= 1; i += 0.025f)
        {
            Color color = new Vector4(0, 0, 0, i);
            FADE.color = color;
            yield return 0;
        }
        SceneManager.LoadScene(targetScene);
    }
}
