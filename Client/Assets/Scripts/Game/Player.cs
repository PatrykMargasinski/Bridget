﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;
public class Player : MonoBehaviour
{
    public Controller controller;
    public GameObject card;
    public Text playerText;
    public string nick;
    public char position;
    public List<GameObject> cards=new List<GameObject>();
    private Quaternion quaternion;
    public Image cardToPut;
    public Player()
    {
        quaternion=Quaternion.identity;
    }

    public void SetAngle(int option)
    {
        if(option==1) quaternion=Quaternion.AngleAxis(90,Vector3.forward);
        else if(option==2) quaternion=Quaternion.AngleAxis(90,Vector3.forward);
    }

    public void SetNickAndPosition(string n, char pos)
    {
        nick=n;
        position=pos;
        playerText.text=nick;
        playerText.text=this.ToString();
    }

        public void GiveCards(string[] cards)
        {
            foreach(string card in cards)
            {
                AddCard(card);
            }
        } 
        
        public void AddCard(string str)
        {
            GameObject temp = Instantiate(card,new Vector3(0,0,0),quaternion);
            temp.transform.SetParent(this.gameObject.transform);
            temp.gameObject.GetComponent<Image>().sprite = CardSprites.sprites[str];
            CardValues cardValues=temp.GetComponent<CardValues>();
            if(str!="back") cardValues.setValues(str);
            cardValues.owner=this;
            cards.Add(temp);
        }

    public string GetAllCards()
    {
        StringBuilder stringBuilder=new StringBuilder();
        foreach(GameObject card in cards)
        {
            stringBuilder.Append(":"+card.GetComponent<CardValues>().ToString());
        }
        return stringBuilder.ToString();
    }

    public void RemoveAllCards()
    {
        while(cards.Count!=0)
        { 
            Destroy(cards[0]);
            cards.Remove(cards[0]);
        }
    }

    public void RemoveOneCard(string cardName)
    {
        GameObject cardToRemove = cards.Find(card => card.GetComponent<CardValues>().ToString()==cardName);
        RemoveOneCard(cardToRemove);
    }

    public void RemoveOneCard(GameObject card)
    {
        if(cards.Remove(card)==true)
            Destroy(card);
    }

    public void RemoveOneCard()
    {
        if(cards.Count!=0)
        {
        Destroy(cards[0]);
        cards.RemoveAt(0);
        }
    }

    public override string ToString()
    {
        return $"{nick} - {position}";
    }
}

