using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeenemymovc : MonoBehaviour
{
    [Header("For patrolling")]
    [SerializeField] float movespeed;
    private float moveDirection = 1;
    private bool facingRight = true;
    [SerializeField] Transform groundcheck;
    [SerializeField] Transform wallcheck;
    [SerializeField] float circleradius;
    [SerializeField] LayerMask groundLayer;
    private bool checkingGround;
    private bool checkingWall;

    [Header("For Jump Attack")]
    [SerializeField] float jumpheight1;
    [SerializeField] float jumpheight2;
    [SerializeField] Transform player;
    [SerializeField] Transform groundcheckattack;
    [SerializeField] Vector2 boxsize;
    [SerializeField] private bool isgrounded;

    [Header("For Seeing Player")]
    [SerializeField] Vector2 lineofsite;
    [SerializeField] LayerMask playerLayer;
    private bool canseeplayer;
    private Animator enemyAnim;


    [Header("Others")]
    private Rigidbody2D enemyRB;
    private SpriteRenderer renderer;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkingGround = Physics2D.OverlapCircle(groundcheck.position, circleradius, groundLayer);
        checkingWall = Physics2D.OverlapCircle(wallcheck.position, circleradius, groundLayer);
        isgrounded = Physics2D.OverlapBox(groundcheckattack.position, boxsize, 0, groundLayer);
        canseeplayer = Physics2D.OverlapBox(transform.position, lineofsite, 0, playerLayer);
        AnimationController();
        if (!canseeplayer && isgrounded)
        {

            Patrolling();
        }




    }

    void Patrolling()
    {
        if(!checkingGround || checkingWall)
        {
            if(facingRight)
            {
                Flip();
            }
            else if (!facingRight)
            {
                Flip();
            }
        }

        enemyRB.velocity = new Vector2(movespeed * moveDirection, enemyRB.velocity.y);
    }

    void JumpAttack()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;

        if (isgrounded)
        {
            enemyRB.AddForce(new Vector2(distanceFromPlayer * 4, Random.Range(jumpheight1,jumpheight2)), ForceMode2D.Impulse);
        }


    }

    void FlipTowardsPlayer()
    {

        float playerposition = player.position.x - transform.position.x;
        if(playerposition < 0 && facingRight)
        {
            Flip();
        }
        else if (playerposition > 0 && facingRight)
        {
            Flip();
        }

    }


    void Flip()
    {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void AnimationController()
    {
        enemyAnim.SetBool("canseeplayer", canseeplayer);
        enemyAnim.SetBool("isgrounded", isgrounded);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, lineofsite);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ShotgunBullet" || collision.gameObject.tag == "HandgunBullet")
        {
            Destroy(gameObject);
        }
        else
        {
            Flip();
        }
    }

}
