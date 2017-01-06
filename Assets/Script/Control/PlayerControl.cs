using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    //공개 항목
    public int PlayerNumber;
    public int Score;
    public float speed;
    public float angle;
    public float turnspeed;
    
    public List<GameObject> SheepList;
    //비공개 항목
    
    string HorizontalControlName;
    string VerticalControlName;
    float HorizontalInputValue;
    float VerticalInputValue;

    public void Start()
    {
        HorizontalControlName = "Horizontal" + PlayerNumber;
        VerticalControlName = "Vertical" + PlayerNumber;
    }

    public void PlayerInput()
    {
        HorizontalInputValue = Input.GetAxisRaw(HorizontalControlName);
        VerticalInputValue = Input.GetAxisRaw(VerticalControlName);
    }
    
    void KeyboardInput()
    {
        PlayerInput();
        if (HorizontalInputValue != 0)
        {
            Quaternion targetrotation = Quaternion.AngleAxis(angle*HorizontalInputValue, this.transform.up) * this.transform.rotation;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetrotation, turnspeed * Time.deltaTime);
        }
    }

    public void AddSheepList(GameObject Sheep)
    {
        SheepList.Add(Sheep);
    }

    public void ChangeMaster(GameObject Sheep, GameObject target)
    {
        int index = SheepList.IndexOf(Sheep);

        for (int temp = index; temp <= SheepList.Count - 1; temp++)
        {
            SheepList[temp].GetComponent<SheepControl>().Master = target;
            target.GetComponent<PlayerControl>().SheepList.Add(this.SheepList[temp]);
        }
        SheepList.RemoveRange(index, SheepList.Count - index);
    }

    void GoStraight()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void Update()
    {
        GoStraight();
        KeyboardInput();
    }

}
