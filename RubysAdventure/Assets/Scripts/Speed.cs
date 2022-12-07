using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public AudioClip collectedClip;
    public ParticleSystem HealEffect;

    // Start is called before the first frame update
    void Start()
    {
        HealEffect.Stop();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {  
           Destroy(gameObject);

           controller.PlaySound(collectedClip);
        }
    }
}
