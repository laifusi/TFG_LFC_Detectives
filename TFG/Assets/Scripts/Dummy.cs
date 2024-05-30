using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : ObjetoEstatico
{
    private int dummyActual;
    private GameObject[] dummys = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        dummyActual = 0;
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        for(int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "dummy")
            {
                dummys[dummyActual] = children[i].gameObject;
                if (dummyActual != 0)
                    dummys[dummyActual].SetActive(false);
                dummyActual++;
            }
        }
        dummyActual = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused)
        {
            base.Update();
            if(observando && thisObject)
            {
                Personaje.ActionText("dummy",0);

                if (Input.GetKeyDown("e"))
                {
                    dummys[dummyActual].gameObject.SetActive(false);
                    if (dummyActual + 1 < dummys.Length)
                        dummyActual++;
                    else
                        dummyActual = 0;
                    dummys[dummyActual].gameObject.SetActive(true);
                }
            }
        }
    }
}
