using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosLuz : MonoBehaviour
{
    public bool thisLight = false;
    public Light light;
    public string tipo;
    public GameObject persiana;

    void Start()
    {
        int lucesON = PlayerPrefs.GetInt("NumeroLucesON");
        for(int i = 0; i < lucesON; i++)
        {
            string nombreLuz = PlayerPrefs.GetString("Luz" + i.ToString());
            if(nombreLuz == light.name)
            {
                Encender();
                break;
            }
        }
    }

    void Update()
    {
        if(!Pause.paused)
        {
            if (thisLight)
                Personaje.ActionText(tipo, System.Convert.ToInt32(light.enabled));

            if (thisLight && Input.GetKeyDown("e"))
            {
                if (!light.enabled)
                    Encender();
                else
                    Apagar();
            }

            thisLight = false;
        }
    }

    void Encender()
    {
        light.enabled = true;
        if(tipo == "ventana")
        {
            persiana.SetActive(false);
        }
    }

    void Apagar()
    {
        light.enabled = false;
        if (tipo == "ventana")
        {
            persiana.SetActive(true);
        }
    }
}
