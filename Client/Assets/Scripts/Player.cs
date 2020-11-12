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
    public Client client;
    public GameObject card;
    private List<GameObject> cards=new List<GameObject>();

    void Start()
    {
        Screen.SetResolution(620,454,false);
        client=new Client(this);
        client.SetupClient();
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
            Debug.Log("Od gracza: " + client.message);
            Reaction(client.GetClientSocket(),client.message);
        }
    }

    void Reaction(Socket socket, string message)
    {
            if(message.IndexOf("Cards")!=-1)
            {
                string cardsString=message.Substring(message.IndexOf(':')+1);
                GiveCards(cardsString);
                Debug.Log("Cards Acquired");
                client.SendMessage("CardsAcquired");
            }
            else if(message=="masakra")
            {
                Debug.Log("You crazy son of a bitch, you did it!");
            }
            client.message="";
    }

    void OnApplicationQuit()
    {
        client.Disconnect();
        Environment.Exit(Environment.ExitCode);
    }
}

