using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SewerKiller : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackDis = 1.0f;

    private NavMeshAgent agent;
    private Animator anim;
    private float dis;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        dis = Vector3.Distance(agent.transform.position, target.position);

        if (dis < attackDis)
        {
            agent.isStopped = true;
            anim.SetBool("Attack", true);
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("Attack", false);
            agent.destination = target.position;
        }
    }

    private void OnAnimatorMove()
    {
        if (!anim.GetBool("Attack"))
        {
            agent.speed = (anim.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
