using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyes_chase : MonoBehaviour
{
    Animator chaser_animator;
    public GameObject chase_position;
    public bool close;
    public float timer = 0.0f;
    public bool child_enable = false;

    //UI
    public GameObject chase_eyes_warning;//감시의눈 경고이미지

    //애니메이션 플래그
    bool remove;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = chase_position.transform.root;
        chaser_animator = this.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = chase_position.transform.position;
        if (close == false && child_enable == true)
        {
            if (timer > 6.5f)
            {
                close = true;
                remove = true;
                StartCoroutine("remove_chaser");

                //애니메이션
                chaser_animator.SetBool("REMOVE", remove);
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

    }

    IEnumerator remove_chaser()//새장밑으로
    {
        yield return new WaitForSeconds(1.0f);
        close = false;
        remove = false;
        timer = 0.0f;


        // 애니메이션
        chaser_animator.SetBool("REMOVE", remove);

        //UI
        this.chase_eyes_warning.SetActive(false);

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.child_enable = false;
    }
}
