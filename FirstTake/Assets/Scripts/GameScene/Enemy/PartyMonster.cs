using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMonster : Enemy
{
    private void Start()
    {
        maxHealth = 3.0f;
        currentHealth = maxHealth;
    }

}
