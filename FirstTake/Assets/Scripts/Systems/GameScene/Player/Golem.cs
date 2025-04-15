using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Golem : Player
{

    public TextMeshProUGUI golemHealth;

    private void Start()
    {
        maxHealth = 40.0f;
        currentHealth = maxHealth;

        Init();
    }
    private void Update()
    {
        golemHealth.text = currentHealth.ToString(); 
    }
}
