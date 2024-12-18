using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    private Rigidbody rb;
    public float walkspeed = 0.5f, runspeed = 1f;
    private Transform PlayerOrientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public Animator anim;
    float hitPoints = 150f;
    public Slider sliderHealth;

    [Header("Jump Logic")]
    public float jumpForce;
    public float fallSpeed;
    public float airMultiplier;
    bool grounded = true;
    bool aerialBoost = true;

    [Header("SFX")]
    public AudioClip StepAudio;
    AudioSource PlayerAudio;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        PlayerOrientation = this.GetComponent<Transform>();
        PlayerAudio = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hitPoints);
        Movement();
        Jump();
    }

    private void Movement()
    {
        if (hitPoints != 0f)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            moveDirection = PlayerOrientation.forward * verticalInput + PlayerOrientation.right * horizontalInput;
            if (moveDirection != Vector3.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("Run", true);
                    rb.AddForce(moveDirection.normalized * runspeed * 10f, ForceMode.Force);
                }
                else
                {
                    anim.SetBool("Walk", true);
                    anim.SetBool("Run", false);
                    rb.AddForce(moveDirection.normalized * walkspeed * 10f, ForceMode.Force);
                }
            }
            else
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            grounded = false;
            anim.SetBool("Jump", true);
        }
        else if (!grounded)
        {
            rb.AddForce(Vector3.down * fallSpeed * rb.mass, ForceMode.Force);
            if (aerialBoost)
            {
                rb.AddForce(moveDirection.normalized * walkspeed * 10f * airMultiplier, ForceMode.Impulse);
                aerialBoost = false;
            }
        }
    }

    public void groundedChanger()
    {
        grounded = true;
        aerialBoost = true;
        anim.SetBool("Jump", false);
    }

    public void step()
    {
        PlayerAudio.clip = StepAudio;
        PlayerAudio.Play();
    }

    public float HitPoints()
    {
        return hitPoints;
    }

    public void PlayerGetHit(float damage, float slider)
    {
        Debug.Log("Player Receive Damage - " + damage);
        hitPoints = hitPoints - damage;
        sliderHealth.value = sliderHealth.value - slider;

        if (hitPoints == 0f)
        {
            anim.SetBool("Death", true);
        }
    }
}
