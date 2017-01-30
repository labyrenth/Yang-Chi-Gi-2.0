using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQControl : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Head" && col.gameObject.GetComponent<PlayerControltwo>().HQ == this.gameObject)
        {
            StartCoroutine(col.gameObject.GetComponent<PlayerControltwo>().EnterHQ());
            col.gameObject.GetComponent<PlayerControltwo>().InHQ = true;
        }
        else if (col.gameObject.tag == "Dog")
        {
            if (col.gameObject.GetComponent<Dog>().SheepList.Count == 0)
            {
                return;
            }
            else
            {
                StartCoroutine(col.gameObject.GetComponent<Dog>().EnterHQ());
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Head" && col.gameObject.GetComponent<PlayerControltwo>().HQ == this.gameObject)
        {
            col.gameObject.GetComponent<PlayerControltwo>().InHQ = false;
        }
    }
}
