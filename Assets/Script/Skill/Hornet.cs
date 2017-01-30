using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : SkillBase {

    public float Speed;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        StartCoroutine(HornetLife());
	}

    void HornetAction(GameObject Target)
    {
        float slowspeed = Speed / 10;
        float Gospeed;

        if (!Target.GetComponent<PlayerControltwo>().InHQ)
        {
            Gospeed = Speed;
        }
        else
        {
            Gospeed = slowspeed;
        }
        
        if (IsLaunched)
        {
            Quaternion targetrotation = Quaternion.LookRotation(this.transform.position - Target.transform.position);
            Parent.rotation = Quaternion.RotateTowards(Parent.rotation, targetrotation, Gospeed * Time.deltaTime);
        }
    }

    IEnumerator HornetLife()
    {
        yield return new WaitForSeconds(5f);
        IsLaunched = true;
        yield return new WaitForSeconds(10f);
        Parent.gameObject.SetActive(false);
    }

    public override void SkillAction(Collider other)
    {
        if (other.gameObject.tag == "Head" && other.gameObject == TG)
        {
            if (!other.gameObject.GetComponent<PlayerControltwo>().InHQ)
            {
                other.gameObject.GetComponent<PlayerControltwo>().StartCoroutine(other.gameObject.GetComponent<PlayerControltwo>().HornetAttack());
            }
            Parent.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        HornetAction(TG);
    }
}
