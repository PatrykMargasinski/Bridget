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
    public Text messageForPlayer;
    private List<GameObject> cards=new List<GameObject>();
    private Queue<Action> requestQueue = new Queue<Action>();

    void Start()
    {
        Debug.Log(ConnectButton.ip);
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
        if(requestQueue.Count!=0)
        {
            requestQueue.Dequeue().Invoke();
        }
    }

    public void AddRequest(Action action)
    {
        requestQueue.Enqueue(action);
    }

    public void Reaction(Socket socket, string message)
    {
            if(message.IndexOf("Cards")!=-1)
            {
                string cardsString=message.Substring(message.IndexOf(':')+1);
                GiveCards(cardsString);
                SetMessageForPlayer("Cards received");
                client.SendMessage("CardsAcquired");
            }
            else if(message=="masakra")
            {
                Debug.Log("You crazy son of a bitch, you did it!");
            }
    }

    public void OnApplicationQuit()
    {
        client.Disconnect();
    }

    public void SetMessageForPlayer(string message)
    {
        Debug.Log(message);
        messageForPlayer.text=message;
    }
}

