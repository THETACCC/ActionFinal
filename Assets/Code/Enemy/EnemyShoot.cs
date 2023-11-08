using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawning;
    public GameObject spawningblock;
    public GameObject spawningslime;
    public Transform bulletpos;
    public Transform spawnpos;

    private float timer;

    private float spamtimer;

    private float bulletspamcount = 0;


    private float ultimatetimer;

    private float ultimatebulletcount;

    //vertical attack
    private bool playerposChecked = false;
    private float verticaltimer;

    private float bulletverticalcount  = 0;



    private GameObject player;

    public float detectdistance;
    [SerializeField] float force;

    //Self related
    private Rigidbody2D Myrb;
    [Header("For patrolling")]
    [SerializeField] float movespeed;
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] Transform playerpos;


    //Movement
    private float current_speed = 0f;
    public float acceleration = 5f;
    public float decceleration = 10f;
    private float lerpspeed = 2f;
    private float rotatetimer = 0f;
    private float max_hspeed = 20f;
    //vertical movement

    private float current_vspeed = 0f;
    private float max_vspeed = 20f;
    private float verticaldirection = 1f;

    //find and shoot the player
    public float ShootingRange;


    private enum State
    {
        Idle,
        SpotPlayer,
        NormalAttack,
        HeavyAttack,
        SpamAttack,
        SpamAttackSpawnBlock,
        SpamAttackSpawnSlime,
        VerticalAttack,
        UltimateAttack,

    }


    private State state;

    void Start()
    {
        Myrb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        current_speed = Mathf.Clamp(current_speed,0,max_hspeed);
        current_vspeed = Mathf.Clamp(current_vspeed, 0, max_vspeed);

        switch (state)
        {
            default: 
            case State.Idle:
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (current_speed > 0)
                {
                    current_speed -= decceleration * Time.deltaTime;
                }

                Myrb.velocity = new Vector2(current_speed * moveDirection, Myrb.velocity.y);
                if (distance < detectdistance)
                {
                     
                   state = State.SpotPlayer;
                }
                break;

            case State.SpotPlayer:
                float distancefromplayer = Vector2.Distance(transform.position, player.transform.position);
                float playerpositionturn = playerpos.position.x - transform.position.x;
                float playerverticaldist = transform.position.y - playerpos.position.y;

                rotatetimer += Time.deltaTime;
                if(rotatetimer > 2)
                {
                    if (playerpositionturn < 0)
                    {
                        moveDirection = -1;
                    }
                    else if (playerpositionturn > 0)
                    {
                        moveDirection = 1;
                    }
                    else if (playerpositionturn == 0)
                    {
                        moveDirection = 0;
                    }
                    rotatetimer = 0;
                }

                if (playerverticaldist < 10)
                {
                    verticaldirection = 1;
                    current_vspeed += acceleration * Time.deltaTime;
                }
                else if (playerverticaldist > 10 && playerverticaldist < 15)
                {
                    verticaldirection = 0;
                    current_vspeed -= decceleration * Time.deltaTime;
                }
                else if (playerverticaldist > 15)
                {
                    verticaldirection = -1;
                    current_vspeed += acceleration * Time.deltaTime;
                }







                if (distancefromplayer < detectdistance && distancefromplayer > ShootingRange)
                {
                    current_speed += acceleration * Time.deltaTime;
                }
                else if (current_speed > 0)
                {
                    current_speed -= decceleration * Time.deltaTime;
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer > 0.5)
                    {
                        timer = 0;

                        int attack = UnityEngine.Random.Range(0, 16);
                        Debug.Log(attack);
                        if (attack == 0)
                        {
                            state = State.NormalAttack;
                        }
                        else if (attack == 1 || attack == 2)
                        {
                            state = State.HeavyAttack;
                        }
                        else if (attack == 3 || attack == 4 || attack == 5)
                        {
                            state = State.SpamAttack;
                        }
                        else if (attack == 6 || attack == 7 || attack == 8 || attack == 9)
                        {
                            state = State.VerticalAttack;
                        }
                        else if (attack == 10 || attack == 11 || attack == 12)
                        {
                            state = State.SpamAttackSpawnBlock;
                        }
                        else if (attack == 13 || attack == 14 || attack == 15)
                        {
                            state = State.SpamAttackSpawnSlime;
                        }

                    }
                }
                Myrb.velocity = Vector2.Lerp(Myrb.velocity, new Vector2(current_speed * moveDirection, current_vspeed * verticaldirection), Time.deltaTime * lerpspeed);







                break;

            case State.NormalAttack:
                NormalShoot();



                break;

            case State.HeavyAttack:
                HeavyShoot();





                break;

            case State.SpamAttack:
                SpamAttack();

                break;
            case State.SpamAttackSpawnBlock:
                SpamAttackSpawnBlock();

                break;
            case State.SpamAttackSpawnSlime:
                SpamAttackSpawnSlime();

                break;
            case State.VerticalAttack:
                if(!playerposChecked)
                {
                    float playerposition = playerpos.position.x - transform.position.x;
                    if (playerposition < 0)
                    {
                        moveDirection = -1;
                    }
                    else if (playerposition > 0)
                    {
                        moveDirection = 1;
                    }
                    playerposChecked = true;
                }
                VerticalAttack();








                break;





        }








    }

    void NormalShoot()
    {
        GameObject bullet_obj = Instantiate(bullet,bulletpos.position, Quaternion.identity);
        Vector3 direction = player.transform.position - transform.position;
        Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        state = State.Idle;
    }
    void HeavyShoot()
    {
        for (int i = 0;i < 4; i++)
        {
            GameObject bullet_obj = Instantiate(bullet, bulletpos.position, Quaternion.identity);
            Vector3 direction = player.transform.position - transform.position;
            Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(direction.x + i*5, direction.y + i * 5).normalized * force;
        }


        state = State.Idle;
    }
    void SpamAttack()
    {
        Myrb.velocity = new Vector2(0,0);
        if (bulletspamcount < 5)
        {

            spamtimer += Time.deltaTime;
            if (spamtimer > 0.15)
            {

                spamtimer = 0;
                GameObject bullet_obj = Instantiate(spawning, bulletpos.position, Quaternion.identity);
                Vector3 direction = player.transform.position - transform.position;
                Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(direction.x + UnityEngine.Random.Range(-5,5), direction.y + UnityEngine.Random.Range(-5, 5)).normalized * force;
                bulletspamcount += 1;
            }
        }
        else if (bulletspamcount >= 5)
        {
            bulletspamcount = 0;
            state = State.Idle;
        }
    }

    void SpamAttackSpawnBlock()
    {
        Myrb.velocity = new Vector2(0, 0);
        if (bulletspamcount < 2)
        {

            spamtimer += Time.deltaTime;
            if (spamtimer > 0.15)
            {

                spamtimer = 0;
                GameObject bullet_obj = Instantiate(spawningblock, spawnpos.position, Quaternion.identity);
                Vector3 direction = player.transform.position - transform.position;
                Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(direction.x + UnityEngine.Random.Range(-5, 5), direction.y + UnityEngine.Random.Range(-5, 5)).normalized * force;
                bulletspamcount += 1;
            }
        }
        else if (bulletspamcount >= 2)
        {
            bulletspamcount = 0;
            state = State.Idle;
        }
    }

    void SpamAttackSpawnSlime()
    {
        Myrb.velocity = new Vector2(0, 0);
        if (bulletspamcount < 2)
        {

            spamtimer += Time.deltaTime;
            if (spamtimer > 0.15)
            {

                spamtimer = 0;
                GameObject bullet_obj = Instantiate(spawningslime, spawnpos.position, Quaternion.identity);
                Vector3 direction = player.transform.position - transform.position;
                Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(direction.x + UnityEngine.Random.Range(-5, 5), direction.y + UnityEngine.Random.Range(-5, 5)).normalized * force;
                bulletspamcount += 1;
            }
        }
        else if (bulletspamcount >= 2)
        {
            bulletspamcount = 0;
            state = State.Idle;
        }
    }

    void VerticalAttack()
    {
        current_speed += acceleration * Time.deltaTime;


        float playerverticaldist = transform.position.y - playerpos.position.y;
        if (playerverticaldist < 10)
        {
            verticaldirection = 1;
            current_vspeed += acceleration * Time.deltaTime;
        }
        else if (playerverticaldist > 10 && playerverticaldist < 15)
        {
            verticaldirection = 0;
            current_vspeed -= decceleration * Time.deltaTime;
        }
        else if (playerverticaldist > 15)
        {
            verticaldirection = -1;
            current_vspeed += acceleration * Time.deltaTime;
        }
        Myrb.velocity = Vector2.Lerp(Myrb.velocity, new Vector2(current_speed * moveDirection, current_vspeed * verticaldirection), Time.deltaTime * lerpspeed);
        if (bulletverticalcount < 10)
        {

            verticaltimer += Time.deltaTime;
            if (verticaltimer > 0.3)
            {
                verticaltimer = 0;
                GameObject bullet_obj = Instantiate(bullet, bulletpos.position, Quaternion.identity);
                Vector3 direction = player.transform.position - transform.position;
                Rigidbody2D rb = bullet_obj.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(0, -1).normalized * force;
                bulletverticalcount += 1;
            }
        }
        else
        {
            playerposChecked = false;
            bulletverticalcount = 0;
            state = State.Idle;
        }




    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectdistance);
        Gizmos.DrawWireSphere(transform.position, ShootingRange);
    }

}
