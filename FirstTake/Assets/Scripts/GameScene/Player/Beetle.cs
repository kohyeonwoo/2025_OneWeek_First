using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : Player
{
    private void Start()
    {
        maxHealth = 30.0f;
        currentHealth = maxHealth;
    }
}
