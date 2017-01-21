using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    // Attach this script to you camera
    // This script should likely be used in conjunction with a CameraLookAt script in unity's standard assets.
    // Script created by Alexander MacLeod - 14th December 2013

    public float tumbleSensitivity = 0.3f;        //NB. This value should be adjusted until a natural result is achieved. It affects the amount the camera will tumble around the object as a swipe takes place.
    public Transform pivotPoint;                //this should be the location the camera tumbles around
    public bool naturalMotion = true;            //this determines whether a left swipe will make the camera tumble clockwise or anticlockwise around the object

    private GameObject camParent;                //this will be the rotating parent to which the camera is attached. Rotating this object will have the effect of making the camera a specified location.
    private Vector2 oldInputPosition;            //records the position of the finger last update

    void Start()
    {
        Transform originalParent = transform.parent;            //check if this camera already has a parent
        camParent = new GameObject("camParent");                //create a new gameObject
        camParent.transform.position = pivotPoint.position;        //place the new gameObject at pivotPoint location
        transform.parent = camParent.transform;                    //make this camera a child of the new gameObject
        camParent.transform.parent = originalParent;            //make the new gameobject a child of the original camera parent if it had one
    }

    void CameraAction()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            oldInputPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            float xDif = Input.mousePosition.x - oldInputPosition.x;
            float yDif = Input.mousePosition.y - oldInputPosition.y;
            if (!naturalMotion) { xDif *= -1; yDif *= -1; }
            if (xDif != 0) { camParent.transform.Rotate(Vector3.up * xDif * tumbleSensitivity); }
            if (yDif != 0) { camParent.transform.Rotate(Vector3.right * yDif * tumbleSensitivity); }
            oldInputPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            oldInputPosition = Input.mousePosition;
        }

#elif UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && touch.fingerId == 0)
                {
                    oldInputPosition = touch.position;
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touch.fingerId == 0)
                    {
                        float xDif = touch.position.x - oldInputPosition.x;    //this calculates the horizontal distance between the current finger location and the location last frame.
                        float yDif = touch.position.y - oldInputPosition.y;
                        if (!naturalMotion) { xDif *= -1; yDif *= 1;}
                        if (xDif != 0) { camParent.transform.Rotate(Vector3.up * xDif * tumbleSensitivity); }
                        if (yDif != 0) { camParent.transform.Rotate(Vector3.right * yDif * tumbleSensitivity); }
                        oldInputPosition = touch.position;
                    }
                }
                if (touch.phase == TouchPhase.Ended && touch.fingerId == 0)
                {
                    oldInputPosition = touch.position;
                }
            }
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        CameraAction();
    }
}

