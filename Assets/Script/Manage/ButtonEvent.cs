using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour {

    public string targetScene;
    public Button B;

    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        B.onClick.AddListener(LoadScene);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(targetScene);
    }
}
