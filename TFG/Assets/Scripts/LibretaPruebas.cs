using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LibretaPruebas : MonoBehaviour
{
    private int[] pruebasPersonales = new int[18]; //18 es el número de líneas que caben en el diseño que hemos hecho de la libreta de pruebas
    private int[] pruebasProfesionales = new int[18];
    private int[] pruebasOtros = new int[18];
    public GameObject libreta;
    private bool open = false;
    public GameObject panelPersonal;
    public GameObject panelProfesional;
    public GameObject panelOtros;

    private string generalpath = "LanguageFiles/pistas";
    private string espfile = "ESP";
    private string catfile = "CAT";
    private string engfile = "ENG";
    private string fileContents = "";

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < PlayerPrefs.GetInt("NumeroPistasPersonales"); j++)
        {
            pruebasPersonales[j] = PlayerPrefs.GetInt("PistasPersonales" + j.ToString());
        }
        for (int j = 0; j < PlayerPrefs.GetInt("NumeroPistasProfesionales"); j++)
        {
            pruebasProfesionales[j] = PlayerPrefs.GetInt("PistasProfesionales" + j.ToString());
        }
        for (int j = 0; j < PlayerPrefs.GetInt("NumeroPistasOtros"); j++)
        {
            pruebasOtros[j] = PlayerPrefs.GetInt("PistasOtros" + j.ToString());
        }

        BuscarPruebaAparatoElectronico("inicial");

        Idioma();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Pause.paused)
        {
            if (Input.GetKeyDown("l") && !open && Time.timeScale == 1)
            {
                AbrirLibreta();
            }
            else if (Input.GetKeyDown("l") && Time.timeScale == 1)
            {
                CerrarLibreta();
            }

            if (open)
            {
                Personaje.ActionText("libreta", 0);
                if (Input.GetKeyDown("h"))
                {
                    PasarPagina();
                }
            }
        }
    }

    public void BuscarPrueba(int parte, int giro, string objectName)
    {
        string[] lines = fileContents.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] sections = lines[i].Split(";"[0]);
            if (sections[0] == objectName && sections[3] == parte.ToString() && sections[4] == giro.ToString())
            {
                string pista = sections[1];
                string tipoPista = sections[2];
                if (tipoPista == "personal")
                {
                    for (int j = 0; j < pruebasPersonales.Length; j++)
                    {
                        if (pruebasPersonales[j] == i) //Comprobamos si el id de la pista (= línea del archivo) coincide con alguna pista ya añadida
                        {
                            break;
                        }
                        else if (pruebasPersonales[j] == 0) //0 es el valor inicial de todos los elementos, y coincide con la línea de descripción del archivo
                        {
                            pruebasPersonales[j] = i; //Ponemos el número de línea del archivo como id de la prueba
                            InsertarVisualmente(j, panelPersonal, pista);
                            break;
                        }
                    }
                }
                else if (tipoPista == "profesional")
                {
                    for (int j = 0; j < pruebasProfesionales.Length; j++)
                    {
                        if (pruebasProfesionales[j] == i)
                        {
                            break;
                        }
                        else if (pruebasProfesionales[j] == 0)
                        {
                            pruebasProfesionales[j] = i;
                            InsertarVisualmente(j, panelProfesional, pista);
                            break;
                        }
                    }
                }
                else if(tipoPista == "otro")
                {
                    for (int j = 0; j < pruebasOtros.Length; j++)
                    {
                        if (pruebasOtros[j] == i)
                        {
                            break;
                        }
                        else if (pruebasOtros[j] == 0)
                        {
                            pruebasOtros[j] = i;
                            InsertarVisualmente(j, panelOtros, pista);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void BuscarPruebaAparatoElectronico(string name)
    {
        string[] lines = fileContents.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] sections = lines[i].Split(";"[0]);
            if (sections[0] == name)
            {
                string pista = sections[1];
                string tipoPista = sections[2];
                if (tipoPista == "personal")
                {
                    for (int j = 0; j < pruebasPersonales.Length; j++)
                    {
                        if (pruebasPersonales[j] == i) //Comprobamos si el id de la pista (= línea del archivo) coincide con alguna pista ya añadida
                        {
                            break;
                        }
                        else if (pruebasPersonales[j] == 0) //0 es el valor inicial de todos los elementos, y coincide con la línea de descripción del archivo
                        {
                            pruebasPersonales[j] = i; //Ponemos el número de línea del archivo como id de la prueba
                            InsertarVisualmente(j, panelPersonal, pista);
                            break;
                        }
                    }
                }
                else if (tipoPista == "profesional")
                {
                    for (int j = 0; j < pruebasProfesionales.Length; j++)
                    {
                        if (pruebasProfesionales[j] == i)
                        {
                            break;
                        }
                        else if (pruebasProfesionales[j] == 0)
                        {
                            pruebasProfesionales[j] = i;
                            InsertarVisualmente(j, panelProfesional, pista);
                            break;
                        }
                    }
                }
                else if (tipoPista == "otro")
                {
                    for (int j = 0; j < pruebasOtros.Length; j++)
                    {
                        if (pruebasOtros[j] == i)
                        {
                            break;
                        }
                        else if (pruebasOtros[j] == 0)
                        {
                            pruebasOtros[j] = i;
                            InsertarVisualmente(j, panelOtros, pista);
                            break;
                        }
                    }
                }
            }
        }
    }

    private void InsertarVisualmente(int i, GameObject panel, string prueba)
    {
        Text[] textobjs = panel.GetComponentsInChildren<Text>();
        textobjs[i+1].text = prueba;
    }

    private void AbrirLibreta()
    {
        libreta.SetActive(true);
        panelPersonal.SetActive(true);
        open = true;
    }

    private void PasarPagina()
    {
        if(panelPersonal.active)
        {
            panelPersonal.SetActive(false);
            panelProfesional.SetActive(true);
        }
        else if(panelProfesional.active)
        {
            panelProfesional.SetActive(false);
            panelOtros.SetActive(true);
        }
        else if(panelOtros.active)
        {
            panelOtros.SetActive(false);
            panelPersonal.SetActive(true);
        }
    }

    private void CerrarLibreta()
    {
        libreta.SetActive(false);
        panelPersonal.SetActive(false);
        panelProfesional.SetActive(false);
        panelOtros.SetActive(false);
        open = false;
    }

    public void Idioma()
    {
        int idioma = PlayerPrefs.GetInt("Idioma");

        switch (idioma)
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

        Text tituloPers = panelPersonal.GetComponentInChildren<Text>(true);
        Text tituloProf = panelProfesional.GetComponentInChildren<Text>(true);
        Text tituloOtros = panelOtros.GetComponentInChildren<Text>(true);

        string liniaTitulos = fileContents.Split("\n"[0])[0]; //La primera línea del fichero corresponde con los títulos de cada página
        string[] titulos = liniaTitulos.Split(";"[0]);

        tituloPers.text = titulos[1];
        tituloProf.text = titulos[2];
        tituloOtros.text = titulos[3];

        TraducirPistas();
    }

    private void TraducirPistas()
    {
        string[] lines = fileContents.Split("\n"[0]);
        for (int j = 0; j < pruebasPersonales.Length; j++)
        {
            if (pruebasPersonales[j] != 0)
            {
                string[] sections = lines[pruebasPersonales[j]].Split(";"[0]);
                InsertarVisualmente(j, panelPersonal, sections[1]);
            }
        }
        for (int j = 0; j < pruebasProfesionales.Length; j++)
        {
            if (pruebasProfesionales[j] != 0)
            {
                string[] sections = lines[pruebasProfesionales[j]].Split(";"[0]);
                InsertarVisualmente(j, panelProfesional, sections[1]);
            }
        }
        for (int j = 0; j < pruebasOtros.Length; j++)
        {
            if (pruebasOtros[j] != 0)
            {
                string[] sections = lines[pruebasOtros[j]].Split(";"[0]);
                InsertarVisualmente(j, panelOtros, sections[1]);
            }
        }
    }

    public int[] getPistas(string tipo)
    {
        if (tipo == "personales")
            return pruebasPersonales;
        else if (tipo == "profesionales")
            return pruebasProfesionales;
        else if (tipo == "otros")
            return pruebasOtros;
        else
            return null;
    }

    public int getTotalPistas()
    {
        string[] lines = fileContents.Split("\n"[0]);
        return lines.Length - 2;
    }
}
