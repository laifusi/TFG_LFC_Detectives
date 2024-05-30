using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int detective;
    public string name;
    public float timeplayed;
    public float[] position;
    public float[] camrotation;
    public float[] charrotation;
    public int idioma;
    public int[] pistasProf;
    public int[] pistasPers;
    public int[] pistasOtro;
    public string[] inventario;
    public string[] lights;

    public GameData(Personaje p, LibretaPruebas l, Inventario inv, Light[] ligh)
    {
        position = new float[3];
        charrotation = new float[3];
        camrotation = new float[3];

        idioma = PlayerPrefs.GetInt("Idioma");
        detective = PlayerPrefs.GetInt("Detective");
        name = PlayerPrefs.GetString("Nombre");
        timeplayed = PlayerPrefs.GetFloat("TiempoJugado");

        pistasPers = l.getPistas("personales");
        pistasProf = l.getPistas("profesionales");
        pistasOtro = l.getPistas("otros");

        inventario = inv.getObjects();

        position[0] = p.transform.position.x;
        position[1] = p.transform.position.y;
        position[2] = p.transform.position.z;

        charrotation[0] = p.transform.eulerAngles.x;
        charrotation[1] = p.transform.eulerAngles.y;
        charrotation[2] = p.transform.eulerAngles.z;

        camrotation[0] = Camera.main.transform.eulerAngles.x;
        camrotation[1] = Camera.main.transform.eulerAngles.y;
        camrotation[2] = Camera.main.transform.eulerAngles.z;

        lights = new string[ligh.Length];
        for(int i = 0; i < ligh.Length; i++)
        {
            if(ligh[i].isActiveAndEnabled)
                lights[i] = ligh[i].name;
        }
    }
}
