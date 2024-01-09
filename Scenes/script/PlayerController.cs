using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Collider2D Discoll;

    public Transform CellingCheck,GroundCheck;

    public Rigidbody2D rb;
    //
    public Animator anim;
    //
    public Collider2D coll;
    //
    public float speed = 10f;
    // 
    public float jumpforce = 400f ;
    // 设定跳跃力量
    public Text CherryNumber;

    public AudioSource JumpAudio,HurtAudio,CherryAudio;

    public LayerMask ground;
    //
    public bool jumpPressed,isGround,isJump;

    // layerMask:只选定Layermask层内的碰撞器,其它层内碰撞器忽略。
    public int cherry = 0;

    private bool isHurt = false;

    public int extraJump = 2,TestNumber = 3;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            jumpPressed = true;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        
        Jump();
        isGround = Physics2D.OverlapCircle(GroundCheck.position,0.1f,ground);
        if (!isHurt)
        {
            Movement();
        }        
        SwitchAnim();


    }

    void Movement()
    {
      
        float horizontalmove = Input.GetAxis("Horizontal");

        float facedirection = Input.GetAxisRaw("Horizontal");

        //角色移动控制
        if ( horizontalmove != 0)
        {
            rb.velocity = new Vector2(horizontalmove * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("running",Mathf.Abs(horizontalmove));
        
        }
        
        if(facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
        }
        //角色跳跃
        //if (Input.GetButtonDown("Jump"))   
        //{
        //    JumpAudio.Play();
        //    rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
        //    anim.SetBool("jumping", true);
        //}

        Crouch();

    }

    void Jump()
    {
        if (isGround)
        {
            extraJump = 2;
            isJump = false;
            TestNumber = 1;
        }
        else
        {
            isJump = true;
        }

        if (jumpPressed && isGround) 
        {
            rb.velocity = new Vector2(rb.velocity.x , jumpforce);
            isJump = true;  
            extraJump--;
            jumpPressed = false;
            JumpAudio.Play();
            anim.SetBool("jumping", true);
           
            TestNumber = 3;

        }
        else if (jumpPressed && extraJump > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            extraJump--;
            isJump = true;
            jumpPressed = false;
            anim.SetBool("jumping", true);
            TestNumber = 3;
        }
    }

    void SwitchAnim()
    {

        anim.SetBool("idle", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }

        }
        else if (isHurt)
        {
           
            anim.SetBool("hurt", true);
            anim.SetFloat("running", 0);
            
            if (Mathf.Abs(rb.velocity.x)<0.1f )
            { 
                
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集樱桃
        if ( collision.tag == "collection" )
        {
            CherryAudio.Play();
            Destroy(collision.gameObject);
            cherry++;
            CherryNumber.text = cherry.ToString();
        }

        if( collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;
            Invoke("ReStart",2f);
        }

        if (collision.tag == "DangerThing")
        {
            if (transform.position.x <= collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
            else
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
        }

    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Enemy") 
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.fixedDeltaTime);
                anim.SetBool("jumping", true);
                
            }else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                HurtAudio.Play();
                isHurt = true;
            }
        }

        
    }


    void Crouch()
    {
        if (!Physics2D.OverlapCircle(CellingCheck.position,0.2f,ground)) 
        {
            if (Input.GetButton("Crouch"))
            { 
                anim.SetBool("crouching", true);
                Discoll.enabled = false;

            }
            else 
            {
                 anim.SetBool("crouching", false);
                 Discoll.enabled = true;
            }   
        }


    }

    void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
