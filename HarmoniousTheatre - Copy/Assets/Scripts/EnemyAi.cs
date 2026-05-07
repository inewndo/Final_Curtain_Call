using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack, Death }
    public EnemyState currentState;

    private Transform player;
    private NavMeshAgent agent; //for enemy's ai navigation around obstacles

    //patrol settings
    public Transform[] patrolPoints; //waypoints
    private int currentPatrolIndex;
    private int health = 20;
    //private float speed = 2f;
    private float detectionRange = 10f;
    private float attackRange = 3f;
    private int attackPower = 5;
    private float attackCoolDown = 5f;

    public int currentHealth;

    private bool isAttacking;
    private float lastAttackTime;
    private float timer;
    [SerializeField] private HealthBar healthbar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //LoadEnemyData(enemyType);
        currentState = EnemyState.Patrol; //start w patrolling
        MoveToNextPatrolPoint();
        //find and assign the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentHealth = health;
        //healthbar.UpdateHealthBar(health, currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        //switch statement, determines which behavior enemy should perform based on its current State
        // checks current state and decides which bahavior to execute
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleBahavior();
                //break make sure program doesnt check other cases once a match is found
                break;
            case EnemyState.Patrol:
                PatrolBehavior();
                if (distanceToPlayer <= detectionRange) ChangeState(EnemyState.Chase);
                break;
            case EnemyState.Chase:
                ChaseBehavior();
                if (distanceToPlayer <= attackRange) ChangeState(EnemyState.Attack);
                else if (distanceToPlayer > detectionRange) ChangeState(EnemyState.Patrol);
                break;
            case EnemyState.Attack:
                AttackBehavior();
                if (distanceToPlayer > attackRange) ChangeState(EnemyState.Chase);
                break;
            case EnemyState.Death:
                break;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Died");
        }
    }
    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    void IdleBahavior()
    {
        //animation
    }

    void PatrolBehavior()
    {
        //ensures enmy switches patrol points only after reaching target
        //if enmy close enough to patrol point, .5 moves to next patrol point
        if (!agent.pathPending && agent.remainingDistance < .5f)
        {
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        //if no control point - exit the function
        if (patrolPoints.Length == 0) return;
        //set destination moves to next patrol point
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        //update index so it moves by 1
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void ChaseBehavior()
    {
        //destination set to follow player
        agent.SetDestination(player.position);
    }

    void AttackBehavior()
    {

        if (timer <= 0)
        {
            isAttacking = true;


            timer = attackCoolDown;
        }

        //WaitForSeconds(attackCoolDown)
        //animator.SetTrigger(Attack);

        if (Time.time >= lastAttackTime + attackCoolDown)
        {
            lastAttackTime = Time.time;
            Debug.Log("Enemy Attacked Player");
            FindFirstObjectByType<CCPlayer>().TakeDamage(attackPower);
        }
    }
}
