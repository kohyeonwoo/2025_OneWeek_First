using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using TMPro;

public class Rhino : Player
{

    public TextMeshProUGUI rhinoHealth;

    private void Start()
    {
        Init();

        maxHealth = 30.0f;
        currentHealth = maxHealth;
    }

    private void Update()
    {

        rhinoHealth.text = currentHealth.ToString();
    }

}
