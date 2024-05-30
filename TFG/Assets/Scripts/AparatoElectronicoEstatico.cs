using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class AparatoElectronicoEstatico : ObjetoEstatico
{
    public GameObject canvas;
    public string password;
    private GameObject panelInicio;
    private GameObject panelContrasenya;
    private GameObject panelEscritorio;
    private GameObject folderParent;
    private GameObject panelOpcionesCorreo;
    private GameObject panelCorreos;
    private GameObject panelCorreoConcreto;
    private GameObject panelHistorial;
    public GameObject escritorioTextsPanel;
    public GameObject internetTextsPanel;
    private bool iniciado = false;
    public Text hora;
    public Text dia;
    private int h = 17;
    private int m = 32;
    private int passedminutes = 0;
    private int d = 12;
    private int mes = 11;
    private int a = 2019;
    private string fileContent = "";
    private string[] carpetasAbiertas = new string[10];
    private int nCarpetas = 0;

    // Start is called before the first frame update
    void Start()
    {
        hora.text = h.ToString() + ":" + m.ToString();
        dia.text = d.ToString() + "/" + mes.ToString() + "/" + a.ToString();

        base.Start();

        Transform[] canvasChildren = canvas.GetComponentsInChildren<Transform>(true);
        for(int i = 0; i < canvasChildren.Length; i++)
        {
            if (canvasChildren[i].name == "inicio")
            {
                panelInicio = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "contrasenya")
            {
                panelContrasenya = canvasChildren[i].gameObject;
            }
            else if (canvasChildren[i].name == "escritorio")
            {
                panelEscritorio = canvasChildren[i].gameObject;
            }
            else if(canvasChildren[i].name == "panel folder")
            {
                folderParent = canvasChildren[i].gameObject;
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
            else if (canvasChildren[i].name == "panel historial")
            {
                panelHistorial = canvasChildren[i].gameObject;
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
                        dia.text = d.ToString() + "/" + mes.ToString() + "/" + a.ToString();
                    }
                }

                if (h == 0 && m < 10)
                    hora.text = "0" + h.ToString() + ":0" + m.ToString();
                else if (h == 0)
                    hora.text = "0" + h.ToString() + ":" + m.ToString();
                else if (m < 10)
                    hora.text = h.ToString() + ":0" + m.ToString();
                else
                    hora.text = h.ToString() + ":" + m.ToString();

            }

            base.Update();

            if (panelInicio.active)
            {
                Inicio();
            }

            if(thisObject && !iniciado)
            {
                Personaje.ActionText("luz",0); //Activamos el texto "encender"

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
        if(inicio.GetCurrentAnimatorStateInfo(0).IsName("fin"))
        { 
            panelInicio.SetActive(false);
            panelContrasenya.SetActive(true);
        }
    }

    public void Desbloquear(InputField inpf)
    {
        string pwd = inpf.text;
        if(pwd == password)
        {
            panelContrasenya.SetActive(false);
            panelEscritorio.SetActive(true);
        }
    }

    public void Idioma()
    {
        int i = PlayerPrefs.GetInt("Idioma");
        switch(i)
        {
            case 0:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/ordenadorESP").text;
                break;
            case 1:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/ordenadorCAT").text;
                break;
            case 2:
                fileContent = Resources.Load<TextAsset>("LanguageFiles/ordenadorENG").text;
                break;
        }

        string[] lines = fileContent.Split("\n"[0]);
        for (int j = 0; j < lines.Length; j++)
        {
            string[] thisLine = lines[j].Split(";"[0]);
            if(thisLine[0] == "escritorio")
            {
                Text[] escritorioTexts = escritorioTextsPanel.GetComponentsInChildren<Text>();
                for(int x = 2; x < thisLine.Length - 1; x++)
                {
                    escritorioTexts[x-2].text = thisLine[x];
                    if (escritorioTexts[x - 2].GetComponentInChildren<Button>() != null)
                        escritorioTexts[x - 2].GetComponentInChildren<Button>().name = thisLine[x];
                }
            }
            else if(thisLine[0] == "internet")
            {
                Text[] internetTexts = internetTextsPanel.GetComponentsInChildren<Text>();
                for (int x = 2; x < thisLine.Length - 1; x++)
                {
                    internetTexts[x - 2].text = thisLine[x];
                }
            }
        }
    }

    public void Historial()
    {
        string[] lines = fileContent.Split("\n"[0]);
        for(int i = 0; i < lines.Length; i++)
        {
            string[] thisLine = lines[i].Split(";"[0]);
            if(thisLine[0] == "historial")
            {
                Text[] txts = panelHistorial.GetComponentsInChildren<Text>();
                for(int j = 0; j < txts.Length - 1; j++)
                {
                    txts[j+1].text = thisLine[j + 1];
                }
                break;
            }
        }

        libretaPruebas.BuscarPruebaAparatoElectronico("historial");
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
                    if(j < n_opciones)
                    {
                        opciones[j].text = thisLine[j+3];
                        opciones[j].transform.parent.name = thisLine[j+3];
                    }
                    else
                    {
                        correos[j-opciones.Length].text = thisLine[j+3];
                        correos[j-opciones.Length].transform.parent.name = thisLine[j+3];
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
                    for(int x = 0; x < message.Length; x++)
                    {
                        correoTexts[j].text += message[x] + "\n\n";
                    }
                }
                break;
            }
        }
        
        if(!existe)
        {
            panelCorreos.transform.parent.gameObject.SetActive(true);
            panelCorreoConcreto.transform.parent.gameObject.SetActive(false);
        }
    }

    public void Carpeta()
    {
        libretaPruebas.BuscarPruebaAparatoElectronico("carpeta:" + EventSystem.current.currentSelectedGameObject.name);

        string[] lines = fileContent.Split("\n"[0]);
        for(int i = 0; i < lines.Length; i++)
        {
            string[] thisLine = lines[i].Split(";"[0]);
            if(thisLine[0] == "carpeta:" + EventSystem.current.currentSelectedGameObject.name)
            {
                //Guardamos la carpeta en la lista de carpetas abiertas
                carpetasAbiertas[nCarpetas] = EventSystem.current.currentSelectedGameObject.name;
                nCarpetas++;

                /*Borramos los elementos de la carpeta que se había abierto anteriormente*/
                Transform[] elementos = folderParent.GetComponentsInChildren<Transform>();
                for (int x = 1; x < elementos.Length; x++)
                {
                    Destroy(elementos[x].gameObject);
                }

                int n_folders = int.Parse(thisLine[1]); //Sacamos el número de carpetas que contiene esa carpeta
                int n_documents = int.Parse(thisLine[2]); //Sacamos el número de documentos que contiene la carpeta
                int n_images = int.Parse(thisLine[3]); //Sacamos el número de imágenes que contiene la carpeta
                int n_music = int.Parse(thisLine[4]); //Sacamos el número de arhivos de audio que contiene la carpeta
                int n_total = n_folders + n_documents + n_images + n_music;

                for (int j = 0; j < n_total; j++)
                { 
                    /*Creamos cada uno de los elementos de tipo carpeta, con su imagen, su texto y su botón correspondiente*/
                    GameObject elemento = new GameObject(thisLine[j+5].Split("_"[0])[0], typeof(RectTransform));
                    elemento.transform.parent = folderParent.transform;
                    elemento.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    elemento.transform.localPosition = new Vector3(elemento.transform.localPosition.x, elemento.transform.localPosition.y, 0);
                    elemento.transform.Rotate(0,-90,0);

                    Image im = elemento.AddComponent<Image>();
                    im.preserveAspect = true;
                    Button b = elemento.AddComponent<Button>();

                    if (j < n_folders)
                    {
                        im.sprite = Resources.Load<Sprite>("Sprites/pantallasCarpetaArchivos");
                        b.onClick.AddListener(Carpeta);
                    }
                    else if(j < n_folders + n_documents)
                    {
                        im.sprite = Resources.Load<Sprite>("Sprites/archivodoc");
                    }
                    else if(j < n_folders + n_documents + n_images)
                    {
                        im.sprite = Resources.Load<Sprite>("Sprites/fotos/" + elemento.name);
                    }
                    else if(j < n_folders + n_documents + n_music)
                    {
                        im.sprite = Resources.Load<Sprite>("Sprites/archivomusic");
                    }

                    GameObject texto = new GameObject("texto", typeof(RectTransform));
                    texto.transform.parent = elemento.transform;
                    texto.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    texto.transform.localPosition = new Vector3(0, 0, 0);
                    texto.transform.Rotate(0,-90,0);
                    Text text = texto.AddComponent<Text>();
                    text.font = Resources.Load<Font>("Fonts/Typo");
                    text.text = thisLine[j+5];
                    text.alignment = TextAnchor.LowerCenter;
                    text.color = Color.black;
                    text.fontSize = 10;
                }

                break; //Salimos del bucle for cuando ya hemos encontrado la carpeta
            }
        }
    }

    public void CarpetaAnterior()
    {
        if(nCarpetas > 1)
        {
            carpetasAbiertas[nCarpetas - 1] = "";
            nCarpetas--;

            //Repetimos el proceso de abrir carpeta con la carpeta anterior = carpetasAbiertas[nCarpetas - 1]
            string[] lines = fileContent.Split("\n"[0]);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] thisLine = lines[i].Split(";"[0]);
                if (thisLine[0] == "carpeta:" + carpetasAbiertas[nCarpetas - 1])
                {
                    /*Borramos los elementos de la carpeta que se había abierto anteriormente*/
                    Transform[] elementos = folderParent.GetComponentsInChildren<Transform>();
                    for (int x = 1; x < elementos.Length; x++)
                    {
                        Destroy(elementos[x].gameObject);
                    }

                    int n_folders = int.Parse(thisLine[1]); //Sacamos el número de carpetas que contiene esa carpeta
                    int n_documents = int.Parse(thisLine[2]); //Sacamos el número de documentos que contiene la carpeta
                    int n_images = int.Parse(thisLine[3]); //Sacamos el número de imágenes que contiene la carpeta
                    int n_music = int.Parse(thisLine[4]); //Sacamos el número de arhivos de audio que contiene la carpeta
                    int n_total = n_folders + n_documents + n_images + n_music;

                    for (int j = 0; j < n_total; j++)
                    {
                        /*Creamos cada uno de los elementos de tipo carpeta, con su imagen, su texto y su botón correspondiente*/
                        GameObject elemento = new GameObject(thisLine[j + 5].Split("_"[0])[0], typeof(RectTransform));
                        elemento.transform.parent = folderParent.transform;
                        elemento.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        elemento.transform.localPosition = new Vector3(elemento.transform.localPosition.x, elemento.transform.localPosition.y, 0);
                        elemento.transform.Rotate(0, -90, 0);

                        Image im = elemento.AddComponent<Image>();
                        im.preserveAspect = true;
                        Button b = elemento.AddComponent<Button>();

                        if (j < n_folders)
                        {
                            im.sprite = Resources.Load<Sprite>("Sprites/pantallasCarpetaArchivos");
                            b.onClick.AddListener(Carpeta);
                        }
                        else if (j < n_folders + n_documents)
                        {
                            im.sprite = Resources.Load<Sprite>("Sprites/archivodoc");
                        }
                        else if (j < n_folders + n_documents + n_images)
                        {
                            im.sprite = Resources.Load<Sprite>("Sprites/fotos/" + elemento.name);
                        }
                        else if (j < n_folders + n_documents + n_music)
                        {
                            im.sprite = Resources.Load<Sprite>("Sprites/archivomusic");
                        }

                        GameObject texto = new GameObject("texto", typeof(RectTransform));
                        texto.transform.parent = elemento.transform;
                        texto.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        texto.transform.localPosition = new Vector3(0, 0, 0);
                        texto.transform.Rotate(0, -90, 0);
                        Text text = texto.AddComponent<Text>();
                        text.font = Resources.Load<Font>("Fonts/Typo");
                        text.text = thisLine[j + 5];
                        text.alignment = TextAnchor.LowerCenter;
                        text.color = Color.black;
                        text.fontSize = 10;
                    }

                    break; //Salimos del bucle for cuando ya hemos encontrado la carpeta
                }
            }
        }
    }

    public void CerrarCarpeta()
    {
        for(int i = 0; i < carpetasAbiertas.Length; i++)
        {
            carpetasAbiertas[i] = "";
            nCarpetas = 0;
        }
    }
}
