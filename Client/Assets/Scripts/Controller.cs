using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Linq;

public class Controller : MonoBehaviour
{
    //Main
    public Client client;
    public Text messageForPlayer;
    public Player[] players;
    private Queue<Action> requestQueue = new Queue<Action>();
    private AuctionPhase auctionPhase;

    void Start()
    {
        players=new Player[4];
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
        players[0]=GameObject.Find("MyCards").GetComponent<Player>();
        players[1]=GameObject.Find("EnemyBoard2").GetComponent<Player>();
        players[1].SetAngle(1);
        players[2]=GameObject.Find("PartnerCards").GetComponent<Player>();
        players[3]=GameObject.Find("EnemyBoard1").GetComponent<Player>();
        players[3].SetAngle(2);
    }

    public void AddRequest(Action action)
    {
        requestQueue.Enqueue(action);
    }

    public void Reaction(Socket socket, string message)
    {
        string[] mes=message.Split(':');
            if(mes[0]=="Cards")
            {
                players[0].GiveCards(mes.Skip(1).ToArray());
                for(int i=1;i<4;i++)
                {
                    for(int j=0;j<13;j++)players[i].AddCard("back");
                }
                SetMessageForPlayer("Cards acquired");
                client.SendMessage("CardsAcquired");
            }
            else if(mes[0]=="NewPlayerJoined")
            {
                if(mes[1]!=ConnectButton.nick) SetMessageForPlayer($"{mes[1]} joined the game");
                else SetMessageForPlayer($"Congratulation {ConnectButton.nick}, you joined the game");
            }
            else if(mes[0]=="Players")
            {
                int index=1;
                while(mes[index]!=ConnectButton.nick) index+=2;
                for(int i=0;i<4;i++)
                {
                    if(index>8)index=1;
                    players[i].SetNickAndPosition(mes[index],mes[index+1][0]);
                    index+=2;
                }

            }
            else if(mes[0]=="Bidding")
            {
                Debug.Log(mes);
                auctionPhase=gameObject.GetComponent<AuctionPhase>();
                auctionPhase.highestNumber=Int32.Parse(mes[2]);
                auctionPhase.highestColor=(TrumpColor)Enum.Parse(typeof(TrumpColor),mes[3]);
                auctionPhase.Initialization();
                if(mes[1][0]!=players[0].position)
                {
                    SetMessageForPlayer($"It's {mes[1]}'s turn. Highest bid is {auctionPhase.highestNumber}{auctionPhase.highestColor.ToString()}");
                }
                else
                {
                    SetMessageForPlayer($"It's your turn. Highest bid is {auctionPhase.highestNumber}{auctionPhase.highestColor.ToString()}");
                    auctionPhase.auctionPhaseScreen.gameObject.SetActive(true);
                }
            }
            else if(mes[0]=="Game")
            {
                SetMessageForPlayer("Normal game starts");
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
