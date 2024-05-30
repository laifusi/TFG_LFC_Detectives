using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class FinJuego : MonoBehaviour
{
    public GameObject panelConfirmacion;
    public GameObject panelPuntuacion;
    public LibretaPruebas l;
    private string generalpath = "LanguageFiles/fin";
    private string espfile = "ESP";
    private string catfile = "CAT";
    private string engfile = "ENG";
    private string fileContent = "";
    public Animator exitAnimation;
    public bool thisObject = false;

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused && thisObject)
        {
            if(!panelConfirmacion.activeSelf && !panelPuntuacion.activeSelf)
                Personaje.ActionText("fin",0);

            if(Input.GetKeyDown("f"))
            {
                panelConfirmacion.SetActive(true);
            }
        }

        thisObject = false;
    }

    public void Resolver()
    {
        Time.timeScale = 0;

        int pistas = 0;
        int pistas_totales = l.getTotalPistas();
        float porcentaje;
        int[] pistasPers = l.getPistas("personales");
        int[] pistasProf = l.getPistas("profesionales");
        int[] pistasOtro = l.getPistas("otros");

        PlayerPrefs.SetFloat("TiempoJugado", Time.unscaledTime - Personaje.tiempoDescanso);

        for (int i = 0; i < pistasPers.Length; i++)
        {
            if (pistasPers[i] != 0)
                pistas++;
        }

        for (int i = 0; i < pistasProf.Length; i++)
        {
            if (pistasProf[i] != 0)
                pistas++;
        }

        for (int i = 0; i < pistasOtro.Length; i++)
        {
            if (pistasOtro[i] != 0)
                pistas++;
        }

        porcentaje = pistas * 100 / pistas_totales;
        
        Text[] textElements = panelPuntuacion.GetComponentsInChildren<Text>();
        for(int i = 0; i < textElements.Length; i++)
        {
            if(textElements[i].name.Split(" "[0])[0] == "texto")
            {
                string[] lines = fileContent.Split("\n"[0]);
                for(int j = 0; j < lines.Length; j++)
                {
                    string[] sections = lines[j].Split(";"[0]);
                    if(sections[0] == textElements[i].name.Split(" "[0])[1])
                    {
                        textElements[i].text = sections[1];
                        break;
                    }
                }
            }
            else if(textElements[i].name.Split(" "[0])[0] == "valor")
            {
                if (textElements[i].name.Split(" "[0])[1] == "jugador")
                {
                    textElements[i].text = PlayerPrefs.GetString("Nombre");
                }
                else if (textElements[i].name.Split(" "[0])[1] == "tiempo")
                {
                    float tiempo = PlayerPrefs.GetFloat("TiempoJugado");
                    int minutos = (int)tiempo / 60;
                    int segundos = (int)(tiempo - (60 * minutos));
                    int horas = (int)minutos / 60;
                    minutos = (int)(minutos - (60 * horas));
                    if (horas > 0)
                        textElements[i].text = horas.ToString() + "h " + minutos.ToString() + "min " + segundos.ToString() + "s";
                    else
                        textElements[i].text = minutos.ToString() + "min " + segundos.ToString() + "s";
                }
                else if (textElements[i].name.Split(" "[0])[1] == "pistas")
                {
                    textElements[i].text = pistas.ToString();
                }
                else if (textElements[i].name.Split(" "[0])[1] == "porcentaje")
                {
                    textElements[i].text = porcentaje.ToString();
                }
            }
        }
        panelPuntuacion.SetActive(true);
    }

    public void Idioma()
    {
        int idioma = PlayerPrefs.GetInt("Idioma");

        switch (idioma)
        {
            case 0:
                fileContent = Resources.Load<TextAsset>(generalpath + espfile).text;
                break;
            case 1:
                fileContent = Resources.Load<TextAsset>(generalpath + catfile).text;
                break;
            case 2:
                fileContent = Resources.Load<TextAsset>(generalpath + engfile).text;
                break;
        }

        Text[] textosConfirmacion = panelConfirmacion.GetComponentsInChildren<Text>();
        for (int i = 0; i < textosConfirmacion.Length; i++)
        {
            string[] lines = fileContent.Split("\n"[0]);
            for (int j = 0; j < lines.Length; j++)
            {
                string[] sections = lines[j].Split(";"[0]);
                if (sections[0] == textosConfirmacion[i].name)
                {
                    textosConfirmacion[i].text = sections[1];
                    break;
                }
            }
        }

        Text[] textosCreditos = exitAnimation.GetComponentsInChildren<Text>();
        for (int i = 0; i < textosCreditos.Length; i++)
        {
            string[] lines = fileContent.Split("\n"[0]);
            for (int j = 0; j < lines.Length; j++)
            {
                string[] sections = lines[j].Split(";"[0]);
                if (sections[0] == textosCreditos[i].name)
                {
                    textosCreditos[i].text = sections[1];
                    break;
                }
            }
        }
    }

    public void Menu()
    {
        StartCoroutine(ExitAnimationMenu());
    }

    public void Salir()
    {
        StartCoroutine(ExitAnimationSalir());
    }

    IEnumerator ExitAnimationMenu()
    {
        Time.timeScale = 1;
        exitAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    IEnumerator ExitAnimationSalir()
    {
        Time.timeScale = 1;
        exitAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(10f);
        Application.Quit();
    }
}
