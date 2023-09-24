using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

[Serializable]
public class RoomProfile : MonoBehaviourPunCallbacks
{
    public int IdRoom { get; private set; }
    private RoomInfo _roomInfo;
    private IEnumerator _updateQuantity;
    [SerializeField] private TMP_Text _hostName;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_Text _quanlity;

    //Add Room Lobby
    public void InitLobby(string hostName, RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        IdRoom = hostName.GetHashCode();
        UpdateStateFromLobby(hostName, roomInfo);
    }
    public void InitCreateRoom(Player player, string roomName)
    {
        UpdateStateCreateRoom(player.NickName, roomName);
    }

    private void UpdateStateCreateRoom(string hostName, string roomName)
    {
        _hostName.SetText($"Host: {hostName}");
        _roomName.SetText($"Room Name: {roomName}");
    }

    private void UpdateStateFromLobby(string hostName, RoomInfo roomInfo)
    {
        _hostName.SetText($"Host: {hostName}");
        _roomName.SetText($"Room Name: {roomInfo.Name}");

        _updateQuantity = UpdateQuanlityPlayer();
        StartCoroutine(_updateQuantity);
    }
    private IEnumerator UpdateQuanlityPlayer()
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
        while (true)
        {
            _quanlity.SetText($"Quality: {_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers}");
            yield return fixedUpdate;
        }
    }
    private void OnDestroy()
    {
        if(_updateQuantity != null)
            StopCoroutine(_updateQuantity);
    }

}
