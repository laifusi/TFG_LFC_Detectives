using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Menu : MonoBehaviour
{
    public Button iniciar;
    public Button continuar;
    public Button[] partidasGuardadas;
    public Animator exitAnimation;
    public Text cargandoText;
    public Text textoNuevaPartida;

    private bool detSelected, nameSelected;
    private string[] cargando = new string[3];
    private string[] vacio = new string[3];
    private string[] inicio = new string[3];

    // Start is called before the first frame update
    void Start()
    {
        /*Borramos los datos guardados anteriores*/
        PlayerPrefs.DeleteAll();

        cargando[0] = "Cargando...";
        cargando[1] = "Carregant...";
        cargando[2] = "Loading...";
        vacio[0] = "Vacío";
        vacio[1] = "Buit";
        vacio[2] = "Empty";
        inicio[0] = "Selecciona un personaje y escribe un nombre";
        inicio[1] = "Selecciona un personatge i escriu un nom";
        inicio[2] = "Select a character and write a name";

        continuar.interactable = false;
        for (int i = 0; i < partidasGuardadas.Length; i++)
        {
            Text text = partidasGuardadas[i].GetComponentInChildren<Text>();
            if(File.Exists(SavingManager.path + i.ToString()))
            {
                continuar.interactable = true;
                partidasGuardadas[i].interactable = true;
                float tiempo_total = SavingManager.Cargar(i).timeplayed;
                int minutos = (int)tiempo_total / 60;
                int segundos = (int)(tiempo_total - (60 * minutos));
                text.text = SavingManager.Cargar(i).name + " " + minutos + ":" + segundos;
            }
            else
            {
                partidasGuardadas[i].interactable = false;
            }
        }
        
        /*Inicializamos el volumen a 1 y el idioma a español*/
        PlayerPrefs.SetFloat("Music",1f);
        PlayerPrefs.SetFloat("Effects", 1f);
        Idioma(0);
        Volumen();
    }

    public void setCharacter(int i)
    {
        PlayerPrefs.SetInt("Detective", i);
        detSelected = true;
        if (nameSelected)
            iniciar.interactable = true;
    }

    public void setName(InputField ipf)
    {
        PlayerPrefs.SetString("Nombre", ipf.text);
        nameSelected = true;
        if (detSelected)
            iniciar.interactable = true;
    }

    public void iniciarPartida()
    {
        StartCoroutine(ExitAnimation(1));
    }

    public void cargarPartida(int i)
    {
        GameData data = SavingManager.Cargar(i);
        PlayerPrefs.SetInt("Idioma", data.idioma);
        PlayerPrefs.SetInt("Detective", data.detective);
        PlayerPrefs.SetString("Nombre", data.name);
        PlayerPrefs.SetFloat("PositionX", data.position[0]);
        PlayerPrefs.SetFloat("PositionY", data.position[1]);
        PlayerPrefs.SetFloat("PositionZ", data.position[2]);
        PlayerPrefs.SetFloat("CharRotationX", data.charrotation[0]);
        PlayerPrefs.SetFloat("CharRotationY", data.charrotation[1]);
        PlayerPrefs.SetFloat("CharRotationZ", data.charrotation[2]);
        PlayerPrefs.SetFloat("RotationX", data.camrotation[0]);
        PlayerPrefs.SetFloat("RotationY", data.camrotation[1]);
        PlayerPrefs.SetFloat("RotationZ", data.camrotation[2]);
        PlayerPrefs.SetFloat("TiempoJugado", data.timeplayed);
        PlayerPrefs.SetInt("NumeroPistasPersonales", data.pistasPers.Length);
        PlayerPrefs.SetInt("NumeroPistasProfesionales", data.pistasProf.Length);
        PlayerPrefs.SetInt("NumeroPistasOtros", data.pistasOtro.Length);
        for (int j = 0; j < data.pistasPers.Length; j++)
        {
            PlayerPrefs.SetInt("PistasPersonales" + j.ToString(), data.pistasPers[j]);
        }
        for (int j = 0; j < data.pistasProf.Length; j++)
        {
            PlayerPrefs.SetInt("PistasProfesionales" + j.ToString(), data.pistasProf[j]);
        }
        for (int j = 0; j < data.pistasOtro.Length; j++)
        {
            PlayerPrefs.SetInt("PistasOtros" + j.ToString(), data.pistasOtro[j]);
        }
        for(int j = 0; j < data.inventario.Length; j++)
        {
            PlayerPrefs.SetString("ObjetoInventario" + j.ToString(), data.inventario[j]);
        }
        PlayerPrefs.SetInt("NumeroLucesON", data.lights.Length);
        for (int j = 0; j < data.lights.Length; j++)
        {
            PlayerPrefs.SetString("Luz" + j.ToString(), data.lights[j]);
        }

        StartCoroutine(ExitAnimation(2));
    }

    IEnumerator ExitAnimation(int i)
    {
        exitAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(i);
    }

    public void Idioma(int i)
    {
        PlayerPrefs.SetInt("Idioma", i);

        string lang = "";
        if (i == 0)
            lang = "esp";
        else if (i == 1)
            lang = "cat";
        else if (i == 2)
            lang = "eng";

        Image[] objs = Resources.FindObjectsOfTypeAll(typeof(Image)) as Image[];
        //Le asignamos el sprite cuyo nombre coincide con el nombre del gameobject + el idioma seleccionado
        for (int j = 0; j < objs.Length; j++)
        {
            Sprite s = Resources.Load<Sprite>("Sprites/" + objs[j].name + lang);
            if (s != null)
                objs[j].sprite = s;
        }

        cargandoText.text = cargando[i];
        textoNuevaPartida.text = inicio[i];

        for (int j = 0; j < partidasGuardadas.Length; j++)
        {
            if (!partidasGuardadas[j].interactable)
            {
                partidasGuardadas[j].GetComponentInChildren<Text>().text = vacio[i];
            }
        }
    }

    public void MusicVolume(Slider s)
    {
        PlayerPrefs.SetFloat("Music",s.value);
        Volumen();
    }

    public void EffectsVolume(Slider s)
    {
        PlayerPrefs.SetFloat("Effects", s.value);
        Volumen();
    }

    private void Volumen()
    {
        AudioSource[] sounds = FindObjectsOfType<AudioSource>();
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clip.name == "menuMusic")
                sounds[i].volume = PlayerPrefs.GetFloat("Music");
            else
                sounds[i].volume = PlayerPrefs.GetFloat("Effects");
        }
    }

    public void Salir()
    {
        Application.Quit();
    }
}
