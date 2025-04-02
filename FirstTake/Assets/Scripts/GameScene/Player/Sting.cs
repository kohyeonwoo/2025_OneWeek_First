using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;
public class Sting : Player
{

    public TextMeshProUGUI stingHealth;

    private void Awake()
    {
        maxHealth = 20.0f;
        currentHealth = maxHealth;
        Init();
    }

    private void Update()
    {
        stingHealth.text = currentHealth.ToString();  
    }

}

