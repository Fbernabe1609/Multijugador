using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using Unity.Services.Lobbies;


public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    public const int MAX_PLAYERS = 2;
    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;
    public event EventHandler OnJoinStarted;
    public event EventHandler OnJoinFailed;
    public event EventHandler OnQuickJoinFailed;
    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

    public class OnLobbyListChangedEventArgs : EventArgs
    {
        public List<Lobby> LobbyList;
    }

    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float LobbyListTimer;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        LobbyListHandler();
    }

    private void LobbyListHandler()
    {
        if (joinedLobby == null && UnityServices.State == ServicesInitializationState.Initialized)
        {
            LobbyListTimer -= Time.deltaTime;
            if (LobbyListTimer < 0f)
            {
                LobbyListTimer = 3f;
                LobbyList();
            }
        }
    }

    private async void LobbyList()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {
                LobbyList = queryResponse.Results
            });
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void NewLobby(string nombreSala, bool isPrivate)
    {
        MultiplayerManager.Instance.StartHost();
    }

    public async void QuickJoin()
    {     
        MultiplayerManager.Instance.StartClient();

    //    OnJoinStarted?.Invoke(this, EventArgs.Empty);
    //    try
    //    {
    //        joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
    //        string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

    //        JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
    //        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
    //        MultiplayerManager.Instance.StartClient();
    //    }
    //    catch (LobbyServiceException ex)
    //    {
    //        Debug.Log(ex);
    //        OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
    //    }
    }

    public async void JoinWithId(string lobbyId)
    {
    //    OnJoinStarted?.Invoke(this, EventArgs.Empty);
    //    try
    //    {
    //        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

    //        string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

    //        JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

    //        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

    //        MultiplayerManager.Instance.StartClient();
    //    }
    //    catch (LobbyServiceException e)
    //    {
    //        Debug.Log(e);
    //        OnJoinFailed?.Invoke(this, EventArgs.Empty);
    //    }
    }

    public async void JoinWithCode(String codigo)
    {
        //    OnJoinStarted?.Invoke(this, EventArgs.Empty);
        //    try
        //    {
        //        joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(codigo);
        //        string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

        //        JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
        //        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
        //        MultiplayerManager.Instance.StartClient();
        //    }
        //    catch (LobbyServiceException ex)
        //    {
        //        Debug.Log(ex);
        //        OnJoinFailed?.Invoke(this, EventArgs.Empty);
        //    }
    }

    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
            }
            catch (LobbyServiceException ex)
            {
                Debug.Log(ex);
            }
        }
    }

    public async void LeaveLobby()
    {
    //    if (joinedLobby != null)
    //    {
    //        try
    //        {
    //            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
    //            joinedLobby = null;
    //        }
    //        catch (LobbyServiceException ex)
    //        {
    //            Debug.Log(ex);
    //        }
    //    }
    }

    //public async void KickPlayer(string playerId)
    //{
    //    if (IsLobbyHost())
    //    {
    //        try
    //        {
    //            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
    //        }
    //        catch (LobbyServiceException ex)
    //        {
    //            Debug.Log(ex);
    //        }
    //    }
    //}

    public Lobby GetLobby()
    {
        return joinedLobby;
    }
}
