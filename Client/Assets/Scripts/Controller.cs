using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;

public class Controller : MonoBehaviour
{
    public Client client;
    public Text messageForPlayer;
    public Player[] player;
    private Queue<Action> requestQueue = new Queue<Action>();

    void Start()
    {
        player=new Player[4];
        SetPlayers();
        Screen.SetResolution(620,454,false);
        client=new Client(this);
        client.SetupClient();
    } 
    void Update()
    {
        if(requestQueue.Count!=0)
        {
            requestQueue.Dequeue().Invoke();
        }
    }

    private void SetPlayers()
    {
        player[0]=GameObject.Find("MyCards").GetComponent<Player>();
        player[1]=GameObject.Find("EnemyBoard2").GetComponent<Player>();
        player[1].SetAngle(1);
        player[2]=GameObject.Find("PartnerCards").GetComponent<Player>();
        player[3]=GameObject.Find("EnemyBoard1").GetComponent<Player>();
        player[3].SetAngle(2);
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
                player[0].GiveCards(cardsString);
                for(int i=1;i<4;i++)
                {
                    for(int j=0;j<13;j++)player[i].AddCard("back");
                }
                SetMessageForPlayer("Cards acquired");
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
