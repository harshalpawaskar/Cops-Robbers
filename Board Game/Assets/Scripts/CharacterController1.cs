using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterController1 : MonoBehaviour
{
    public Material selectedRobber;
    public Material defaultRobber;
    public Material selectedCop;
    public Material defaultCop;
    public Text turnText;
    public Text resultText;
    public Text wrongPieceText;
    public GameObject resultPanel;
    public GameObject invalidMove;
    public GameObject wrongPiece;
    public GameObject turnPanel;
    public GameObject pausePanel;
    public GameObject[] playerTextPanel;
    public GameObject[] platforms;
    public GameObject[] Robbers;
    public GameObject[] Cops;
    public Text[] playerTexts;
    private bool turn = false;
    private bool selected = false;
    private bool start = true;
    private bool gameEnded;
    private string emptyPlatform;
    private RaycastHit hit1;
    private string selectedPlatform;
    //Shader shader1;
    //Shader shader2;
    public Color color;

    void Start()
    {
        gameEnded = false;
        turnText.text = PlayerPrefs.GetString("Player1Name") + "'s Turn";
        turnText.color = Color.red;
        //shader1 = Shader.Find("Standard");
        //shader2 = Shader.Find("PlayerShader");
        playerTexts[0].text = PlayerPrefs.GetString("Player1Name");
        playerTexts[1].text = PlayerPrefs.GetString("Player1Name");
        playerTexts[2].text = PlayerPrefs.GetString("Player2Name");
        playerTexts[3].text = PlayerPrefs.GetString("Player2Name");
    }

    void Update()
    {
        playerTexts[0].transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere1").transform.position);
        playerTexts[1].transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere2").transform.position);
        playerTexts[2].transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere3").transform.position);
        playerTexts[3].transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Sphere4").transform.position);

        #region Movement
        if (!turn)
        {
            turnText.text = PlayerPrefs.GetString("Player1Name") + "'s Turn";
            turnText.color = Color.red;
            if(!CheckAnyValidMove())
            {
                gameEnded = true;
                //Time.timeScale = 0f;
                turnPanel.SetActive(false);
                resultPanel.SetActive(true);
                resultText.text = PlayerPrefs.GetString("Player2Name") + " Won";
            }
            if (Input.touchCount > 0 && !gameEnded)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;
                    if (selection.CompareTag("Robber"))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = selectedRobber;
                            //selectionRenderer.material.shader = shader2;
                            selected = true;
                        }
                        if (start)
                        {
                            hit1 = hit;
                            start = false;
                        }
                        if (!start)
                        {
                            if (hit.collider.name != hit1.collider.name)
                            {
                                selection = hit1.transform;
                                selectionRenderer = selection.GetComponent<Renderer>();
                                selectionRenderer.material = defaultRobber;
                                //selectionRenderer.material.shader = shader1;
                                hit1 = hit;
                            }
                        }
                    }
                    /*else if (selection.CompareTag("Cop") && selected == false)
                    {
                        Handheld.Vibrate();
                        //wrongPiece.SetActive(true);
                        //wrongPieceText.text = "ROBBER'S TURN";
                        //StartCoroutine(Coroutine2());
                    }*/
                    else
                    {
                        if (selected)
                        {
                            RaycastHit platHit;
                            Physics.Raycast(hit1.transform.position, Vector3.down, out platHit);
                            selectedPlatform = platHit.collider.name;
                            emptyPlatform = CheckEmpty();
                            if (hit.collider.CompareTag("Platform") && hit.collider.name == emptyPlatform)
                            {
                                if (CheckValidMove())
                                {
                                    Vector3 pos = hit.transform.position;
                                    pos.y = 0.3f;
                                    hit1.transform.position = pos;
                                    selection = hit1.transform;
                                    var selectionRenderer = selection.GetComponent<Renderer>();
                                    selectionRenderer.material = defaultRobber;
                                    //selectionRenderer.material.shader = shader1;
                                    selected = false;
                                    start = true;
                                    turn = true;
                                }
                                else
                                {
                                    Debug.Log("Cannot Move");
                                    invalidMove.SetActive(true);
                                    Handheld.Vibrate();
                                    StartCoroutine(Coroutine1());
                                    selection = hit1.transform;
                                    var selectionRenderer = selection.GetComponent<Renderer>();
                                    selectionRenderer.material = defaultRobber;
                                    //selectionRenderer.material.shader = shader1;
                                    selected = false;
                                    start = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            turnText.text = PlayerPrefs.GetString("Player2Name") + "'s Turn";
            turnText.color = color;
            if (!CheckAnyValidMove())
            {
                gameEnded = true;
                turnPanel.SetActive(false);
                //Time.timeScale = 0f;
                resultPanel.SetActive(true);
                resultText.text = PlayerPrefs.GetString("Player1Name") + " Won"; 
            }
            if (Input.touchCount > 0 && !gameEnded)
            {
                var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var selection = hit.transform;
                    if (selection.CompareTag("Cop"))
                    {
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material = selectedCop;
                            //selectionRenderer.material.shader = shader2;
                            selected = true;
                        }
                        if (start)
                        {
                            hit1 = hit;
                            start = false;
                        }
                        if (!start)
                        {
                            if (hit.collider.name != hit1.collider.name)
                            {
                                selection = hit1.transform;
                                selectionRenderer = selection.GetComponent<Renderer>();
                                selectionRenderer.material = defaultCop;
                                //selectionRenderer.material.shader = shader1;
                                hit1 = hit;
                            }
                        }
                    }
                    /*else if (selection.CompareTag("Robber") && selected == false)
                    {
                        Handheld.Vibrate();
                        //wrongPiece.SetActive(true);
                        //wrongPieceText.text = "COP'S TURN";
                        //StartCoroutine(Coroutine2());
                    }*/
                    else
                    {
                        if (selected)
                        {
                            RaycastHit platHit;
                            Physics.Raycast(hit1.transform.position, Vector3.down, out platHit);
                            selectedPlatform = platHit.collider.name;
                            emptyPlatform = CheckEmpty();
                            if (hit.collider.CompareTag("Platform") && hit.collider.name == emptyPlatform)
                            {
                                if (CheckValidMove())
                                {
                                    Vector3 pos = hit.transform.position;
                                    pos.y = 0.3f;
                                    hit1.transform.position = pos;
                                    selection = hit1.transform;
                                    var selectionRenderer = selection.GetComponent<Renderer>();
                                    selectionRenderer.material = defaultCop;
                                    //selectionRenderer.material.shader = shader1;
                                    selected = false;
                                    start = true;
                                    turn = false;
                                }
                                else
                                {
                                    Debug.Log("Cannot Move");
                                    invalidMove.SetActive(true);
                                    Handheld.Vibrate();
                                    StartCoroutine(Coroutine1());
                                    selection = hit1.transform;
                                    var selectionRenderer = selection.GetComponent<Renderer>();
                                    selectionRenderer.material = defaultCop;
                                    //selectionRenderer.material.shader = shader1;
                                    selected = false;
                                    start = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        if(!gameEnded)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameEnded = true;
                turnPanel.SetActive(false);
                pausePanel.SetActive(true);
            }
        }
    }

    private bool CheckAnyValidMove()
    {
        RaycastHit raycastHit1;
        RaycastHit raycastHit2;
        if(!turn)
        {
            Physics.Raycast(Robbers[0].transform.position, Vector3.down, out raycastHit1);
            emptyPlatform = CheckEmpty();
            selectedPlatform = raycastHit1.collider.name;
            if (CheckValidMove())
                return true;
            else
            {
                Physics.Raycast(Robbers[1].transform.position, Vector3.down, out raycastHit2);
                emptyPlatform = CheckEmpty();
                selectedPlatform = raycastHit2.collider.name;
                if (CheckValidMove())
                    return true;
            }
            return false;
        }
        else
        {
            Physics.Raycast(Cops[0].transform.position, Vector3.down, out raycastHit1);
            emptyPlatform = CheckEmpty();
            selectedPlatform = raycastHit1.collider.name;
            if (CheckValidMove())
                return true;
            else
            {
                Physics.Raycast(Cops[1].transform.position, Vector3.down, out raycastHit2);
                emptyPlatform = CheckEmpty();
                selectedPlatform = raycastHit2.collider.name;
                if (CheckValidMove())
                    return true;
            }
            return false;
        }
    }

    private bool CheckValidMove()
    {
        if (selectedPlatform == "0")
            if (emptyPlatform == "1" || emptyPlatform == "4" || emptyPlatform == "2")
                return true;
        if (selectedPlatform == "1")
            if (emptyPlatform == "0" || emptyPlatform == "4" || emptyPlatform == "3")
                return true;
        if (selectedPlatform == "2")
            if (emptyPlatform == "0" || emptyPlatform == "4") 
                return true;
        if (selectedPlatform == "3")
            if (emptyPlatform == "4" || emptyPlatform == "1")
                return true;
        if (selectedPlatform == "4")
            if (emptyPlatform != "4")
                return true;
        return false;
    }

    private string CheckEmpty()
    {
        for(int i=0;i<=4;i++)
        {
            var pos = platforms[i].transform;
            RaycastHit platHit;
            Physics.Raycast(pos.transform.position,Vector3.up, out platHit);
            if (platHit.collider != null)
                continue;
            else
                return platforms[i].name;
        }
        return null;
    }

    IEnumerator Coroutine1()
    {
        yield return new WaitForSeconds(1f);
        invalidMove.SetActive(false);
    }

    IEnumerator Coroutine2()
    {
        yield return new WaitForSeconds(1f);
        wrongPiece.SetActive(false);
    }

    public void PlayAgain()
    {
        //Time.timeScale = 1f;
        turn = false;
        SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void resetGameEnded()
    {
        gameEnded = false;
    }
}