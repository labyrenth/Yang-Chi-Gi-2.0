using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour {

    // Use this for initialization
    public float speed;
    public float angle;
    public float score;
    public float distance;
    public bool IsBoost;
    public string explaintext1;
    public string explaintext2;
    public string explaintext3;
    Button B;
    Text ET;
    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        B.onClick.AddListener(SendInfotoGlobalObject);
        B.onClick.AddListener(ShowExplain);
        ET = GameObject.Find("ExplainText").GetComponent<Text>();
    }

    void SendInfotoGlobalObject()
    {
        PlayManage.Instance.speed = this.speed;
        PlayManage.Instance.angle = this.angle;
        PlayManage.Instance.score = this.score;
        PlayManage.Instance.distance = this.distance;
        PlayManage.Instance.IsBoost = this.IsBoost;
    }

    void ShowExplain()
    {
        ET.text = this.explaintext1 + "\n" + this.explaintext2 + "\n" + explaintext3;
    }

}