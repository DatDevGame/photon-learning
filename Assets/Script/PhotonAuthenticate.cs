using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UniRx;
using System;
using Photon.Realtime;

public class PhotonAuthenticate : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private TMP_Text _protonStatus;
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _logoutButton;
    private void Start()
    {
        _loginButton.onClick.AddListener(Login);
        _logoutButton.onClick.AddListener(Logout);
    }
    private void Update() => _protonStatus.SetText($"{PhotonNetwork.NetworkClientState}");
    private void Login()
    {
        string name = _inputName.text;
        if (String.IsNullOrWhiteSpace(name) || FilterLetterOrDigit.FillerSpecialCharacters(name))
        {
            Debug.LogError("User Null or Special Characters");
            return;
        }

        Debug.Log("Login: " + name);
        PhotonNetwork.LocalPlayer.NickName = name;
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Logout()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Disconnected) return;

        string name = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("Logout: " + name);
        PhotonNetwork.Disconnect();
    }
    private IEnumerator DelayJoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("On Connected To Master");
        StartCoroutine(DelayJoinLobby());
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("On Joined Lobby");
    }
}
