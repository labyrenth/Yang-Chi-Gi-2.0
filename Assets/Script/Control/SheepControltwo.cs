using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// 항상 일정 거리를 유지하도록!
public class SheepControltwo : MonoBehaviour
{
    // 공개 항목
    public GameObject leader;
    public GameObject Master;
 
    public SheepState SS;

    // Use this for initialization

    void OnTriggerEnter(Collider col)       //부딪힌 오브젝트의 종류에 따른 반응 정리
    {
        if (col.gameObject.tag == "Head" && col.gameObject != this.Master)
        {
            CheckOwner(col.gameObject);
        }
    }

    void CheckOwner(GameObject target)          //태그가 Head 인 오브젝트와 부딪혔을 시에 시행하는 함수
    {
        if (SS == SheepState.NOOWNER)
        {
            this.Master = target;
            ChangeLeader(target);
            SS = SheepState.HAVEOWNER;
            Master.GetComponent<PlayerControltwo>().AddSheepList(this.gameObject);
        }
        else
        {
            ChangeLeader(target);
            Master.GetComponent<PlayerControltwo>().ChangeMaster(this.gameObject, target);
        }
    }

    void ChangeLeader(GameObject target)
    {
        if (target.GetComponent<PlayerControltwo>().SheepList.Count == 0)
        {
            this.leader = target;
        }
        else
        {
            this.leader = target.GetComponent<PlayerControltwo>().SheepList[target.GetComponent<PlayerControltwo>().SheepList.Count - 1];
        }
    }
}