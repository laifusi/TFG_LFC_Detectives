using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoMovil : ObjetosObservables
{
    public Vector3 finalObjAngle;
    public float tam;
    public bool coger;
    public bool gira;
    public int n_partes;
    public string matName;
    public GameObject matObj;
    private int pActual = 0;
    private int nGiro = 0;
    private Transform[] paginas;
    public Inventario inventario;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        if(!Pause.paused)
        {
            if(thisObject)
            {
                if(!observando && Time.timeScale == 1)
                    initialCameraAngle = cam.transform.eulerAngles;

                Personaje.ActionText("observable", System.Convert.ToInt32(!observando));

                if (Input.GetKeyDown("q"))
                {
                    cam.transform.position = new Vector3(100, 100, 0);
                    cam.transform.eulerAngles = new Vector3(0,0,0);
                    Vector3 finalCamPos = cam.transform.position;
                    Vector3 newPos = finalCamPos + cam.transform.forward * tam;
                    transform.position = newPos;
                    transform.eulerAngles = cam.transform.eulerAngles;
                    transform.Rotate(finalObjAngle);
                    observando = true;
                    Time.timeScale = 0;
                    RenderSettings.ambientLight = new Color(0.5f,0.5f,0.5f);
                    Personaje.puntodedeteccion.enabled = false;
                }

                if (!observando && coger)
                {
                    Personaje.ActionText("observable", 2);

                    if (Input.GetKeyDown("c"))
                    {
                        inventario.Coger(transform.gameObject);
                    }
                }

                if (observando && transform.name == "libreta" && pActual == 4)
                {
                    if (inventario.hasObject("lapiz"))
                    {
                        Personaje.ActionText("observable", 3);

                        if (Input.GetKeyDown("e"))
                        {
                            //Usamos la variable de giro para diferenciar pintado y sin pintar, en el fichero de pistas la posición de giro es 1.
                            nGiro++;
                            Transform[] c = GetComponentsInChildren<Transform>();
                            for (int i = 0; i < c.Length; i++)
                            {
                                if (c[i].name == "hoja arrancada")
                                    c[i].GetComponent<Renderer>().material = Resources.Load<Material>("Materials/hojaArrancada");
                            }
                        }
                    }
                    else
                    {
                        Personaje.ActionText("observable", 4);
                    }
                }
                
                if(observando && n_partes > 1)
                {
                    Personaje.ActionText("observable", 5);

                    if (Input.GetKeyDown("n"))
                    {
                        if (matName == "no mat")
                        {
                            paginas = GetComponentsInChildren<Transform>();
                            PasarPagina();
                        }
                        else
                            PasarMaterial();
                    }
                }

                if (observando && gira)
                {
                    Personaje.ActionText("observable", 6);

                    if (Input.GetKeyDown("t"))
                    {
                        Girar();
                    }
                }

                if(observando && transform.name == "amenaza")
                {
                    Transform[] chi = GetComponentsInChildren<Transform>(true);
                    for (int i = 0; i < chi.Length; i++)
                    {
                        if (chi[i].name == "amenaza arrugada" && chi[i].gameObject.activeSelf)
                        {
                            Personaje.ActionText("puerta", 0); //Buscamos la opción "Abrir"
                            break;
                        }
                    }

                    if (Input.GetKeyDown("e"))
                    {
                        for (int i = 0; i < chi.Length; i++)
                        {
                            if (chi[i].name == "amenaza arrugada")
                            {
                                chi[i].gameObject.SetActive(false);
                            }
                            if (chi[i].name == "amenaza abierta")
                            {
                                chi[i].gameObject.SetActive(true);
                                pActual++;
                            }
                        }
                    }

                    if(Input.GetKeyDown("x"))
                    {
                        for (int i = 0; i < chi.Length; i++)
                        {
                            if (chi[i].name == "amenaza arrugada")
                            {
                                chi[i].gameObject.SetActive(true);
                                pActual--;
                            }
                            if (chi[i].name == "amenaza abierta")
                            {
                                chi[i].gameObject.SetActive(false);
                            }
                        }
                    }
                }

                if(observando && transform.name == "papel mesa")
                {
                    Personaje.ActionText("observable", 7);
                }
            }

            base.Update();

            if (!observando)
                thisObject = false;
            else if (observando && pista)
                libretaPruebas.BuscarPrueba(pActual, nGiro, transform.name);
        }
    }

    void Girar()
    {
        transform.Rotate(0, 180, 0);

        if (nGiro == 0)
            nGiro = 1;
        else
            nGiro = 0;
    }

    void PasarMaterial()
    {
        string actualMat = matName + pActual.ToString();
        Material[] mats = matObj.GetComponent<Renderer>().materials;
        if (n_partes - 1 > pActual)
        {
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name == actualMat + " (Instance)")
                {
                    pActual++;
                    string path = "Materials/" + matName + pActual.ToString();
                    Material m = Resources.Load<Material>(path);
                    mats[i] = m;
                }
            }
        }
        else
        {
            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i].name == actualMat + " (Instance)")
                {
                    pActual = 0;
                    string path = "Materials/" + matName + pActual.ToString();
                    Material m = Resources.Load<Material>(path);
                    mats[i] = m;
                }
            }
        }
        matObj.GetComponent<Renderer>().materials = mats;
    }

    void PasarPagina()
    {
        if (n_partes - 1 > pActual)
        {
            for(int i = 0; i < paginas.Length; i++)
            {
                string[] splitname = paginas[i].name.Split(' ');
                if (splitname.Length > 1)
                    if (splitname[1] == pActual.ToString())
                        paginas[i].Rotate(0, 0, 120);
            }
            pActual++;
        }
        else
        {
            while (pActual >= 0)
            {
                for (int i = 0; i < paginas.Length; i++)
                {
                    string[] splitname = paginas[i].name.Split(' ');
                    if (splitname.Length > 1)
                        if (splitname[1] == pActual.ToString())
                            paginas[i].Rotate(0, 0, -120);
                }
                pActual--;
            }
            pActual = 0;
        }
    }
}
