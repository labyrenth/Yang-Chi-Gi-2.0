using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillState
{
    ACTIVATED,
    LAUNCHED
}

public class SkillBase : MonoBehaviour {

    public GameManager GM;
    public GameObject Owner;

    public GameObject TG;       //Skill의 Target.

    public SkillState SS;

    public Transform Parent;

    public bool IsSkillNeedCameraFix;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.Owner)
        {
            SkillAction(other);
        }
    }

    public virtual void SkillAction(Collider other) { }

    public virtual void Start()
    {

        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public virtual bool SetInstance(GameObject IO, GameObject ITG)
    {
        this.Owner = IO;
        this.TG = ITG;
        return ((Owner != null && TG != null) ? true : false);
    }

    public virtual bool FindSkillNeedCameraFix()
    {
        return IsSkillNeedCameraFix;
    }
}
