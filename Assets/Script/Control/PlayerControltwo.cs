using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControltwo : MonoBehaviour
{

	//공개 항목
    public int PlayerNumber;
    public int Score;
    public float speed;
    public float angle;
    public float turnspeed;
    public float mindistance;

    public List<GameObject> SheepList;
    //비공개 항목

    string HorizontalControlName;
    string VerticalControlName;
    float HorizontalInputValue;
    float VerticalInputValue;

    Transform curtransform;
    Transform prevtransform;

    public void Start()
    {
        HorizontalControlName = "Horizontal" + PlayerNumber;
        VerticalControlName = "Vertical" + PlayerNumber;
        mindistance = 5f;
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
            Quaternion targetrotation = Quaternion.AngleAxis(angle * HorizontalInputValue, this.transform.up) * this.transform.rotation;
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

    void LeaderSheep()
    {
        for (int i = 0; i < SheepList.Count; i++)
        {
            if (i == 0)
            {
                curtransform = SheepList[i].transform;
                prevtransform = this.transform;
            }
            else
            {
                curtransform = SheepList[i].transform;
                prevtransform = SheepList[i - 1].transform;
            }

            float dis = Vector3.Distance(prevtransform.position,curtransform.position);
            Vector3 newpos = prevtransform.position;

            float T = Time.deltaTime * dis / mindistance * speed;
            if (T > 0.5f)
                T = 0.5f;
            curtransform.position = Vector3.Slerp(curtransform.position, newpos, T);
            curtransform.rotation = Quaternion.Slerp(curtransform.rotation, prevtransform.rotation, T);
        }
    }

    public void Update()
    {
        GoStraight();
        KeyboardInput();
        LeaderSheep();
    }
}
