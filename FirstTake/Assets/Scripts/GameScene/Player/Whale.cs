using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Whale : Player
{

    public TextMeshProUGUI whaleHealth;

    private void Start()
    {
        maxHealth = 28.0f;

        Init(); 
    }

    private void Update()
    {
        whaleHealth.text = currentHealth.ToString();
    }

}
