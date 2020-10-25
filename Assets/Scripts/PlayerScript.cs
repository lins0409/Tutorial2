using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text winText;
    public Text livesText;
    private int lives = 3;
    private int scoreValue = 0;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    Animator anim;
    private bool facingRight = true;

    public bool isOnGround;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        livesText.text = "Lives: " + lives.ToString();
        winText.text = "";
    }
    
    void Update(){
   
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        if(Input.GetKeyDown(KeyCode.D) && isOnGround){
            anim.SetInteger("State", 2);
        } 
        if(Input.GetKeyUp(KeyCode.D)){
            anim.SetInteger("State", 0);
        }

        if(Input.GetKeyDown(KeyCode.A) && isOnGround){
            anim.SetInteger("State", 2);
        }

        if(Input.GetKeyUp(KeyCode.A)){
            anim.SetInteger("State", 0);
        }

        if(Input.GetKeyDown(KeyCode.W )){
            anim.SetInteger("State", 1);
        }
        if(isOnGround == false){
            anim.SetInteger("State", 1);
        }
    }

    void Flip(){
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape")){
                Application.Quit();
            }

        if (facingRight == false && hozMovement > 0){
            Flip();
            }

        else if (facingRight == true && hozMovement < 0){
            Flip();
            }
        if(hozMovement > 0 || hozMovement < 0){
            anim.SetInteger("State", 2);
        }
        else if(vertMovement == 0){
            anim.SetInteger("State", 0);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin"){
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue == 4){
            transform.position = new Vector3(45.0f, 0.0f, 0.0f);
            lives = 3;
            livesText.text = "Lives: " + lives.ToString();
            }

            if(scoreValue == 8){
            winText.text = "You Win! This game was created by Sue Lin";
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = false;
            }
            
        }

        if(collision.collider.tag == "Enemy"){
            lives -= 1;
            livesText.text = "Lives: " + lives.ToString();
            Destroy(collision.collider.gameObject);

            if (lives == 0){
            winText.text = "You Lose! Good luck next time! Game created by Sue Lin";
            Destroy(gameObject);
            }
        } 
    
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3),ForceMode2D.Impulse);
            }
        }
    }

}
