using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;//navmaesh사용

public class Hansel_Script : MonoBehaviour
{
    CharacterController Hansel_cc;
    Animator Hansel_animator;
    NavMeshAgent agent;

    //그레텔 스크립트
    main_player_1 Gretel_Script;

    //과자스크립트
    snack_item snack_item_script;

    public GameObject clock_lever;//시계레버
    clock_Lever clock_lever_script;//시계레버 스크립트

    //엘리베이터
    public GameObject elevator;
    public bool in_elevator = false;

    //게임매니저
    public GameObject management;
    game_management game_Management;

    public GameObject clock_work;//시계태엽
    clock_work_script clock_work_script;//시계태엽 스크립트

    //애니메이션 플래그
    bool normal;
    bool walk;
    bool run;
    bool h_g_normal;
    bool h_g_walk;
    bool hungry;
    bool power_up;
    public bool power_up_fine;
    bool rush;
    public bool chaos;//cage_Script와공유

    //레이캐스트
    RaycastHit hit;

    //좌표
    public bool start = false;
    public bool transform_start = false;
    float x_horizontal = 0.0f;
    float z_vertical = 0.0f;
    public bool x_arrive = false;
    public bool z_arrive = false;
    float rotate_speed = 320.0f;//회전속도
    float move_speed = 5f;//이동속도
    public bool V_Start = false;

    //솔로모드
    public bool solo = false;
    public bool hold_hands_X = false;

    //과자리스트 UI
    public GameObject snack_list;
    public Sprite []snack_image = new Sprite[6];
    public int snack_point = 0;
    public int snack_point_before = 0; 

    //과자 획득
    Vector3 snack_item_position;
    public bool snack_eat;//과자먹은직후 (게임매니지먼트 공유용)

    //파워업
    public Material hear;
    float emission_color_x = 0.0f;
    float emission_color_y = 0.0f;
    float emission_color_z = 0.0f;
    float rush_speed = 0.5f;
    public bool power_up_enable = false;
    public bool feild_change;//스테이지 이동용 (게임매니지먼트)

    //헨젤 자동이동
    public bool escape_current_location = false;

    //게임오버
    public bool gameover_be_caught2_hansel = false;//마녀에게 잡혔음(핸젤)

    //stage1-21
    public GameObject target_stage1_21;
    //stage1-22
    public GameObject target_stage1_22;


    public enum STEP //동작
    {
        NORMAL, //0 기본
        WALK, //1 걷기
        RUN, //2 뛰기
        H_G_NORMAL, //3 H_G기본
        H_G_WALK, //4 H_G뛰기 
        HUNGRY,//5 배고픔,아이템획득시
        POWERUP,//6파워업
        RUSH,//7돌진
        NUM //8 동작수
    };
    public STEP step = STEP.NORMAL;//현재상태
    public STEP next_step;//다음상태

    void Start()
    {
        Hansel_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
        Hansel_animator = this.GetComponent<Animator>();//애니메이션 컨트롤러
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        clock_work_script = clock_work.GetComponent<clock_work_script>();
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        clock_lever_script = this.clock_lever.GetComponent<clock_Lever>();//시계 레버 스크립트
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        this.hear.SetColor("_EmissionColor", new Color(emission_color_x, emission_color_y, emission_color_z));
        Debug.Log(agent.destination);

        Debug.DrawRay(this.transform.position + this.transform.forward * 0.03f, -this.transform.up * 20f, Color.red);
        //레이캐스트
        int layerMask = 1 << 12;
        layerMask = ~layerMask; //레이어 12을 제외한 모든레이어에 충돌
        if (Physics.Raycast(this.transform.position + this.transform.forward * 0.03f, -this.transform.up, out hit, 20f,layerMask))
        {
            //레이캐스트를 이용한 메인카메라 시점변경
            if ((hit.collider.gameObject.tag == "elevator" && this.clock_lever_script.on == true)
                || (hit.collider.gameObject.tag == "elevator" && this.clock_work_script.kill >= 1))
            {

                if (this.step == STEP.H_G_NORMAL || this.step == STEP.H_G_WALK)
                {
                    this.in_elevator = false;
                }
                else
                {
                    this.Hansel_cc.enabled = false;//캐릭터컨트롤러off
                    this.in_elevator = true;
                    this.transform.parent = this.elevator.gameObject.transform;
                }
            }
            else if(in_elevator == true)
            {
                this.in_elevator = false;
                this.transform.parent = null;
                this.Hansel_cc.enabled = true;//캐릭터컨트롤러on
            }

            //헨젤이 특정 위치에있을경우 게임진행불가 오류를 수정하기위해 특정위치에서 자동으로 탈출시켜줌
            if (hit.collider.gameObject.tag == "clock_closet" || (hit.collider.gameObject.tag == "clock_Lever"&& this.clock_work_script.kill < 18f))
            {
                if (escape_current_location == false)//한번만실행
                {
                    this.GetComponent<CharacterController>().enabled = false;
                    this.GetComponent<NavMeshAgent>().enabled = true;

                    this.transform.parent = null;
                    this.solo = true;
                    this.hold_hands_X = true;
                }
                escape_current_location = true;
            }
            else if(escape_current_location == true)//한번만실행
            {
                this.GetComponent<CharacterController>().enabled = true;
                this.GetComponent<NavMeshAgent>().enabled = false;
                this.next_step = STEP.NORMAL;
                this.step = STEP.NORMAL;

                this.solo = false;
                this.hold_hands_X = false;
                escape_current_location = false;
            }
        }

        //핸젤특정위치탈출
        if (escape_current_location == true)
        {
            this.agent.destination = this.target_stage1_21.transform.position;
            this.next_step = STEP.WALK;
            this.step = STEP.WALK;
        }

            switch (this.step)//캐릭터 상태별 동작
        {
            case STEP.NORMAL:
               

                //애니메이션 플래그
                this.h_g_normal = false;
                this.h_g_walk = false;
                normal = true;
                run = false;
                walk = false;
                hungry = false;
                power_up = false;

                //항상 땅에고정
                if (this.Hansel_cc.enabled == true)
                {
                    Hansel_cc.Move(Vector3.down * 10.0f * Time.deltaTime);
                }

                break;
            case STEP.WALK:
                if (Hansel_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Hansel_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[Hansel]hungry"))
                    {
                        if (this.game_Management.stage_num_1_22 == true)//이동시작
                        {
                            this.GetComponent<NavMeshAgent>().enabled = true;
                        }
                    }
                }

                //애니메이션 플래그
                this.h_g_normal = false;
                this.h_g_walk = false;
                normal = false;
                run = false;
                walk = true;
                hungry = false;
                power_up = false;

                break;
            case STEP.RUN:
                break;

            case STEP.H_G_NORMAL:
                this.h_g_normal = true;
                this.h_g_walk = false;
                normal = false;
                run = false;
                walk = false;
                hungry = false;
                power_up = false;

                //항상 땅에고정
                if (Hansel_cc.enabled == true)
                {
                    Hansel_cc.Move(Vector3.down * 10.0f * Time.deltaTime);
                }
                break;
            case STEP.H_G_WALK:
                this.h_g_normal = false;
                this.h_g_walk = true;
                normal = false;
                run = false;
                walk = false;
                hungry = false;
                power_up = false;

                break;

            case STEP.HUNGRY:
                //이동불가
                this.GetComponent<NavMeshAgent>().enabled = false;
                if (Hansel_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Hansel_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[Hansel]hungry"))
                    {
                        this.next_step = STEP.NORMAL;
                        if (this.game_Management.stage_num_1_13 == false)
                        {
                            this.solo = false;
                        }
                        
                    }
                }

                //항상 땅에고정
                Hansel_cc.Move(Vector3.down * 10.0f * Time.deltaTime);
                //애니메이션 플래그
                this.h_g_normal = false;
                this.h_g_walk = false;
                normal = false;
                run = false;
                walk = false;
                hungry = true;
                power_up = false;

                break;
            case STEP.POWERUP:             

                //머리색변경
                emission_color_x += 0.1f * Time.deltaTime;
                emission_color_y += 0.1f * Time.deltaTime;
                emission_color_z += 0.1f * Time.deltaTime;
                this.hear.SetColor("_EmissionColor", new Color(emission_color_x, emission_color_y, emission_color_z));

                //애니메이션 플래그
                this.h_g_normal = false;
                this.h_g_walk = false;
                normal = false;
                run = false;
                walk = false;
                hungry = false;
                power_up = true;

                //항상 땅에고정
                Hansel_cc.Move(Vector3.down * 10.0f * Time.deltaTime);

                break;
            case STEP.RUSH:
                Hansel_cc.Move(-Vector3.left * rush_speed * Time.deltaTime);//이동처리
                rush_speed += 3.0f * Time.deltaTime;

                if (Hansel_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Hansel_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[Hansel]rush"))
                    {
                        //헨젤기본상태로 초기화
                        this.next_step = STEP.NORMAL;
                        this.solo = false;
                        this.hold_hands_X = false;
                        //머리색 원래대로
                        this.hear.SetColor("_EmissionColor", new Color(0, 0, 0));
                        emission_color_x = 0f;
                        emission_color_y = 0f;
                        emission_color_z = 0f;
                        rush_speed = 0.5f;

                        //1-14로이동
                        this.feild_change = true;
                    }
                }

                //애니메이션 플래그
                rush = true;
                this.h_g_normal = false;
                this.h_g_walk = false;
                normal = false;
                run = false;
                walk = false;
                hungry = false;
                power_up = false;
                break;

        }//end of switch 


        

        //스테이지 1_13좌표이동
        if (power_up_enable == true && this.game_Management.stage_num_1_13 == true)
        {
            this.next_step = STEP.POWERUP;
            //문으로이동코루틴
            StartCoroutine("destination_door");
            power_up_enable = false;//한번만실행
        }

        if (solo == true)
        {
            if (this.step == STEP.WALK && V_Start == true)
            {
                start = true;
                transform_start = true;
                V_Start = false;
            }
        }
        else
        {
            //H&G
            if (this.Gretel_Script.h_g_normal == true)
            {
                this.transform.parent = Gretel_Script.gameObject.transform;
                this.next_step = STEP.H_G_NORMAL;
            }
            if (this.Gretel_Script.h_g_walk == true)
            {
                this.transform.parent = Gretel_Script.gameObject.transform;
                this.next_step = STEP.H_G_WALK;
            }
            if ((this.Gretel_Script.normal == true || this.Gretel_Script.run == true || this.Gretel_Script.walk == true)
                && this.in_elevator == false)
            {
                this.transform.parent = null;
                this.next_step = STEP.NORMAL;
            }
        }


        //좌표까지 이동법
        if (start == true)//좌표이동시작
        {
            if (transform_start == true)//이동시작 ,한번만 실행
            {

                if (this.transform.position.x > snack_item_position.x)
                {
                    x_horizontal = -1;
                }
                else
                {
                    x_horizontal = 1;
                }

                if (this.transform.position.z > snack_item_position.z)
                {
                    z_vertical = -1;
                }
                else
                {
                    z_vertical = 1;
                }


                transform_start = false;

            }//end of if(한번만실행)

            //x축도착시
            if (x_horizontal == -1)
            {
                if (this.transform.position.x < snack_item_position.x)
                {
                    x_arrive = true;
                }
            }
            else if (x_horizontal == 1)
            {
                if (this.transform.position.x > snack_item_position.x)
                {
                    x_arrive = true;
                }
            }

            //z축도착시
            if (z_vertical == -1)
            {
                if (this.transform.position.z < snack_item_position.z)
                {
                    z_arrive = true;
                }
            }
            else if (z_vertical == 1)
            {
                if (this.transform.position.z > snack_item_position.z)
                {
                    z_arrive = true;
                }
            }


            //도착시이동X
            if (x_arrive == true)
            {
                x_horizontal = 0;
            }
            if (z_arrive == true)
            {
                z_vertical = 0;
            }



            //도착완료
            if (x_arrive == true && z_arrive == true)
            {
                //초기화
                x_arrive = false;
                z_arrive = false;
                start = false;

                //그레텔 과 헨젤 다시 충돌
                Gretel_Script.gameObject.layer = 11;


                if (this.game_Management.stage_num_1_13 == true && this.power_up_fine == true)
                {
                    this.next_step = STEP.RUSH;
                }
               
            }


            Vector3 direcction = new Vector3(x_horizontal, 0, z_vertical); //이동방향
            if (game_Management.stage_num_1_8 == true && z_arrive == false)//1-8스테이지는 z축먼저 이동후 x축이동
            {
                direcction = new Vector3(0, 0, z_vertical); //이동방향
            }

            if (direcction.sqrMagnitude > 0.01f)//이동상태
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                      transform.forward, direcction,//현재방향,바라볼방향
                      rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                      );

                    transform.LookAt(transform.position + forward);//회전처리

            }
            //Hansel_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * 0.025f);//이동처리

            if (this.game_Management.stage_num_1_13 == true && this.step != STEP.RUSH) 
            {
                if (Vector3.Distance(this.transform.position, Gretel_Script.transform.position) < 7.5f)
                {
                    Hansel_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * 0.025f);//이동처리
                }
            }
            else
            {
                Hansel_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * 0.025f);//이동처리
            }

        }//end of start 좌표이동

        //nev_mesh 좌표이동
        if (this.game_Management.hansel_move_1_21 == true )
        {
            this.solo = true;
            this.hold_hands_X = true;
            this.agent.destination = this.target_stage1_21.transform.position;
            this.next_step = STEP.WALK;
            if (Vector3.Distance(this.transform.position, this.target_stage1_21.transform.position) < 3.0f)
            {
                //이동종료
                this.game_Management.hansel_move_1_21 = false;
                this.solo = false;
                this.hold_hands_X = false;
                this.step = STEP.NORMAL;
            }
        }

        if (this.game_Management.hansel_move_1_22 == true)
        {
            this.solo = true;
            this.hold_hands_X = true;
            this.agent.destination = this.target_stage1_22.transform.position;
            this.next_step = STEP.WALK;
            if (Vector3.Distance(this.transform.position, this.target_stage1_22.transform.position) < 3.0f)
            {
                //이동종료
                this.game_Management.hansel_move_1_22 = false;
                this.solo = false;
                this.hold_hands_X = false;
                this.step = STEP.NORMAL;
            }
        }

        //게임오버 - 마녀에게 붙잡힘 판정2 스테이지1-2 핸젤
        if (this.gameover_be_caught2_hansel == true)
        {
            //핸젤 위치고정
            this.transform.position = new Vector3(this.Gretel_Script.target_position.transform.position.x,
            this.Gretel_Script.target_position.transform.position.y - 0.2f, this.Gretel_Script.target_position.transform.position.z);//핸젤위치고정
            this.transform.localEulerAngles = new Vector3(0, Gretel_Script.target_position.transform.root.localEulerAngles.y - 180f, 0);//회전값고정 
            
        }

        //과자리스트관리
        this.snack_list.GetComponent<Image>().sprite = snack_image[snack_point];
        this.snack_list.transform.localRotation = Quaternion.Euler(0,0,0);


        //애니메이션 관리
        Hansel_animator.SetBool("WALK", walk);
        Hansel_animator.SetBool("RUN", run);
        Hansel_animator.SetBool("NORMAL", normal);
        Hansel_animator.SetBool("H_G_NORMAL", h_g_normal);
        Hansel_animator.SetBool("H_G_WALK", h_g_walk);
        Hansel_animator.SetBool("HUNGRY",hungry);
        Hansel_animator.SetBool("POWERUP", power_up);
        Hansel_animator.SetBool("POWERUP_FINE", power_up_fine);
        Hansel_animator.SetBool("RUSH", rush);
        Hansel_animator.SetBool("CHAOS", chaos);

        //동작관리
        this.step = this.next_step;
    }//end of Update

    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "snack_item")
            {
                this.hold_hands_X = true;//헨젤과 그레텔 모드 불가
                this.transform.parent = null;
                this.next_step = STEP.WALK;
                solo = true;
                snack_item_position = other.transform.position;
                V_Start = true;
                snack_item_script = GameObject.FindGameObjectWithTag("snack_item").GetComponent<snack_item>();//과자스크립트

               //그레텔 과 헨젤충돌X
               Gretel_Script.gameObject.layer = 10;

                //컬라이더 크기변경
                this.GetComponent<BoxCollider>().center = new Vector3(-0.206f,3.280f,-0.545f);
                this.GetComponent<BoxCollider>().size = new Vector3(5.257f, 11.247f, 5.358f);
            
            }


        //핸젤트랩에갇힘
        if (other.tag == "Enemy_Trap")
        {
            this.hold_hands_X = true;//헨젤과 그레텔 모드 불가
            this.transform.parent = null;
            this.next_step = STEP.NORMAL;
            solo = true;

            other.transform.parent.GetComponent<cage_Script>().hold = true;//새장사라지지않고 유지
            other.transform.parent.GetComponent<cage_Script>().rock1.SetActive(true);//자물쇠등장
            other.transform.parent.GetComponent<cage_Script>().rock2.SetActive(true);//자물쇠등장
            other.transform.GetComponent<BoxCollider>().enabled = false;//새장컬라이더해제

            BoxCollider[] c = other.transform.GetChild(3).GetComponents<BoxCollider>();
            c[0].enabled = false;
            c[1].enabled = false;
            c[2].enabled = false;
            c[3].enabled = false;

            other.transform.parent.GetComponent<BoxCollider>().enabled = true;


            this.GetComponent<CharacterController>().enabled = false;
            this.transform.position = new Vector3(other.transform.position.x,0.172f,other.transform.position.z);//헨젤위치고정
            this.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정
            this.GetComponent<CharacterController>().enabled = true;

            this.GetComponent<BoxCollider>().enabled = false;//컬라이더해제

            snack_point_before = snack_point;
            snack_point = 4;

            this.chaos = true;//애니메이션플래그
            Debug.Log("헨젤갇힘");
        }

        //데드씬 - 마녀에게잡힘
        if (other.tag == "witch_hand")
        {
            //game-over (마녀에게 잡힘)
            this.solo = true;
            this.hold_hands_X = true;
            this.gameover_be_caught2_hansel = true;

            //컬라이더 off
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator destination_door()
    {
        yield return new WaitForSeconds(1.5f);
        snack_point++;
        yield return new WaitForSeconds(3.5f);
        power_up_fine = true;

        yield return new WaitForSeconds(1.0f);//게이지충전시간 4초동안 POWERUP모드

        //문으로 이동
        this.next_step = STEP.WALK;
        snack_item_position = new Vector3(16.667f, 0.147f, -1.272f);
        V_Start = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "snack_item")
        {
            this.next_step = STEP.HUNGRY;
            this.step = STEP.HUNGRY;

            this.snack_eat = true;//과자먹은직후
            this.snack_item_script.remove_snack_item = true;//게임매니지먼트 공유
            //snack_item_script.remove_snack_item = true;
            snack_point++;//스낵포인트증가

            if (this.game_Management.stage_num_1_12 == true)
            {
                power_up_enable = true;
            }
        }

    }
}
