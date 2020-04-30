using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Text turnText;
    public GameObject guidePanel1;
    public GameObject guidePanel2;
    public GameObject guidePanel3;
    public GameObject panel2;
    public GameObject resultPanel;
    public GameObject skipPanel;
    public Material selectedRobber;
    public Material defaultRobber;
    public Material selectedCop;
    public Material defaultCop;
    private int i;
    private bool selected;
    private RaycastHit hit1;
    private bool panel2Activated = false;

    void Start()
    {
        i = 0;
        selected = false;
    }

    void Update()
    {
        if (panel2Activated)
        {
            turnText.text = "Robber's Turn";
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
                        selectionRenderer.material = defaultRobber;
                        selected = false;
                        turnText.text = "Cop's Turn";
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
                        selected = false;
                        guidePanel3.SetActive(false);
                        panel2.SetActive(false);
                        resultPanel.SetActive(true);
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
        turnText.text = "Robber's Turn";
        guidePanel3.SetActive(true);
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
