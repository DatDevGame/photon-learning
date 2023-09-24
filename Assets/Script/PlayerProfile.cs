using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProfile : MonoBehaviour
{
    public string NameProfile { get; private set; }

    [SerializeField] private TMP_Text _nameText;
    public void InitPlayer(string playerName)
    {
        _nameText.SetText(playerName);
        NameProfile = playerName;
    }
}
