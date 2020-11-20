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

    //Auction phase
    public Image auctionPhaseScreen;
    public int highestNumber;
    public TrumpColor highestColor;
    public int bidNumber=0;
    public TrumpColor bidColor=TrumpColor.undefined;
    public Button[] numberButtons;
    public Button[] colorButtons;
    public Button[] actionButtons;
    void Start()
    {
        players=new Player[4];
        SetPlayers();
        Screen.SetResolution(620,454,false);
        auctionPhaseScreen.gameObject.SetActive(false);
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
                highestNumber=Int32.Parse(mes[2]);
                highestColor=(TrumpColor)Enum.Parse(typeof(TrumpColor),mes[3]);
                BidNumberInitialization();
                BidColorInitialization();
                BidActionInitialization();
                if(mes[1][0]!=players[0].position)
                {
                    SetMessageForPlayer($"It's {mes[1]}'s turn. Highest bid is {highestNumber}{highestColor.ToString()}");
                }
                else
                {
                    SetMessageForPlayer($"It's your turn. Highest bid is {highestNumber}{highestColor.ToString()}");
                    auctionPhaseScreen.gameObject.SetActive(true);
                }
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

    public void BidNumberInitialization()
    {
        foreach(Button b in numberButtons) {b.interactable=true;SetWhite(b);}
        for(int i=0;i<highestNumber-1;i++)
        {
            numberButtons[i].interactable=false;
        }
        if(highestColor==TrumpColor.BA) numberButtons[highestNumber-1].interactable=false;

    }
    public void BidColorInitialization()
    {
        foreach(Button b in colorButtons) {b.interactable=true;SetWhite(b);}
        if(highestColor!=TrumpColor.BA)
        {
            for(int i=0;i<=(int)highestColor;i++)
            {
                colorButtons[i].interactable=false;
            }
        }
    }
    public void BidActionInitialization()
    {
        foreach(Button b in actionButtons) {b.interactable=false;}
    }

    public void BidNumberClicked(int number)
    {
        Debug.Log("Numer: "+number +" BidNumber:" + bidNumber);
        if(bidNumber!=0) SetWhite(numberButtons[bidNumber-1]);
        SetRed(numberButtons[number-1]);
        bidNumber=number;
        Debug.Log("Bid number: "+bidNumber);
        if(bidNumber!=0 && bidColor != TrumpColor.undefined)
        {
            actionButtons[3].interactable=true;
        }

    }
    public void BidColorClicked(int color)
    {
        if(bidColor!=TrumpColor.undefined) SetWhite(colorButtons[(int)bidColor]);
        SetRed(colorButtons[color]);
        bidColor=(TrumpColor)color;
        Debug.Log("Bid color: "+bidColor.ToString());
        if(bidNumber!=0 && bidColor != TrumpColor.undefined)
        {
            actionButtons[3].interactable=true;
        }
    }

    public void BidActions(string action)
    {
        switch(action)
        {
            case "Pass":
                client.SendMessage($"Bid:{players[0].position}:Pass");
            break;

            case "Counter":
                client.SendMessage($"Bid:{players[0].position}:Counter");
            break;

            case "Recounter":
                client.SendMessage($"Bid:{players[0].position}:Recounter");
            break;

            case "Bid":
                client.SendMessage($"Bid:{players[0].position}:Bid:{bidNumber}:{bidColor.ToString()}");
            break;
        }
        auctionPhaseScreen.gameObject.SetActive(false);
    }

    public void SetRed(Button b)
    {
        var colors=b.colors;
        colors.normalColor=Color.red;
        colors.pressedColor=Color.red;
        colors.selectedColor=Color.red;
        b.colors=colors;
    }
    public void SetWhite(Button b)
    {
        var colors=b.colors;
        colors.normalColor=Color.white;
        colors.pressedColor=Color.white;
        colors.selectedColor=Color.white;
        b.colors=colors;
    }

}
