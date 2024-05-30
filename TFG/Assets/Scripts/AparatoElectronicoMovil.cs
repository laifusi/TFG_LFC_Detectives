using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class AparatoElectronicoMovil : ObjetoMovil
{
    public GameObject canvas;
    public string password;
    private string pwd = "";
    public Text[] passwordTexts = new Text[4];
    private GameObject panelInicio;
    private GameObject panelBarra;
    private GameObject panelBloqueo;
    private GameObject panelContrasenya;
    private GameObject panelEscritorio;
    private GameObject panelOpcionesCorreo;
    private GameObject panelCorreos;
    private GameObject panelCorreoConcreto;
    public Image[] chatImages;
    private Button buttonCentro;
    private bool iniciado = false;
    public Text[] hora;
    public Text[] dia;
    public Text carga;
    private int h = 17;
    private int m = 32;
    private int passedminutes = 0;
    private int d = 12;
    private int mes = 11;
    private int a = 2019;
    private int porcentaje = 100;
    private string fileContent = "";
    public Text pinText;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        for (int i = 0; i < hora.Length; i++)
            hora[i].text = h.ToString() + ":" + m.ToString();
        for(int i = 0; i < dia.Length; i++)
            dia[i].text = d.ToString() + "/" + mes.ToString() + "/" + a.ToString();
        carga.text = porcentaje.ToString() + "%";

        Transform[] canvasChildren = canvas.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < canvasChildren.Length; i++)
        {
            if (canvasChildren[i].name == "inicio")
            {
                panelInicio = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "barra superior")
            {
                panelBarra = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "bloqueo")
            {
                panelBloqueo = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "contrasenya")
            {
                panelContrasenya = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "escritorio")
            {
                panelEscritorio = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "opciones correo")
            {
                panelOpcionesCorreo = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "correos")
            {
                panelCorreos = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "correo concreto")
            {
                panelCorreoConcreto = canvasChildren[i].gameObject;
            }
            else if(canvasChildren[i].name == "Centro")
            {
                buttonCentro = canvasChildren[i].GetComponent<Button>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused)
        {
            if (Time.unscaledTime - 60 * passedminutes >= 60)
            {
                if (iniciado)
                {
                    if (porcentaje > 1)
                    {
                        porcentaje--;
                        carga.text = porcentaje.ToString() + "%";
                    }
                }

                passedminutes++;
                m++;
                if (m >= 60)
                {
                    m = 0;
                    h++;
                    if (h >= 24)
                    {
                        h = 0;
                        d++;
                        for (int i = 0; i < dia.Length; i++)
                            dia[i].text = d.ToString() + "/" + mes.ToString() + "/" + a.ToString();
                    }
                }

                if (h == 0 && m < 10)
                    for (int i = 0; i < hora.Length; i++)
                        hora[i].text = "0" + h.ToString() + ":0" + m.ToString();
                else if (h == 0)
                    for (int i = 0; i < hora.Length; i++)
                        hora[i].text = "0" + h.ToString() + ":" + m.ToString();
                else if (m < 10)
                    for (int i = 0; i < hora.Length; i++)
                        hora[i].text = h.ToString() + ":0" + m.ToString();
                else
                    for (int i = 0; i < hora.Length; i++)
                        hora[i].text = h.ToString() + ":" + m.ToString();

            }

            base.Update();

            if (panelInicio.active)
            {
                Inicio();
            }

            if (thisObject && !iniciado)
            {
                Personaje.ActionText("luz", 0); //Activamos el texto "encender"

                if (Input.GetKeyDown("e"))
                {
                    panelInicio.SetActive(true);
                    iniciado = true;
                }
            }
        }
    }

    void Inicio()
    {
        Animator inicio = panelInicio.GetComponent<Animator>();
        if (inicio.GetCurrentAnimatorStateInfo(0).IsName("fin"))
        {
            panelInicio.SetActive(false);
            panelBloqueo.SetActive(true);
            panelBarra.SetActive(true);
        }
    }

    public void PantallaBloqueo()
    {
        panelBloqueo.SetActive(false);
        panelContrasenya.SetActive(true);
    }

    public void Desbloquear()
    {
        string boton = EventSystem.current.currentSelectedGameObject.name;
        if (boton == "c")
        {
            pwd = "";
            for (int i = 0; i < passwordTexts.Length; i++)
                passwordTexts[i].text = "";
        }
        else if (boton == "x")
        {
            if (pwd.Length > 0)
            {
                passwordTexts[pwd.Length - 1].text = "";
                pwd = pwd.Substring(0, pwd.Length - 1);
            }
        }
        else
        {
            if(pwd.Length < 4)
            {
                pwd += boton;
                passwordTexts[pwd.Length - 1].text = boton;
                if (pwd == password)
                {
                    panelContrasenya.SetActive(false);
                    buttonCentro.interactable = true;
                    panelEscritorio.SetActive(true);
                }
            }
        }
    }

    public void Idioma()
    {
        int i = PlayerPrefs.GetInt("Idioma");
        switch (i)
        {
            case 0:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/movilESP").text;
                break;
            case 1:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/movilCAT").text;
                break;
            case 2:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/movilENG").text;
                break;
        }

        for(int j = 0; j < chatImages.Length; j++)
        {
            if (i == 0)
                chatImages[j].sprite = Resources.Load<Sprite>("Sprites/chat" + chatImages[j].name + "ESP");
            else if (i == 1)
                chatImages[j].sprite = Resources.Load<Sprite>("Sprites/chat" + chatImages[j].name + "CAT");
            else if (i == 2)
                chatImages[j].sprite = Resources.Load<Sprite>("Sprites/chat" + chatImages[j].name + "ENG");
        }

        string[] lines = fileContent.Split("\n"[0]);
        for(int j = 0; j < lines.Length; j++)
        {
            string[] sections = lines[j].Split(";"[0]);
            if(sections[0] == "pin")
            {
                pinText.text = sections[1];
            }
        }
    }

    public void Chat()
    {
        libretaPruebas.BuscarPruebaAparatoElectronico("chat" + EventSystem.current.currentSelectedGameObject.name);
    }

    public void Correo()
    {
        libretaPruebas.BuscarPruebaAparatoElectronico("correo:" + EventSystem.current.currentSelectedGameObject.name);

        bool existe = false;
        string[] lines = fileContent.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] thisLine = lines[i].Split(";"[0]);
            if ("correo:" + EventSystem.current.currentSelectedGameObject.name == "correo:correo" && thisLine[0] == "correo:correo")
            {
                existe = true;

                int n_opciones = int.Parse(thisLine[1]);
                int n_correos = int.Parse(thisLine[2]);
                int n_total = n_opciones + n_correos;

                Text[] opciones = panelOpcionesCorreo.GetComponentsInChildren<Text>();
                Text[] correos = panelCorreos.GetComponentsInChildren<Text>();

                for (int j = 0; j < n_total; j++)
                {
                    if (j < n_opciones)
                    {
                        opciones[j].text = thisLine[j + 3];
                        opciones[j].transform.parent.name = thisLine[j + 3];
                    }
                    else
                    {
                        correos[j - opciones.Length].text = thisLine[j + 3];
                        correos[j - opciones.Length].transform.parent.name = thisLine[j + 3];
                    }
                }
                break;
            }
            else if (thisLine[0] == "correo:" + EventSystem.current.currentSelectedGameObject.name)
            {
                existe = true;
                Text[] correoTexts = panelCorreoConcreto.GetComponentsInChildren<Text>();
                for (int j = 0; j < correoTexts.Length; j++)
                {
                    correoTexts[j].text = "";
                    string[] message = thisLine[j + 1].Split("/"[0]);
                    for (int x = 0; x < message.Length; x++)
                    {
                        correoTexts[j].text += message[x] + "\n\n";
                    }
                }
                break;
            }
        }

        if (!existe)
        {
            panelCorreos.gameObject.SetActive(true);
            panelCorreoConcreto.gameObject.SetActive(false);
        }
    }
}
