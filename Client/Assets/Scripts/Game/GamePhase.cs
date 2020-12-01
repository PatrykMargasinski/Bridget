using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePhase : MonoBehaviour
{
    static public char dummy;
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
        controller.client.SendMessage($"GamePhase:Move:{controller.players[0].position}:{card}");
        if(controller.players[0].cards.Count==0)
            controller.client.SendMessage("GamePhase:NoCards");
    }

    public void ChangeTrickNumber()
    {
        string[] temp = gameInformations.text.Split(':');
        SetGameInformations($"{temp[0]}:{tricks}");
    }
}
