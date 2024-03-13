using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : StateMachineBehaviour
{
    private float time;
    //��ȡѲ�ߵ�
    private List<Transform> patrolPoints = new List<Transform>();
    private NavMeshAgent agent;
    private float chaseRange = 15f;
    private Transform player;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
        Transform patrolPointsObject = GameObject.FindWithTag("PatrolPoint").transform;
        foreach (Transform patrolPoint in patrolPointsObject)
        {
            patrolPoints.Add(patrolPoint);
        }
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[0].position);//���ó�ʼλ��

        player = GameObject.FindWithTag("Player").transform;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Count)].position);//�Զ�Ѱ·
        }
        time += Time.deltaTime;
        if (time > 10)
        {
            animator.SetBool("isPatrolling", false);
        }

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance<=chaseRange)
        {
            animator.SetBool("isChasing", true);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // ��agent��Ŀ��λ������ΪAnimator��transformλ��
        // ������agent��Ŀ��λ��Ϊ��ǰλ��
        agent.destination = animator.transform.position;
        agent.isStopped = true;
        // agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
