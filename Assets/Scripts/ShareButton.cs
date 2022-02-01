using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShareButton : MonoBehaviour
{
private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
private const string TWEET_LANGUAGE = "en";
public static string descriptionParam;
    public void Share(string linkParameter)
    {
        string nameParameter = "YOUR AWESOME GAME MESSAGE!";//this is limited in text length 
        Application.OpenURL(TWITTER_ADDRESS +
        "?text=" + UnityWebRequest.EscapeURL(nameParameter + "\n" + descriptionParam + "\n" + "Get the Game!\n"));
    }   
}
