using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy Movement")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform playerlook;
    public LayerMask isGround, isPlayer;
    public float hitPoints;
    public Animator anim;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    [Header("Enemy Attack")]
    //attack
    public float timeBetweenAttacks;
    bool alreadyAttack;

    //state
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //GAMEOVER
    public GameObject GameOver;


    [Header("SFX")]
    public AudioClip enemyStep;
    public AudioClip jumpScareAudio;
    AudioSource EnemyAudio;

    [Header("VFX")]
    public GameObject screen;
    public VideoClip jumpscareVideo;
    VideoPlayer jumpscare;

    void Start()
    {
        // playerlook = GameObject.Find("Swat").transform;
        agent = GetComponent<NavMeshAgent>();
        EnemyAudio = GetComponent<AudioSource>();
        jumpscare = GetComponent<VideoPlayer>();
        anim = GetComponent<Animator>();
    }

    private void Update() 
    {
        //Cek sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            agent.stoppingDistance = 0f;
        }
        if (player.GetComponent<PlayerLogic>().HitPoints() == 0)
        {
            Patroling();
            agent.stoppingDistance = 0f;
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
                agent.stoppingDistance = 9f;
            }

            if (playerInSightRange && playerInAttackRange)
                AttackPlayer();
        }
        
    }

    private void Patroling()
    {
        if(!walkPointSet) SearchWalkPoint();
        
        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //waypoint reached
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            anim.SetBool("Run", false);
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
            anim.SetBool("Run", true);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerlook.position);
        anim.SetBool("Run",true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(playerlook.position);

        transform.LookAt(playerlook);

        if(!alreadyAttack)
        {
            //Code Attack here
            anim.SetBool("Attack",true);

            alreadyAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttack = false;
        anim.SetBool("Attack",false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public void TakeDamage (float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(gameObject, 5f);
        }
    }

    public void HitConnect()
    {
        if(playerInAttackRange)
        {
            player.GetComponent<PlayerLogic>().PlayerGetHit(50f, 1f);
            Debug.Log("Damaged!!!");
            jumpscare.clip = jumpscareVideo;
            screen.SetActive(true);
            jumpscare.Play();
            EnemyAudio.clip = jumpScareAudio;
            EnemyAudio.Play();

            StartCoroutine(DisableJumpscare());
        }
    }

    IEnumerator DisableJumpscare()
    {
        yield return new WaitForSeconds(4);
        jumpscare.Stop();
        screen.SetActive(false);
        if (player.GetComponent<PlayerLogic>().HitPoints() == 0)
        {
            GameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void StepEnemy()
    {
        EnemyAudio.clip = enemyStep;
        EnemyAudio.Play();
    }

    public void GameResultDecision(bool TryAgain)
    {
        if (TryAgain) SceneManager.LoadScene("Gameplay");
        else SceneManager.LoadScene("Menu");
    }
}
