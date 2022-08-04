using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_camera : MonoBehaviour
{

    GameObject main_player;
    main_player_1 Gretel_Script;//그레텔 스크립트   
    public GameObject clock_lever;//시계레버
    clock_Lever clock_lever_script;//시계레버 스크립트
    public GameObject clock_closet;//clock모드 벽장
    clock_closet clock_closet_script;//clock모드 벽장 스크립트

    //이동 - 로테이션(rotation)
    float rotate_speed = 320.0f;//회전속도
    float move_speed = 10f;//이동속도
    float move_dev_xz;
    float move_dev_y;

    //좌표
    Vector3 destination;//목적좌표
    public bool start = false;
    public bool transform_start = false;
    float x_horizontal = 0.0f;
    float z_vertical = 0.0f;
    float y_height = 0.0f;
    public bool x_arrive = false;
    public bool y_arrive = false;
    public bool z_arrive = false;
    bool rotation_flag = false;//카메라 좌표이동시 회전유무

    //좌표리스트
    public bool ground_mode = false;
    public bool bridge2_mode = false;
    public bool bridge3_mode = false;
    public bool clock_tower_mode = false;
    public bool clock_lever_mode_control = false;//5
    public bool e_mode;//동 1
    public bool w_mode;//서 2
    public bool s_mode;//남 3
    public bool n_mode;//북 4
    int before_mode = 0;//이전모드

    //레이캐스트가 충돌하는 컬라이더 태그
    public bool ray_e;
    public bool ray_w;
    public bool ray_s;
    public bool ray_n;

    //이동방향
    public bool clockwise;//시계방향
    public bool anticlockwise;//반시계방향

    public bool camera_rotation_flag = false;//카메라 회전중일시 //y축
    public bool camera_rotation_flag2 = false;//카메라 회전중일시 //x축
    public bool camera_rotation_flag_lever = false;//카메라레버모드 회전중일때

    public bool vertical_freeze = false;//카메라 회전중일때 캐릭터vertical방향키 입력 X
    public bool v_and_h_freeze = false;//카메라 회전중일때 캐릭터 방향키 입력 X
    public float v_and_h_freeze_timer = 0.0f;

    //좌표초기화용
    public bool V_flag_ground = true;//스테이지별 카메라좌표
    public bool V_flag_bridge2 = false;//스테이지별 카메라좌표
    public bool V_flag_bridge3 = false;//스테이지별 카메라좌표
    public bool V_flag_clock_tower = false;//스테이지별 카메라좌표
    public bool V_flag_clock_lever = false;//스테이지별 카메라좌표
    bool V_flag_E = false;//동
    bool V_flag_W = false;//서
    bool V_flag_S = false;//남
    bool V_flag_N = false;//북
    bool V_flag2_E = false;//동, 텔레포트 180도 회전용
    bool V_flag2_W = false;//서, 텔레포트 180도 회전용
    public float Current_camera_rotation_y;//y축카메라 회전이동
    public float Bridge_Current_camera_rotation_y;//bridge모드 y축
    float Current_camera_rotation_x = 9.73f;//x축카메라 회전이동

    //bridge3모드 컬라이더 태그변경
    public GameObject bridge_collider;

    CharacterController Camera_cc;//캐릭터컨트롤러

    //시계탑 외곽 4방향 카메라회전 - 회전하면서 캐릭터점프시 카메라워킹부자연스러움 방지 
    bool ewns_rotation = false;

    // Start is called before the first frame update
    void Start()
    {
        main_player = GameObject.FindGameObjectWithTag("Player");
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        Camera_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
        clock_lever_script = this.clock_lever.GetComponent<clock_Lever>();//시계 레버 스크립트
        clock_closet_script = clock_closet.GetComponent<clock_closet>();//clock모드 텔레포트 벽장 스크립트
    }

    // Update is called once per frame
    void Update()
    {
        //clock_tower모드
        if (clock_tower_mode == true)
        {
            if (this.clock_lever_mode_control == true)// E->CLOCKTOWER
            {
                if (this.Current_camera_rotation_y > 180f)
                {
                    vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                    Current_camera_rotation_y -= 120.0f * Time.deltaTime;

                }
                else
                {
                    vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                    Current_camera_rotation_y = 180f;
                    camera_rotation_flag = false;
                    camera_rotation_flag_lever = false;//시계레버
                }
                if (this.Current_camera_rotation_x < 16.88f)
                {
                    vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                    Current_camera_rotation_x += 10.0f * Time.deltaTime;

                }
                else
                {
                    vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                    Current_camera_rotation_x = 16.88f;
                    camera_rotation_flag2 = false;
                    camera_rotation_flag_lever = false;//시계레버
                }
                if (this.clock_lever_script.on == false) //->E
                {
                    //좌표이동
                    this.V_flag_E = true;

                    this.e_mode = true;
                    //다른플래그 초기화
                    this.s_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    this.clock_lever_mode_control = false;

                    x_arrive = false;
                    y_arrive = false;
                    z_arrive = false;

                    //이전 방향값 저장
                    before_mode = 5;
                    camera_rotation_flag = true;//카메라회전on
                    camera_rotation_flag2 = false;//카메라회전on
                    camera_rotation_flag_lever = true;//시계레버
                    //시계or반시계 방향
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
            }
            if (this.e_mode == true) //E->N OR E->S OR E->Clock_Lever
            {

                //e_mode는 카메라 y축 270도
                if (this.before_mode == 3 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y > -90f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y -= 120.0f * Time.deltaTime;

                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 270f;
                        camera_rotation_flag = false;
                    }
                }
                else if (this.before_mode == 4 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y < 270f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y += 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 270f;
                        camera_rotation_flag = false;
                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종료
                        }
                    }
                }
                else if (this.before_mode == 2 && camera_rotation_flag == true) //텔레포트
                {
                    if (this.V_flag2_E == true)
                    {
                        this.transform.Translate(Vector3.left * 60.0f * Time.deltaTime);
                    }
                    if (this.Current_camera_rotation_y < 270f)
                    {
                        //이동불가
                        move_dev_xz = 0.5f;
                        this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
                        this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;

                        Current_camera_rotation_y += 80.0f * Time.deltaTime;
                    }
                    else
                    {
                        //이동불가 해제
                        this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
                        this.Gretel_Script.step = main_player_1.STEP.NORMAL;

                        Current_camera_rotation_y = 270f;
                        camera_rotation_flag = false;

                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종료
                        }
                    }
                }
                else if(this.before_mode == 5 && (camera_rotation_flag == true || camera_rotation_flag2 == true))
                {
                    if (this.Current_camera_rotation_y < 270f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y += 120.0f * Time.deltaTime;

                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 270f;
                        camera_rotation_flag = false;
                        camera_rotation_flag_lever = false;//시계레버
                    }
                    if (this.Current_camera_rotation_x > 9.73f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_x -= 10.0f * Time.deltaTime;

                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_x = 9.73f;
                        camera_rotation_flag2 = false;
                        camera_rotation_flag_lever = false;//시계레버
                    }

                }

                if (this.ray_s == true && this.vertical_freeze == false) //E->S
                {
                    //좌표이동
                    this.V_flag_S = true;

                    Debug.Log("확인");
                    this.s_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    //이전 방향값 저장
                    before_mode = 1;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
                if (this.ray_n == true && this.vertical_freeze == false) //E->N 
                {
                    //좌표이동
                    this.V_flag_N = true;

                    this.n_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 1;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
                if (this.ray_w == true && this.vertical_freeze == false) //E->W , 텔레포트이동
                {
                    //좌표이동
                    StartCoroutine("T_W");
                    this.V_flag2_W = true;
                    this.transform.localPosition = new Vector3(23.0f, 4.46f, 0.68f);

                    this.w_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.n_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 1;
                    camera_rotation_flag = true;//카메라회전on

                }
                if (this.clock_lever_script.on == true) //E->CLOCK_LEVER
                {
                    //좌표이동
                    this.V_flag_clock_lever = true;

                    this.clock_lever_mode_control = true;
                    //다른플래그 초기화
                    this.s_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    this.e_mode = false;
                    //이전 방향값 저장
                    before_mode = 1;
                    camera_rotation_flag = true;//카메라회전on
                    camera_rotation_flag2 = true;//카메라회전on
                    camera_rotation_flag_lever = true;//시계레버

                    Debug.Log("회전C");
                }

            }//e_mode

            if (this.w_mode == true) //W->S OR W->E
            {
                //w_mode는 카메라 y축 90도
                if (this.before_mode == 3 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y < 90f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y += 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 90f;
                        camera_rotation_flag = false;
                    }
                }
                else if (this.before_mode == 4 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y > 90f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y -= 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 90f;
                        camera_rotation_flag = false;

                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종류
                        }
                    }
                }
                else if (this.before_mode == 1 && camera_rotation_flag == true)//E->W텔레포트
                {
                    if (this.V_flag2_W == true)
                    {
                        this.transform.Translate(Vector3.left * 60.0f * Time.deltaTime);
                    }
                    if (this.Current_camera_rotation_y < 450f)
                    {
                        this.move_dev_xz = 0.5f;
                        //이동불가
                        this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
                        this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;

                        Current_camera_rotation_y += 80.0f * Time.deltaTime;
                    }
                    else
                    {
                        //이동불가해제
                        this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
                        this.Gretel_Script.step = main_player_1.STEP.NORMAL;

                        Current_camera_rotation_y = 90f;
                        camera_rotation_flag = false;

                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종류
                        }
                    }
                }

                if (this.ray_s == true && this.vertical_freeze == false) //W->S
                {
                    //좌표이동
                    this.V_flag_S = true;

                    this.s_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    //이전 방향값 저장
                    before_mode = 2;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
                if (this.ray_n == true && this.vertical_freeze == false) //W->N 
                {
                    //좌표이동
                    this.V_flag_N = true;

                    this.n_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 2;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
                if (this.ray_e == true && this.vertical_freeze == false) //W->E 텔레포트이동 
                {
                    //좌표이동
                    StartCoroutine("T_E");
                    this.V_flag2_E = true;
                    //this.V_flag_E = true;
                    this.transform.localPosition = new Vector3(-23.3f, 4.46f, 0.68f);

                    this.e_mode = true;
                    //다른플래그 초기화
                    this.n_mode = false;
                    this.w_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 2;
                    camera_rotation_flag = true;//카메라회전on
                    //순간이동
                   // this.Current_camera_rotation_y = 245f;
                }
            }//w_mode

            if (this.s_mode == true) //S->E OR S->W
            {

                //s_mode는 카메라 y축 0도
                if (this.before_mode == 1 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y < 360f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y += 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 0f;
                        camera_rotation_flag = false;
                    }
                }
                else if (this.before_mode == 2 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y > 0f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y -= 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 0f;
                        camera_rotation_flag = false;
                    }
                }

                if (this.ray_e == true && this.vertical_freeze == false) //S->E
                {
                    //좌표이동
                    this.V_flag_E = true;

                    this.e_mode = true;
                    //다른플래그 초기화
                    this.s_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    //이전 방향값 저장
                    before_mode = 3;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
                if (this.ray_w == true && this.vertical_freeze == false) //S->W 
                {
                    //좌표이동
                    this.V_flag_W = true;

                    this.w_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.n_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 3;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
            }//s_mode
            if (this.n_mode == true) //N->E OR N->W
            {
                //n_mode는 카메라 y축 180도
                if (this.before_mode == 1 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y > 180f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y -= 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 180f;
                        camera_rotation_flag = false;

                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종류
                        }
                    }
                }
                else if (this.before_mode == 2 && camera_rotation_flag == true)
                {
                    if (this.Current_camera_rotation_y < 180f)
                    {
                        vertical_freeze = true;//캐릭터 위아래 방향키입력불가
                        Current_camera_rotation_y += 120.0f * Time.deltaTime;
                    }
                    else
                    {
                        vertical_freeze = false;//캐릭터 위아래 방향키입력가능
                        Current_camera_rotation_y = 180f;
                        camera_rotation_flag = false;

                        if (clock_closet_script.t_ing == true)//텔레포트 도중일경우
                        {
                            clock_closet_script.t_ing = false;//텔레포트종료
                            clock_closet_script.t_fine = true;//텔레포트종류
                        }
                    }
                }

                if (this.ray_e == true && this.vertical_freeze == false) //N->E
                {
                    //좌표이동
                    this.V_flag_E = true;

                    this.e_mode = true;
                    //다른플래그 초기화
                    this.s_mode = false;
                    this.w_mode = false;
                    this.n_mode = false;
                    //이전 방향값 저장
                    before_mode = 4;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = true;
                    this.anticlockwise = false;
                }
                if (this.ray_w == true && this.vertical_freeze == false) //N->W 
                {
                    //좌표이동
                    this.V_flag_W = true;

                    this.w_mode = true;
                    //다른플래그 초기화
                    this.e_mode = false;
                    this.n_mode = false;
                    this.s_mode = false;
                    //이전 방향값 저장
                    before_mode = 4;
                    camera_rotation_flag = true;//카메라회전on
                    //시계or반시계 방향
                    this.clockwise = false;
                    this.anticlockwise = true;
                }
            }//n_mode

            this.transform.localRotation = Quaternion.Euler(Current_camera_rotation_x, Current_camera_rotation_y, 0);//회전값고정 
        }//end of if clock_tower모드


        //좌표변경
        if (V_flag_ground == true)
        {
            destination = new Vector3(1.36f, 0.9f, -3.76f);//좌표초기화
            move_dev_xz = 0.1f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            V_flag_ground = false;//다음 스테이지 변환때 true
        }
        if (V_flag_bridge2 == true)
        {
            destination = new Vector3(0.554f, 1.79f, -5.04f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            V_flag_bridge2 = false;//다음 스테이지 변환때 true
        }
        if (V_flag_bridge3 == true)
        {
            destination = new Vector3(-0.234f, 1.71f, 4.77f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            V_flag_bridge3 = false;//다음 스테이지 변환때 true
        }
        if (V_flag_clock_tower == true)
        {
            destination = new Vector3(-0.07f, 4.46f, -9.21f);//좌표초기화
            move_dev_xz = 0.1f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            V_flag_clock_tower = false;//다음 스테이지 변환때 true

            //clock_towermode온
            s_mode = true;
        }
        if (V_flag_clock_lever == true)
        {
            destination = new Vector3(7.6f, 8.91f, 9.73f);//좌표초기화
            move_dev_xz = 0.1f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            V_flag_clock_lever = false;//다음 스테이지 변환때 true
        }
        if (V_flag_E == true)
        {
            destination = new Vector3(9f, 4.46f, 0.68f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.12f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            //카메라워킹 부자연스러움 방지, 4방향회전
            ewns_rotation = true;

            V_flag_E = false;//다음 스테이지 변환때 true
        }
        if (V_flag_W == true)
        {
            destination = new Vector3(-9.3f, 4.46f, 0.68f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

           //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            //카메라워킹 부자연스러움 방지, 4방향회전
            ewns_rotation = true;

            V_flag_W = false;//다음 스테이지 변환때 true
        }
        if (V_flag_S == true)
        {
            destination = new Vector3(-0.07f, 4.46f, -9.21f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            //카메라워킹 부자연스러움 방지, 4방향회전
            ewns_rotation = true;

            V_flag_S = false;//다음 스테이지 변환때 true         
        }
        if (V_flag_N == true)
        {
            destination = new Vector3(0.3f, 4.46f, 8.52f);//좌표초기화
            move_dev_xz = 0.25f;
            move_dev_y = 0.012f;
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            rotation_flag = false;//회전은X

            //arrive초기화
            x_arrive = false;
            y_arrive = false;
            z_arrive = false;

            //카메라워킹 부자연스러움 방지, 4방향회전
            ewns_rotation = true;

            V_flag_N = false;//다음 스테이지 변환때 true
        }

        //특정 모드 카메라회전
        if (this.bridge3_mode == true)
        {
            //경계 컬라이더 태그변경
            this.bridge_collider.tag = "bridge3";
            if (this.Bridge_Current_camera_rotation_y < 180f)
            {
                this.Bridge_Current_camera_rotation_y += 250 * Time.deltaTime;
                //방향키 이동불가
                this.v_and_h_freeze = true;
                v_and_h_freeze_timer = 0.0f;
            }
            else
            {
                this.Bridge_Current_camera_rotation_y = 180f;
                //방향키 이동불가 해제
                if (v_and_h_freeze_timer < 0.2f)
                {
                    v_and_h_freeze_timer += Time.deltaTime;
                }
                else
                {
                    this.v_and_h_freeze = false;
                }
            }
            this.transform.localRotation = Quaternion.Euler(9.73f, Bridge_Current_camera_rotation_y, 0);//회전값고정 
        }
        if (this.bridge2_mode == true)
        {
            this.bridge_collider.tag = "bridge2";
            if (this.Bridge_Current_camera_rotation_y > 0.0f)
            {
                this.Bridge_Current_camera_rotation_y += -250 * Time.deltaTime;
                //방향키 이동불가
                this.v_and_h_freeze = true;
                v_and_h_freeze_timer = 0.0f;
            }
            else
            {
                this.Bridge_Current_camera_rotation_y = 0.0f;
                //방향키 이동불가 해제
                if (v_and_h_freeze_timer < 0.2f)
                {
                    v_and_h_freeze_timer += Time.deltaTime;
                }
                else
                {
                    this.v_and_h_freeze = false;
                }
            }
            this.transform.localRotation = Quaternion.Euler(9.73f, this.Bridge_Current_camera_rotation_y, 0);//회전값고정 
        }

        //좌표까지 이동법
        if (start == true)//좌표이동시작
        {
            //Debug.Log("목표y" + destination.y + " 현재y" + this.transform.localPosition.y + " y이동방향" + y_height);
            if (transform_start == true)//이동시작 ,한번만 실행
            {

                if (this.transform.localPosition.x > destination.x)
                {
                    x_horizontal = -1;
                }
                else
                {
                    x_horizontal = 1;
                }

                if (this.transform.localPosition.z > destination.z)
                {
                    z_vertical = -1;
                }
                else
                {
                    z_vertical = 1;
                }


                if (this.transform.localPosition.y > destination.y)
                {
                    y_height = -1;
                }
                else
                {
                    y_height = 1;
                }

                transform_start = false;

            }//end of if(한번만실행)

            //x축도착시
            if (x_horizontal == -1)
            {
                if (this.transform.localPosition.x < destination.x)
                {
                    x_arrive = true;
                }
            }
            else if (x_horizontal == 1)
            {
                if (this.transform.localPosition.x > destination.x)
                {
                    x_arrive = true;
                }
            }

            //z축도착시
            if (z_vertical == -1)
            {
                if (this.transform.localPosition.z < destination.z)
                {
                    z_arrive = true;
                }
            }
            else if (z_vertical == 1)
            {
                if (this.transform.localPosition.z > destination.z)
                {
                    z_arrive = true;
                }
            }

            //y축도착시
            if (y_height == -1)
            {
                if (this.transform.localPosition.y < destination.y)
                {
                    y_arrive = true;
                }
            }
            else if (y_height == 1)
            {
                if (this.transform.localPosition.y > destination.y)
                {
                    y_arrive = true;
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
            if (y_arrive == true)
            {
                y_height = 0;
            }



            //도착완료
            if (x_arrive == true && z_arrive == true && y_arrive == true)
            {
                //초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;
                start = false;
                ewns_rotation = false;

                //카메라시점고정
                if (this.ground_mode == true)
                {
                    this.transform.localPosition = new Vector3(1.36f, 0.9f, -3.76f);
                }
                if (this.bridge2_mode == true)
                {
                    this.transform.localPosition = new Vector3(0.554f, 1.79f, -5.04f);
                }
                if (this.clock_tower_mode == true)
                {
                    this.transform.localPosition = new Vector3(-0.07f, 4.46f, -9.21f);
                }
                if (this.clock_lever_script.on == true)
                {
                    this.transform.localPosition = new Vector3(7.6f, 8.91f, 9.73f);
                }
                   
            }

            Vector3 direcction = new Vector3(x_horizontal, y_height, z_vertical); //이동방향
            //Vector3 direction_y = new Vector3(0, y_height, 0); //이동방향
            if (direcction.sqrMagnitude > 0.01f)//이동상태
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                      transform.forward, direcction,//현재방향,바라볼방향
                      rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                      );
                if (rotation_flag == true)//카메라상태시 회전할지 안할지 선택
                {
                    transform.LookAt(transform.localPosition + forward);//회전처리
                }

            }

            if ((this.Gretel_Script.step == main_player_1.STEP.JUMP || this.Gretel_Script.step == main_player_1.STEP.LAND || this.Gretel_Script.step == main_player_1.STEP.STOP))
            {
                if (this.ewns_rotation == true)
                {
                    Camera_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * move_dev_xz);//이동처리
                }
            }
            else
            {
                Camera_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * move_dev_xz);//이동처리
            }

        }
    }//end of Update
    IEnumerator T_E()//E모드로 텔레포트
    {      
        yield return new WaitForSeconds(0.5f);
        this.V_flag_E = true;
        this.V_flag2_E = false;
    }
    IEnumerator T_W()//E모드로 텔레포트
    {
        yield return new WaitForSeconds(0.5f);
        this.V_flag_W = true;
        this.V_flag2_W = false;
    }
}


