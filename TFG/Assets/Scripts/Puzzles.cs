using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzles : ObjetoEstatico
{
    public int id;
    public bool resuelto = false;

    private Color red = new Color(0.9607844f, 0, 0);
    private Color blue = new Color(0, 0.3359981f, 1);
    private Color green = new Color(0.4705883f, 0.8235295f, 0.4588236f);
    private Color yellow = new Color(0.9837487f, 1, 0);
    private Color white = new Color(1, 1, 1);

    private Color[] colores = new Color[6];
    private Color lastColor;

    private Transform selectedCD = null;
    private string[] cds = new string[12];

    private Transform[] rueda;
    private float[] rot = new float[3];

    private string path = "LanguageFiles/puzzles";
    public GameObject infoPanel;

    public Animator solvedAnimation;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        colores[0] = yellow;
        colores[1] = red;
        colores[2] = blue;
        colores[3] = white;
        colores[4] = green;
        colores[5] = blue;

        cds[0] = "cdpuzzle3";
        cds[1] = "cdpuzzle8";
        cds[2] = "cdpuzzle6";
        cds[3] = "cdpuzzle10";
        cds[4] = "cdpuzzle7";
        cds[5] = "cdpuzzle2";
        cds[6] = "cdpuzzle5";
        cds[7] = "cdpuzzle4";
        cds[8] = "cdpuzzle12";
        cds[9] = "cdpuzzle11";
        cds[10] = "cdpuzzle1";
        cds[11] = "cdpuzzle9";

        if (id == 3)
        {
            rueda = GetComponentsInChildren<RectTransform>();
        }

        rot[0] = -60;
        rot[1] = -210;
        rot[2] = -150;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if(id == 2 && !resuelto)
        {
            if (Input.GetMouseButtonDown(0) && !Pause.paused)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objT = hit.transform;
                    if (objT.CompareTag("Puzzle2"))
                    {
                        StartCoroutine(Sound(objT));
                    }
                }
            }
        }

        if(!Pause.paused && Input.GetKeyDown("q") && thisObject && !resuelto)
        {
            infoPanel.SetActive(true);
        }
    }
    
    IEnumerator Sound(Transform objT)
    {
        objT.GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(2);
        Puzzle2(objT);
    }

    private void Puzzle2(Transform objT)
    {
        if (selectedCD == null)
        {
            selectedCD = objT;
        }
        else if (selectedCD == objT)
        {
            selectedCD = null;
        }
        else
        {
            Material aux = objT.GetComponent<Renderer>().material;
            Material aux2 = selectedCD.GetComponent<Renderer>().material;
            objT.GetComponent<Renderer>().material = aux2;
            selectedCD.GetComponent<Renderer>().material = aux;
            AudioClip aux3 = objT.GetComponent<AudioSource>().clip;
            AudioClip aux4 = selectedCD.GetComponent<AudioSource>().clip;
            objT.GetComponent<AudioSource>().clip = aux4;
            selectedCD.GetComponent<AudioSource>().clip = aux3;
            selectedCD = null;
            int n = 0, m = 0;
            for (int i = 0; i < 12; i++)
            {
                string[] x = aux.name.Split(' ');
                string[] x2 = aux2.name.Split(' ');
                if (x[0] == cds[i])
                    n = i;
                else if (x2[0] == cds[i])
                    m = i;
            }
            cds[n] = aux2.name.Split(' ')[0];
            cds[m] = aux.name.Split(' ')[0];

            if (cds[0] == "cdpuzzle1"
                && cds[1] == "cdpuzzle2"
                && cds[2] == "cdpuzzle3"
                && cds[3] == "cdpuzzle4"
                && cds[4] == "cdpuzzle5"
                && cds[5] == "cdpuzzle6"
                && cds[6] == "cdpuzzle7"
                && cds[7] == "cdpuzzle8"
                && cds[8] == "cdpuzzle9"
                && cds[9] == "cdpuzzle10"
                && cds[10] == "cdpuzzle11"
                && cds[11] == "cdpuzzle12")
            {
                StartCoroutine(SolvedAnimation2());
            }
        }
    }

    public void ChangeColor(Button b)
    {
        if(b.GetComponent<Image>().color == blue)
            b.GetComponent<Image>().color = green;
        else if (b.GetComponent<Image>().color == green)
            b.GetComponent<Image>().color = red;
        else if(b.GetComponent<Image>().color == red)
            b.GetComponent<Image>().color = yellow;
        else if (b.GetComponent<Image>().color == yellow)
            b.GetComponent<Image>().color = white;
        else if(b.GetComponent<Image>().color == white)
            b.GetComponent<Image>().color = blue;

        lastColor = b.GetComponent<Image>().color;
    }

    public void ComprobarPuzzle1(int n)
    {
        colores[n] = lastColor;

        if ((colores[0] == red && colores[1] == blue || colores[0] == blue && colores[1] == red)
            && (colores[2] == green && colores[3] == yellow || colores[2] == yellow && colores[3] == green)
            && (colores[4] == red && colores[5] == white || colores[4] == white && colores[5] == red))
        {
            StartCoroutine(SolvedAnimation1());
        }
        
    }

    public void RotarCirculo(Transform circulo)
    {
        circulo.Rotate(0,0,-30);
        if(circulo.name == "rueda pequena")
        {
            rot[2] -= 30;
            if(rot[2] <= -360)
            {
                rot[2] += 360;
                circulo.Rotate(0, 0, 360);
            }
        }
        else if(circulo.name == "rueda mediana pequena")
        {
            rot[1] -= 30;
            if (rot[1] <= -360)
            {
                rot[1] += 360;
                circulo.Rotate(0, 0, 360);
            }
        }
        else if (circulo.name == "rueda grande mediana")
        {
            rot[0] -= 30;
            if (rot[0] <= -360)
            {
                rot[0] += 360;
                circulo.Rotate(0, 0, 360);
            }
        }

        if (rot[0] == 0 && rot[1] == 0 && rot[2] == 0)
        {
            StartCoroutine(SolvedAnimation3());
        }
    }

    public void Idioma()
    {
        int idioma = PlayerPrefs.GetInt("Idioma");
        string fileContents = "";

        switch (idioma)
        {
            case 0:
                fileContents = Resources.Load<TextAsset>(path+"ESP").text;
                break;
            case 1:
                fileContents = Resources.Load<TextAsset>(path + "CAT").text;
                break;
            case 2:
                fileContents = Resources.Load<TextAsset>(path + "ENG").text;
                break;
        }

        Text[] infoTexts = infoPanel.GetComponentsInChildren<Text>();

        string[] lines = fileContents.Split("\n"[0]);
        for (int i = 0; i < lines.Length; i++)
        {
            string[] sections = lines[i].Split(";"[0]);
            if (sections[0] == id.ToString())
            {
                for (int j = 1; j < sections.Length; j++)
                {
                    infoTexts[j - 1].text = sections[j];
                }
            }
        }

        solvedAnimation.GetComponentInChildren<Text>().text = lines[lines.Length - 1].Split(";"[0])[0];
    }

    IEnumerator SolvedAnimation1()
    {
        Back();
        solvedAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        resuelto = true;
        transform.localPosition = new Vector3(0, 0.0826841f, -0.08445093f);
        transform.localEulerAngles = new Vector3(50.7f, 0, 0);
        initialObjPos = transform.localPosition;
        initialObjAngle = transform.localEulerAngles;
    }

    IEnumerator SolvedAnimation2()
    {
        Back();
        solvedAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        resuelto = true;
    }

    IEnumerator SolvedAnimation3()
    {
        Back();
        solvedAnimation.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        resuelto = true;
        cajon = true;
        AbrirCajon();
    }
}
