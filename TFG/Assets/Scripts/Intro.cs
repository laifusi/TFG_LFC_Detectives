using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    private string pathM = "LanguageFiles/introTextMale";
    private string pathF = "LanguageFiles/introTextFemale";
    private string espfile = "ESP";
    private string catfile = "CAT";
    private string engfile = "ENG";
    private string[] lines;
    private int currentline = 0;
    private float tiempoNoJugado;

    public Image personaje;
    public Text texto;
    public Text indicacion;
    public Text cargando;
    public GameObject intro;
    public Animator exitAnimation;
    public AudioSource introSound;

    void Start()
    {
        //Guardamos el tiempo usado en el menú
        tiempoNoJugado = Time.unscaledTime;
        Idioma();
        UpdateText();
    }

    void Update()
    {
        Volumen();
        if(!intro.active)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                UpdateText();
            }
        }
    }

    private void UpdateText()
    {
        if(currentline < lines.Length)
        {
            string character = lines[currentline].Split(";"[0])[0];
            string text = lines[currentline].Split(";"[0])[1];

            if (character == "detective")
            {
                int det = PlayerPrefs.GetInt("Detective");
                if (det == 0)
                {
                    personaje.sprite = Resources.Load<Sprite>("Sprites/personajesintroDetectiveH");
                }
                else
                {
                    personaje.sprite = Resources.Load<Sprite>("Sprites/personajesintroDetectiveM");
                }
            }
            else
            {
                personaje.sprite = Resources.Load<Sprite>("Sprites/personajesintroLucas");
            }

            texto.text = text;

            if(currentline == 0)
            {
                texto.text += " " + PlayerPrefs.GetString("Nombre") + "?";
            }

            currentline++;
        }
        else
        {
            StartCoroutine(ExitAnimation());
        }
    }

    IEnumerator ExitAnimation()
    {
        //Guardamos en "TiempoJugado" el tiempo que llevamos (sin contar el menú)
        PlayerPrefs.SetFloat("TiempoJugado", Time.unscaledTime - tiempoNoJugado);
        exitAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }

    public void Idioma()
    {
        int id = PlayerPrefs.GetInt("Idioma");
        int det = PlayerPrefs.GetInt("Detective");
        string fileContents = "";

        if(det == 0)
            switch (id)
            {
                case 0:
                    fileContents = Resources.Load<TextAsset>(pathM + espfile).text;
                    break;
                case 1:
                    fileContents = Resources.Load<TextAsset>(pathM + catfile).text;
                    break;
                case 2:
                    fileContents = Resources.Load<TextAsset>(pathM + engfile).text;
                    break;
            }
        else
            switch (id)
            {
                case 0:
                    fileContents = Resources.Load<TextAsset>(pathF + espfile).text;
                    break;
                case 1:
                    fileContents = Resources.Load<TextAsset>(pathF + catfile).text;
                    break;
                case 2:
                    fileContents = Resources.Load<TextAsset>(pathF + engfile).text;
                    break;
            }

        lines = fileContents.Split("\n"[0]);

        switch(id)
        {
            case 0:
                indicacion.text = "pulsa ENTER para continuar";
                cargando.text = "Cargando...";
                break;
            case 1:
                indicacion.text = "polsa ENTER per continuar";
                cargando.text = "Carregant...";
                break;
            case 2:
                indicacion.text = "press ENTER to continue";
                cargando.text = "Loading...";
                break;
        }
    }

    private void Volumen()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music");
        if (!intro.active)
        {
            if (introSound.volume > 0)
            {
                introSound.volume -= 0.005f;
            }
        }
        else
        {
            if (introSound.volume < PlayerPrefs.GetFloat("Effects"))
            {
                introSound.volume += 0.005f;
            }
        }
    }
}
