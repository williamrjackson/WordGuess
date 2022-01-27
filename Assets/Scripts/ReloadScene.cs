using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrj;
public class ReloadScene : MonoBehaviour
{
    public static ReloadScene Instance;
    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple ReloadScene's instantiated. Component removed from " + gameObject.name + ". Instance already found on " + Instance.gameObject.name + "!");
            Destroy(this);
        }
    }
    public void Reload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
