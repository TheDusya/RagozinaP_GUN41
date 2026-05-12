using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TextController : MonoBehaviour
{
    [Inject]
    SharedDataManager _dataManager;
    private const string _player1Text = "Player1, go!";
    private const string _player2Text = "Player2, go!";
    private const string _confirmText = "Are you sure?\n[Press Space to confirm]";
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _dataManager.OnNextPlayer += WritePlayerText;
        _dataManager.OnWaitForConfirm += WriteConfirmText;
        _text = gameObject.GetComponent<TextMeshProUGUI>();
        _text.fontSize = 120;
        _text.text = _player1Text;
    }

    private void WritePlayerText()
    {
        _text.fontSize = 120;
        _text.text = _dataManager.CurrentPlayer switch
        {
            Team.Player1 => _player1Text,
            Team.Player2 => _player2Text,
            _ => "Sorry, something is wrong:("
        };
    }
    private void WriteConfirmText()
    {
        _text.fontSize = 80;
        _text.text = _confirmText;
    }

    private void OnDestroy()
    {
        _dataManager.OnNextPlayer -= WritePlayerText;
        _dataManager.OnWaitForConfirm -= WriteConfirmText;
    }
}
