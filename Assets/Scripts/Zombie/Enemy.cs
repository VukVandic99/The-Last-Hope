using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //  health:
    public int HP;
    private Animator animator;
    //  gasimo animaciju nakon sto je zombie ubijen:
    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDMG(int dmgAmount)
    {
        HP -= dmgAmount;

        if (HP <= 0)
        {
            int randomDieValue = Random.Range(0, 2);
            if (randomDieValue == 0)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            isDead = true;
            //  zvuk za smrt
            SoundManager.Instance.zombieChannelHurtDeath.PlayOneShot(//  moramo ponovo iz managera da nadjemo
                SoundManager.Instance.zombieDeath);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            //  zvuk za pogodak

            SoundManager.Instance.zombieChannelHurtDeath.PlayOneShot(//  moramo ponovo iz managera da nadjemo
                SoundManager.Instance.zombieHurt);
        }
    }
}
