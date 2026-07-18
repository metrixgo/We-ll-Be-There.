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
    private float speed = 5.0f;
    private float runSpeed = 6.5f;

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
        else if (dis < sightDis)
        {
            agent.isStopped = false;
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            agent.destination = target.position;
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("Attack", false);
            anim.SetBool("Run", false);
            agent.destination = target.position;
        }
    }

    private void OnAnimatorMove()
    {
        if (!anim.GetBool("Attack"))
        {
            if (anim.GetBool("Run")) agent.speed = runSpeed * (anim.deltaPosition / Time.deltaTime).magnitude;
            else agent.speed = speed * (anim.deltaPosition / Time.deltaTime).magnitude;
        }
    }
}
