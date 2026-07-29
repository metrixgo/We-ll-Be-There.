using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SewerKiller : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Image screen;

    private bool killed = false;
    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetInteger("State", 1);
    }

    private void Update()
    {
        if (MainManager.instance.gameState == 1 && !killed) agent.isStopped = false;
        else agent.isStopped = true;

        if (Vector3.Distance(transform.position, player.position) < 0.2f)
        {
            killed = true;
            animator.SetInteger("State", 0);
            StartCoroutine(KillIt());
        }

        if (agent.SetDestination(player.position))
        {
            screen.color = Color.red * Mathf.Clamp(0.7f - Mathf.Clamp(agent.remainingDistance, 0, 15.0f) / 15.0f, 0, 0.7f);
        }
    }

    private IEnumerator KillIt()
    {
        yield return null;
    }
}