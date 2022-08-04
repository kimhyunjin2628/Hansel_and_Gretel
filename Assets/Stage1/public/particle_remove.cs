using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_remove : MonoBehaviour
{
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 8.0f)
        {
            this.gameObject.SetActive(false);
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
