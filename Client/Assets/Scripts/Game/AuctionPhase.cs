using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuctionPhase : MonoBehaviour
{
    private Controller controller;
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
        auctionPhaseScreen.gameObject.SetActive(false);
        controller=gameObject.GetComponent<Controller>();
    }

    public void Initialization()
    {
        BidNumberInitialization();
        BidColorInitialization();
        BidActionInitialization();
    }

    public void BidNumberInitialization()
    {
        if(bidNumber!=0){SetWhite(numberButtons[bidNumber-1]);bidNumber=0;}
        foreach(Button b in numberButtons) {b.interactable=true;SetWhite(b);}
        for(int i=0;i<highestNumber-1;i++)
        {
            numberButtons[i].interactable=false;
        }
        Debug.Log(highestColor.ToString()+" "+highestNumber);
        if(highestColor==TrumpColor.BA && highestNumber!=0) numberButtons[highestNumber-1].interactable=false;
    }
    public void BidColorInitialization()
    {
        if(bidColor!=TrumpColor.undefined){SetWhite(colorButtons[(int)bidColor]);bidColor=TrumpColor.undefined;}
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
        actionButtons[2].interactable=true;
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
        foreach(Button b in colorButtons) b.interactable=true;
        if(bidNumber==highestNumber)
        {
            for(int i=0;i<=(int)highestColor;i++)colorButtons[i].interactable=false;
            if(bidColor<=highestColor && bidColor!=TrumpColor.undefined)
            {
                SetWhite(colorButtons[(int)bidColor]);
                Debug.Log("Size: "+actionButtons.Length);
                actionButtons[3].interactable=false;

                bidColor=TrumpColor.undefined;
            }
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
        Controller.TakeScreen();
        switch(action)
        {
            case "Pass":
                controller.client.SendMessage($"Bidding:{controller.players[0].position}:Pass");
            break;

            case "Counter":
                controller.client.SendMessage($"Bidding:{controller.players[0].position}:Counter");
            break;

            case "Recounter":
                controller.client.SendMessage($"Bidding:{controller.players[0].position}:Recounter");
            break;

            case "Bid":
                controller.client.SendMessage($"Bidding:{controller.players[0].position}:Bid:{bidNumber}:{bidColor.ToString()}");
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
