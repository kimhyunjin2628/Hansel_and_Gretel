using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//navmaesh사용

public class witch_script : MonoBehaviour
{
   
    //게임매니저
    public GameObject management;
    game_management game_Management;

    //마녀
    public GameObject witch;

    //레이캐스트
    RaycastHit hit;

    public GameObject main_player;//그레텔
    main_player_1 Gretel_Script;//그레텔 스크립트   
    public GameObject main_camera;//메인카메라
    main_camera main_camera_Script;//메인카메라 스크립트
    public GameObject Hansel;//헨젤
    Hansel_Script hansel_script;//헨젤

    Animator Witch_animator;
    NavMeshAgent agent;

    //추적
    public bool target_main = true;
    public bool target_enable = true;
    public GameObject clock_tower_center;

    //RUN
    bool run_enable = true;//RUN 가능
    float run_timer = 0.0f;//RUN쿨타임

    //STOP
    float stop_timer = 0.0f;//stop지속시간
    float stop_cool = 0.0f;

    //이동속도
    float speed_constant = 1.0f;
    float threat_constant = 1.0f;

    //패턴변경
    float pattern_time = 0.0f;
    float pattern_timer = 0.0f;

    //1-2스테이지 클리어타겟
    public GameObject target_1_2_0;
    public GameObject target_1_2;
    public GameObject material_bag;

    //Mode
    public bool e_mode;//동 1
    public bool w_mode;//서 2
    public bool s_mode;//남 3
    public bool n_mode;//북 4
    public bool se_mode;//남동
    public bool sw_mode;//남서
    public bool nw_mode;//북서
    public bool ne_mode;//북동
    public GameObject e_target;
    public GameObject w_target;
    public GameObject s_target;
    public GameObject n_target;
    public GameObject se_target;
    public GameObject sw_target;
    public GameObject ne_target;
    public GameObject nw_target;

    //이동방향
    public bool clockwise;//시계방향
    public bool anticlockwise;//반시계방향

    //정지상태
    public bool freeze = true;
    public float freeze_timer = 0.0f;

    //애니메이션 플래그
    bool walk = false;
    bool run = false;
    bool stop = false;
    bool catch_ = false;
    bool threat = false;
    public bool game_over = false;
    public bool clear = false;

    public enum STEP //동작
    {
        WALK, //0 걷기
        RUN, //1 뛰기
        STOP,// 2 정지
        THREAT,//3 위협
        NUM //4 동작수
    };
    public STEP step = STEP.WALK;//현재상태
    public STEP next_step;//다음상태

    // Start is called before the first frame update
    void Start()
    {
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        Witch_animator = this.witch.GetComponent<Animator>();//애니메이터

        Gretel_Script = main_player.GetComponent<main_player_1>();//그레텔스크립트

        hansel_script = Hansel.GetComponent<Hansel_Script>();//헨젤스크립트

        main_camera_Script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트

        agent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(this.transform.position,this.main_player.transform.position));
        //레이캐스트
        Debug.DrawRay(this.transform.position + Vector3.down * 0.5f, this.transform.forward * 10f, Color.red);
        int layerMask = 1 << 11;//레이어 11만충돌
        if (Physics.Raycast(this.transform.position + Vector3.down * 0.5f, this.transform.forward, out hit, 100f, layerMask))
        {
            if (run_enable == true)//RUN상태로 전환
            {
                this.next_step = STEP.RUN;
                run_enable = false;
                StartCoroutine("RUN_DURATION");//지속시간
                run_timer = 0.0f;//RUN지속시간 초기화
                Debug.Log("충돌");
            }
        }

        //마녀정지상태
        if (freeze == true)
        {
            this.step = STEP.STOP;
            this.next_step = STEP.STOP;

            if (this.hansel_script.in_elevator == true || this.Gretel_Script.field1 == true)//마녀정지상태 해제
            {
                freeze = false;
            }

            if (freeze_timer <= 25.0f)
            {
                freeze_timer += Time.deltaTime;
            }
            else
            {
                freeze_timer = 0.0f;
                freeze = false;
            }
        }
        //스테이지클리어 ,마녀삭제
        else if (game_Management.stage_num >= 19 && game_Management.witch_trigger_stage1_19 == true)
        {
            this.material_bag.SetActive(true);

            // 타겟변경
            agent.destination = target_1_2.transform.position;
            target_main = false;
            target_enable = false;

            //애니메이션 플래그
            clear = true;
            stop = false;
            run = false;
            walk = false;
            threat = false;

            if (Vector3.Distance(this.transform.position, this.agent.destination) < 2.0f)
            {
                this.gameObject.SetActive(false);
            }
        }

        //타겟추적
        if (target_main == true && game_Management.stage_num < 15)
        {
            if (Vector3.Distance(this.transform.position, Hansel.transform.localPosition) < 2.0f)
            {
                agent.destination = this.Hansel.transform.localPosition;
            }
            else
            {
                agent.destination = this.main_player.transform.position;
            }
        }

        //플레이어와 가까워질때 CATCH
        if (Vector3.Distance(this.transform.position, this.main_player.transform.position) < 2.5f ||
            Vector3.Distance(this.transform.position, this.Hansel.transform.localPosition) < 2.0f)
        {
            catch_ = true;
        }
        else
        {
            catch_ = false;
        }
        //너무가까워서 판정오류날때
        if (Vector3.Distance(this.transform.position, this.main_player.transform.position) < 0.8f && this.catch_ == false)
        {
            this.transform.Translate(Vector3.back * 2.0f * Time.deltaTime);
        }

        //이동속도 상수(플레이어와의 거리만큼 늘어남)
        if (this.main_camera_Script.clock_tower_mode == true)
        {
            if (Vector3.Distance(this.transform.position, this.main_player.transform.position) < 10.0f ||
                Vector3.Distance(this.transform.position, this.clock_tower_center.transform.position) > 15.0f)
            {
                this.speed_constant = 1.05f;
            }
            else
            {
                this.speed_constant = Vector3.Distance(this.transform.position, this.main_player.transform.position) * -0.04f + 1.45f;
                if (this.speed_constant < 0.0f)
                {
                    this.speed_constant = 1.0f;
                }
                else if (this.speed_constant < 0.3f)//최저속도
                {
                    this.speed_constant = 0.3f;
                }
                if (this.game_Management.stage_num >= 15)//클리어이후 속도고정
                {
                    this.speed_constant = 1.3f;
                }
            }          
        }
        else
        {
            this.speed_constant = 1.0f;
        }
        //Debug.Log("거리 :" + Vector3.Distance(this.transform.position, this.main_player.transform.position) + "  속도 :" + speed_constant);
        //Stop 쿨타임
        if (stop_cool > 0.0f)
        {
            stop_cool -= Time.deltaTime;
        }
        else
        {
            stop_cool = 0.0f;
        }


        //게임오버 , 플레이어잡음
        if (Gretel_Script.gameover_be_caught2 == true || this.hansel_script.gameover_be_caught2_hansel == true)
        { 
            //동작정지
            this.next_step = STEP.STOP;
            this.step = STEP.STOP;

            //애니메이션플래그
            game_over = true;

            //플래그초기화
            this.e_mode = false;
            this.s_mode = false;
            this.w_mode = false;
            this.n_mode = false;
            this.nw_mode = false;
            this.ne_mode = false;
            this.se_mode = false;
            this.sw_mode = false;
        }
        Debug.Log(agent.destination);


        //상태
        switch (this.step)//캐릭터 상태별 동작
        {
            case STEP.WALK:
                //이동속도
                agent.speed = 1.5f * speed_constant * threat_constant;
                //WALK -> RUN모드 쿨타임 5초 
                if (run_timer <= 5.0f)
                {
                    run_timer += Time.deltaTime;
                    if (speed_constant > 1.0f || mode_judge() == true) //RUN모드로 바로변환
                    {
                        //RUN모드 전환시 
                        this.next_step = STEP.RUN;
                        run_enable = false;
                        StartCoroutine("RUN_DURATION");//지속시간
                        run_timer = 0.0f;//RUN지속시간 초기화
                    }
                }
                else
                {
                    run_enable = true;
                }

                //RUN모드 전환
                if (Vector3.Distance(this.transform.position, this.main_player.transform.position) > 8.0f)
                {                  
                    //RUN모드 전환시 
                    this.next_step = STEP.RUN;
                    run_enable = false;
                    StartCoroutine("RUN_DURATION");//지속시간
                    run_timer = 0.0f;//RUN지속시간 초기화
                }

                //애니메이션 플래그
                walk = true;
                run = false;
                stop = false;
                threat = false;
                game_over = false;
                break;
            case STEP.RUN:

                //이동속도
                if (main_camera_Script.camera_rotation_flag == true && stop_cool == 0.0f)//이동속도 잠시 감소
                {
                    agent.speed = 1.5f * speed_constant * threat_constant;
                    if (Random.Range(0, 3) == 1)//랜덤 0~2 33.33&
                    {
                        this.next_step = STEP.STOP;
                    }
                    else if (Random.Range(0, 2) == 1)
                    {
                        this.next_step = STEP.THREAT;
                    }
                    //쿨타임
                    stop_cool = 3.7f;
                }
                else
                {
                    agent.speed = 3.5f * speed_constant;
                }

                //애니메이션 플래그
                run = true;
                walk = false;
                stop = false;
                threat = false;
                break;

            case STEP.STOP:
                //이동속도
                agent.speed = 0.0f;
                //지속시간
                if (run_timer < 2.0f)
                {
                    run_timer += Time.deltaTime;
                }
                else
                {
                    this.next_step = STEP.WALK;
                    run_timer = 0.0f;
                }

                if (game_over == true)
                {
                    //애니메이션 플래그
                    stop = false;
                    run = false;
                    walk = false;
                    threat = false;
                    break;
                } 
                else if (game_Management.stage_num >= 15)//위치변경
                {
                    //타겟변경
                    agent.destination = target_1_2_0.transform.position;
                    target_main = false;
                    target_enable = false;

                    //애니메이션 플래그
                    clear = true;
                    stop = false;
                    run = false;
                    walk = false;
                    threat = false;
                }
                else
                {
                    //애니메이션 플래그
                    stop = true;
                    run = false;
                    walk = false;
                    threat = false;
                }
                
                break;
            case STEP.THREAT:

                //이동속도
                agent.speed = 0.0f;
                //지속시간
                if (run_timer < 0.5f)
                {
                    run_timer += Time.deltaTime;
                }
                else
                {
                    //RUN모드 전환시 
                    this.next_step = STEP.RUN;
                    run_enable = false;
                    StartCoroutine("RUN_DURATION");//지속시간
                    run_timer = 0.0f;//RUN지속시간 초기화

                    //THREAT모드
                    StartCoroutine("THREAT_MODE");
                    threat_constant = 1.5f;
                }

                //애니메이션 플래그
                stop = false;
                run = false;
                walk = false;
                threat = true;
                game_over = false;
                break;
        }

        if ((this.step == STEP.WALK || this.step == STEP.RUN) && game_Management.stage_num < 15)//플레이어추적
        {
            //플레이어추적
            //agent.destination = this.Gretel_Script.gameObject.transform.position;
            if (Vector3.Distance(this.transform.position, agent.destination) <= 2.5f)
            {
                target_main = true;
                target_enable = true;
            }

            //일정시간마다 패턴변경
            if (this.pattern_timer <= this.pattern_time)
            {
                this.pattern_timer += Time.deltaTime;
            }
            else
            {
                this.target_enable = true;//패턴변경가능
                this.pattern_timer = 0;
                this.pattern_time = Random.Range(5.0f,15.0f); 
            }

            //추적패턴
            if (target_enable == true && main_camera_Script.clock_tower_mode == true)
            {
                //플래그초기화
                target_enable = false;

                if (this.e_mode == true || this.ne_mode == true || this.se_mode == true)//E
                {
                    if (this.main_camera_Script.e_mode == true)//같은라인
                    {
                        this.target_main = true;
                        this.target_enable = true;
                    }
                    else if (this.main_camera_Script.w_mode == true)//반대편라인
                    {
                        if (Random.Range(0, 100) % 2 == 0) //50%
                        {
                            this.target_main = false;
                            this.agent.destination = n_target.transform.position;
                        }
                        else
                        {
                            this.target_main = false;
                            this.agent.destination = s_target.transform.position;
                        }
                    }
                    else if(this.main_camera_Script.n_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = se_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                    else if (this.main_camera_Script.s_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = ne_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                }//end of if E
                else if (this.w_mode == true || this.nw_mode == true || this.sw_mode == true)//W
                {
                    if (this.main_camera_Script.w_mode == true)//같은라인
                    {
                        this.target_main = true;
                        this.target_enable = true;
                    }
                    else if (this.main_camera_Script.e_mode == true)//반대편라인
                    {
                        if (Random.Range(0, 100) % 2 == 0) //50%
                        {
                            this.target_main = false;
                            this.agent.destination = n_target.transform.position;
                        }
                        else
                        {
                            this.target_main = false;
                            this.agent.destination = s_target.transform.position;
                        }
                    }
                    else if (this.main_camera_Script.s_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = nw_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                    else if (this.main_camera_Script.n_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = sw_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                }//end of if W
                else if (this.s_mode == true || this.sw_mode == true || this.se_mode == true)//S
                {
                    if (this.main_camera_Script.s_mode == true)//같은라인
                    {
                        this.target_main = true;
                        this.target_enable = true;
                    }
                    else if (this.main_camera_Script.n_mode == true)//반대편라인
                    {
                        if (Random.Range(0, 100) % 2 == 0) //50%
                        {
                            this.target_main = false;
                            this.agent.destination = w_target.transform.position;
                        }
                        else
                        {
                            this.target_main = false;
                            this.agent.destination = e_target.transform.position;
                        }
                    }
                    else if (this.main_camera_Script.e_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = sw_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                    else if (this.main_camera_Script.w_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = se_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                }//end of if S
                else if (this.n_mode == true || this.nw_mode == true || this.ne_mode == true)//N
                {
                    if (this.main_camera_Script.n_mode == true)//같은라인
                    {
                        this.target_main = true;
                        this.target_enable = true;
                    }
                    else if (this.main_camera_Script.s_mode == true)//반대편라인
                    {
                        if (Random.Range(0, 100) % 2 == 0) //50%
                        {
                            this.target_main = false;
                            this.agent.destination = w_target.transform.position;
                        }
                        else
                        {
                            this.target_main = false;
                            this.agent.destination = e_target.transform.position;
                        }
                    }
                    else if (this.main_camera_Script.e_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = nw_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                    else if (this.main_camera_Script.w_mode == true)//옆라인
                    {
                        if (Random.Range(0, 100) % 3 == 0) //33%
                        {
                            this.target_main = false;
                            this.agent.destination = ne_target.transform.position;
                            Debug.Log("옆라인");
                        }
                        else //플레이어추적
                        {
                            this.target_main = true;
                            StartCoroutine("target_enable_delay");
                            Debug.Log("옆라인2");
                        }
                    }
                }//end of if N
            }
           
        }

        

        //Mode
        if (main_camera_Script.clock_tower_mode == true && this.game_Management.stage_num < 15)
        {
            if (this.transform.position.x >= 41.5)
            {
                if (this.transform.position.z >= 22.8f && this.transform.position.z < 39f)//E
                {
                    //시계or반시계 방향판단
                    if (this.ne_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if(this.se_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.e_mode = true;
                    this.w_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;
                    this.se_mode = false;
                    this.sw_mode = false;
                    this.ne_mode = false;
                    this.nw_mode = false;
                }
                else if (this.transform.position.z >= 39f)//NE
                {
                    //시계or반시계 방향판단
                    if (this.n_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if (this.e_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.ne_mode = true;
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;
                    this.se_mode = false;
                    this.sw_mode = false;
                    this.nw_mode = false;
                }
                else //SE
                {
                    //시계or반시계 방향판단
                    if (this.e_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if (this.s_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.se_mode = true;
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;                 
                    this.sw_mode = false;
                    this.nw_mode = false;
                    this.ne_mode = false;
                }
               
            }
            else if (this.transform.position.x <= 23.9f)
            {
                if (this.transform.position.z >= 22.8f && this.transform.position.z < 39f)//W
                {
                    //시계or반시계 방향판단
                    if (this.sw_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if (this.nw_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.w_mode = true;
                    this.e_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;
                    this.se_mode = false;
                    this.sw_mode = false;
                    this.ne_mode = false;
                    this.nw_mode = false;
                }
                else if (this.transform.position.z >= 39f)//NW
                {
                    //시계or반시계 방향판단
                    if (this.w_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if (this.n_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.nw_mode = true;
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;
                    this.se_mode = false;
                    this.sw_mode = false;                  
                    this.ne_mode = false;
                }
                else //SW
                {
                    //시계or반시계 방향판단
                    if (this.s_mode == true)//시계방향
                    {
                        this.clockwise = true;
                        this.anticlockwise = false;
                    }
                    else if (this.w_mode == true)//반시계방향
                    {
                        this.clockwise = false;
                        this.anticlockwise = true;
                    }
                    //모드초기화
                    this.sw_mode = true;
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    this.n_mode = false;
                    this.se_mode = false;
                    this.nw_mode = false;
                    this.ne_mode = false;
                }
            }
            else if (this.transform.position.x > 23.9f && this.transform.position.x < 41.5f && this.transform.position.z < 22.8f)//S
            {
                //시계or반시계 방향판단
                if (this.se_mode == true)//시계방향
                {
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
                else if (this.sw_mode == true)//반시계방향
                {
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
                //모드초기화
                this.s_mode = true;
                this.e_mode = false;
                this.w_mode = false;
                this.n_mode = false;
                this.se_mode = false;
                this.nw_mode = false;
                this.ne_mode = false;
                this.sw_mode = false;
            }
            else//N
            {
                //시계or반시계 방향판단
                if (this.nw_mode == true)//시계방향
                {
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
                else if (this.ne_mode == true)//반시계방향
                {
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
                //모드초기화
                this.n_mode = true;
                this.e_mode = false;
                this.w_mode = false;
                this.s_mode = false;
                this.se_mode = false;
                this.nw_mode = false;
                this.ne_mode = false;
                this.sw_mode = false;
            }
        }

        //애니메이션관리
        this.Witch_animator.SetBool("WALK",walk);
        this.Witch_animator.SetBool("RUN",run);
        this.Witch_animator.SetBool("STOP", stop);
        this.Witch_animator.SetBool("CATCH", catch_);
        this.Witch_animator.SetBool("threat", threat);
        this.Witch_animator.SetBool("GAME_OVER", game_over);
        this.Witch_animator.SetBool("CLEAR", clear);

        //상태관리
        this.step = this.next_step;

    }

    //타겟전환
    private void target_change()
    {
        if (this.main_camera_Script.e_mode == true)
        {
            if (this.s_mode == true || this.n_mode == true)
            {
                agent.destination = this.w_target.transform.position;
            }
        }
        else if (this.main_camera_Script.w_mode == true)
        {
            if (this.s_mode == true || this.n_mode == true)
            {
                agent.destination = this.e_target.transform.position;
            }
        }
        else if (this.main_camera_Script.s_mode == true)
        {
            if (this.e_mode == true || this.w_mode == true)
            {
                agent.destination = this.n_target.transform.position;
            }
        }
        else
        {
            if (this.e_mode == true || this.w_mode == true)
            {
                agent.destination = this.s_target.transform.position;
            }
        }
    }
    //같은라인판독
    private bool mode_judge()
    {
        if (this.e_mode || this.ne_mode || this.se_mode)
        {
            if (this.main_camera_Script.e_mode == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (this.w_mode || this.nw_mode || this.sw_mode)
        {
            if (this.main_camera_Script.w_mode == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (this.s_mode || this.sw_mode || this.se_mode)
        {
            if (this.main_camera_Script.s_mode == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (this.n_mode || this.nw_mode || this.ne_mode)
        {
            if (this.main_camera_Script.n_mode == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    IEnumerator target_enable_delay()//TarGet_Enable 비활성화
    {
        yield return new WaitForSeconds(Random.Range(3, 11));
        this.target_enable = true;
    }

    IEnumerator RUN_DURATION()//RUN 지속시간
    {
        yield return new WaitForSeconds(8.0f);
        this.next_step = STEP.WALK;//지속시간 끝난후 다시 WALK상태로 돌아옴
    }

    IEnumerator THREAT_MODE()//THREAT 지속시간
    {
        yield return new WaitForSeconds(1.0f);
        this.next_step = STEP.WALK;//지속시간 끝난후 다시 WALK상태로 돌아옴
        threat_constant = 1.0f;
    }

}
