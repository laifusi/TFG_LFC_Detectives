using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baffle : MonoBehaviour
{
    private AudioSource baffle;
    public bool thisbaffle = false;
    private int audioActual = 0;
    private Baffle[] allbaffles;
    private bool playing = false;
    private int tracks = 4;
    // Start is called before the first frame update
    void Start()
    {
        baffle = GetComponent<AudioSource>();
        allbaffles = FindObjectsOfType<Baffle>();
        string path = "Sound/Music/baffle 0";
        AudioClip musica = Resources.Load<AudioClip>(path);
        baffle.clip = musica;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Pause.paused)
        {
            if(thisbaffle)
            {
                Personaje.ActionText("baffle", System.Convert.ToInt32(baffle.isPlaying));

                if (Input.GetKeyDown("e") && !baffle.isPlaying)
                {
                    float t = 0;
                    for (int i = 0; i < allbaffles.Length; i++)
                    {
                        if (allbaffles[i].GetComponent<AudioSource>().isPlaying)
                        {
                            t = allbaffles[i].GetComponent<AudioSource>().time;
                        }
                    }
                    baffle.time = t;
                    baffle.Play();
                    playing = true;
                }
                else if (Input.GetKeyDown("e") && baffle.isPlaying)
                {
                    baffle.Pause();
                    playing = false;
                }

                if (Input.GetKeyDown("n") && baffle.isPlaying)
                {
                    CambiarCancion();
                }

                if (Input.GetKeyDown("v") && baffle.isPlaying)
                {
                    baffle.GetComponent<AudioSource>().volume += 0.1f;
                }

                if (Input.GetKeyDown("c") && baffle.isPlaying)
                {
                    baffle.GetComponent<AudioSource>().volume -= 0.1f;
                }
            }

            if(playing && !baffle.isPlaying)
            {
                CambiarCancion();
            }

            thisbaffle = false;
        }
    }

    void CambiarCancion()
    {
        if (int.Parse(baffle.clip.name.Split(" "[0])[1]) < tracks - 1)
        {
            audioActual = int.Parse(baffle.clip.name.Split(" "[0])[1]) + 1;
        }
        else
        {
            audioActual = 0;
        }

        string path = "Sound/Music/baffle " + audioActual.ToString();
        AudioClip musica = Resources.Load<AudioClip>(path);
        for (int i = 0; i < allbaffles.Length; i++)
        {
            bool on = false;
            if (allbaffles[i].GetComponent<AudioSource>().isPlaying || allbaffles[i].playing)
                on = true;

            allbaffles[i].GetComponent<AudioSource>().clip = musica;

            if (on)
            {
                allbaffles[i].GetComponent<AudioSource>().time = 0;
                allbaffles[i].GetComponent<AudioSource>().Play();
            }
        }
    }
}
