using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_position : MonoBehaviour
{
    GameObject main_player;
    public GameObject management;
    main_player_1 Gretel_Script;//그레텔 스크립트             
    game_management game_Management;//게임관리 스크립트
    public GameObject Soul;//핸젤 영혼 
    Hansel_Script hansel_script;//헨젤스크립트

    CharacterController Camera_cc;//캐릭터컨트롤러
   

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

    //좌표초기화용
    public bool V_flag = true;//스테이지별 카메라좌표
    public bool T_flag = false;//텔레포트용 카메라좌표1
    public bool T_flag2 = false;//텔레포트용 카메라좌표2
    public bool V_flag_f = true;//필드이동용 카메라좌표

    //씬전환관련
    public bool stage_num_1_2_fin = false;
    public bool stage_num_1_3_fin = false;

    //카메라이동범위
    float limit_x_max_point;
    float limit_y_max_point;
    float limit_x_min_point;
    float limit_y_min_point;
    bool limit_max_x = false;
    bool limit_max_y = false;
    bool limit_min_x = false;
    bool limit_min_y = false;
    //카메라 필드이동이후 이동정지
    bool stop = false;
    float stop_x_position = 0.0f;

    //벽장텔레포트완료
    bool teleport_during =false;
    public bool teleport_comp = false;


    //필드별 사양 필드1 -> x축:max존재 y축:max존재     필드2 -> x축:max,min존재 y축:제한없음

    // Start is called before the first frame update
    void Start()
    {
        main_player = GameObject.FindGameObjectWithTag("Player");
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔관리스크립트
        Camera_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러텔스크립트
        game_Management = management.GetComponent<game_management>();//게임매니지먼트
        hansel_script = GameObject.FindGameObjectWithTag("Player2").GetComponent<Hansel_Script>();//헨젤관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        //필드1
        if (this.Gretel_Script.field1 == true)
        {
            limit_x_max_point = 15.5f;
            limit_y_max_point = 20.3f;
        }
        else if (this.Gretel_Script.field2 == true)//필드2
        {
            limit_x_max_point = 50f;
            limit_x_min_point = 18f;
        }
        //카메라이동범위제한
        if (this.Gretel_Script.transform.position.x >= limit_x_max_point)//x축범위제한
        {
            limit_max_x = true;
        }
        else
        {
            limit_max_x = false;
        }
        if (this.Gretel_Script.transform.position.x <= limit_x_min_point)//x축범위제한
        {
            limit_min_x = true;
        }
        else
        {
            limit_min_x = false;
        }


        if (this.Gretel_Script.transform.position.y >= limit_y_max_point)//y축범위제한
        {
            limit_max_y = true;

        }
        else
        {
            limit_max_y = false;
        }

        if (this.Gretel_Script.transform.position.y <= limit_y_max_point)//y축범위제한
        {
            limit_min_y = true;

        }
        else
        {
            limit_min_y = false;
        }

        //필드이동후 카메라이동정지
        if (this.stop == true)
        {
            if (Gretel_Script.field2 == true)
            {
                stop_x_position = 20f;
                if (this.Gretel_Script.transform.position.x > stop_x_position)
                {
                    stop = false;
                }
            }
            else if (Gretel_Script.field1 == true)
            {
                stop_x_position = 15.5f;
                if (this.Gretel_Script.transform.position.x < stop_x_position)
                {
                    stop = false;
                }
            }
        }

        //스테이지1
        if (game_Management.step == game_management.STEP.STAGE1)
        {
            //필드변경 (우선순위1)
            if (V_flag_f == true && Gretel_Script.field1 == true)
            {
                destination = new Vector3(15.5f, this.transform.position.y, this.transform.position.z);//좌표초기화
                x_arrive = false;
                y_arrive = true;//y축이동X
                z_arrive = true;//z축이동X

                move_dev_xz = 0.1f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag_f = false;//다음 스테이지 변환때 true

                this.stop = true;//필드이동시 이동후 카메라정지
            }
            else if (V_flag_f == true && Gretel_Script.field2 == true)
            {
                destination = new Vector3(20.0f, this.transform.position.y, this.transform.position.z);//좌표초기화
                x_arrive = false;
                y_arrive = true;//y축이동X
                z_arrive = true;//z축이동X

                move_dev_xz = 0.1f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag_f = false;//다음 스테이지 변환때 true

                this.stop = true;//필드이동시 이동후 카메라정지
            }
            //스테이지별 카메라 좌표변경 (우선순위2)
            if (game_Management.stage_num_1_1 == true && V_flag == true) //1-1좌표
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(9.3f, 1.71f, -4.08f);//좌표초기화
                move_dev_xz = 0.25f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            else if (game_Management.stage_num_1_2 == true && V_flag == true) //1-2좌표
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(8.25f, 1.5f, 5.05f);//좌표초기화
                move_dev_xz = 0.2f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            else if (game_Management.stage_num_1_3 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(-0.4f, 0.5f, -1.14f);//좌표초기화
                move_dev_xz = 0.06f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            else if (game_Management.stage_num_1_4 == true)
            {
                this.transform.position = this.main_player.transform.position;
            }
            else if (game_Management.stage_num_1_6 == true)
            {
                this.transform.position = this.main_player.transform.position;
            }
            else if (game_Management.stage_num_1_7 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(-12.466f, -0.11f, -4.1f);//좌표초기화
                move_dev_xz = 0.1f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            

            else if (T_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(4.261f, 9.1249f, 0.446f);//좌표초기화
                move_dev_xz = 0.0f;
                move_dev_y = 0.0f;
                StartCoroutine("move_xz_accel");//xz속도 점점가속
                StartCoroutine("move_y_accel");//y속도 점점가속
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                teleport_during = true;//텔레포트도중
                T_flag = false;//다음 스테이지 변환때 true

            }
            else if (T_flag2 == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(3.88f, 0.551f, -4.36f);//좌표초기화
                move_dev_xz = 0.1f;
                move_dev_y = 0.0f;
                StartCoroutine("move_y_accel");//y속도 점점가속
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                teleport_during = true;//텔레포트도중
                T_flag2 = false;//다음 스테이지 변환때 true
            }

            else if ((game_Management.stage_num_1_8 == true || game_Management.stage_num_1_10 == true || game_Management   //스테이지 1-8 1-10 1-12 1-13
                .stage_num_1_12 == true || game_Management.stage_num_1_13 == true || game_Management.stage_num_1_14 == true
                || game_Management.stage_num_1_16 == true || game_Management.stage_num_1_18 == true || game_Management.stage_num_1_19 == true ||
                 game_Management.stage_num_1_21 == true || game_Management.stage_num_1_22 == true) && start == false)//스테이지 변환시 실행X -> start == false
            {
                if (game_Management.stage_num == 14 && hansel_script.gameover_be_caught2_hansel == true)//헨젤game over 카메라position
                {
                    this.transform.position = hansel_script.gameObject.transform.position;
                }
                else if (stop == false)//필드이동완료,x축고정해제
                {
                    if (Gretel_Script.field1 == true)
                    {
                        if (this.limit_max_x == false && this.limit_max_y == false)
                        {
                            this.transform.position = this.main_player.transform.position;
                        }
                        else if (this.limit_max_x == true && this.limit_max_y == false)
                        {
                            this.transform.position = new Vector3(this.limit_x_max_point, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                        else if (this.limit_max_x == false && this.limit_max_y == true)
                        {
                            this.transform.position = new Vector3(this.main_player.transform.position.x, this.limit_y_max_point, this.main_player.transform.position.z);
                        }
                        else
                        {
                            this.transform.position = new Vector3(this.limit_x_max_point, this.limit_y_max_point, this.main_player.transform.position.z);
                        }
                    }
                    else if (Gretel_Script.field2 == true)
                    {
                        if (this.limit_max_x == false && this.limit_min_x == false)
                        {
                            this.transform.position = this.main_player.transform.position;
                        }
                        else if (this.limit_max_x == true && this.limit_min_x == false)
                        {
                            this.transform.position = new Vector3(this.limit_x_max_point, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                        else if (this.limit_max_x == false && this.limit_min_x == true)
                        {
                            this.transform.position = new Vector3(this.limit_x_min_point, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                    }
                }              
                else //필드이동 직후, x축이동불가
                {
                    if (Gretel_Script.field1 == true)
                    {
                        if (this.limit_max_x == false && this.limit_max_y == false)
                        {
                            this.transform.position = new Vector3(this.stop_x_position,this.main_player.transform.position.y,this.main_player.transform.position.z );
                        }
                        else if (this.limit_max_x == true && this.limit_max_y == false)
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                        else if (this.limit_max_x == false && this.limit_max_y == true)
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.limit_y_max_point, this.main_player.transform.position.z);
                        }
                        else
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.limit_y_max_point, this.main_player.transform.position.z);
                        }
                    }
                    else if (Gretel_Script.field2 == true)
                    {
                        if (this.limit_max_x == false && this.limit_min_x == false)
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                        else if (this.limit_max_x == true && this.limit_min_x == false)
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                        else if (this.limit_max_x == false && this.limit_min_x == true)
                        {
                            this.transform.position = new Vector3(this.stop_x_position, this.main_player.transform.position.y, this.main_player.transform.position.z);
                        }
                    }
                }
               

            }
            else if (game_Management.stage_num_1_9 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(8.26f, 2.35f, 1.90f);//좌표초기화
                move_dev_xz = 0.25f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            
            else if (game_Management.stage_num_1_11 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = false;
                z_arrive = false;

                destination = new Vector3(0.69f, 8.44f, -3.09f);//좌표초기화
                move_dev_xz = 0.1f;
                move_dev_y = 0.012f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            else if (game_Management.stage_num_1_17 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = true;//y축고정
                z_arrive = false;

                destination = new Vector3(29.14f, -3.09f, 29.93f);//좌표초기화
                move_dev_xz = 0.1f;
                move_dev_y = 0.1f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
            else if (game_Management.stage_num_1_20 == true && V_flag == true)
            {
                //도착플래그초기화
                x_arrive = false;
                y_arrive = true;//y축고정
                z_arrive = false;

                destination = new Vector3(-26.87f, -13.96f, 6.07f);//좌표초기화
                move_dev_xz = 0.1f;
                move_dev_y = 0.1f;
                start = true;//좌표스타트
                transform_start = true;//백터방향초기화
                rotation_flag = false;//회전은X
                V_flag = false;//다음 스테이지 변환때 true
            }
        }
        else
        {
            this.transform.position = this.main_player.transform.position;
        }


        //좌표까지 이동법
        if (start == true)//좌표이동시작
        {
           // Debug.Log("목표x"+ destination.x + " 현재x" + this.transform.position.x + " x이동방향" + x_horizontal);
            if (transform_start == true)//이동시작 ,한번만 실행
            {

                if (this.transform.position.x > destination.x)
                {
                    x_horizontal = -1;
                }
                else
                {
                    x_horizontal = 1;
                }

                if (this.transform.position.z > destination.z)
                {
                    z_vertical = -1;
                }
                else
                {
                    z_vertical = 1;
                }


                if (this.transform.position.y > destination.y)
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
                if (this.transform.position.x < destination.x)
                {
                    x_arrive = true;
                }
            }
            else if (x_horizontal == 1)
            {
                if (this.transform.position.x > destination.x)
                {
                    x_arrive = true;
                }
            }

            //z축도착시
            if (z_vertical == -1)
            {
                if (this.transform.position.z < destination.z)
                {
                    z_arrive = true;
                }
            }
            else if (z_vertical == 1)
            {
                if (this.transform.position.z > destination.z)
                {
                    z_arrive = true;
                }
            }

            //y축도착시
            if (y_height == -1)
            {
                if (this.transform.position.y < destination.y)
                {
                    y_arrive = true;
                }
            }
            else if (y_height == 1)
            {
                if (this.transform.position.y > destination.y)
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

                //씬1-2끝
                if (this.game_Management.stage_num_1_2 == true)
                {
                    this.stage_num_1_2_fin = true;
                }
                if (this.game_Management.stage_num_1_3 == true)//씬1-3끝
                {
                    this.stage_num_1_3_fin = true;
                }
                //씬1-9일경우
                if (this.game_Management.stage_num_1_9 == true)
                {
                    StartCoroutine("camera_position_hansel");
                }
                //씬1-10일경우 텔레포트완료
                if (teleport_during == true)
                {
                    //그레텔 초기화
                    this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
                    //플래그 초기화
                    this.teleport_comp = true;//텔레포트완료
                    this.teleport_during = false;
                }

            }

            Vector3 direcction = new Vector3(x_horizontal,0, z_vertical); //이동방향
            Vector3 direction_y = new Vector3(0, y_height, 0); //이동방향

            if (direcction.sqrMagnitude > 0.01f)//이동상태
            {
                //회전처리
                Vector3 forward = Vector3.Slerp(
                      transform.forward, direcction,//현재방향,바라볼방향
                      rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direcction) //회전속도
                      );
                if (rotation_flag == true)//카메라상태시 회전할지 안할지 선택
                {
                    transform.LookAt(transform.position + forward);//회전처리
                }

            }

            Camera_cc.Move(((direction_y * move_speed * 0.1f * Time.deltaTime).normalized) * move_dev_y);//이동처리
            Camera_cc.Move(((direcction * move_speed * Time.deltaTime).normalized) * move_dev_xz);//이동처리

            if (this.game_Management.stage_num_1_17 == true)//y축고정
            {
                this.transform.localPosition = new Vector3(this.transform.position.x, -3.09f, this.transform.position.z);
            }
            if (this.game_Management.stage_num_1_20 == true)//y축고정
            {
                this.transform.localPosition = new Vector3(this.transform.position.x, -13.96f, this.transform.position.z);
            }
            if (stop == true)//필드이동중
            {
                this.transform.position = new Vector3(this.transform.position.x, this.main_player.transform.position.y, this.main_player.transform.position.z);
            }

        }//end of start 좌표이동

    }//end of Update


    IEnumerator camera_position_hansel()//3초후 스테이지진행
    {
        yield return new WaitForSeconds(6f);
        this.transform.position = new Vector3(-10.74f, 0.32f, -4.86f);
    }
    IEnumerator move_y_accel()//카메라워킹 y축속도 점점가속
    {
        yield return new WaitForSeconds(0.1f);
        if (move_dev_y < 0.4f)
        {
            move_dev_y += 0.015f;
            StartCoroutine("move_y_accel");
        }
    }
    IEnumerator move_xz_accel()//카메라워킹 xz축속도 점점가속
    {
        yield return new WaitForSeconds(0.1f);
        if (move_dev_xz < 0.4f)
        {
            move_dev_xz += 0.015f;
            StartCoroutine("move_xz_accel");
        }
    }

}
