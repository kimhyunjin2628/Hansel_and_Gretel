using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mini_witch_script : MonoBehaviour
{
    Animator mini_witch_animator;//마녀병사 애니메이터
    public GameObject coll;//충돌영역
    public GameObject explosion_particle;
    public GameObject clock_ring_effect;

    //삭제
    public bool remove = false;
    //애니메이션플래그
    bool start;
    bool finish;
    bool normal;
    public bool death;

    public  float random_time;//랜덤시간
    public float timer = 0.0f;//지속시간
    // Start is called before the first frame update
    void Start()
    {
        mini_witch_animator = this.GetComponent<Animator>();
        random_time = Random.Range(2.5f,4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //충돌
        if (this.death == true && remove == true)
        {
            //StartCoroutine("remove_delay1");
            StartCoroutine("remove_delay2");
            explosion_particle.SetActive(true);//파티클온
            StartCoroutine("Ring_effect");
            remove = false;
        }
        
        if(this.death == false)//일반
        {
            if (timer <= random_time)
            {
                timer += Time.deltaTime;
            }
            else
            {
                finish = true;
                remove = true;


                //컬라이더초기화
                this.coll.GetComponent<BoxCollider>().enabled = false;
            }


            if (finish == true && remove == true) //삭제플래그시
            {
                StartCoroutine("remove_delay2");
                remove = false;
            }
        }

        //애니메이션관리
        mini_witch_animator.SetBool("FINISH", finish);
        mini_witch_animator.SetBool("DEATH", death);
    }
   /* IEnumerator remove_delay1()//삭제딜레이
    {
        yield return new WaitForSeconds(0.3f);
       
        

    }*/
    IEnumerator remove_delay2()//삭제딜레이
    {
        yield return new WaitForSeconds(1.0f);
        //플래그초기화
        timer = 0.0f;
        normal = false;
        finish = false;
        death = false;
        start = false;

        //컬라이더초기화
        this.coll.GetComponent<BoxCollider>().enabled = true;

        this.explosion_particle.SetActive(false);
        this.gameObject.SetActive(false);
    }
    IEnumerator Ring_effect()//이팩트생성
    {
        yield return new WaitForSeconds(0.4f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(61.83f, -21.82f, 8.6f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(40.964f, -74.255f, -65.453f);
        clock_ring_effect.transform.localScale = new Vector3(6.877721f, 6.877721f, 6.877721f);
    }

}
