using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieChaseState : StateMachineBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 10f;
    //public float stopChaseDistance = 21f;
    public float attackingDistance = 3.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*probao sam player = GameObject.FindGameObjectsWithTag("Player").transform;
         ali to ne radi jer GameObjet... vraca niz GameObjecta cak i kada postoji jedan
        tag player, zbog toga ne moze se direktno pristupiti plejeru
        umesto samo pristupi prvom elementu u nizu a zatim transform*/
        //  inicijalizacija igraca i agenta:
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //  zatim odmah cim nadje playerObjet pristupiti prvom elementu u nizu
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        //  zatim trazimo komponentu agenta preko navmeshagent
        agent = animator.GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = chaseSpeed;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  ovde pozivamo zvuk:
        if(SoundManager.Instance.zombieChannel.isPlaying == false)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieChase;
            //  pauza 1f da ne ide konstantno na repeat
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }


        //  pomeramo zombija u poziciju igraca:
        agent.SetDestination(player.position);
        //  da se okrene prema igracu dok ga juri(agent)
        animator.transform.LookAt(player);

        //  distanca igraca da li da juri ili da napada
        float distanceFromPlayer = Vector3.Distance(player.position,
            animator.transform.position);
        if (distanceFromPlayer < attackingDistance)
        {
            animator.SetBool("ISATTACKING", true);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  zaustavljanje agenta da se krece:
        //  uzima trenutnu poziciju kao poslednju
        agent.SetDestination(animator.transform.position);
        //  cilj ovoga je da zaustavi agenta iz stanja jurnjave kada dostigne igraca

        SoundManager.Instance.zombieChannel.Stop();
    }
}
