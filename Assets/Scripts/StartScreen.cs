using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{

    /// <summary>
    /// sceneLoader - referencja do obiektu odpwiadającego za zmiane scen
    /// popup - prefabrykat ktory pojawia się po wciśnięciu przycisku opcji
    /// </summary>
    SceneLoader sceneLoader;
    GameObject popup;

 
    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        popup = GameObject.FindGameObjectWithTag("Popup");
        popup.SetActive(false);
    }

    public void EnablePopup() // funkcja wyswietlajaca okienko z platnoscia
    {
        popup.SetActive(true);
    }
    public void DisablePopup() // funkcja zamyka okienko z platnoscia
    {
        popup.SetActive(false);
    }

    public void LoadNextSceneButton() // funkcja oblsugujaca przycisk startu gry
    {
        sceneLoader.StartGameButton(); 

    }
  
    

}
