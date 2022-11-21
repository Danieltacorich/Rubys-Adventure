using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    //Health & Invincibility & Ammo
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    //Ammo
    public GameObject projectilePrefab;
    public int ammo {get {return currentAmmo; }}
    int currentAmmo = 4;
    public TextMeshProUGUI ammoText;
    //Invincibility & Effects
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    public ParticleSystem HealEffect;  // test
    public ParticleSystem DmgEffect;

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
    public AudioClip winSound;
    public AudioClip victory;
    AudioSource audioSource;

    //Fixed Robots Score & Level
    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;
    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    bool gameOver;
    bool winGame;
    public static int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        HealEffect.Stop();
        DmgEffect.Stop();
        
        //Ammo
        rigidbody2d = GetComponent<Rigidbody2D>();    
        AmmoText();

        currentHealth = maxHealth;
        // Tester currentHealth = 1;
        audioSource= GetComponent<AudioSource>();

        // Score n Win/Lose Text
        
        fixedText.text = "Bots fixed: " + scoreFixed.ToString() + "/4";
        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        gameOver = false;
        winGame = false;

        level = 1;
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
            if (currentAmmo > 0)
            {
                ChangeAmmo(-1);
                AmmoText();
            }
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
                if (scoreFixed >= 4)
                {
                    SceneManager.LoadScene("Level 2");
                    level = 2;
                }
                else
                {
                    character.DisplayDialog(); 
                }
            }
        }
        // restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
            }
            if (winGame == true)
            {
                SceneManager.LoadScene("Level 1");
                level = 1;
            }
        }
        
    }
    // Scoring n Teleport
    public void FixedRobots(int amount)
    {
        // Win Text Appears
        scoreFixed += amount;

        fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";

        Debug.Log("Fixed Robots: " + scoreFixed);

        if (scoreFixed == 4 && level == 1)
        {   
            //Mini sound Win Plays
            PlaySound(winSound);

        }

        if (scoreFixed >=4 && level == 2)
        {
            WinTextObject.SetActive(true);

            winGame = true;

            speed = 0;

            Destroy(gameObject.GetComponent<SpriteRenderer>());
            PlaySound(victory);
        }

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
            DmgEffect.Play();

            PlaySound(hitSound);
        }
        if (amount > 0)
        {
            HealEffect.Play();        
        }
        
        
        if (health == 1)
        {
            LoseTextObject.SetActive(true);
            
            transform.position = new Vector3(.08f, 1.03f, -100f);
            speed = 0;
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            

            gameOver = true;
           
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    // Projectile & Ammo
   
    void Launch()
    {
        if (currentAmmo >0)
        {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
        }
    }
    public void AmmoText()
    {
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }
    public void ChangeAmmo(int amount)
    {
        currentAmmo = Mathf.Abs(currentAmmo + amount);
        Debug.Log("Ammo: " + currentAmmo);
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
