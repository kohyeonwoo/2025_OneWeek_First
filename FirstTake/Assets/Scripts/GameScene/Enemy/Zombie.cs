using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private void Start()
    {
        maxHealth = 5.0f;
        currentHealth = maxHealth;
    }

}
