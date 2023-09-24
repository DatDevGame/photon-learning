using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class PhotonPlayerList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _contentPlayer;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<PlayerProfile> _playerLists = new List<PlayerProfile>();
    private void PlayerAdd()
    {
        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            PlayerProfile playerObject = Instantiate(_playerPrefab, _contentPlayer.transform).GetComponent<PlayerProfile>();
            _playerLists.Add(playerObject);
            playerObject.InitPlayer(players[i].NickName);
            playerObject.gameObject.GetComponent<TMP_Text>().SetText($"{players[i].NickName}");

            Debug.Log("Add Player: "+ players[i].NickName);
        }
    }
    private void PlayerRemove()
    {
        _playerLists = _playerLists.Where(v => v != null).ToList();
        foreach (PlayerProfile player in _playerLists)
            Destroy(player.gameObject);
    }
    public override void OnJoinedRoom()
    {
        PlayerAdd();
    }
    public override void OnLeftRoom()
    {
        PlayerRemove();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerProfile playerObject = Instantiate(_playerPrefab, _contentPlayer.transform).GetComponent<PlayerProfile>();
        _playerLists.Add(playerObject);
        playerObject.InitPlayer(newPlayer.NickName);
        Debug.Log("Player Enter: " + newPlayer.NickName);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        foreach (PlayerProfile player in _playerLists)
        {
            if (player.NameProfile.ToString() == otherPlayer.NickName)
                Destroy(player.gameObject);
        }
        _playerLists = _playerLists.Where(v => v != null).ToList();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PlayerRemove();
        PlayerAdd();
    }
}
