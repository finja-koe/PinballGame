using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{

    RectTransform rectTransform;
    Vector2 originPos;
    Vector2 targetPos;
    Vector2 originSize;
    Vector2 targetSize;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform= GetComponent<RectTransform>();
        originPos= rectTransform.anchoredPosition;
        targetPos= new Vector2(originPos.x+27,originPos.y);
        originSize=rectTransform.sizeDelta;
        targetSize= new Vector2(52,originSize.y);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("q"))
        {
            Debug.Log("Animate");
            rectTransform.sizeDelta= Vector2.Lerp(rectTransform.sizeDelta, targetSize, 0.01f);
            rectTransform.anchoredPosition= Vector2.Lerp(rectTransform.anchoredPosition, targetPos, 0.01f);
        }
    }
}
