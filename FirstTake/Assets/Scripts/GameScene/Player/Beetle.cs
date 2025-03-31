using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;

public class Beetle : Player
{

    public TextMeshProUGUI beetleHealth;

    private void Start()
    {
        maxHealth = 30.0f;
        currentHealth = maxHealth;

        Init();
    }

    private void Update()
    {
       
        beetleHealth.text = currentHealth.ToString();
 
    }

}
