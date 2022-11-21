using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;
    bool broken = true;
    private RubyController rubyController;

    //Animation
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        
        GameObject rubyControllerObject = GameObject.FindWithTag("Player");
        rubyController = rubyControllerObject.GetComponent<RubyController>();
    }

    void Update()
    {
         if(!broken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }
    
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
        
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        
        if (rubyController != null)
        {
            rubyController.FixedRobots(1);
        }
    }
}
