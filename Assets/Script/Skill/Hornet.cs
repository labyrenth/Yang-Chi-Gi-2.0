using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : SkillBase {

    public float Speed;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        Parent = this.gameObject.GetComponentsInParent<Transform>()[1];
        StartCoroutine(HornetLife());
        SS = SkillState.ACTIVATED;
	}

    void HornetAction(GameObject Target)
    {
        float slowspeed = Speed / 10;
        float Gospeed;

        if (Target != null && !Target.GetComponent<PlayerControlThree>().InHQ )
        {
            Gospeed = Speed;
        }
        else
        {
            Gospeed = slowspeed;
        }
        
        if (SS == SkillState.LAUNCHED)
        {
            Quaternion targetrotation = Quaternion.LookRotation(this.transform.position - Target.transform.position);
            Parent.rotation = Quaternion.RotateTowards(Parent.rotation, targetrotation, Gospeed * Time.deltaTime);
        }
    }

    IEnumerator HornetLife()
    {
        yield return new WaitForSeconds(5f);
        SS = SkillState.LAUNCHED;
        yield return new WaitForSeconds(10f);
        Parent.gameObject.SetActive(false);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "Head" && other.gameObject == TG)
        {
            if (!other.gameObject.GetComponent<PlayerControlThree>().InHQ)
            {
                other.gameObject.GetComponent<PlayerControlThree>().StartCoroutine(other.gameObject.GetComponent<PlayerControlThree>().HornetAttack());
            }
            Parent.gameObject.SetActive(false);
        }
    }

    public override bool SetInstance(GameObject IO, GameObject ITG)
    {
        return base.SetInstance(IO, ITG);
    }

    public override bool FindSkillNeedCameraFix()
    {
        return base.FindSkillNeedCameraFix();
    }

    private void Update()
    {
        HornetAction(TG);
    }
}
