using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Player
{
    private void Start()
    {
        maxHealth = 30.0f;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        RandomMove();
    }
}
