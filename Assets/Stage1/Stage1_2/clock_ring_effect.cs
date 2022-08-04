using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_ring_effect : MonoBehaviour
{
    public GameObject ring_effect;
    Animator ring_animator;
    // Start is called before the first frame update
    void Start()
    {
        ring_animator = ring_effect.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ring_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
        {
            if (ring_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[clock_ring_effect]normal"))
            {
                Destroy(this.gameObject);
                // this.transform.localRotation = Quaternion.Euler(0, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z);//
            }
        }
    }
}
