using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardValues : MonoBehaviour
{
    static int ace;
    public Player owner;
    public int number;
    public CardSymbol symbol;
    public Image cardWithoutRaycast;

    public void setValues(string value)
    {
        number=Int32.Parse(value.Substring(0,value.Length-1));
        string symbolChar=value[value.Length-1].ToString();
        symbol=(CardSymbol)Enum.Parse(typeof(CardSymbol),symbolChar);
    }
    public override string ToString()
    {
        return number.ToString()+symbol.ToString();
    }
    public void testValue()
    {
        Debug.Log("Click: "+this.ToString());
    }

    public void OnEnter()
    {
        Debug.Log("On Enter");
        cardWithoutRaycast.sprite=CardSprites.sprites[ToString()];
        cardWithoutRaycast.transform.position=this.transform.position;
        cardWithoutRaycast.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        Debug.Log("On Exit");
        cardWithoutRaycast.gameObject.SetActive(false);
    }
}
