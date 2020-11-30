using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Linq;

public class Controller : MonoBehaviour
{
    public Client client;
    public Text messageForPlayer;
    public Player[] players;
    public Dictionary<char,Player> playerByPosition=new Dictionary<char, Player>();
    private Queue<Action> requestQueue = new Queue<Action>();
    private AuctionPhase auctionPhase;
    private GamePhase gamePhase;

    void Start()
    {
        players=new Player[4];
        SetPlayers();
        Screen.SetResolution(620,454,false);
        auctionPhase=gameObject.GetComponent<AuctionPhase>();
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
        //players[1].SetAngle(1);
        players[2]=GameObject.Find("PartnerCards").GetComponent<Player>();
        players[3]=GameObject.Find("EnemyBoard1").GetComponent<Player>();
       // players[3].SetAngle(2);
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
                else SetMessageForPlayer($"Congratulation {mes[1]}, you joined the game");
            }
            else if(mes[0]=="Players")
            {
                int index=1;
                while(mes[index]!=ConnectButton.nick) index+=2;
                for(int i=0;i<4;i++)
                {
                    if(index>8)index=1;
                    players[i].SetNickAndPosition(mes[index],mes[index+1][0]);
                    playerByPosition.Add(mes[index+1][0],players[i]);
                    index+=2;
                }
                foreach(char s in playerByPosition.Keys) Debug.Log(s);

            }
            else if(mes[0]=="Bidding")
            {
                Debug.Log("Mes5"+mes[5]);
                auctionPhase.highestNumber=Int32.Parse(mes[2]);
                auctionPhase.highestColor=(TrumpColor)Enum.Parse(typeof(TrumpColor),mes[3]);
                auctionPhase.Initialization();
                if(mes[1][0]!=players[0].position)
                {
                    SetMessageForPlayer($"It's {mes[1]}'s turn. Highest bid is {auctionPhase.highestNumber}{auctionPhase.highestColor.ToString()}\n{mes[4]} {mes[5]}");
                }
                else
                {
                    SetMessageForPlayer($"It's your turn. Highest bid is {auctionPhase.highestNumber}{auctionPhase.highestColor.ToString()}\n{mes[4]} {mes[5]}");
                    if(mes[5]=="countered") auctionPhase.actionButtons[1].interactable=true;
                    if(mes[5].IndexOf("bid")!=-1) auctionPhase.actionButtons[0].interactable=true;
                    auctionPhase.auctionPhaseScreen.gameObject.SetActive(true);
                }
            }
            else if(mes[0]=="GamePhase")
            {
                if(mes[1]=="DummyInitialization")
                {
                    GamePhase.dummy=mes[2][0];
                    if(players[0].position==mes[2][0])
                    {
                        //he is dummy
                        client.SendMessage($"GamePhase:DummyCards{players[0].GetAllCards()}");
                        players[2].RemoveAllCards(); //remove all partner's cards
                    }
                    else
                    {
                        playerByPosition[mes[2][0]].RemoveAllCards();//remove all dummy's cards
                    }
                    if(players[2].position==mes[2][0])
                    {
                        client.SendMessage($"GamePhase:PartnerCards{players[0].GetAllCards()}");
                    }
                }
                else if(mes[1]=="DummyCards")
                {
                    StringBuilder stringBuilder=new StringBuilder("Got dummy cards");
                    Debug.Log("GamePhase dummy"+GamePhase.dummy);
                    Debug.Log("No i?"+playerByPosition[GamePhase.dummy]);
                    foreach(string s in mes.Skip(2)) {playerByPosition[GamePhase.dummy].AddCard(s);}
                }
                else if(mes[1]=="PartnerCards")
                {
                    StringBuilder stringBuilder=new StringBuilder("Got partner cards");
                    foreach(string s in mes.Skip(2)) {players[2].AddCard(s);}
                }
            }
            /*
            else if(mes[0].IndexOf("Game")!=-1)
            {
                //SetMessageForPlayer($"Contract is {mes[1]}{mes[2]} for {mes[3]} team"+(mes[4]=="0"?"":(mes[4]=="C"?" with counter":" with recounter")));
                SetMessageForPlayer(mes[0]+mes[1]);
                Debug.Log(players[0].GetAllCards());
            }*/
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
