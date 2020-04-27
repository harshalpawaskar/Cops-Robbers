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
    public GameObject resultPanel;
    public GameObject invalidMove;
    public GameObject wrongPiece;
    public Text wrongPieceText;
    public GameObject[] platforms;
    public GameObject[] Robbers;
    public GameObject[] Cops;
    private bool turn = false;
    private string emptyPlatform;
    private bool start = true;
    private RaycastHit hit1;
    private bool selected = false;
    private string selectedPlatform;
    private bool gameEnded;
    private int i = 0;

    void Start()
    {
        gameEnded = false;
        turnText.text = "Robber's Turn";
    }

    void Update()
    {
        if (!turn)
        {
            turnText.text = "Robber's Turn";
            if(!CheckAnyValidMove())
            {
                gameEnded = true;
                Time.timeScale = 0f;
                resultPanel.SetActive(true);
                resultText.text = "COPS WON";
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
                                hit1 = hit;
                            }
                        }
                    }
                    else if (selection.CompareTag("Cop") && selected == false)
                    {
                        wrongPiece.SetActive(true);
                        wrongPieceText.text = "ROBBER'S TURN";
                        StartCoroutine(Coroutine2());
                    }
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
                                    selected = false;
                                    start = true;
                                    turn = true;
                                    i = 0;
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
            turnText.text = "Cop's Turn";
            if (!CheckAnyValidMove())
            {
                gameEnded = true;
                Time.timeScale = 0f;
                resultPanel.SetActive(true);
                resultText.text = "ROBBERS WON"; 
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
                                hit1 = hit;
                            }
                        }
                    }
                    else if (selection.CompareTag("Robber") && selected == false)
                    {
                        wrongPiece.SetActive(true);
                        wrongPieceText.text = "COP'S TURN";
                        StartCoroutine(Coroutine2());
                    }
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
                                    selected = false;
                                    start = true;
                                    turn = false;
                                    i = 0;
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
                                    selected = false;
                                    start = true;
                                }
                            }
                        }
                    }
                }
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

    [Obsolete]
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        turn = false;
        Application.LoadLevel(0);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}