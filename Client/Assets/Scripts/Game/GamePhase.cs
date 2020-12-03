using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhase : MonoBehaviour
{
    public char dummy;
    public bool dummyMove=false;
    Controller controller;
    public Text gameInformations;
    public string bid;
    public int tricks=0;
    void Start()
    {
        controller=gameObject.GetComponent<Controller>();
        gameInformations.text="";
    }

    public void SetGameInformations(string mes)
    {
        gameInformations.text=mes;
    }

    public void SendCard(string card)
    {
        Player player=dummyMove?controller.players[2]:controller.players[0];
        dummyMove=false;
        controller.client.SendMessage($"GamePhase:Move:{player.position}:{card}");
        if(controller.players[0].cards.Count==0)
            controller.client.SendMessage("GamePhase:NoCards");
    }

    public void ChangeTrickNumber()
    {
        string[] temp = gameInformations.text.Split(':');
        SetGameInformations($"{temp[0]}:{tricks}");
    }
}
