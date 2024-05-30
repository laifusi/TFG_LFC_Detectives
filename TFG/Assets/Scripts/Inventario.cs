using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    private string[] objetosGuardados = new string[4];
    public GameObject inventario;
    private bool open = false;
    Image[] imagenes = new Image[4];

    // Start is called before the first frame update
    void Start()
    {
        Image[] todasimagenes = inventario.GetComponentsInChildren<Image>();
        int n = 0;
        for(int i = 0; i < todasimagenes.Length; i++)
        {
            if(todasimagenes[i].name == "objeto")
            {
                imagenes[n] = todasimagenes[i];
                n++;
            }
        }

        for(int i = 0; i < objetosGuardados.Length; i++)
        {
            objetosGuardados[i] = PlayerPrefs.GetString("ObjetoInventario" + i.ToString());
            ObjetoMovil[] objetos = FindObjectsOfType<ObjetoMovil>();
            for(int j = 0; j < objetos.Length; j++)
            {
                if(objetos[j].coger)
                    if(objetos[j].name == objetosGuardados[i])
                    {
                        objetos[j].gameObject.SetActive(false);
                        Sprite imagen = Resources.Load<Sprite>("Sprites/inventario " + objetos[j].name.ToString());
                        imagenes[i].sprite = imagen;
                        break;
                    }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused)
        {
            if (Input.GetKeyDown("i") && !open && Time.timeScale == 1)
            {
                AbrirInventario();
            }
            else if (Input.GetKeyDown("i") && Time.timeScale == 1)
            {
                CerrarInventario();
            }
        }
    }

    public void Coger(GameObject obj)
    {
        for(int i = 0; i < objetosGuardados.Length; i++)
        {
            if(objetosGuardados[i] == "")
            {
                objetosGuardados[i] = obj.name;
                obj.SetActive(false);
                Sprite imagen = Resources.Load<Sprite>("Sprites/inventario " + obj.name.ToString());
                imagenes[i].sprite = imagen;
                break;
            }
        }
    }

    private void AbrirInventario()
    {
        inventario.SetActive(true);
        open = true;
    }

    private void CerrarInventario()
    {
        inventario.SetActive(false);
        open = false;
    }

    public bool hasObject(string obj)
    {
        bool cogido = false;

        for (int i = 0; i < objetosGuardados.Length; i++)
        {
            if(objetosGuardados[i] != "")
                if(objetosGuardados[i] == obj)
                {
                    cogido = true;
                    break;
                }
        }

        return cogido;
    }

    public string[] getObjects()
    {
        return objetosGuardados;
    }
}
