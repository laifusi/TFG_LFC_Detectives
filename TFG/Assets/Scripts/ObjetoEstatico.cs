using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoEstatico : ObjetosObservables
{
    public Vector3 finalCameraPos;
    public Vector3 finalCameraAngle;
    public bool cajon;
    private bool open = false;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!Pause.paused)
        {
            if (cajon && thisObject)
            {
                Personaje.ActionText("puerta", System.Convert.ToInt32(open));

                if (Input.GetKeyDown("e") && !open)
                {
                    AbrirCajon();
                }
                else if (Input.GetKeyDown("e") && open)
                {
                    CerrarCajon();
                }
            }
            else if(thisObject)
            {
                Personaje.ActionText("observable", System.Convert.ToInt32(!observando));

                if (Input.GetKeyDown("q") && !observando)
                {
                    initialCameraAngle = cam.transform.eulerAngles;
                    cam.transform.position = finalCameraPos;
                    cam.transform.eulerAngles = finalCameraAngle;
                    Time.timeScale = 0;
                    observando = true;
                    Personaje.puntodedeteccion.enabled = false;
                }
            }

            base.Update();

            if (!observando)
                thisObject = false;
            else if (observando && pista && thisObject)
                libretaPruebas.BuscarPrueba(0, 0, transform.name);
        }
    }

    /*Esta función debe ser protected para que el script Puzzle puede abrir el cajón del Puzzle 3*/
    protected void AbrirCajon()
    {
        transform.Translate(0, 0, 1);
        open = true;
    }

    void CerrarCajon()
    {
        transform.Translate(0, 0, -1);
        open = false;
    }
}
