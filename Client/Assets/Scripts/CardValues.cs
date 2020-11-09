using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CardValues : MonoBehaviour
{
    static int ace;
    public Player owner;
    public int number;
    public CardSymbol symbol;

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
}
