﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepControlThree : MonoBehaviour {

    // 공개 항목
    //public GameObject leader;
    public GameObject Master;

    public GameObject player1;
    public GameObject player2;

    public SheepState SS;
    public GameManager GM;
    public float SmoothMove;
    public int SheepScore;

    // Use this for initialization

    void Start()
    {
        player1 = GameObject.Find("PlayerOne");
        player2 = GameObject.Find("PlayerTwo");
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider col)       //부딪힌 오브젝트의 종류에 따른 반응 정리
    {
        if (col.gameObject.tag == "Head" && col.gameObject != this.Master)
        {
            CheckOwner(col.gameObject);
            ResetTarget(col.gameObject);
        }
    }

    void CheckOwner(GameObject target)          //태그가 Head 인 오브젝트와 부딪혔을 시에 시행하는 함수
    {
        if (SS == SheepState.NOOWNER)
        {
            this.Master = target;
            //ChangeLeader(target);
            SS = SheepState.HAVEOWNER;
            Master.GetComponent<PlayerControlThree>().AddSheepList(this.gameObject);
            this.transform.parent = Master.GetComponent<PlayerControlThree>().SheepArea.transform;
            SetthisLocalPosition();
            GM.FindAndRemoveAtSheepList(this.gameObject);
        }
        else
        {
            //ChangeLeader(target);
            if (Master.gameObject.tag == "Head")
            {
                Master.GetComponent<PlayerControlThree>().ChangeMaster(this.gameObject, target);
                this.transform.parent = target.GetComponent<PlayerControlThree>().SheepArea.transform;
                SetthisLocalPosition();
            }
            else if (Master.gameObject.tag == "Dog" && Master.GetComponent<Dog>().Owner != target)
            {
                Master.GetComponent<Dog>().ChangeMaster(this.gameObject, target);
                ResetTarget(target.gameObject);
            }
        }
    }

    public void SetthisLocalPosition()
    {
        Vector2 Circleposition = Random.insideUnitCircle * 3;
        this.transform.localPosition = new Vector3(Circleposition.x, 0, Circleposition.y);
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void ResetTarget(GameObject col)
    {
        col.GetComponent<PlayerControlThree>().TargetSheep = null;
    }
}
