using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private Button _leftRoomButton;
    [SerializeField] private Button _startGameButton;

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private List<RoomProfile> _roomProfiles = new List<RoomProfile>();
    private void Start()
    {
        _createRoomButton.onClick.AddListener(CreateRoom);
        _joinRoomButton.onClick.AddListener(JoinRoom);
        _leftRoomButton.onClick.AddListener(LeaveRoom);
    }

    private void CreateRoom()
    {
        Hashtable customPropertiesToSet = new Hashtable();
        customPropertiesToSet.Add("HostName", $"{PhotonNetwork.LocalPlayer.NickName}");
        string[] customProperties = new string[1];
        customProperties[0] = "HostName";

        PhotonNetwork.CreateRoom(_roomNameInput.text,
            new RoomOptions() { MaxPlayers = 20,
                IsVisible = true,
                IsOpen = true,
                CustomRoomProperties = customPropertiesToSet,
                CleanupCacheOnLeave = true,
                CustomRoomPropertiesForLobby = customProperties
                }, null);
    }
    private void JoinRoom() => PhotonNetwork.JoinRoom(_roomNameInput.text);
    private void LeaveRoom() => PhotonNetwork.LeaveRoom();
    private void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = Instantiate(_roomPrefab, _content.transform).GetComponent<RoomProfile>();
        string hostName = roomInfo.CustomProperties["HostName"].ToString();
        roomProfile.InitLobby(hostName, roomInfo);
        _roomProfiles.Add(roomProfile);
    }
    private void RoomRemove(RoomInfo roomInfo)
    {
        foreach (RoomProfile roomProfile in _roomProfiles)
        {
            if (roomProfile.IdRoom == roomInfo.Name.GetHashCode())
            {
                Destroy(roomProfile.gameObject);
                _roomProfiles.Remove(roomProfile);
            }
        }
    }
    private void FilterMasterClient()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
            _startGameButton.gameObject.SetActive(players[i].IsMasterClient);
    }
    public override void OnCreatedRoom()
    {
        string roomName = _roomNameInput.text;
        RoomProfile roomProfile = Instantiate(_roomPrefab, _content.transform).GetComponent<RoomProfile>();
        roomProfile.InitCreateRoom(PhotonNetwork.LocalPlayer, roomName);
        _roomProfiles.Add(roomProfile);
        _startGameButton.gameObject.SetActive(true);
        Debug.Log("Create Room: " + roomName);
    }
    public override void OnJoinedRoom()
    {
        string roomName = _roomNameInput.text;

        foreach (RoomProfile roomProfile in _roomProfiles)
            Destroy(roomProfile.gameObject);
        _roomProfiles.Clear();

        FilterMasterClient();
        Debug.Log("Join Room: " + roomName);
    }
    public override void OnLeftRoom()
    {
        _startGameButton.gameObject.SetActive(false);
        Debug.Log("Left Room: " + _roomNameInput.text);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Faild Create Room: " + message);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo rif in roomList)
        {
            if (rif.RemovedFromList)
                RoomRemove(rif);
            else
                RoomAdd(rif);
        }
        Debug.Log("Update Room");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        FilterMasterClient();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        FilterMasterClient();
    }
}
