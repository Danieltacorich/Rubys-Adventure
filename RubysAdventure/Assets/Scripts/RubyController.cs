using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    //Health & Invincibility
    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public int health { get { return currentHealth; }}
    int currentHealth;
    bool isInvincible;
    float invincibleTimer;
    public GameObject projectilePrefab;

    //Rigidbody and Movement
    public float speed = 3.0f;
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    //Animation
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    //Audio
    public AudioClip throwSound;
    public AudioClip hitSound;
    AudioSource audioSource;
    public TextMeshProUGUI repairedText;
    private int repaired;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        // Tester currentHealth = 1;
        audioSource= GetComponent<AudioSource>();

        // Score n Win/Lose Text
        
        repaired = 0;

        SetCountText ();
    }
    

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        //Invicibility
         if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        // Projectile
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
             RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        
    }
    // Scoring n Teleport
    void SetCountText()
    {
        repairedText.text = "repaired: " + repaired.ToString();


    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + 3.0f * horizontal * Time.deltaTime;
        position.y = position.y + 3.0f * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
    //Health & Invincibility 
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitSound);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    // Projectile
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
