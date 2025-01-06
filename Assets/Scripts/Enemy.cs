using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public float hp;    
    public float maxHp; 
    public float speed;
    public Rigidbody2D target;

    bool isDead = false;

    Animator animator;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;
    // Start is called before the first frame update
    private void Awake() 
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isDead || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        Vector2 dir = target.position - rb.position;
        Vector2 nextVec = dir.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isDead)
        {
            return;
        }

        spriteRenderer.flipX = (target.position.x < rb.position.x);
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isDead = false;
        col.enabled = true;
        rb.simulated = true;
        spriteRenderer.sortingOrder = 2;
        animator.SetBool("Dead", false);
        hp = maxHp;
    }

    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHp = data.health;
        hp = data.health;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || isDead)
            return;

        hp -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (hp > 0)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            isDead = true;
            col.enabled = false;
            rb.simulated = false;
            spriteRenderer.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }
    IEnumerator KnockBack()
    {
        yield return wait;  //다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rb.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
