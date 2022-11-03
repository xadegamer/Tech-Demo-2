using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private static int ANIMATOR_PARAM_WALK_SPEED = Animator.StringToHash("WalkSpeed");

    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack,
    }

    [SerializeField] private EnemyState currentState = EnemyState.Patrol;
    [SerializeField] private float attackRate;
    [SerializeField] private Radar attackRadar;
    [SerializeField] private Radar chaseRadar;

    private float timer;

    private NavMeshAgent agent;
    private Vector2 smoothDeltaPosition = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    private Animator anim;
    private Transform target;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        EnemyAIActions.RandomPosition(transform.position, 10, out Vector3 newPos);
        agent.SetDestination(newPos);
    }

    void Update()
    {
        HandleState();
    }

    public void HandleState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Chase: Chase(); break;
            case EnemyState.Attack:Attack(); break;
        }
    }

    public void Patrol()
    {     
        if (agent.velocity.magnitude <= 0)
        {
            EnemyAIActions.RandomPosition(transform.position, 10, out Vector3 newPos);
            agent.SetDestination(newPos);
            EnemyAIActions.LookAtTargetSmooth(transform, newPos);
        }

        if (chaseRadar.TargetInRange())
        {
            target = chaseRadar.TargetObjectInRange().transform;         
            currentState = EnemyState.Chase;
        } 
        if (attackRadar.TargetInRange()) currentState = EnemyState.Attack;
    }

    public void Chase()
    {
        EnemyAIActions.LookAtTargetSmooth(transform, target);
        agent.SetDestination(target.position);
        
        if (!chaseRadar.TargetInRange()) currentState = EnemyState.Patrol;
        if (attackRadar.TargetInRange()) currentState = EnemyState.Attack;
    }

    public void Attack()
    {
        agent.SetDestination(transform.position);

        Transform target = attackRadar.TargetObjectInRange().transform;

        if(target) EnemyAIActions.LookAtTargetSmooth(transform, target);

        if (timer >= attackRate)
        {
            anim.SetTrigger("Attack");
            timer = 0;
        }
        else timer += Time.deltaTime;
        
        if (!chaseRadar.TargetInRange()) currentState = EnemyState.Patrol;
        if (!attackRadar.TargetInRange()) currentState = EnemyState.Chase;
    }

    private void LateUpdate()
    {
        anim.SetFloat(ANIMATOR_PARAM_WALK_SPEED, agent.velocity.magnitude);
    }

    public void AdvancedAIMove()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);
    }
}
