using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_work_script : MonoBehaviour
{
    public GameObject[] mini_witch = new GameObject[8];//마녀병사

    public GameObject clock_lever;//시계레버
    clock_Lever clock_lever_script;//시계레버 스크립트

    public GameObject elevator;//엘리베이터
    public GameObject elevator_tag;//엘리베이터태그(컬라이더)
    //엘리베이터이동
    public bool arrive_x = false;
    public bool arrive_z = false;
    float arrive_x_point = 45.82f;
    float arrive_z_point = 28.25f;
    public GameObject pointer;

    //기어
    public GameObject[] gear = new GameObject[10];

    //킬포인트
    public int kill = 0;
    public float enable_timer = 0.0f;

    //애니메이션플래그
    bool stop = false;
    public  bool summon = true;//마녀병사소환
    public bool summon2 = true;//마녀병사소환
    //소환시작
    public bool summon_start = false;

    //헨젤
    public GameObject Hansel;//핸젤

    // Start is called before the first frame update
    void Start()
    {
        clock_lever_script = this.clock_lever.GetComponent<clock_Lever>();//시계 레버 스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (this.clock_lever_script.on == true)
        {
            if (summon_start == true)
            {
                summon = true;
                summon2 = true;

                //플래그초기화
                summon_start = false;
            }
            if (summon == true)//소환가능
            {
                mini_witch[Random.Range(0, 7)].SetActive(true);//소환
                summon = false;//소환불가
                StartCoroutine("summon_cooltime");//쿨타임
                Debug.Log("소환1");
            }
            if (summon2 == true)//소환가능
            {
                mini_witch[Random.Range(0, 7)].SetActive(true);//소환
                summon2 = false;//소환불가
                StartCoroutine("summon_cooltime2");//쿨타임
                Debug.Log("소환2");
            }
        }
        else {
            stop = false;
        }
        
        //작동
        if (this.kill >= enable_timer)
        {
             this.elevator.GetComponent<Animator>().enabled = true;
            //화살표표시
            this.pointer.SetActive(true);
            //기어회전
            for (int i = 0; i <= 13; i++) //톱니바퀴회전
            {
                this.gear[i].transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);
            }

            this.enable_timer += Time.deltaTime;

            //엘리베이터이동
            if (this.arrive_x == false && this.arrive_z == false)
            {
                this.elevator.transform.Translate(Vector3.back * 2.0f * Time.deltaTime);
                if (this.elevator.transform.position.x > arrive_x_point)
                {
                    this.arrive_x = true;
                }
                //레버작동가능 - 게임오버이후 플래그초기화
                this.clock_lever_script.enable = true;

            }
            else if (this.arrive_x == true && this.arrive_z == false)
            {
                this.elevator.transform.Translate(Vector3.right * 2.0f * Time.deltaTime);
                if (this.elevator.transform.eulerAngles.y < 0)
                {
                    this.elevator.transform.Rotate(0, 30f * Time.deltaTime, 0);
                }
                if (this.elevator.transform.position.z > arrive_z_point)
                {
                    this.arrive_z = true;
                }

            }
            else if(this.arrive_x = true && this.arrive_z == true && this.clock_lever_script.enable == true)//도착완료
            {
                this.elevator.transform.position = new Vector3(this.arrive_x_point,0.712f,this.arrive_z_point);
                this.elevator.transform.localRotation = Quaternion.Euler(0, 0, 0);
                this.elevator_tag.tag = "Clock_E";
                Hansel.GetComponent<CharacterController>().enabled = true;//캐릭터컨트롤러on
                //더이상레버작동X
                this.clock_lever_script.enable = false;          
            }
        }
        else
        {
            this.elevator.GetComponent<Animator>().enabled = false;
            //화살표숨기기
            this.pointer.SetActive(false);
        }

    }


    IEnumerator summon_cooltime()//소환쿨타임
    {
        yield return new WaitForSeconds(Random.Range(2.0f,6.0f));
        summon = true;//소환가능
    }
    IEnumerator summon_cooltime2()//소환쿨타임
    {
        yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
        summon2 = true;//소환가능
    }

}
