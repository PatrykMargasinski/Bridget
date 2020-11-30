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

    void Start()
    {
        controller=gameObject.GetComponent<Controller>();
        gameInformations.text="";
    }

    void onCardClick()
    {
        
    }
}
