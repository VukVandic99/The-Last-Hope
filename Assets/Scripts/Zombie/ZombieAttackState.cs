using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float stopAttackingDistance = 4f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  inicijalizacija igraca i agenta:
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieAttack;
        }

        //  metoda da agent gleda u igraca dok ga udara konstantno da gleda u njega:
        LookAtPlayer();

        //  provera da li prestati da napada
        //  distanca igraca i agenta:
        float distanceFromPlayer = Vector3.Distance(player.position,
            agent.transform.position);

        //  provera da li je blizu
        if (distanceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("ISATTACKING", false);
        }
    }
    //  ovde nam ne treba metoda onStateExit isto kao i u idleState
    public void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
