using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HQControl : MonoBehaviour {

    public SpriteRenderer Arrow;
    public Transform ArrowPivot;

    private void Start()
    {
        Arrow = this.GetComponentInChildren<SpriteRenderer>();
        ArrowPivot = Arrow.GetComponentsInParent<Transform>()[1];
        Arrow.gameObject.SetActive(false);
    }

    void CameraAction()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            float xDif = Input.mousePosition.x - Screen.width / 2;
            float yDif = Input.mousePosition.y - Screen.height / 2;
            float angle = Mathf.Atan2(xDif, yDif) * Mathf.Rad2Deg;
            ArrowPivot.rotation = Quaternion.AngleAxis(angle, this.transform.up);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Head" && col.gameObject.GetComponent<PlayerControlThree>().HQ == this.gameObject)
        {
            StartCoroutine(col.gameObject.GetComponent<PlayerControlThree>().EnterHQ());
            col.gameObject.GetComponent<PlayerControlThree>().InHQ = true;
        }
        else if (col.gameObject.tag == "Dog")
        {
            if (col.gameObject.GetComponent<Dog>().SheepList.Count == 0)
            {
                if (col.gameObject.GetComponent<Dog>().DS == DogState.GO)
                {
                    return;
                }
                else
                {
                    col.gameObject.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(col.gameObject.GetComponent<Dog>().EnterHQ());
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Head" && col.gameObject.GetComponent<PlayerControlThree>().HQ == this.gameObject)
        {
            col.gameObject.GetComponent<PlayerControlThree>().InHQ = false;
        }
    }

    private void Update()
    {
        if (Arrow.gameObject.activeSelf)
        {
            CameraAction();
        }
    }
}
