using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyes_pattern : MonoBehaviour
{
    //애니메이터
    Animator Witch_animator;

    //애니메이션 플래그
    public bool normal;
    public bool attack;
    public bool attack_1_9;
    public bool searching;
    public bool found;
    //게임오버 애니메이션 플래그
    public bool catch_;
    

    public GameObject root_eye_effect;
    public GameObject[] eyes_effect = new GameObject[30];
    public GameObject eyes_chaser;

    //게임매니저
    public GameObject management;
    game_management game_Management;
    //메인플레이어
    GameObject Gretel;
    main_player_1 Gretel_script;

    //공격종료
    public bool attack_fin_1_10 = false;
    public bool attack_fin = false;

    //UI
    public GameObject chase_eyes_warning;//감시의눈 경고이미지

    public enum STEP //동작
    {
        NORMAL, //0 기본
        ATTACK, //1 공격
        ATTACK_stage1_9,//2애니메이션용
        SEARCHING, //3그레텔수색
        CATCH, //4그레텔잡음
        NUM //동작수
    };
    public STEP step = STEP.NORMAL;//현재상태
    public STEP next_step;//다음상태

    //패턴종류
    int pattern_num;

    //그레텔수색패턴
    public float searching_delay = 0.0f;
    public float searching_delay_timer = 0.0f;
    public float found_delay_timer = 0.0f;
    public bool found_enable = false;

    // Start is called before the first frame update
    void Start()
    {
        Witch_animator = this.GetComponent<Animator>();//애니메이션 컨트롤러
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        Gretel = GameObject.FindGameObjectWithTag("Player");
        Gretel_script = Gretel.GetComponent<main_player_1>();//그레텔스크립트
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(this.transform.position, this.Gretel.transform.position));
        //동작
        switch (step)
        {
            case STEP.NORMAL:

                //애니메이션 플래그
                this.normal = true;
                this.attack = false;
                this.attack_1_9 = false;
                this.searching = false;
                this.found = false;
                this.catch_ = false;
                //1-10
                if (game_Management.stage_num_1_10 == true && Gretel.transform.position.y <= 4.5f)//공격재실행
                {
                    attack_fin = false;
                }
                //가까이 접근시 패턴변경
                if (Gretel.transform.position.x > 3.5f && Gretel.transform.position.y < 4.8 && Gretel.transform.position.z > 0f)
                {
                    //수색딜레이
                    if (searching_delay < searching_delay_timer)
                    {
                        searching = true;
                        this.next_step = STEP.SEARCHING;
                        this.step = STEP.SEARCHING;

                        //감시의눈 매쉬형태 보이게
                        this.eyes_chaser.transform.GetChild(0).gameObject.SetActive(true);
                        this.eyes_chaser.GetComponent<eyes_chase>().child_enable = true;

                        //UI-경고
                        this.chase_eyes_warning.SetActive(true);

                        searching_delay = Random.Range(4.5f, 22.0f);//딜레이 랜덤 3~16초
                        searching_delay_timer = 0.0f;
                    }
                    else
                    {
                        if (Vector3.Distance(this.transform.position, this.Gretel.transform.position) < 11.5f && this.Gretel.transform.position.y < 7.0f)
                        {
                            searching_delay_timer += Time.deltaTime;
                        }
                    }
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, this.Gretel.transform.position) < 11.5f && this.Gretel.transform.position.y < 7.0f)
                    {
                        searching_delay_timer += Time.deltaTime;
                    }
                }

                break;
            case STEP.ATTACK:
                //수색딜레이
                if (Vector3.Distance(this.transform.position, this.Gretel.transform.position) < 11.5f && this.Gretel.transform.position.y < 7.0f)
                {
                    searching_delay_timer += Time.deltaTime;
                }


                if (Witch_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Witch_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[witch]sit_attack1_finish"))
                    {
                       
                        pattern_num = Random.Range(1,3);
                        if (pattern_num == 1)
                        {
                            trap_down_1();
                        }
                        else if (pattern_num == 2)
                        {
                            trap_down_2();
                        }

                        if (Gretel.transform.position.y <= 4.5f)
                        {
                            attack_fin = false;//공격종료플래그 초기화, 바로 다음 공격
                        }

                        this.next_step = STEP.NORMAL;//상태변환
                    }
                }



                //애니메이션 플래그
                this.normal = false;
                this.attack = true;
                this.attack_1_9 = false;
                this.searching = false;
                this.found = false;

                break;
            case STEP.ATTACK_stage1_9:
                if (Witch_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Witch_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[witch]sit_attack1_finish"))
                    {
                        this.next_step = STEP.NORMAL;
                    }
                }

              

                //애니메이션 플래그
                this.normal = false;
                this.attack = false;
                this.attack_1_9 = true;
                this.searching = false;
                this.found = false;
                break;

            case STEP.SEARCHING :
                if (found_delay_timer > 3.5f)
                {
                    found_enable = true;
                }
                else
                {
                    found_delay_timer += Time.deltaTime;
                }

                if (found_enable == true)//수색중일때만
                {
                    if (this.Gretel.transform.position.y > 1.5f)
                    {
                        if (this.Gretel.transform.position.x > 6.3 && this.Gretel.transform.position.x < 12.1f
                       && this.Gretel.transform.position.z > -1f && this.Gretel.transform.position.z < 4.95f)
                        {
                            found = true;
                            Witch_animator.SetBool("FOUND", found);
                        }
                    }
                    else
                    {
                        if (this.Gretel.transform.position.z > -1f && this.Gretel.transform.position.z < 2.0f &&
                            this.Gretel.transform.position.x > 6.3f && this.Gretel.transform.position.x < 11.0f)
                        {
                            found = true;
                            Witch_animator.SetBool("FOUND", found);
                        }
                    }
                }


                if (Witch_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Witch_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[witch_stand]search"))
                    {
                        this.next_step = STEP.NORMAL;
                        found_enable = false;//수색모드off
                        found_delay_timer = 0.0f;
                    }
                }
                //catch모드
                if (found == true)
                {
                    StartCoroutine("Catch");
                    this.next_step = STEP.CATCH;

                    //딜레이초기화
                    this.found_delay_timer = 0.0f;
                    this.searching_delay_timer = 0.0f;
                }

                //애니메이션 플래그 + attack_fin
                this.attack_fin = true;
                this.normal = false;
                this.attack = false;
                this.attack_1_9 = false;
                this.searching = true;
                break;
            case STEP.CATCH:
                

                //애니메이션 플래그 
                //this.attack_fin = false;
                this.normal = false;
                this.attack = false;
                this.attack_1_9 = false;
                this.searching = false;
                break;
        }
        Witch_animator.SetBool("FOUND", found);

        //씬 1-9 핸젤가두기
        if (this.game_Management.stage_num_1_9 == true && attack_fin == false)
        {
            StartCoroutine("Attack_1_9");
            attack_fin = true;//공격실행은 한번만
        }

        //씬 1-10 공격시작
        if (this.game_Management.stage_num_1_10 == true && attack_fin_1_10 == false)
        {
            StartCoroutine("Attack");
            root_eye_effect.SetActive(true);
            attack_fin_1_10 = true;//공격실행은 한번만
        }
        //공격지속
        if (game_Management.stage_num == 10 && attack_fin == false)
        {
            StartCoroutine("Attack");
            attack_fin = true;
        }

        //애니메이션 관리
        Witch_animator.SetBool("NORMAL", normal);
        Witch_animator.SetBool("ATTACK", attack);
        Witch_animator.SetBool("ATTACK_1_9", attack_1_9);
        Witch_animator.SetBool("SEARCHING", searching);
        Witch_animator.SetBool("CATCH", catch_);


        //동작관리
        this.step = this.next_step;
    }

    IEnumerator trap_down()//새장밑으로
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i <= 20; i++)
        {
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false)//비활성화 상태일때만실행
            {
                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
        }
        yield return new WaitForSeconds(8.5f);
        for (int a = 0; a <= 25; a++)
        {
            if (eyes_effect[a].GetComponent<cage_Script>().enabled == false)//비활성화 상태일때만실행
            {
                eyes_effect[a].SetActive(true);
                eyes_effect[a].GetComponent<cage_Script>().enabled = true;
            }
        }
    }


    //패턴1 그레텔위치기준 x,z축1자로 생성
    void trap_down_1()
    {
        for (int i = 0; i <= 36; i++)
        {
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && (Player_Z_LV(Gretel.transform.position.z) == 
                Z_LV(eyes_effect[i].transform.localPosition.z)|| Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x)))//비활성화 상태일때만실행
            {
               
                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
        }
    }
    //패턴2 그레텔위치기준 z축1자로 생성
    void trap_down_2()
    {
        for (int i = 0; i <= 36; i++)
        {
            //플레이어기준 같은줄 3개
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
                Z_LV(eyes_effect[i].transform.localPosition.z) && Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x))//비활성화 상태일때만실행 , (x,z) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
                Z_LV(eyes_effect[i].transform.localPosition.z)-1 && Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x))//비활성화 상태일때만실행, (x,z+1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
               Z_LV(eyes_effect[i].transform.localPosition.z) + 1 && Player_X_LV(Gretel.transform.position.x) ==
               X_LV(eyes_effect[i].transform.localPosition.x))//비활성화 상태일때만실행 (x,z-1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            //플레이어기준 윗줄3개
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
                Z_LV(eyes_effect[i].transform.localPosition.z) && Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x) -1 )//비활성화 상태일때만실행 , (x+1,z) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
                Z_LV(eyes_effect[i].transform.localPosition.z) - 1 && Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x) -1 )//비활성화 상태일때만실행, (x+1,z-1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
               Z_LV(eyes_effect[i].transform.localPosition.z) - 1 && Player_X_LV(Gretel.transform.position.x) ==
               X_LV(eyes_effect[i].transform.localPosition.x) -1 )//비활성화 상태일때만실행 (x+1,z+1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            //플레이어기준 아랫줄3개
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
               Z_LV(eyes_effect[i].transform.localPosition.z) && Player_X_LV(Gretel.transform.position.x) ==
               X_LV(eyes_effect[i].transform.localPosition.x) + 1)//비활성화 상태일때만실행 , (x-1,z) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
                Z_LV(eyes_effect[i].transform.localPosition.z) + 1 && Player_X_LV(Gretel.transform.position.x) ==
                X_LV(eyes_effect[i].transform.localPosition.x) + 1)//비활성화 상태일때만실행, (x-1,z-1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
            if (eyes_effect[i].GetComponent<cage_Script>().enabled == false && Player_Z_LV(Gretel.transform.position.z) ==
               Z_LV(eyes_effect[i].transform.localPosition.z) - 1 && Player_X_LV(Gretel.transform.position.x) ==
               X_LV(eyes_effect[i].transform.localPosition.x) + 1)//비활성화 상태일때만실행 (x-1,z+1) = (p_x,p_z)
            {

                eyes_effect[i].SetActive(true);
                eyes_effect[i].GetComponent<cage_Script>().enabled = true;
            }
        }
    }

    IEnumerator Attack_1_9()//1-9스테이지 공격
    {
        yield return new WaitForSeconds(2.0f);
        this.next_step = STEP.ATTACK_stage1_9;
    }

    IEnumerator Attack()//1-10스테이지 공격
    {
        yield return new WaitForSeconds(1.0f);
        if (this.searching == false)
        {
            this.next_step = STEP.ATTACK;
        }
    }

    IEnumerator Catch()//잡힘
    {
        yield return new WaitForSeconds(1.0f);
        this.catch_ = true;
       

        //애니메이션 관리
        found = false;
        Witch_animator.SetBool("FOUND", found);
    }


    int X_LV(float x) //x레벨 1~8 왼쪽부터1
    {
        if (x > -7 && x < -6)
        {
            return 1;
        }
        else if (x > -4 && x < -3)
        {
            return 2;
        }
        else if (x > -1 && x < 0)
        {
            return 3;
        }
        else if (x > 3 && x < 4)
        {
            return 4;
        }
        else if (x > 6 && x < 7)
        {
            return 5;
        }
        else if (x > 9 && x < 10)
        {
            return 6;
        }
        else if (x > 12 && x < 13)
        {
            return 7;
        }
        else
        {
            return 8;
        }

    }
    int Z_LV(float z) //x레벨 0~4 아래쪽 부터0
    {
        if (z < -1)
        {
            return 0;
        }
        else if (z > -1 && z < 0)
        {
            return 1;
        }
        else if (z > 2 && z < 3)
        {
            return 2;
        }
        else if (z > 5 && z < 6)
        {
            return 3;
        }
        else
        {
            return 4;
        }
      

    }

    int Player_X_LV(float x) //플레이어용 x레벨 1~8 왼쪽부터1
    {
        if (x > 13f)
        {
            return 8;
        }
        else if (x > 9.65f)
        {
            return 7;
        }
        else if (x > 6.6f)
        {
            return 6;
        }
        else if (x > 3.35f)
        {
            return 5;
        }
        else if (x > 0.25f)
        {
            return 4;
        }
        else if (x > -3.1f)
        {
            return 3;
        }
         else if (x > -6.35f)
        {
            return 2;
        }
        else
        {
            return 1;
        }

    }
    int Player_Z_LV(float z) //플레이어용 x레벨 0~4 아래쪽 부터0
    {
        if (z > 2.1f)
        {
            return 4;
        }
        else if (z > -1.08f)
        {
            return 3;
        }
        else if (z > -4.13)
        {
            return 2;
        }
        else if (z > -7)
        {
            return 1;
        }
        else
        {
            return 0;
        }


    }
}
