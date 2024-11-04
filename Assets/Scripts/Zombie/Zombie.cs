using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;

    public int zombieDmg;

    private void Start()
    {
        zombieHand.damage = zombieDmg;
    }
}
