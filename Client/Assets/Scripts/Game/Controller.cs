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
    //private Queue<Action> requestQueue = new Queue<Action>();
    public AuctionPhase auctionPhase;
    public GamePhase gamePhase;
    public Scoring scoring;
    static public int screenNumber=0;
    void Start()
    {
        players=new Player[4];
        SetPlayers();
        Screen.SetResolution(620,454,false);
        auctionPhase=gameObject.GetComponent<AuctionPhase>();
        gamePhase=gameObject.GetComponent<GamePhase>();
        scoring=gameObject.GetComponent<Scoring>();
        client=new Client();
        client.SetupClient();
    } 
    void Update()
    {
        if(client.requestQueue.Count!=0)
        {
            var mesPair = client.requestQueue.Dequeue();
            Reaction(mesPair.Item1,mesPair.Item2);
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
       foreach(Player pl in players) pl.controller=this;
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
                playerByPosition.Clear();
                for(int i=0;i<4;i++)
                {
                    if(index>8)index=1;
                    players[i].SetNickAndPosition(mes[index],mes[index+1][0]);
                    playerByPosition.Add(mes[index+1][0],players[i]);
                    index+=2;
                }
                players[2].playerText.text+=" (partner)";

            }
            else if(mes[0]=="MessageForPlayer")
            {
                SetMessageForPlayer(mes[1].Replace(';',':'));
            }
            else if(mes[0]=="Bidding")
            {
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
                if(mes[1]=="Initialization")
                {
                    gamePhase.dummy=mes[2][0];
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
                    string counterRecounter="";
                    if(Int32.Parse(mes[5])==1) counterRecounter=" with counter";
                    else if(Int32.Parse(mes[5])==2) counterRecounter=" with recounter";
                    int ind=0;
                    foreach(string mess in mes)
                    {
                        ind++;
                    }
                    gamePhase.SetGameInformations($"Contract is {mes[3]} for {mes[4]}{counterRecounter}. {mes[2]} is dummy. Got/Lost tricks: 0/0");
                    gamePhase.tricks=0;
                    gamePhase.lostTricks=0;
                }
                else if(mes[1]=="DummyCards")
                {
                    StringBuilder stringBuilder=new StringBuilder("Got dummy cards");
                    foreach(string s in mes.Skip(2)) {playerByPosition[gamePhase.dummy].AddCard(s);}
                    client.SendMessage("GamePhase:ReadyToPlay");

                }
                else if(mes[1]=="PartnerCards")
                {
                    StringBuilder stringBuilder=new StringBuilder("Got partner cards");
                    foreach(string s in mes.Skip(2)) {players[2].AddCard(s);}
                    client.SendMessage("GamePhase:ReadyToPlay");
                }
                else if(mes[1]=="Move")
                {
                    if(players[0].cardToPut.IsActive() && players[1].cardToPut.IsActive() && players[2].cardToPut.IsActive() && players[3].cardToPut.IsActive())
                    {
                        foreach(Player p in players) p.cardToPut.gameObject.SetActive(false);
                    }
                    Player player;
                    if(mes[2][0]==gamePhase.dummy && players[2].position==gamePhase.dummy) 
                    {
                        gamePhase.dummyMove=true;
                        player=players[2];
                    }
                    else player=players[0];

                    if((mes[2][0]==players[0].position && players[0].position!=gamePhase.dummy) || gamePhase.dummyMove==true) 
                    {
                        SetMessageForPlayer("It's your turn" +(gamePhase.dummyMove?" as dummy":""));
                        bool isSomethingClickable=false;
                        foreach(GameObject card in player.cards)
                        {
                            if(mes[3][0]=='0' || card.GetComponent<CardValues>().GetColor()==mes[3][0]) 
                            {
                                isSomethingClickable=true;
                                card.GetComponent<CardValues>().clickable=true;
                            }
                        }

                        if(isSomethingClickable==false)
                            foreach(GameObject card in player.cards)
                                card.GetComponent<CardValues>().clickable=true;

                    }
                    else
                    {
                        SetMessageForPlayer($"It's {mes[2][0]}'s turn");
                    }
                }
                else if(mes[1]=="MoveDone")
                {
                    playerByPosition[mes[2][0]].cardToPut.GetComponent<Image>().sprite=CardSprites.sprites[mes[3]];
                    playerByPosition[mes[2][0]].cardToPut.gameObject.SetActive(true);
                    bool dummyPartnerCondition=(players[0].position==gamePhase.dummy && mes[2][0]==players[2].position);
                    if(mes[2][0]==players[0].position || mes[2][0]==gamePhase.dummy || dummyPartnerCondition) playerByPosition[mes[2][0]].RemoveOneCard(mes[3]);
                    else playerByPosition[mes[2][0]].RemoveOneCard();
                }
                else if(mes[1]=="Winner")
                {
                    int gotTrick=Int32.Parse(mes[2]);
                    if(gotTrick==1) 
                    {
                        gamePhase.tricks++;
                        SetMessageForPlayer($"Trick acquired by player {mes[3]}");
                    }
                    else if(gotTrick==0)
                    {
                        gamePhase.lostTricks++;
                        SetMessageForPlayer("Trick lost");
                    }
                    else throw new Exception("Is there or not a trick?");
                    gamePhase.ChangeTrickNumber();
                }
            }
            else if(mes[0]=="Score")
            {
                SetMessageForPlayer("Scoring");
                //scoring.gameObject.SetActive(true);
                foreach(Player player in players)
                {
                    player.cardToPut.gameObject.SetActive(false);
                }
                StringBuilder score=new StringBuilder("");
                foreach(string str in mes.Skip(1))
                {
                    score.Append(" "+str);
                }
                score.Remove(0,1);
                gamePhase.SetGameInformations(score.ToString());
                client.SendMessage("PlayAgain");
            }
            else if(mes[0]=="Endgame")
            {
                SetMessageForPlayer("The game has ended");
            }

    }

    public void OnApplicationQuit()
    {
        client.Disconnect();
    }

    public void SetMessageForPlayer(string message)
    {
        messageForPlayer.text=message;
    }

}
