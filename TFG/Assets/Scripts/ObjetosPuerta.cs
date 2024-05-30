using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetosPuerta : MonoBehaviour
{
    public int estado;
    public Inventario inventory;
    public bool thisDoor = false;
    private AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!Pause.paused)
        {
            if(thisDoor)
                Personaje.ActionText("puerta",estado);

            if (Input.GetKeyDown("e") && thisDoor)
            {
                if (estado == 0)
                    Open();
                else if (estado == 1)
                    Close();
            }

            if (estado == 2 && inventory.hasObject("Key"))
            {
                estado = 0;
            }

            thisDoor = false;
        }
    }

    void Open()
    {
        transform.Rotate(0, 90, 0);
        estado = 1;
        if(sound != null)
        {
            sound.clip = Resources.Load<AudioClip>("Sound/Effects/effect Door_Open");
            sound.Play();
        }
    }

    void Close()
    {
        transform.Rotate(0, -90, 0);
        estado = 0;
        if(sound != null)
        {
            sound.clip = Resources.Load<AudioClip>("Sound/Effects/effect Door_Close");
            sound.Play();
        }
    }
}
