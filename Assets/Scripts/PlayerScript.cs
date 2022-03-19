using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    //UI variables
    public TextMeshProUGUI scoreText;
    public GameObject winTextObject;
    public TextMeshProUGUI livesText;
    public GameObject lossTextObject;
    private int score;
    private int lives;
    
    //music clip variables

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    //collision variables

    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
        
    //animation variables

    Animator anim;

    //groundcheck variables

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    //flip variables

    private bool facingRight = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score = 0;
        lives = 3;

        SetScoreText();
        SetLivesText();
        winTextObject.SetActive(false);
        lossTextObject.SetActive(false);

        anim = GetComponent<Animator>();
        
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop =  true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip();  
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }


        if (vertMovement > 0 || vertMovement < 0)
        {
            anim.SetInteger("State", 1);
        }
        if(isOnGround)
        {

            if (vertMovement == 0 && hozMovement == 0)
            {
                anim.SetInteger("State", 0);
            }
            if (hozMovement > 0 || hozMovement < 0)
            {
                anim.SetInteger("State", 2);
            }
        }
                  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
            {
                Destroy(collision.collider.gameObject);
                score += 1;
                SetScoreText();
                if(score == 4)
                {
                    transform.position = new Vector2(80.0f, 0.0f);
                    lives = 3;
                    SetLivesText();
                }              
            }
        if(collision.collider.tag == "Enemy")
        {
            Destroy(collision.collider.gameObject);
            lives -= 1;
            SetLivesText();
            if(lives == 0)
            {
                speed = 0;
                jumpForce = 0;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void SetScoreText()
    {
        scoreText.text = score.ToString();
        if (score == 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
    }
    void SetLivesText()
    {
        livesText.text = "Health: " + lives.ToString();
        if (lives == 0)
        {
            lossTextObject.SetActive(true);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }           
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
    
}