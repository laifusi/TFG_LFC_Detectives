using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosObservables : MonoBehaviour
{
    protected Vector3 initialCameraPos;
    protected Vector3 initialCameraAngle;
    protected Vector3 initialObjAngle;
    protected Vector3 initialObjPos;
    public bool pista;
    public bool thisObject = false;
    protected bool observando = false;
    protected Camera cam;
    protected LibretaPruebas libretaPruebas;

    protected void Start()
    {
        cam = Camera.main;
        initialCameraPos = cam.transform.localPosition;
        initialObjPos = transform.localPosition;
        initialObjAngle = transform.eulerAngles;
        libretaPruebas = FindObjectOfType<LibretaPruebas>();
    }

    protected void Update()
    {
        if (observando && Input.GetKeyDown("x") && thisObject && !Pause.paused)
            Back();
    }

    public void Back()
    {
        Personaje.puntodedeteccion.enabled = true;

        transform.localPosition = initialObjPos;
        transform.eulerAngles = initialObjAngle;
        if (Time.timeScale == 0)
        {
            cam.transform.localPosition = initialCameraPos;
            cam.transform.eulerAngles = initialCameraAngle;
        }

        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.04f);
        
        Time.timeScale = 1;
        observando = false;
        thisObject = false;
    }
}
