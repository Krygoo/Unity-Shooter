using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{/// <summary>
/// rotation_angle - zakres obrotu wzgledem osi Z
/// rotation_period - czas w ktorym wykona sie pelny obrot modelu postaci
/// modelRotation  - kat obrotu postaci w danym momencie
/// playerModel - model do obrocenia
/// </summary>
    float rotation_angle = 10f; 
    float rotation_period = 0.5f;
  
    float modelRotation;
  
    [SerializeField] GameObject playerModel; 


    //Funkcja odpowiedzialna za obracanie modelu podczas ruchu gracza
    public void RotateModel(bool moving)
    {
        if(moving)
        {
            float cycles = Time.time / rotation_period;
            modelRotation = Mathf.Sin(cycles * Mathf.PI * 2) * 10;
            playerModel.transform.localRotation = Quaternion.Euler(0, 0, modelRotation);
        }
        else
        {
            playerModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
    }
}
