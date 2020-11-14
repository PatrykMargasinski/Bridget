using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{
    public static string ip="Hej";
    public InputField inputField;
    public void Connection()
    {
        ip=inputField.text;
        SceneManager.LoadScene("GameScene");
    }
    public void Exit()
	{
		Application.Quit();
	}
}
