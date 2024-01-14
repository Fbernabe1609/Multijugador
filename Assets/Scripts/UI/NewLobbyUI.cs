using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewLobbuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private Button publicLobbyButton;
    [SerializeField] private Button privateLobbyButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject newLobbyUI;

    // Start is called before the first frame update
    void Start()
    {
        publicLobbyButton.interactable = false;
        privateLobbyButton.interactable = false;

        lobbyNameInputField.onValueChanged.AddListener(delegate
        {
            InputValueCheck();
        });

        publicLobbyButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.NewLobby(lobbyNameInputField.text, false);
        });

        privateLobbyButton.onClick.AddListener(() => 
        {
            LobbyManager.Instance.NewLobby(lobbyNameInputField.text, true);
        });

        closeButton.onClick.AddListener(() =>
        {
            newLobbyUI.SetActive(false);
        });
    }

    public void InputValueCheck()
    {
        string lobbyName = lobbyNameInputField.text;
        bool isLobbyNameValid = !string.IsNullOrEmpty(lobbyName);

        publicLobbyButton.interactable = isLobbyNameValid;
        privateLobbyButton.interactable = isLobbyNameValid;
    }
 }
