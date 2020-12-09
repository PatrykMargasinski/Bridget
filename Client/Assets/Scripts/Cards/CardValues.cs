using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardValues : MonoBehaviour
{
    public Player owner;
    public int number;
    public CardSymbol symbol;
    public Image cardWithoutRaycast;
    public bool clickable;

    public void setValues(string value)
    {
        number=Int32.Parse(value.Substring(0,value.Length-1));
        string symbolChar=value[value.Length-1].ToString();
        symbol=(CardSymbol)Enum.Parse(typeof(CardSymbol),symbolChar);
        clickable=false;
    }
    public override string ToString()
    {
        return number.ToString()+symbol.ToString();
    }

    public char GetColor()
    {
        return symbol.ToString()[0];
    }
    public void OnEnter()
    {
        Debug.Log("On Enter");
        Debug.Log(clickable?"Clickable":"Not clickable");
        if(this.ToString()!="0C")
        {
            cardWithoutRaycast.sprite=CardSprites.sprites[ToString()];
            cardWithoutRaycast.transform.position=this.transform.position;
            cardWithoutRaycast.gameObject.SetActive(true);
        }
    }

    public void OnExit()
    {
        Debug.Log("On Exit");
        if(this.ToString()!="0C")
        {
            cardWithoutRaycast.gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        //Debug.Log($"On click. Card: {ToString()} Player: {owner.nick} - {owner.position}");
        if(clickable==true)
        {
            foreach(GameObject card in owner.cards) card.GetComponent<CardValues>().clickable=false;
            owner.controller.gamePhase.SendCard(ToString());
            owner.cardToPut.GetComponent<Image>().sprite=gameObject.GetComponent<Image>().sprite;
            owner.cardToPut.gameObject.SetActive(true);
            owner.RemoveOneCard(ToString());
            cardWithoutRaycast.gameObject.SetActive(false);
        }
    }
}
