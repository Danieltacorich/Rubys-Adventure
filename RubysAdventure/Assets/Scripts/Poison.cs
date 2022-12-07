using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
     public ParticleSystem PoisonEffect;
     public AudioClip PoisonClip;
        void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
            Destroy(gameObject);
            controller.PlaySound(PoisonClip);
        }
    }
}
