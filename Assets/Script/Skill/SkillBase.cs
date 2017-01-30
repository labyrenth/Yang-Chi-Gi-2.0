using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour {

    public GameManager GM;
    public GameObject Owner;
    public CameraControl maincamera;
    public Transform SP;        //Skill의 StartingPoint.
    public GameObject TG;       //Skill의 Target.
    public bool IsSkillActive;
    public bool IsLaunched;

    public Transform Parent;

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
        Parent = this.gameObject.GetComponentsInParent<Transform>()[1];
        Parent.rotation = SP.rotation;
        IsLaunched = false;
        GM = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }
}
