using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsVisualizer : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;

    private void Start()
    {
        CoinsManager.instance.coinsModified += UpdateCoinsString;
    }

    private void UpdateCoinsString(string coins)
    {
        coinsText.text = coins;
    }
}