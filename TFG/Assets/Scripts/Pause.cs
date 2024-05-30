using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Pause : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject[] panels;
    public Slider[] sliders;
    public static bool paused;
    public Text[] partidasGuardadasText;
    public GameObject panelSobrescribir;
    private bool sobrescribir = false;
    private int selectedSlotToOverwrite;
    private string generalpath = "LanguageFiles/menuPause";
    private string espfile = "ESP";
    private string catfile = "CAT";
    private string engfile = "ENG";

    private void Start()
    {
        sliders[0].value = PlayerPrefs.GetFloat("Music");
        sliders[1].value = PlayerPrefs.GetFloat("Effects");
        Volumen();

        for(int i = 0; i < partidasGuardadasText.Length; i++)
        {
            if (File.Exists(SavingManager.path + i.ToString()))
            {
                partidasGuardadasText[i].text = SavingManager.Cargar(i).name + " " + SavingManager.Cargar(i).timeplayed;
                partidasGuardadasText[i].name = "usedSlot";
            }
        }

        paused = false;
        pauseCanvas.SetActive(false);
        panels[0].SetActive(true);

        Idioma(PlayerPrefs.GetInt("Idioma"));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            if (paused) //Desactivamos todos los paneles excepto el del menú principal, que debe coincidir con panels[0]
            {
                Personaje.tiempoDescanso = Time.unscaledTime - PlayerPrefs.GetFloat("TiempoJugado"); //Si salimos de la pausa actualizamos el tiempo de descanso
                panels[0].SetActive(true);

                for (int i = 1; i < panels.Length; i++)
                {
                    panels[i].SetActive(false);
                }
            }
            else
            {
                PlayerPrefs.SetFloat("TiempoJugado", Time.unscaledTime - Personaje.tiempoDescanso); //Si pausamos el juego, actualizamos el tiempo jugado
            }

            paused = !paused;
            pauseCanvas.SetActive(!pauseCanvas.active);
        }
    }

    public void GuardarPartida(int selectedSlot)
    {
        if(!File.Exists(SavingManager.path + selectedSlot.ToString()) || sobrescribir)
        {
            PlayerPrefs.SetFloat("TiempoJugado", Time.unscaledTime - Personaje.tiempoDescanso); //Actualizamos el tiempo jugado antes de guardar partida
            SavingManager.Guardar(selectedSlot, FindObjectOfType<Personaje>(), FindObjectOfType<LibretaPruebas>(), FindObjectOfType<Inventario>(), FindObjectsOfType<Light>());
            float tiempo_total = SavingManager.Cargar(selectedSlot).timeplayed;
            int minutos = (int)tiempo_total / 60;
            int segundos = (int)(tiempo_total - (60 * minutos));
            partidasGuardadasText[selectedSlot].text = SavingManager.Cargar(selectedSlot).name + " " + minutos + ":" + segundos;
            partidasGuardadasText[selectedSlot].name = "usedSlot";
            sobrescribir = false;
        }
        else
        {
            panelSobrescribir.SetActive(true);
            selectedSlotToOverwrite = selectedSlot;
        }
    }

    public void Sobrescribir()
    {
        sobrescribir = true;
        GuardarPartida(selectedSlotToOverwrite);
    }

    public void Idioma(int i)
    {
        PlayerPrefs.SetInt("Idioma", i);
        string fileContents = "";
        switch (i)
        {
            case 0:
                fileContents = Resources.Load<TextAsset>(generalpath + espfile).text;
                break;
            case 1:
                fileContents = Resources.Load<TextAsset>(generalpath + catfile).text;
                break;
            case 2:
                fileContents = Resources.Load<TextAsset>(generalpath + engfile).text;
                break;
        }
        string[] lines = fileContents.Split("\n"[0]);
        Text[] pauseTexts = GetComponentsInChildren<Text>(true);
        for(int j = 0; j < pauseTexts.Length; j++)
        {
            for(int h = 0; h < lines.Length; h++)
            {
                string[] sections = lines[h].Split(";"[0]);
                if (pauseTexts[j].name == sections[0])
                {
                    pauseTexts[j].text = sections[1];
                }
            }
        }
        FindObjectOfType<Personaje>().Idioma();
        FindObjectOfType<AparatoElectronicoMovil>().Idioma();
        FindObjectOfType<AparatoElectronicoEstatico>().Idioma();
        FindObjectOfType<LibretaPruebas>().Idioma();
        FindObjectOfType<FinJuego>().Idioma();
        Puzzles[] puzz = FindObjectsOfType<Puzzles>();
        for (int j = 0; j < puzz.Length; j++)
        {
            puzz[j].Idioma();
        }
        if (i == 0)
            Resources.Load<Material>("Materials/amenaza").mainTexture = Resources.Load<Texture>("Textures/amenaza" + "ESP");
        else if (i == 1)
            Resources.Load<Material>("Materials/amenaza").mainTexture = Resources.Load<Texture>("Textures/amenaza" + "CAT");
        else if (i == 2)
            Resources.Load<Material>("Materials/amenaza").mainTexture = Resources.Load<Texture>("Textures/amenaza" + "ENG");
        for (int j = 1; j < 13; j++)
        {
            if (i == 0)
                Resources.Load<Material>("Materials/calendario" + j.ToString()).mainTexture = Resources.Load<Texture>("Textures/calendario" + j.ToString() + "ESP");
            else if (i == 1)
                Resources.Load<Material>("Materials/calendario" + j.ToString()).mainTexture = Resources.Load<Texture>("Textures/calendario" + j.ToString() + "CAT");
            else if (i == 2)
                Resources.Load<Material>("Materials/calendario" + j.ToString()).mainTexture = Resources.Load<Texture>("Textures/calendario" + j.ToString() + "ENG");
        }
    }

    public void MusicVolume(Slider s)
    {
        PlayerPrefs.SetFloat("Music", s.value);
        Volumen();
    }

    public void EffectsVolume(Slider s)
    {
        PlayerPrefs.SetFloat("Effects", s.value);
        Volumen();
    }

    private void Volumen()
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        for(int i = 0; i < audios.Length; i++)
        {
            if(audios[i].clip.name.Split(" "[0])[0] == "music")
            {
                audios[i].volume = PlayerPrefs.GetFloat("Music");
            }
            else if(audios[i].clip.name.Split(" "[0])[0] == "effect")
            {
                audios[i].volume = PlayerPrefs.GetFloat("Effects");
            }
        }
    }

    public void Salir()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
