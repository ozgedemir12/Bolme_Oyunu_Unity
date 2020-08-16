using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sonucPanel : MonoBehaviour
{
    public void YenidenBasla()
    {
        SceneManager.LoadScene("GameLevel");
    }
    public void AnaMenu()
    {
        SceneManager.LoadScene("MenuLevel");
    }

}