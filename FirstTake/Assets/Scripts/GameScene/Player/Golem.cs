using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Player
{
    private void Start()
    {
        maxHealth = 40.0f;
        currentHealth = maxHealth;
    }
}
