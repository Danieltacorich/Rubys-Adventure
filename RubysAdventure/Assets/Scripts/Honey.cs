using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Honey : MonoBehaviour
{
    public AudioClip collectedClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

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
