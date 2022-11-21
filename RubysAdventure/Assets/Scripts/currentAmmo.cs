using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class currentAmmo : MonoBehaviour
{
    public AudioClip collectedClip;   

    // Start is called before the first frame update
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            //if (controller.ammo <= controller.currentAmmo)
            {
                controller.ChangeAmmo(4); 
                controller.AmmoText(); 
                Destroy(gameObject);

                controller.PlaySound(collectedClip);
            }
        }
    }
}