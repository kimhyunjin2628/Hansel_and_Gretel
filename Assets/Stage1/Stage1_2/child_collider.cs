using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class child_collider : MonoBehaviour
{
    mini_witch_script mini_witch_script;
    public GameObject clock_work;
    clock_work_script clock_work_script;

    //헨젤
    public GameObject Hansel;//핸젤
    Hansel_Script hansel_script;//핸젤 스크립트

    // Start is called before the first frame update
    void Start()
    {
        mini_witch_script = this.transform.parent.GetComponent<mini_witch_script>();
        clock_work_script = clock_work.GetComponent<clock_work_script>();
        hansel_script = Hansel.GetComponent<Hansel_Script>();//핸젤스크립트
    }

    //피격
    private void OnTriggerEnter(Collider other)
    {
        this.mini_witch_script.death = true;
        this.mini_witch_script.remove = true;
        this.GetComponent<BoxCollider>().enabled = false;

        if (this.hansel_script.in_elevator == true)
        {
            this.clock_work_script.kill += 2;
        }
    }
}
