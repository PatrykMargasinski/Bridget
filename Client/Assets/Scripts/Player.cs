using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Net;
using System.Net.Sockets;

public class Player : MonoBehaviour
{
    public GameObject card;
    private List<GameObject> cards=new List<GameObject>();
    private Quaternion quaternion;
    public Player()
    {
        quaternion=Quaternion.identity;
    }

    public void SetAngle(int option)
    {
        if(option==1) quaternion=Quaternion.AngleAxis(90,Vector3.forward);
        else if(option==2) quaternion=Quaternion.AngleAxis(90,Vector3.forward);
    }

    void Start()
    {

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
            if(str!="back") temp.GetComponent<CardValues>().setValues(str);
            cards.Add(temp);
        }
}

