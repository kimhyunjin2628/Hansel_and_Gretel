using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_Lever : MonoBehaviour
{
    Animator clock_lever_animator;//라벨애니메이터

    public GameObject soul;//영혼

    //레버작동가능여부
    public bool enable = true;//작동가능
    
    //애니메이션 플래그
    public bool on = false;//레버 on 
    public bool off = false;//레버 off 

    bool on_one = false;//레버온 한번만실행
    bool off_one = false;//레버off 한번만실행

    public GameObject clock_work;//시계태엽
    clock_work_script clock_work_script;//시계태엽 스크립트

    public GameObject Lever;//레버
    public GameObject gretel;//그레텔
    main_player_1 Gretel_Script;//그레텔스크립트


    // Start is called before the first frame update
    void Start()
    {
        clock_lever_animator = this.GetComponent<Animator>();
        Gretel_Script = gretel.GetComponent<main_player_1>();//그레텔스크립트
        clock_work_script = clock_work.GetComponent<clock_work_script>();//시계태엽스크립트
    }

    // Update is called once per frame
    void Update()
    {

        //ON OFF
        if (Vector3.Distance(this.transform.position, gretel.transform.position) < 2.0f && this.enable == true)
        {
            this.on = true;
            this.off = false;
            this.Lever.GetComponent<MeshRenderer>().materials[0].color = Color.red;

        }
        else
        {
            this.off = true;
            this.on = false;
            this.Lever.GetComponent<MeshRenderer>().materials[0].color = Color.green;

        }

        //레버 해제
        if (Input.GetKey(KeyCode.Escape))
        {
            if (this.on == true)
            {
                this.Gretel_Script.enabled = false;
                this.gretel.transform.position = new Vector3(42.7f, 0.722f, 31.16f);
                this.Gretel_Script.enabled = true;
                Debug.Log("탈출");
            }
            
        }


        //레버온오프시 한번만실행
        if (this.on == true && this.on_one == false)
        {
            //플레이어행동중지
            this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;
            this.gretel.transform.position = new Vector3(45.97f, 0.722f, 31.16f);
            this.gretel.transform.localRotation = Quaternion.Euler(0, 90, 0);
           

            //소울모드변경
            this.soul.SetActive(true);
            this.soul.transform.position = new Vector3(56.46f,7.12f,32.61f);
            this.soul.transform.localRotation = Quaternion.Euler(0, 0, 0);


            //플래그초기화
            this.clock_work_script.summon_start = true;
            this.on_one = true;
            this.off_one = false;
        }
        if (this.off == true && this.off_one == false)
        {
            //플레이어행동
            this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
            this.Gretel_Script.step = main_player_1.STEP.NORMAL;

            //소울모드변경
            this.soul.transform.position = new Vector3(56.46f, 7.12f, 32.61f);
            this.soul.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.soul.SetActive(false);       

            //플래그초기화
            this.on_one = false;
            this.off_one = true;
        }

        //애니메이션관리
        clock_lever_animator.SetBool("ON",on);
        clock_lever_animator.SetBool("OFF",off);
    }
}
