using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gretel_position : MonoBehaviour
{
    public GameObject Hansel;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localEulerAngles = Hansel.transform.localEulerAngles;
    }
}
