using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Personaje : MonoBehaviour
{
    public float vel = 1.0f;
    public float sens = 20.0f;
    private Camera cam;
    private Vector2 center;
    private string generalpath = "LanguageFiles/actionText";
    private string espfile = "ESP";
    private string catfile = "CAT";
    private string engfile = "ENG";
    public static string actionTextContent = "";
    private static string[] lines;
    private static string[,] lineSections = new string[100,5];
    public Canvas actionTextCanvas;
    public static Text[] actionTextObjects;
    public static float tiempoDescanso;
    public Animator startAnimation;
    private bool iniciado = false;
    public Canvas canvasdeteccion;
    public static Image puntodedeteccion;

    // Start is called before the first frame update
    void Start()
    {
        puntodedeteccion = canvasdeteccion.GetComponentInChildren<Image>();

        //Guardamos el tiempo no jugado = tiempo del menú + de cargado de escenas
        tiempoDescanso = Time.unscaledTime - PlayerPrefs.GetFloat("TiempoJugado");

        cam = Camera.main;

        if(PlayerPrefs.GetFloat("PositionY") != 0)
        {
            Vector3 position = new Vector3(PlayerPrefs.GetFloat("PositionX"), PlayerPrefs.GetFloat("PositionY"), PlayerPrefs.GetFloat("PositionZ"));
            transform.position = position;
            Vector3 charrotation = new Vector3(PlayerPrefs.GetFloat("CharRotationX"), PlayerPrefs.GetFloat("CharRotationY"), PlayerPrefs.GetFloat("CharRotationZ"));
            transform.eulerAngles = charrotation;
            Vector3 rotation = new Vector3(PlayerPrefs.GetFloat("RotationX"), PlayerPrefs.GetFloat("RotationY"), PlayerPrefs.GetFloat("RotationZ"));
            cam.transform.eulerAngles = rotation;
        }

        actionTextObjects = actionTextCanvas.GetComponentsInChildren<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused && (startAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1))
        {
            if(!cam.GetComponent<AudioSource>().isPlaying)
            {
                cam.GetComponent<AudioSource>().Play();
            }
            Movimiento();
            RayDetector();
        }
    }

    void Movimiento()
    {
        float h = Input.GetAxis("Horizontal") * vel * Time.deltaTime * 10;
        float v = Input.GetAxis("Vertical") * vel * Time.deltaTime * 10;

        transform.Translate(h, 0, 0);
        transform.Translate(0, 0, v);

        if (!iniciado && (h != 0 || v != 0))
            iniciado = true;

        if ((h != 0 || v != 0) && !transform.GetComponent<AudioSource>().isPlaying)
        {
            transform.GetComponent<AudioSource>().Play();
        }

        float rx = Input.GetAxis("Mouse X") * sens * Time.deltaTime * 5;
        float ry = -Input.GetAxis("Mouse Y") * sens * Time.deltaTime * 2;

        transform.Rotate(0, rx, 0);
        cam.transform.Rotate(ry, 0, 0);
    }

    void RayDetector()
    {
        center = new Vector2(Screen.width / 2, Screen.height / 2);

        for (int i = 0; i < Personaje.actionTextObjects.Length; i++)
        {
            Personaje.actionTextObjects[i].text = "";
        }

        if (!iniciado)
        {
            ActionText("inicial", 0);
        }

        var ray = cam.ScreenPointToRay(center);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3))
        {
            Transform objT = hit.transform;
            if (objT.CompareTag("LookCamera"))
            {
                var obj = objT.GetComponent<ObjetoEstatico>();
                obj.thisObject = true;
            }
            else if (objT.CompareTag("LookObj"))
            {
                var obj = objT.GetComponent<ObjetoMovil>();
                obj.thisObject = true;
            }
            else if (objT.CompareTag("Door"))
            {
                var obj = objT.GetComponent<ObjetosPuerta>();
                obj.thisDoor = true;
            }
            else if (objT.CompareTag("Light"))
            {
                var obj = objT.GetComponent<ObjetosLuz>();
                obj.thisLight = true;
            }
            else if (objT.CompareTag("Baffle"))
            {
                var obj = objT.GetComponent<Baffle>();
                obj.thisbaffle = true;
            }
            else if (objT.CompareTag("Fin"))
            {
                var obj = objT.GetComponent<FinJuego>();
                obj.thisObject = true;
            }
        }
    }

    public void Idioma()
    {
        int idioma = PlayerPrefs.GetInt("Idioma");

        switch (idioma)
        {
            case 0:
                actionTextContent = Resources.Load<TextAsset>(generalpath + espfile).text;
                break;
            case 1:
                actionTextContent = Resources.Load<TextAsset>(generalpath + catfile).text;
                break;
            case 2:
                actionTextContent = Resources.Load<TextAsset>(generalpath + engfile).text;
                break;
        }

        lines = actionTextContent.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] sections = lines[i].Split(";"[0]);
            for(int j = 0; j < sections.Length; j++)
            {
                lineSections[i,j] = sections[j];
            }
        }
    }

    /*Este método define el texto de acción de forma general*/
    public static void ActionText(string type, float number)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            //Buscamos las acciones correspondientes a los objetos "type"
            if (lineSections[i,0] == type)
            {
                //Comprovamos el estado
                if (int.Parse(lineSections[i,1]) == number)
                {
                    for (int j = 0; j < Personaje.actionTextObjects.Length; j++)
                    {
                        //Recorremos los Text hasta que encontramos el que debe usar esta acción
                        if (Personaje.actionTextObjects[j].name == "ActionText " + lineSections[i,3])
                        {
                            Personaje.actionTextObjects[j].text = lineSections[i,2];
                        }
                    }
                }
            }
        }
    }
}
