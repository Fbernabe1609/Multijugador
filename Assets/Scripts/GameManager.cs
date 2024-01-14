using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject multiplayerMenu;
    [SerializeField] private GameObject newLobbyMenu;
    private bool isLocalPlayerReady;

    private void Awake()
    {
        Instance = this;
        isLocalPlayerReady = false;
    }

    void Start()
    {
        mainMenu.SetActive(true);
        multiplayerMenu.SetActive(false);
        newLobbyMenu.SetActive(false);
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public void SetPlayerReady()
    {
        isLocalPlayerReady=true;
    }
}
