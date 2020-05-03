using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Text turnText;
    public GameObject player1Text1;
    public GameObject player1Text2;
    public GameObject player2Text1;
    public GameObject player2Text2;
    public GameObject guidePanel1;
    public GameObject guidePanel2;
    public GameObject guidePanel3;
    public GameObject guidePanel6;
    public GameObject panel2;
    public GameObject resultPanel;
    public GameObject skipPanel;
    public GameObject nextButton1;
    public Material selectedRobber;
    public Material defaultRobber;
    public Material selectedCop;
    public Material defaultCop;
    private int i;
    private bool selected;
    private RaycastHit hit1;
    private bool panel2Activated = false;
    public Color color;
    //Shader shader1;
    //Shader shader2;

    void Start()
    {
        i = 2;
        selected = false;
        //shader1 = Shader.Find("Standard");
        //shader2 = Shader.Find("Outlined");
    }

    void Update()
    {
        player1Text1.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere1").transform.position);
        player1Text2.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere2").transform.position);
        player2Text1.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere3").transform.position);
        player2Text2.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere4").transform.position);
        if (panel2Activated)
        {
            if (Input.touchCount > 0 && i == 0)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.name == "Robber1" && !selected)
                    {
                        var selection = hit.transform;
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            //selectionRenderer.material.shader = shader2;
                            selectionRenderer.material = selectedRobber;
                            selected = true;
                            hit1 = hit;
                            guidePanel1.SetActive(false);
                            guidePanel2.SetActive(true);
                        }
                    }
                    if(hit.collider.name == "4" && selected)
                    {
                        guidePanel2.SetActive(false);
                        Vector3 pos = hit.transform.position;
                        pos.y = 0.3f;
                        hit1.transform.position = pos;
                        var selection = hit1.transform;
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        //selectionRenderer.material.shader = shader1;
                        selectionRenderer.material = defaultRobber;
                        selected = false;
                        turnText.text = "Player2's Turn";
                        turnText.color = color;
                        StartCoroutine(Coroutine1());
                        i = 1;
                    }
                }
            }
            if (Input.touchCount > 0 && i == 1)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.name == "Robber2" && !selected)
                    {
                        var selection = hit.transform;
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = selectedRobber;
                            //selectionRenderer.material.shader = shader2;
                            selected = true;
                            hit1 = hit;
                        }
                    }
                    if (hit.collider.name == "1" && selected)
                    {
                        Vector3 pos = hit.transform.position;
                        pos.y = 0.3f;
                        hit1.transform.position = pos;
                        var selection = hit1.transform;
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        selectionRenderer.material = defaultRobber;
                        //selectionRenderer.material.shader = shader1;
                        turnText.text = "Player2's Turn";
                        turnText.color = color;
                        selected = false;
                        guidePanel3.SetActive(false);
                        guidePanel6.SetActive(true);
                        nextButton1.SetActive(true);
                        i = 2;
                    }
                }
            }
        }
    }

    IEnumerator Coroutine1()
    {
        yield return new WaitForSeconds(1f);
        GameObject cop1 = GameObject.Find("Cop1");
        cop1.transform.position = new Vector3(-6.22f, 0.3f, 6.32f);
        turnText.text = "Player1's Turn";
        turnText.color = Color.red;
        guidePanel3.SetActive(true);
    }

    public void StartTutorial()
    {
        i = 0;
    }

    public void SkipTutorial()
    {
        skipPanel.SetActive(true);
    }

    public void Activate()
    {
        panel2Activated = true;
    }

    public void replayTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void TryGame()
    {
        SceneManager.LoadScene(1);
    }
}
