using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBase : FadeInOut {

    private void Awake()
    {
        PlayManage.Instance.SearchFadeImage();
        StartCoroutine(PlayManage.Instance.FadeIn(PlayManage.Instance.FadeImage));
    }
}
