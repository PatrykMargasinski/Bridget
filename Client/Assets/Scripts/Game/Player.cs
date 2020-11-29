using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Text;
public class Player : MonoBehaviour
{
    public GameObject card;
    public Text playerText;
    public string nick;
    public char position;
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
            if(str!="back") temp.GetComponent<CardValues>().setValues(str);
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
        foreach(GameObject card in cards)
        {
            Destroy(card);
        }
    }

    public override string ToString()
    {
        return $"{nick} - {position}";
    }
}

