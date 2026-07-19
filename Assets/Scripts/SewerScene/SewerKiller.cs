using UnityEngine;
using UnityEngine.AI;

public class SewerKiller : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private Animator anim;
    private float dis;
    private float attackDis = 1.0f;
    private float sightDis = 10.0f;
    private float speed = 2.0f;
    private float runSpeed = 4.5f;

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
            anim.SetBool("Run", dis < sightDis);
            agent.destination = target.position;
        }

        if (anim.GetBool("Run")) agent.speed = runSpeed;
        else agent.speed = speed;
    }
}
