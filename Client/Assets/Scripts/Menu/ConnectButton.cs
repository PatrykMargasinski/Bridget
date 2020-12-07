using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public static string ip="";
    public static string nick="";
    public static string port="";
    public InputField inputFieldForIP;
    public InputField inputFieldForNick;
    public InputField inputFieldForPort;
    public void Connection()
    {
        ip=inputFieldForIP.text;
        nick=inputFieldForNick.text;
        port=inputFieldForPort.text;
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
	{
		Application.Quit();
	}
}
