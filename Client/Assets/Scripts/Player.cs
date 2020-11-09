using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
public class Player : MonoBehaviour
{
    public Client client;
    public GameObject card;
    private List<GameObject> cards=new List<GameObject>();
    public List<string> cardString;

    void Start()
    {
        Screen.SetResolution(620,454,false);
        client=new Client(this);
        client.SetupClient();
        cardString=SomethingToTest.CardGen();
        for(int i=0;i<0;i++)
        {
            GameObject temp = Instantiate(card,new Vector3(0,0,0),Quaternion.identity);
            temp.transform.SetParent(this.gameObject.transform);
            Debug.Log(cardString[i]);
            temp.gameObject.GetComponent<Image>().sprite = CardSprites.sprites[cardString[i]];
            temp.GetComponent<CardValues>().setValues(cardString[i]);
            cards.Add(temp);
        }
    }

        public void GiveCards(string s)
        {
            int first = 0;
            for(int i=0;i<s.Length;i++)
            {
                if(Char.IsLetter(s[i]))
                {
                    string str=s.Substring(first, i - first+1);
                    Debug.Log("Od give cards: "+ str);
                    GameObject temp = Instantiate(card,new Vector3(0,0,0),Quaternion.identity);
                    temp.transform.SetParent(this.gameObject.transform);
                    //Debug.Log(cardString[i]);
                    temp.gameObject.GetComponent<Image>().sprite = CardSprites.sprites[str];
                    temp.GetComponent<CardValues>().setValues(str);
                    cards.Add(temp);
                    first = i+1;
                }
            }
        }  
    void Update()
    {
        if(client.message!="")
        {
            Debug.Log("Fifth thread:" + Thread.CurrentThread.ManagedThreadId);
            string mes=client.message;
            client.message="";
            Debug.Log("Od gracza: " + mes);
            GiveCards(mes);
        }
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
        
    }
}

