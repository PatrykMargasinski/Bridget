﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class TitleScript : MonoBehaviour
{
    Sprite[] sprites;
    int i=0;
    int temp=0;
    void Start()
    {
        sprites=new Sprite[]{Resources.Load<Sprite>("title1"),Resources.Load<Sprite>("title2")};
        InvokeRepeating("Change", 1f, 1f);
    }
    void Change()
    {
        temp++;
        Debug.Log(sprites[0]);
        i=(i+1)%2;
        gameObject.GetComponent<Image>().sprite=sprites[i];
    }
}
