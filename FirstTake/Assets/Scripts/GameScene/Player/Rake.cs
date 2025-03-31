using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rake : Player
{

    public TextMeshProUGUI rakeHealth;

    private void Start()
    {
        maxHealth = 25.0f;
        currentHealth = maxHealth;

        Init();
    }

    private void Update()
    {

        rakeHealth.text = currentHealth.ToString();

    }

}
