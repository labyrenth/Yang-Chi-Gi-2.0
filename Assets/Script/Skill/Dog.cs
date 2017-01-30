using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DogState
{
    GO,
    BACK
}

public class Dog : SkillBase {

    public SpriteRenderer ArrowImage;
    public DogState DS;
    public List<GameObject> SheepList;

    public float Speed;
    public float mindistance;

    Transform curtransform;
    Transform prevtransform;

    //bool IsbackTohome;

    public override void Start()
    {
        base.Start();
        ArrowImage = Parent.gameObject.GetComponentInChildren<SpriteRenderer>();
        //IsbackTohome = false;
        DS = DogState.GO;
        
    }

    void CameraAction()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            float xDif = Input.mousePosition.x - Screen.width / 2;
            float yDif = Input.mousePosition.y - Screen.height / 2;
            float angle = Mathf.Atan2(xDif, yDif) * Mathf.Rad2Deg;
            Parent.rotation = Quaternion.AngleAxis(angle, Parent.up); 
        }
    }

    void GoStraight()
    {
        float betangle = Vector3.Angle(SP.position, this.transform.position);
        if (betangle > 90)
        {
            DS = DogState.BACK;
        }
        if (DS == DogState.GO)
        {
            Parent.Rotate(new Vector3(-Speed * Time.deltaTime, 0, 0));
        }
        else if (DS == DogState.BACK)
        {
            Parent.Rotate(new Vector3(Speed * Time.deltaTime, 0, 0));
        }
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

            float dis = Vector3.Distance(prevtransform.position, curtransform.position);
            Vector3 newpos = prevtransform.position;

            float T = Time.deltaTime * dis / mindistance * Speed;
            if (T > 0.5f)
                T = 0.5f;
            curtransform.position = Vector3.Slerp(curtransform.position, newpos, T);
            curtransform.rotation = Quaternion.Slerp(curtransform.rotation, prevtransform.rotation, T);
        }
    }

    public void ChangeMaster(GameObject Sheep, GameObject target)
    {
        int index = SheepList.IndexOf(Sheep);

        for (int temp = index; temp <= SheepList.Count - 1; temp++)
        {
            SheepList[temp].GetComponent<SheepControltwo>().Master = target;
            GM.FindAndRemoveAtSheepList(this.SheepList[temp]);
            target.GetComponent<PlayerControltwo>().SheepList.Add(this.SheepList[temp]);
        }
        SheepList.RemoveRange(index, SheepList.Count - index);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "BronzeSheep" && other.GetComponent<SheepControltwo>().SS != SheepState.HAVEOWNER)
        {
            bool IsthisSheepInList = false;
            foreach (GameObject i in this.SheepList)
            {
                if (other.gameObject == i)
                {
                    IsthisSheepInList = true;
                }
            }
            if (IsthisSheepInList == false)
            {
                other.GetComponent<SheepControltwo>().Master = this.gameObject;
                other.GetComponent<SheepControltwo>().SS = SheepState.HAVEOWNER;
                this.SheepList.Add(other.gameObject);
            }
        }
    }

    public IEnumerator EnterHQ()
    {
        if (SheepList.Count > 0)
        {
            for (int i = SheepList.Count - 1; i >= 0; i--)
            {
                GM.FindAndRemoveAtSheepList(this.SheepList[i]);
                SheepList[i].SetActive(false);
                SheepList.RemoveAt(i);
                Owner.GetComponent<PlayerControltwo>().Score++;
                yield return new WaitForSeconds(0.1f);
            }
            this.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (IsSkillActive)
        {
            maincamera.IsSkillCutScene = true;
            CameraAction();
        }
        else
        {
            maincamera.IsSkillCutScene = false;
        }
        if (IsLaunched)
        {
            ArrowImage.gameObject.SetActive(false);
            GoStraight();
        }
        LeaderSheep();
    }
}
