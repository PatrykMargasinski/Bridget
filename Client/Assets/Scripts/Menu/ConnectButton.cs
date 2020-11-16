using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public static string ip="Hej";
    public static string nick="";
    public InputField inputFieldForIP;
    public InputField inputFieldForNick;
    public void Connection()
    {
        ip=inputFieldForIP.text;
        nick=inputFieldForNick.text;
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
	{
		Application.Quit();
	}
}
