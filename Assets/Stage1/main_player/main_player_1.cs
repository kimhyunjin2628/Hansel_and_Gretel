using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class main_player_1 : MonoBehaviour
{


    CharacterController Gretel_cc;
    Animator Gretel_animator;
    NavMeshAgent agent; //추적관련
    game_management game_Management;//게임관리 스크립트
    public GameObject management;
    Hansel_Script hansel_script;//헨젤스크립트
    public GameObject Witch_hand;//마녀손
    eyes_pattern eyes_pattern;//마녀스크립트
    public GameObject camera_position;//카메라포지션
    camera_position camera_position_script;//카메라포지션 스크립트
    public GameObject main_camera;//메인카메라
    main_camera main_camera_script;//메인카메라 스크립트
    public GameObject Lever;//레버
    Lever_script lever_script;//레버스크립트
    public GameObject target_position;//마녀에게 잡혔을때 위치
    public GameObject clock_closet;//clock모드 벽장
    clock_closet clock_closet_script;//clock모드 벽장 스크립트

    //자식오브젝트 위치
    public GameObject key_position;

    public enum STEP //동작
    {
        NORMAL, //0 기본
        WALK, //1 걷기
        RUN, //2 뛰기
        JUMP,//3 점프
        LAND,//4 착지
        LAND2,//5 착지2
        STOP,//6 스탑
        H_G_NORMAL, //7 H_G 기본
        H_G_WALK, //8 H_G 걷기
        HANG_JUMP,//9 높은지형 올라가기
        NO_ACTION,//10
        NUM //10 동작수
    };
    public STEP step = STEP.NORMAL;//현재상태
    public STEP next_step;//다음상태



    //이동 - 포지션(position)
    float move_speed = 1.5f;//이동속도
    float move_speed_jump = 0.3f;//점프중일경우 이동속도
    public float move_dev = 10.0f;//이동속도조절

    //이동 - 로테이션(rotation)
    float rotate_speed = 720.0f;//회전속도

    //이동 - 점프
    public float jump_power = 0.0f;
    bool jump_power_enable = false;//점프파워 
    public bool jump_enable = false;//점프
    public bool is_ground = false;//접지플래그
    public float current_y = 0.0f;//이동처리 직전 현재y축(점프하기전 y축)
    public float jump_time = 0.0f;//공중에 떠있는 시간

    //점프시 매달리기 이벤트가 발생하지않는오류 -> 컬라이더낮음
    public GameObject bed_collider;

    //이동 - 착지
    //bool land_delay = true;
    bool normal_delay_flag = true;//낮은착지 끝난후 딜레이
    float normal_delay_flag_reset = 0.0f;//코루틴오류로 인한 점프불가상태방지
    public bool normal_delay_flag2 = true;//높은착지 끝난후 딜레이
    bool land2 = false;//높은착지
    float land2_stop_timer = 0.0f;//land2,stop상태시 0.5초동안 normal상태로 돌아가지 않을경우 오류발생을 방지

    //높은지형에서 Ray인식 실패(착지자세 유지버그)
    float before_position_y = 0.0f;
    bool compare_position_y = true;

    //이동 - 높은지형 올라가기
    GameObject ground_position;//높은지형 있는지 없는지 판단
    bool climb_enable = false;//높은지형 올라갈수 있을때 true
    bool climb_start_up = false;//올라가기전 위치고정
    bool climb_start_up2 = false;//올라가기전 위치고정
    public bool hang_animation_roatate = false; //올라가는중 회전위치 조정
    GameObject ground_hit;//충돌한 높은지형
    float ground_y;//고정위치좌표
    float ground_z;//고정위치좌표
    float ground_x;//고정위치좌표
    bool climb_up_enable = false;//올라가는중 위로이동
    bool climb_forward_enable = false;//올라가는중 앞으로이동
    float climb_up_speed = 0.0f;//올라가는중 캐릭터 이동속도
    float climb_forward_speed = 0.0f;//올라가는중 캐릭터 이동속도
    float noaction_down_speed = 5.0f;//no_action상태에서 중력효과를 내기위한 Vector3.down속도
    bool hang_noaction = false;//hang&jump 상태이후 noaction상태

    //레이캐스트
    RaycastHit hit;
    public float ray_distance = 0.0f;//플레이어와 지면사이의 거리
    Vector3 Rayvec;

    //H&G
    public GameObject gretel_position;//핸젤의 정면
    bool h_g_delay = true;

    //세이브 
    public bool save1 = false;
    public bool save_point1 = false;

    //새장
    public GameObject cage;

    //열쇠획득
    public bool get_key = false;
    public bool on_rock = false;

    //좌표알리미 
    public bool pointer_1_4 = false;//1-4스테이지좌표
    public bool pointer_1_6 = false;//1-6스테이지좌표

    //1-11용 스낵그라운드플래그
    public bool snack_ground = false;

    //게임오버
    public float land_timer = 0.0f;
    public bool fall_flag = false;
    float caught_y = 2.1f;
    float caught_z = 0.677f;
    

    //애니메이션 플래그
    public bool walk = false;
    public bool run = false;
    public bool normal = false;
    public bool jump = false;
    public bool land = false;
    public bool h_g_normal = false;
    public bool h_g_walk = false;
    public bool climb = false;
    //게임오버 애니메이션 플래그
    public bool gameover_fall = false;
    public bool gameover_fall2 = false;
    public bool gameover_be_locked = false;
    public bool gameover_be_caught = false;
    public bool gameover_be_caught2 = false;

    //필드이동
    public bool field1 = true;
    public bool field2 = false;

    //모드에 따라 컬라이더on off
    public GameObject clock_tower_mode_collider_E;
    public GameObject clock_tower_mode_collider_W;
    public GameObject clock_tower_mode_collider_S;
    public GameObject clock_tower_mode_collider_N;
    public GameObject clock_tower_collider;//핸젤 가라앉음방지용
    BoxCollider clock_tower_collider_component;

    //클락패스워드
    public bool clock_pass1 = false;
    public bool clock_pass2 = false;

    // Start is called before the first frame update
    void Start()
    {
        Gretel_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
        Gretel_animator = this.GetComponent<Animator>();//애니메이션 컨트롤러
        agent = this.GetComponent<NavMeshAgent>();//추적관련
        //this.GetComponent<NavMeshAgent>().enabled = true; //offset
        ground_position = GameObject.FindGameObjectWithTag("Ground_true");//높은지형 유무 판단
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        hansel_script = GameObject.FindGameObjectWithTag("Player2").GetComponent<Hansel_Script>();
        eyes_pattern = Witch_hand.transform.root.GetComponent<eyes_pattern>();
        camera_position_script = camera_position.GetComponent<camera_position>();//카메라포지션 스크립트
        main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        lever_script = Lever.GetComponent<Lever_script>();//레버스크립트
        clock_closet_script = clock_closet.GetComponent<clock_closet>();//clock모드 텔레포트 벽장 스크립트
        clock_tower_collider_component = clock_tower_collider.GetComponent<BoxCollider>();//컬라이더
    }

    // Update is called once per frame
    void Update()
    {        
        //레이캐스트
        Rayvec = new Vector3(this.transform.position.x + 0.1f, this.transform.position.y, this.transform.position.z);
        Debug.DrawRay(this.transform.position + this.transform.forward * 0.032f + this.transform.right * 0.021f, -this.transform.up * 10f, Color.red);

        Debug.DrawRay(this.transform.position + this.transform.forward * 0.22f + this.transform.right * 0.021f, -this.transform.up * 10f, Color.red);//F
        Debug.DrawRay(this.transform.position + this.transform.forward * -0.168f + this.transform.right * 0.021f, -this.transform.up * 10f, Color.red);//B
        Debug.DrawRay(this.transform.position + this.transform.forward * 0.032f + this.transform.right * 0.221f, -this.transform.up * 10f, Color.red);//R
        Debug.DrawRay(this.transform.position + this.transform.forward * 0.032f + this.transform.right * -0.179f, -this.transform.up * 10f, Color.red);//L

        int layerMask = 1 << 10;
        layerMask = ~layerMask; //레이어 10을 제외한 모든레이어에 충돌
        if (Physics.Raycast(this.transform.position + this.transform.forward * 0.032f + this.transform.right * 0.021f, -this.transform.up, out hit, 20f, layerMask))
        {
            ray_distance = this.transform.position.y - this.hit.transform.localPosition.y + 1.58f;//1.58->캐릭터 접지상수 , ray_distance 0.5정도로 맞추기위해 hit하는 오브젝트는 ground태그에 y:1.5
            Debug.Log(hit.collider.gameObject.name);
            //레이캐스트를 이용한 메인카메라 시점변경
            if (hit.collider.gameObject.tag == "Ground")
            {
                //카메라 시점 변환시
                if (main_camera_script.ground_mode == false)
                {
                    main_camera_script.V_flag_ground = true;
                }
                main_camera_script.ground_mode = true;
                //다른플래그는 전부false
                main_camera_script.bridge2_mode = false;
                main_camera_script.clock_tower_mode = false;
                main_camera_script.bridge3_mode = false;

            }
            else if (hit.collider.gameObject.tag == "Rock" || hit.collider.gameObject.tag == "Rock_Hansel")
            {
                ray_distance -= 5.8f;
            }
            else if (hit.collider.gameObject.tag == "bridge2")
            {
                //카메라 시점 변환시
                if (main_camera_script.bridge2_mode == false)
                {
                    main_camera_script.V_flag_bridge2 = true;
                }
                main_camera_script.bridge2_mode = true;
                //다른플래그는 전부false
                main_camera_script.ground_mode = false;
                main_camera_script.clock_tower_mode = false;
                main_camera_script.bridge3_mode = false;
            }
            else if (hit.collider.gameObject.tag == "clock_tower" ||
                hit.collider.gameObject.tag == "Clock_E" ||
                hit.collider.gameObject.tag == "Clock_W" ||
                    hit.collider.gameObject.tag == "Clock_S" ||
                    hit.collider.gameObject.tag == "Clock_N")
            {
                //카메라 시점 변환시
                if (main_camera_script.clock_tower_mode == false)
                {
                    main_camera_script.V_flag_clock_tower = true;
                }
                main_camera_script.clock_tower_mode = true;
                //다른플래그는 전부false
                main_camera_script.bridge2_mode = false;
                main_camera_script.ground_mode = false;
                main_camera_script.bridge3_mode = false;
            }
            else if (hit.collider.gameObject.tag == "bridge3")
            {
                //카메라 시점 변환시
                if (main_camera_script.bridge3_mode == false)
                {
                    main_camera_script.V_flag_bridge3 = true;
                }
                main_camera_script.bridge3_mode = true;
                //다른플래그는 전부false
                main_camera_script.ground_mode = false;
                main_camera_script.clock_tower_mode = false;
                main_camera_script.bridge2_mode = false;
            }
            else if (hit.collider.gameObject.tag == "Rock" || hit.collider.gameObject.tag == "snack_item")
            {
                this.ray_distance -= 4.0f;
            }
            else if (hit.collider.gameObject.tag == "closet")
            {
                //텔레포트가동
                this.lever_script.teleport = true;
                this.camera_position_script.T_flag = true;

                //플레이어이동
                this.next_step = STEP.NO_ACTION;
                this.step = STEP.NO_ACTION;

                //컬라이더off
                this.hit.collider.gameObject.SetActive(false);//V_flag = true 한번만실행
            }

            else if (hit.collider.gameObject.tag == "closet2")
            {
                //텔레포트가동
                this.lever_script.teleport2 = true;
                this.camera_position_script.T_flag2 = true;

                //플레이어이동
                this.next_step = STEP.NO_ACTION;
                this.step = STEP.NO_ACTION;

                //컬라이더off
                this.hit.collider.gameObject.SetActive(false);//V_flag2 = true 한번만실행
            }
            else if (hit.collider.gameObject.tag == "none_closet")
            {
                //closet2 컬라이더 on
                this.lever_script.closet2_collider.SetActive(true);
            }
            else if (hit.collider.gameObject.tag == "clock_closet")
            {
                this.clock_closet_script.t_start = true;
                this.clock_closet_script.closet_collider1.SetActive(false);//컬라이더off
                this.clock_closet_script.closet_collider2.SetActive(false);//컬라이더off
                this.clock_closet_script.closet_collider3.SetActive(false);//컬라이더off
                Debug.Log("입장");
            }

            //상시 on off
            if (hit.collider.gameObject.tag == "clock_password")
            {
                this.clock_pass1 = true;
            }
            else
            {
                this.clock_pass1 = false;
            }

            if (hit.collider.gameObject.tag == "clock_password2")
            {
                this.clock_pass2 = true;
            }
            else
            {
                this.clock_pass2 = false;
            }

            //Clock_Tower 모드일 경우
            if (this.main_camera_script.clock_tower_mode == true)
            {
                if (hit.collider.gameObject.tag == "Clock_E")
                {
                    main_camera_script.ray_e = true;
                    main_camera_script.ray_w = false;
                    main_camera_script.ray_s = false;
                    main_camera_script.ray_n = false;
                }
                else if (hit.collider.gameObject.tag == "Clock_W")
                {
                    main_camera_script.ray_w = true;
                    main_camera_script.ray_e = false;
                    main_camera_script.ray_s = false;
                    main_camera_script.ray_n = false;
                }
                else if (hit.collider.gameObject.tag == "Clock_N")
                {
                    main_camera_script.ray_n = true;
                    main_camera_script.ray_e = false;
                    main_camera_script.ray_s = false;
                    main_camera_script.ray_w = false;
                }
                else if (hit.collider.gameObject.tag == "Clock_S")
                {
                    main_camera_script.ray_s = true;
                    main_camera_script.ray_e = false;
                    main_camera_script.ray_w = false;
                    main_camera_script.ray_n = false;
                }

                //컬라이더on
                clock_tower_mode_collider_E.SetActive(true);
                clock_tower_mode_collider_W.SetActive(true);
                clock_tower_mode_collider_S.SetActive(true);
                clock_tower_mode_collider_N.SetActive(true);
                clock_tower_collider_component.enabled = false;
            }
            else
            {
                //컬라이더off
                clock_tower_mode_collider_E.SetActive(false);
                clock_tower_mode_collider_W.SetActive(false);
                clock_tower_mode_collider_S.SetActive(false);
                clock_tower_mode_collider_N.SetActive(false);
                clock_tower_collider_component.enabled = true;
            }
        }//end of raycast about tag

        //게임오버-추락판정 (땅에 닿았을때)
        if (this.step == STEP.LAND)
        {
            this.land_timer += Time.deltaTime;
        }
        else
        {
            this.land_timer = 0.0f;
        }
        if (this.land_timer >= 0.6f && gameover_fall == false)
        {
            fall_flag = true;
        }

        //게임오버- 추락판정2 (땅에 닿지 않았을때)
        if (this.land_timer > 5.0f)
        {
            gameover_fall2 = true;
        }

        //게임오버-마녀에게 붙잡힘 판정
        if (this.eyes_pattern.catch_ == true)
        {
            this.gameover_be_caught = true;
            this.eyes_pattern.eyes_chaser.transform.GetChild(0).gameObject.SetActive(false);

            //핸젤초기화
            this.hansel_script.gameObject.transform.parent = null;

            //상태 즉시변경
            this.next_step = STEP.NO_ACTION;
            this.step = STEP.NO_ACTION;
            //그레텔위치고정
            this.transform.position = new Vector3(10.812f, this.caught_y, this.caught_z);//그레텔위치고정  
            if (this.caught_y < 2.243f)
            {
                this.caught_y += Time.deltaTime * 0.45f;
            }
            else
            {
                this.caught_y = 2.243f;
            }
            if (this.caught_z < 0.987f)
            {
                this.caught_z += Time.deltaTime * 0.1f;
            }
            else
            {
                this.caught_z = 0.987f;
            }

            if (caught_y == 2.243f && caught_z == 0.987f)
            {
                eyes_pattern.catch_ = false;//플래그초기화
                gameover_be_caught = false;//플래그초기화
                caught_z = 0.677f;
                caught_y = 2.1f;
            }
            this.transform.localRotation = Quaternion.Euler(20, -70, -50);//회전값고정                   
        }

        //컬라이더 너무낮아서 매달리기 이벤트 없이 바로올라가질 경우
       if (this.transform.position.y < 1.0f)
        {
            this.bed_collider.GetComponent<BoxCollider>().center = new Vector3(0.01856f, -2.393831f, 0.6669267f);
            this.bed_collider.GetComponent<BoxCollider>().size = new Vector3(3.971085f, 1.746365f, 2.615452f);
        }
        if(climb == true) 
        {
            this.bed_collider.GetComponent<BoxCollider>().center = new Vector3(0.01856f, -2.830026f, 0.6669267f);
            this.bed_collider.GetComponent<BoxCollider>().size = new Vector3(3.971085f, 0.8739778f, 2.615452f);
        }

        switch (this.step)
        {
            case STEP.NORMAL:
                rotate_speed = 720f;
                //애니메이션 플래그
                normal = true;
                walk = false;
                run = false;
                jump = false;
                land = true;//점프거리가 너무 짧아 애니메이션이 jump상태에서 멈춰버릴경우를 대비하여 애니메이션조건을 살짝 수정
                land2 = false;
                h_g_normal = false;
                h_g_walk = false;
                climb = false;

                //점프오류 - ground_true태그를가진 collider에 걸쳤지만 climb판정이 되지않아 코루틴 climb_start, climb_start2만 호출되어 다음climb시 문제발생

                //hang and jump이후 플래그초기화
                hang_noaction = false;
                //오류수정 지면에 붙어있는 상태일때는 코루틴관련 bool변수 false
                climb_start_up = false;
                climb_start_up2 = false;
                //코루틴오류 - 코루틴이 호출되지않아 NOACTION상태의 Vector3.down 속도가 5.0f 가 아닐경우
                noaction_down_speed = 5.0f;
                //코루틴오류 - 코루틴이 호출되지않아 normal_delay_falg가 true값이 되지않아 점프가 불가능한경우
                if (normal_delay_flag_reset <= 0.35f && normal_delay_flag == false)
                {
                    normal_delay_flag_reset += Time.deltaTime;
                }
                else
                {
                    normal_delay_flag = true;
                    normal_delay_flag_reset = 0.0f;
                }

                this.jump_time = 0.0f;//공중에떠있지않음
                break;
            case STEP.WALK:


                rotate_speed = 720f;

                //애니메이션 플래그
                walk = true;
                normal = false;
                run = false;
                land = false;
                land2 = false;
                climb = false;

                //점프오류 - ground_true태그를가진 collider에 걸쳤지만 climb판정이 되지않아 코루틴 climb_start, climb_start2만 호출되어 다음climb시 문제발생
                //오류수정 지면에 붙어있는 상태일때는 코루틴관련 bool변수 false
                climb_start_up = false;
                climb_start_up2 = false;

                this.jump_time = 0.0f;//공중에떠있지않음
                break;
            case STEP.RUN:


                rotate_speed = 720f;
                //애니메이션 플래그               
                run = true;
                normal = false;
                walk = false;
                jump = false;
                land = false;
                land2 = false;
                h_g_normal = false;
                h_g_walk = false;
                climb = false;

                //점프오류 - ground_true태그를가진 collider에 걸쳤지만 climb판정이 되지않아 코루틴 climb_start, climb_start2만 호출되어 다음climb시 문제발생
                //오류수정 지면에 붙어있는 상태일때는 코루틴관련 bool변수 false
                climb_start_up = false;
                climb_start_up2 = false;

                this.jump_time = 0.0f;//공중에떠있지않음
                break;
            case STEP.JUMP:
                normal_delay_flag_reset = 0.0f;
                //점프
                if (this.climb_enable == true)//점프중 높은지형 근처일경우
                {
                    this.next_step = STEP.HANG_JUMP;//상태변환
                    StartCoroutine("climb_start");
                    StartCoroutine("climb_start2");
                    //매달려있을때 이동 플래그
                    this.climb_up_enable = true;//위로이동가능
                    this.climb_forward_enable = true;//앞으로 이동가능

                    if (ground_hit.transform.GetChild(0).tag == "Ground_y_F")
                    {
                        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        //this.transform.localPosition.z = ;
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_B")
                    {
                        this.transform.localRotation = Quaternion.Euler(0, -180, 0);
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_L")
                    {
                        this.transform.localRotation = Quaternion.Euler(0, -90, 0);
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_R")
                    {
                        this.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    }

                }

                Vector3 jump_vec = new Vector3(0, 4.0f, 0);//점프 이동량
                if (Input.GetKeyUp(KeyCode.LeftAlt) || Input.GetKeyUp(KeyCode.RightAlt))
                {
                    jump_power_enable = false;
                }
                if (jump_power_enable)
                {
                    jump_power += 1.0f * Time.deltaTime;
                }

                if (Gretel_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0)
                {
                    if (Gretel_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[Gretel]jump_ready2"))
                    {
                        // Debug.Log("점프");
                        jump_enable = false;
                    }
                }

                if (jump_enable == true)
                {
                    //Gretel_cc.Move(((jump_vec * move_speed * Time.deltaTime).normalized) / move_dev);//이동처리
                    if (current_y + 1.7f >= this.transform.localPosition.y)
                    {
                        Gretel_cc.Move(jump_vec * move_speed * Time.deltaTime);
                    }
                }
                else
                {
                    this.next_step = STEP.LAND;
                    jump = false;
                    jump_power = 0.0f;
                }

                //애니메이션 플래그
                jump = true;
                run = false;
                normal = false;
                walk = false;
                land = false;
                h_g_normal = false;
                h_g_walk = false;

                break;
            case STEP.LAND:
                Gretel_cc.Move(Vector3.down * 5.0f * Time.deltaTime);

                if (this.before_position_y == this.transform.position.y)//걸쳤을때
                {
                    this.transform.Rotate(0, 80f * Time.deltaTime, 0);
                    this.Gretel_cc.Move(-Vector3.forward * 5f * Time.deltaTime);
                    this.Gretel_cc.Move(Vector3.left * 5f * Time.deltaTime);
                    land_timer = 0.0f;
                }
                before_position_y = this.transform.position.y;

                if (ray_distance >= 2.5f)
                {
                    land2 = true;
                }

                if (ray_distance <= 2.0f)
                {
                    if (land2 == true && land_timer > 0.21f)
                    {
                        this.next_step = STEP.LAND2;
                        this.is_ground = true;

                    }
                    else
                    {
                        this.next_step = STEP.STOP;
                        this.is_ground = true;
                    }
                }

                //애니메이션 플래그
                land = true;
                jump = false;
                run = false;
                normal = false;
                walk = false;
                h_g_normal = false;
                h_g_walk = false;
                climb = false;
                break;
            case STEP.LAND2: //(STOP2)
                Gretel_cc.Move(Vector3.down * 5.0f * Time.deltaTime);

                //collider판정 오류시 normal상태로돌아감
                if (land2_stop_timer < 0.5f)
                {
                    land2_stop_timer += Time.deltaTime;
                }
                else
                {
                    this.next_step = STEP.NORMAL;
                    land2_stop_timer = 0.0f;
                }

                //game-over (추락)
                if (fall_flag == true)
                {
                    //상태 즉시변경
                    this.next_step = STEP.NO_ACTION;
                    this.step = STEP.NO_ACTION;
                }

                //애니메이션 플래그,NORMAL상태와 동일
                normal = true;
                walk = false;
                run = false;
                jump = false;
                land2 = true;
                land = false;
                h_g_normal = false;
                h_g_walk = false;
                climb = false;
                break;
            case STEP.STOP:
                Gretel_cc.Move(Vector3.down * 5.0f * Time.deltaTime);

                //collider판정 오류시 normal상태로돌아감
                if (land2_stop_timer < 0.5f)
                {
                    land2_stop_timer += Time.deltaTime;
                }
                else
                {
                    this.next_step = STEP.NORMAL;
                    land2_stop_timer = 0.0f;
                }

                //애니메이션 플래그,NORMAL상태와 동일
                if (this.ray_distance < 1.0f)
                {
                    normal = true;
                    walk = false;
                    run = false;
                    jump = false;
                    land = true; //점프거리가 너무 짧아 애니메이션이 jump상태에서 멈춰버릴경우를 대비하여 애니메이션조건을 살짝 수정
                    land2 = false;
                    h_g_normal = false;
                    h_g_walk = false;
                    climb = false;
                }
                else
                {
                    normal = false;
                    walk = false;
                    run = false;
                    jump = false;
                    land = true; //점프거리가 너무 짧아 애니메이션이 jump상태에서 멈춰버릴경우를 대비하여 애니메이션조건을 살짝 수정
                    land2 = false;
                    h_g_normal = false;
                    h_g_walk = false;
                    climb = false;
                }
                break;
            case STEP.H_G_NORMAL:
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                         Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        this.next_step = STEP.WALK;
                    }
                    else
                    {
                        this.next_step = STEP.NORMAL;
                    }
                }

                //애니메이션 플래그
                h_g_normal = true;
                h_g_walk = false;
                normal = false;
                walk = false;
                run = false;
                jump = false;
                land = false;
                land2 = false;
                climb = false;
                break;
            case STEP.H_G_WALK:
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                         Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        this.next_step = STEP.WALK;
                    }
                    else
                    {
                        this.next_step = STEP.NORMAL;
                    }
                }

                //애니메이션 플래그
                h_g_normal = false;
                h_g_walk = true;
                normal = false;
                walk = false;
                run = false;
                jump = false;
                land = false;
                land2 = false;
                climb = false;
                break;
            case STEP.HANG_JUMP:

                if (this.climb_start_up == true)
                {
                    //애니메이션에 맞춰 위치조정
                    // this.transform.Translate(Vector3.forward * 1.4f * Time.deltaTime);
                    if (this.climb_up_enable == true)
                    {
                        StartCoroutine("climb_position_up");//climb_up_speed값 결정
                        this.climb_up_enable = false;//코루틴 한번만 실행
                    }
                    if (climb_forward_enable == true)
                    {
                        StartCoroutine("climb_position_forward");//climb_up_forward값 결정
                        this.climb_forward_enable = false;//코루틴 한번만 실행
                    }


                    this.transform.Translate(Vector3.up * climb_up_speed * Time.deltaTime);//1.1
                    if (this.transform.localRotation.eulerAngles.x >= 40f)
                    {
                        hang_animation_roatate = true;
                    }

                    if (hang_animation_roatate == false)
                    {
                        //this.transform.Rotate(50f * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        //this.transform.Rotate(-90f * Time.deltaTime, 0, 0);
                    }

                    if (this.climb_start_up2 == true)
                    {
                        this.transform.Translate(Vector3.forward * climb_forward_speed * Time.deltaTime);//1.0
                    }
                }
                else
                {
                    //높은곳 올라갈때 위치고정
                    if (ground_hit.transform.GetChild(0).tag == "Ground_y_F")
                    {
                        this.transform.position = new Vector3(this.transform.position.x, ground_y, ground_z);//위치고정
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_B")
                    {
                        this.transform.position = new Vector3(this.transform.position.x, ground_y, ground_z);//위치고정
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_L")
                    {
                        this.transform.position = new Vector3(ground_x, ground_y , this.transform.position.z);//위치고정
                    }
                    else if (ground_hit.transform.GetChild(0).tag == "Ground_y_R")
                    {
                        this.transform.position = new Vector3(ground_x, ground_y, this.transform.position.z);//위치고정
                    }

                }


                if (Gretel_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                {
                    if (Gretel_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("[Gretel]hang&jump_fix"))
                    {
                        climb_start_up = false;
                        climb_start_up2 = false;
                        hang_animation_roatate = false;
                        StartCoroutine("climb_fine");//일정시간뒤 상태변화
                        hang_noaction = true;//noaction상태 위치고정 이벤트 겹치지 않기위해
                        this.next_step = STEP.NO_ACTION;
                        // this.transform.localRotation = Quaternion.Euler(0, this.transform.localRotation.eulerAngles.y, this.transform.localRotation.eulerAngles.z);//
                    }
                }

                this.jump_time = 0.0f;//공중에떠있지않음
                //애니메이션플래그
                climb = true;
                h_g_normal = false;
                h_g_walk = false;
                normal = false;
                walk = false;
                run = false;
                jump = false;
                land = false;
                land2 = false;
                break;
            case STEP.NO_ACTION:

                this.jump_time = 0.0f;//공중에떠있지않음
                //애니메이션플래그,NORMAL상태와 동일
                climb = false;
                h_g_normal = false;
                h_g_walk = false;
                normal = true;//true
                walk = false;
                run = false;
                jump = false;
                land = false;
                land2 = false;
                break;

        }//end of switch
        
        //땅에 착지한 이후의 상태 -> 착지길이 길면 fall_game_over판정
        if ((this.step == STEP.LAND2 || this.step == STEP.NORMAL || this.step == STEP.STOP || this.step ==STEP.NO_ACTION) && this.fall_flag == true)
        {
            gameover_fall = true;//추락게임오버판정
            fall_flag = false;//플래그초기화
        }

        if (this.step == STEP.NO_ACTION)//동작정지상태(커맨드입력불가)
        {
            if (this.hang_noaction == false)
            {
                if (game_Management.stage_num_1_4 == true || game_Management.stage_num_1_6)//위치고정시 흐트러짐방지하기위해 계속고정
                {
                    this.transform.position = new Vector3(-12.08f, 0.56f, -1.75f);//좌표위로 위치고정
                    this.transform.localRotation = Quaternion.Euler(0, -270, 0);//회전값고정
                }
                if (game_Management.stage_num_1_7 == true)//위치고정시 흐트러짐방지하기위해 계속고정
                {
                    this.transform.position = new Vector3(-13.23f, 0.56f, -6.309f);//좌표위로 위치고정
                    this.transform.localRotation = Quaternion.Euler(0, -270, 0);//회전값고정
                }
               
                if (game_Management.stage_num_1_9 == true)//투명상태에서 위치고정
                {
                    this.transform.position = new Vector3(-5.80f, 0.55f, -6.49f);//좌표위로 위치고정
                }
            }

            if (ray_distance >= 0.7f && this.gameover_be_caught == false && eyes_pattern.step != eyes_pattern.STEP.CATCH)
            {
                Gretel_cc.Move(Vector3.down * noaction_down_speed * Time.deltaTime);
            }
        }

        if ((this.step == STEP.NORMAL || this.step == STEP.WALK || this.step == STEP.RUN) && this.normal_delay_flag2 == true) //방향키이동
        {

            if (ray_distance >= 2.5f)//지면과 일정거리 이상 떨어져있을경우 LAND 상태로전환
            {
                this.is_ground = false;
            }
            else if (ray_distance >= 0.7f)
            {
                Gretel_cc.Move(Vector3.down * 5.0f * Time.deltaTime);
            }

            Vector3 direcction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //방향키입력,기본상태( S모드 )
            if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
            {
                direcction = new Vector3(Input.GetAxis("Horizontal"), 0, -0.2f); //방향키입력,기본상태( S모드 )
            }
            else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
            {
                direcction = new Vector3(0, 0, 0f);
            }

            //상태별
            if (main_camera_script.e_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(0.2f, 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
            }
            if (main_camera_script.w_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-0.2f, 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
            }
            if (main_camera_script.n_mode == true || main_camera_script.bridge3_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, 0.2f); //방향키입력(N모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")); //방향키입력(N모드)
                }
            }

            if (direcction.sqrMagnitude > 0.01f)//이동상태,방향키 입력시 true
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                      transform.forward, direcction,//현재방향,바라볼방향
                      rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                      );
                transform.LookAt(transform.position + forward);//회전처리

                //대쉬
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    move_dev = 25.0f;
                    this.next_step = STEP.RUN;

                }
                else  //플레이어상태관리,걷기
                {
                    move_dev = 45.0f;
                    this.next_step = STEP.WALK;

                }
            }
            else//정지상태
            {
                this.next_step = STEP.NORMAL;

            }

            //점프
            if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && normal_delay_flag == true && this.main_camera_script.start == false
                 && (this.step == STEP.NORMAL || this.step == STEP.WALK || this.step == STEP.RUN))
            {
                this.next_step = STEP.JUMP;
                jump_power_enable = true;
                jump_enable = true;
                Debug.Log("점프");
                //점프직전 현재 y축저장
                current_y = this.transform.localPosition.y;

                //애니메이션 플래그 ,한프레임 빠른 반응속도 위해 
                jump = true;
                run = false;
                normal = false;
                walk = false;
                land = false;
                land2 = false;
            }

            Gretel_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) / move_dev);//이동처리
        }//end of 이동처리 if

        if ((this.step == STEP.JUMP || this.step == STEP.LAND) && this.jump_time <= 0.7f)//점프상태일경우 이동
        {

           Vector3 direcction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //방향키입력,기본상태( N모드 )
            if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
            {
                direcction = new Vector3(Input.GetAxis("Horizontal"), 0, -1); //방향키입력,기본상태( N모드 )
            }
            else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
            {
                direcction = new Vector3(0, 0, 0f);
            }

            //상태별
            if (main_camera_script.e_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(1, 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
            }
            if (main_camera_script.w_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-1, 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
            }
            if (main_camera_script.n_mode == true || main_camera_script.bridge3_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, 1); //방향키입력(S모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")); //방향키입력(S모드)
                }
            }
            

            if (direcction.sqrMagnitude > 0.01f)//이동상태,방향키 입력시 true
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                    transform.forward, direcction,//현재방향,바라볼방향
                    rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                    );
                transform.LookAt(transform.position + forward);//회전처리

            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                move_dev = 13f;
            }
            else
            {
                move_dev = 20f;
            }
            jump_time += Time.deltaTime;//공중에 떠있는 시간
            Gretel_cc.Move(((direcction * move_speed_jump * Time.deltaTime).normalized) / move_dev);//이동처리
        }//end of 점프상태일경우 이동 if
        if (this.step == STEP.STOP && this.jump_time <= 0.7f)//점프상태일경우 이동
        {

            Vector3 direcction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //방향키입력,기본상태( N모드 )
            if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
            {
                direcction = new Vector3(Input.GetAxis("Horizontal"), 0, -1); //방향키입력,기본상태( N모드 )
            }
            else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
            {
                direcction = new Vector3(0, 0, 0f);
            }

            //상태별
            if (main_camera_script.e_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(1, 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
            }
            if (main_camera_script.w_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-1, 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
            }
            if (main_camera_script.n_mode == true || main_camera_script.bridge3_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, 1); //방향키입력(S모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")); //방향키입력(S모드)
                }
            }

            if (direcction.sqrMagnitude > 0.01f)//이동상태,방향키 입력시 true
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                    transform.forward, direcction,//현재방향,바라볼방향
                    rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                    );
                transform.LookAt(transform.position + forward);//회전처리

            }
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                move_dev += 100.0f * Time.deltaTime;
            }
            else
            {
                move_dev = 45f;
            }
            jump_time += Time.deltaTime;//공중에 떠있는 시간
            Gretel_cc.Move(((direcction * move_speed_jump * Time.deltaTime).normalized) / move_dev);//이동처리
        }//end of 점프상태일경우 이동 if



        if (this.is_ground == false)//접지상태가 아닐때
        {
            this.next_step = STEP.LAND;
        }


        //높은지형 올라갈때 Climb
        if (this.step != STEP.JUMP)
        {
            climb_enable = false; //점프상태일때만 활성화
        }
        //H&G전환
        if ((this.step == STEP.NORMAL || this.step == STEP.WALK || this.step == STEP.RUN) && Vector3.Distance(this.transform.position, gretel_position.transform.position) <= 0.5f
            && hansel_script.hold_hands_X == false)
        {
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && this.h_g_delay == true)
            {
                //위치고정

                //this.transform.localPosition = new Vector3(gretel_position.transform.localPosition.x,this.transform.position.y,gretel_position.transform.localPosition.z);
                this.transform.localRotation = Quaternion.Euler(0, gretel_position.transform.localEulerAngles.y, 0);
                this.transform.position = new Vector3(gretel_position.transform.position.x, this.transform.position.y, gretel_position.transform.position.z);


                //딜레이
                this.h_g_delay = false;
                StartCoroutine("H_G_delay");
                //상태변경
                this.next_step = STEP.H_G_NORMAL;
                this.step = STEP.H_G_NORMAL;

            }
        }
        if (this.step == STEP.H_G_NORMAL || this.step == STEP.H_G_WALK) //H&G상태에서 이동, SOLO상태 해제처리
        {
            rotate_speed = 120f;//회전속도 감소
            if (ray_distance >= 0.7f)
            {
                Gretel_cc.Move(Vector3.down * 5.0f * Time.deltaTime);
            }

            Vector3 direcction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //방향키입력,기본상태( N모드 )
            if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
            {
                direcction = new Vector3(Input.GetAxis("Horizontal"), 0, -1); //방향키입력,기본상태( N모드 )
            }
            else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
            {
                direcction = new Vector3(0, 0, 0f);
            }
            //상태별
            if (main_camera_script.e_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(1, 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal")); //방향키입력 (E모드)
                }
            }
            if (main_camera_script.w_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-1, 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")); //방향키입력(W모드)
                }
            }
            if (main_camera_script.n_mode == true || main_camera_script.bridge3_mode == true)
            {
                if (main_camera_script.vertical_freeze == true)//카메라 회전상태에선 위아래방향키입력불가
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, 1); //방향키입력(S모드)
                }
                else if (main_camera_script.v_and_h_freeze == true)//카메라 회전상태 방향키 이동불가
                {
                    direcction = new Vector3(0, 0, 0f);
                }
                else
                {
                    direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")); //방향키입력(S모드)
                }
            }


            if (direcction.sqrMagnitude > 0.01f)//이동상태,방향키 입력시 true
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                    transform.forward, direcction,//현재방향,바라볼방향
                    rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                    );
                transform.LookAt(transform.position + forward);//회전처리
                move_dev = 50.0f;

                this.next_step = STEP.H_G_WALK;
            }
            else//정지상태
            {
                this.next_step = STEP.H_G_NORMAL;

            }

            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && this.h_g_delay == true)
            {
                //딜레이
                this.h_g_delay = false;
                StartCoroutine("H_G_delay");
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                     Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {

                    this.next_step = STEP.WALK;
                    rotate_speed = 720f;
                }
                else
                {
                    this.next_step = STEP.NORMAL;
                    rotate_speed = 720f;
                }
            }
            if (this.h_g_delay == true)
            {
                Gretel_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) / move_dev);//이동처리
            }



            //핸젤 SOLO모드시 그레텔도 SOLO모드

            if (hansel_script.solo == true)//헨젤 solo모드시 그레텔 상태도 solo
            {
                this.next_step = STEP.NORMAL;
                rotate_speed = 720f;
            }
        }

        //그레텔 위치고정필요할경우

        //게임오버 - 마녀에게 붙잡힘 판정2 스테이지1-2
        if (this.gameover_be_caught2 == true)
        {
            //그레텔위치고정
            this.transform.position = target_position.transform.position;//그레텔위치고정
            this.transform.localEulerAngles = new Vector3(0, this.target_position.transform.root.localEulerAngles.y - 180f, 0);//회전값고정  
        }

        //애니메이션 관리
        Gretel_animator.SetBool("WALK", walk);
        Gretel_animator.SetBool("RUN", run);
        Gretel_animator.SetBool("NORMAL", normal);
        Gretel_animator.SetBool("JUMP", jump);
        Gretel_animator.SetBool("LAND", land);
        Gretel_animator.SetBool("LAND2", land2);
        Gretel_animator.SetBool("H_G_NORMAL", h_g_normal);
        Gretel_animator.SetBool("H_G_WALK", h_g_walk);
        Gretel_animator.SetBool("CLIMB", climb);
        if (gameover_fall == true || gameover_fall2 == true)
        {
            Gretel_animator.SetBool("GAMEOVER_FALL", true);
        }
        else
        {
            Gretel_animator.SetBool("GAMEOVER_FALL", false);
        }

        Gretel_animator.SetBool("GAMEOVER_BE_LOCKED", gameover_be_locked);

        if (gameover_be_caught == true || gameover_be_caught2 == true)
        {
            Gretel_animator.SetBool("GAMEOVER_BE_CAUGHT", true);
        }
        else
        {
            Gretel_animator.SetBool("GAMEOVER_BE_CAUGHT", false);
        }
        //동작관리
        this.step = this.next_step;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            this.transform.position = new Vector3(23.52642f,0.6815f,35.7246f);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            this.transform.position = new Vector3(42.083f, 0.6815f, 26.8116f);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            this.transform.position = new Vector3(2.7343f, -12.6f, 10.25f);
        }
    }//end of Update


    private void OnControllerColliderHit(ControllerColliderHit hit)//땅에 닿은 직후
    {
        if (hit.collider.tag == "Ground" || hit.collider.tag == "snack_item" || hit.collider.tag == "Enemy_Trap" || hit.collider.tag == "cookie_position_change"
            || hit.collider.tag == "Player2" || hit.collider.tag == "bridge2" || hit.collider.tag == "clock_tower" || hit.collider.tag == "clock_Lever" ||
            hit.collider.tag == "elevator" || hit.collider.tag == "Rock" || hit.collider.tag == "Rock_Hansel" || hit.collider.tag == "bridge3"
            || hit.collider.tag == "Clock_E" || hit.collider.tag == "Clock_W" || hit.collider.tag == "Clock_S" || hit.collider.tag == "Clock_N"
            || hit.collider.tag == "none_closet" || hit.collider.tag == "clock_password" || hit.collider.tag == "clock_password2") //전부 Ground판정
        {
            //Debug.Log("접지");
            this.is_ground = true;
            if (this.step == STEP.STOP)//공중에서 땅에 닿았을때
            {
                this.is_ground = true;
                //착지후 바로 이동전환
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                         Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    /* if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                     {
                         this.next_step = STEP.RUN;
                     }
                     else
                     {
                         this.next_step = STEP.WALK;
                     }*/
                }
                else
                {
                    if (normal_delay_flag == true)
                    {
                        this.next_step = STEP.NORMAL;
                        StartCoroutine("Normal_delay");
                        normal_delay_flag = false;
                    }
                }

                land2_stop_timer = 0.0f;//초기화

            }
            else if (this.step == STEP.LAND2)
            {
                this.is_ground = true;
                if (normal_delay_flag2 == true)
                {
                    this.next_step = STEP.NORMAL;
                    StartCoroutine("Normal_delay2");
                    normal_delay_flag2 = false;
                }

                land2_stop_timer = 0.0f;//초기화
            }

            //추락

        }//end of if


        //높은지형에 충돌했을때
       /* if (hit.collider.tag == "Ground_true" && (this.step == STEP.JUMP))
        {
            ground_hit = hit.gameObject;
            climb_enable = true;
            if (hit.gameObject.transform.GetChild(0).tag == "Ground_y_F" || hit.gameObject.transform.GetChild(0).tag == "Ground_y_B" ||
                hit.gameObject.transform.GetChild(0).tag == "Ground_y_L" || hit.gameObject.transform.GetChild(0).tag == "Ground_y_R")
            {
                ground_x = hit.gameObject.transform.GetChild(0).transform.localPosition.x;
                ground_y = hit.gameObject.transform.GetChild(0).transform.localPosition.y;
                ground_z = hit.gameObject.transform.GetChild(0).transform.localPosition.z;
            }
        }*/

        //세이브포인트에 충돌했을때 ,세이브 포인트는 각각 번호별로 처리 
        if (hit.collider.tag == "save_point_1")
        {
            save1 = true;
            save_point1 = true;
        }

        //좌표는 스테이지별로 위치고정시킴
        if (hit.collider.tag == "Pointer" && (this.step == STEP.NORMAL || this.step == STEP.WALK || this.step == STEP.RUN || this.step == STEP.H_G_NORMAL
            || this.step == STEP.H_G_WALK))//점프나 착지상태에서는 좌표인식X
        {
            if (game_Management.stage_num_1_4 == true)//1-4스테이지좌표
            {
                this.transform.position = new Vector3(-12.08f, 0.56f, -1.75f);//좌표위로 위치고정
                this.next_step = STEP.NO_ACTION;//동작불가
                this.transform.localRotation = Quaternion.Euler(0, -270, 0);//회전값고정
                pointer_1_4 = true;
            }
            if (game_Management.stage_num_1_6 == true)
            {
                this.transform.position = new Vector3(-13.23f, 0.56f, -6.309f);//좌표위로 위치고정
                this.next_step = STEP.NO_ACTION;//동작불가
                this.transform.localRotation = Quaternion.Euler(0, -270, 0);//회전값고정
                pointer_1_6 = true;
            }
        }
        //열쇠획득
        if (hit.collider.tag == "Key")
        {
            hit.gameObject.transform.parent = this.gameObject.transform;
            hit.gameObject.transform.position = key_position.transform.position;
            hit.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.get_key = true;//키획득 bool변수 true

        }
    }//end of OnControllerColliderHit

    private void OnTriggerEnter(Collider other)
    {
        //자물쇠오픈
         if (other.tag == "Rock" && this.get_key == true)
         { 
             other.gameObject.GetComponent<BoxCollider>().enabled = false;
             this.get_key = false;//열쇠사용
             other.gameObject.GetComponent<cage_Script>().rock_on = true;//자물쇠 열기 가능
             this.on_rock = true;//자물쇠 열기 가능 , 공용 on_rock

            //헨젤스크립트 snack_point원래대로
            hansel_script.snack_point = hansel_script.snack_point_before;
             
         }
        //자물쇠오픈
        if (other.tag == "Rock_Hansel" && this.get_key == true)
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            this.get_key = false;//열쇠사용
            other.transform.root.gameObject.GetComponent<cage_Script_Hensel>().rock_on = true;//자물쇠 열기 가능
            this.on_rock = true;//자물쇠 열기 가능 , 공용 on_rock

            //헨젤스크립트 snack_point원래대로
            hansel_script.snack_point = hansel_script.snack_point_before;
        }

        // 스낵그라운드
        if (other.tag == "snack_Ground")
        {
            if (this.game_Management.stage_num <= 11)
            {
                snack_ground = true;
                other.gameObject.SetActive(false);


                this.transform.localRotation = Quaternion.Euler(0, -90f, 0);//회전값고정
                this.next_step = STEP.NO_ACTION;//동작불가
            }
            else if (this.game_Management.stage_num == 16)
            {
                snack_ground = true;
                other.gameObject.SetActive(false);

                if (this.main_camera_script.clockwise == true)//시계방향이동중
                {
                    this.transform.localRotation = Quaternion.Euler(0, 0f, 0);//회전값고정
                }
                else
                {
                    this.transform.localRotation = Quaternion.Euler(0, 180f, 0);//회전값고정
                }
                this.next_step = STEP.NO_ACTION;//동작불가
            }
            else if (this.game_Management.stage_num == 19)
            {
                snack_ground = true;
                other.gameObject.SetActive(false);

                this.transform.localRotation = Quaternion.Euler(0, 270f, 0);//회전값고정
                this.next_step = STEP.NO_ACTION;//동작불가
            }
        }

        //데드씬 - 마녀에게잡힘
        if (other.tag == "witch_hand")
        {
            Debug.Log("잡혔음!");
            //상태 즉시변경
            this.next_step = STEP.NO_ACTION;
            this.step = STEP.NO_ACTION;

            //game-over (마녀에게 잡힘)
            this.gameover_be_caught2 = true;

            //컬라이더 off
            this.GetComponent<BoxCollider>().enabled = false;
        }

        //데드씬 - 새장에갇힘
        if (other.tag == "Enemy_Trap")
        {
            //game-over (새장에갇힘)
            this.transform.position = new Vector3(other.transform.position.x, 0.55f, other.transform.position.z);//그레텔위치고정
            this.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정
            //상태 즉시변경
            this.next_step = STEP.NO_ACTION;
            this.step = STEP.NO_ACTION;
            this.gameover_be_locked = true;

            //새장
            this.cage = other.gameObject;
            //새장고정
            other.transform.parent.GetComponent<cage_Script>().hold = true;//새장사라지지않고 유지
            other.transform.parent.GetComponent<cage_Script>().rock1.SetActive(true);//자물쇠등장
            other.transform.parent.GetComponent<cage_Script>().rock2.SetActive(true);//자물쇠등장
            other.transform.GetComponent<BoxCollider>().enabled = false;//새장컬라이더해제
            other.transform.parent.GetComponent<BoxCollider>().enabled = true;
        }

        //벽장텔레포트완료
        if (other.tag == "none_closet")
        {
            this.clock_closet_script.t_fine2 = true;
            this.clock_closet_script.none_closet1.SetActive(false);
            this.clock_closet_script.none_closet2.SetActive(false);
            this.clock_closet_script.none_closet3.SetActive(false);
        }

        if (other.tag == "Ground_true" && (this.step == STEP.JUMP))
        {
            ground_hit = other.gameObject;
            climb_enable = true;
            if (other.gameObject.transform.GetChild(0).tag == "Ground_y_F" || other.gameObject.transform.GetChild(0).tag == "Ground_y_B" ||
                other.gameObject.transform.GetChild(0).tag == "Ground_y_L" || other.gameObject.transform.GetChild(0).tag == "Ground_y_R")
            {
                ground_x = other.gameObject.transform.GetChild(0).transform.localPosition.x;
                ground_y = other.gameObject.transform.GetChild(0).transform.localPosition.y;
                ground_z = other.gameObject.transform.GetChild(0).transform.localPosition.z;
            }
        }
    }

    IEnumerator Normal_delay() //낮은점프 착지시 점프딜레이
    {
        yield return new WaitForSeconds(0.35f);
        //this.next_step = STEP.NORMAL;
        normal_delay_flag = true;
    }
    IEnumerator Normal_delay2()//높은점프 착지시 점프딜레이
    {
        yield return new WaitForSeconds(0.8f);
        //this.next_step = STEP.NORMAL;
        normal_delay_flag2 = true;
    }
    IEnumerator H_G_delay()//H&G 모드 딜레이
    {
        yield return new WaitForSeconds(0.5f);
        h_g_delay = true;
    }
    IEnumerator climb_start()//Hang_Jump 매달린후 애니메이션,지형에 맞춰 위치변경
    {
        yield return new WaitForSeconds(1.0f);
        climb_start_up = true;
    }
    IEnumerator climb_start2()//Hang_Jump 매달린후 애니메이션,지형에 맞춰 위치변경
    {
        yield return new WaitForSeconds(1.3f);
        climb_start_up2 = true;
    }
    IEnumerator climb_position_up()//Hang_Jump 매달린후 애니메이션,Vector.up수치조절
    {
        climb_up_speed = 2.0f;
        yield return new WaitForSeconds(0.1f);
        climb_up_speed = 1.3f;
        yield return new WaitForSeconds(0.2f);
        climb_up_speed = 1.8f;
    }
    IEnumerator climb_position_forward()//Hang_Jump 매달린후 애니메이션,Vector.forward수치조절
    {
        climb_forward_speed = 1.75f;
        yield return new WaitForSeconds(0.1f);
        climb_forward_speed = 2.05f;
    }
    IEnumerator climb_fine()//올라가기종료
    {
        noaction_down_speed = 0.75f;
        yield return new WaitForSeconds(0.6f);
        noaction_down_speed = 5.0f;
        this.next_step = STEP.NORMAL;
    }

}


