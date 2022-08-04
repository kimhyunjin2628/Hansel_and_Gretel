using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//navmaesh사용

public class game_over : MonoBehaviour
{
    //게임매니저
    public GameObject management;
    game_management game_Management;

    //메인플레이어
    public GameObject Gretel;
    main_player_1 Gretel_Script;//그레텔 스크립트   

    //헨젤
    public GameObject Hansel;//핸젤
    Hansel_Script hansel_script;//핸젤 스크립트

    //메인카메라
    public GameObject main_camera;
    main_camera main_camera_script;

    //마녀
    public GameObject Witch;
    eyes_pattern Witch_Script;//마녀 스크립트
    public GameObject Witch_field2;
    witch_script Witch_field2_Script;//마녀 스크립트2

    //엘리베이터
    public GameObject elevator;

    //톱니바퀴
    public GameObject clock_work;
    clock_work_script clock_work_script;

    //소울
    public GameObject Soul;

    //쿠키
    public GameObject cookie;
    public GameObject fake_cookie1;
    public GameObject fake_cookie2;
    public GameObject fake_cookie3;
    public GameObject snack_ground;

    //감시의눈
    public GameObject eyes_chaser;
    eyes_chase eyes_chaser_script;

    //열쇠
    public GameObject key;

    //새장
    public GameObject[] eyes_effect = new GameObject[36];

    //게임오버
    public bool gameover = true;

    //UI
    public GameObject UI;
    GUI_stage0_1 UI_script;//ui스크립트

    //페이드 인,아웃
    public Material black_screen_m;
    public GameObject black_screen;
    float opacity_point = 0.0f;
    public bool fade_in_enable = false;
    public bool fade_out_enable = false;

    //데스카운트
    public int death_count = 0; 

    // Start is called before the first frame update
    void Start()
    {
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        Witch_Script = Witch.GetComponent<eyes_pattern>();//마녀스크립트
        Witch_field2_Script = Witch_field2.GetComponent<witch_script>();//마녀스크립트2
        eyes_chaser_script = eyes_chaser.GetComponent<eyes_chase>();//감시의눈
        UI_script = UI.GetComponent<GUI_stage0_1>();
        hansel_script = Hansel.GetComponent<Hansel_Script>();//핸젤스크립트
        main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        clock_work_script = clock_work.GetComponent<clock_work_script>();
    }

    // Update is called once per frame
    void Update()
    {
        //페이드 인,아웃
        if (fade_out_enable == true)//페이드아웃
        {
            if (opacity_point < 1)
            {
                opacity_point += 0.5f * Time.deltaTime;
            }
            else
            {
                opacity_point = 1.0f;
                fade_out_enable = false;
                StartCoroutine("fade_in");
            }
        }
        if (fade_in_enable == true)//페이드아웃후 페이드인
        {
            if (opacity_point > 0.0f)
            {
                opacity_point -= 0.3f * Time.deltaTime;

                    if (game_Management.stage_num == 14f)//게임오버 세이브포인트 오류방지
                { 
                    //게임오버시 위치고정
                    this.main_camera.transform.localPosition = new Vector3(1.36f, 0.9f, -3.76f);//카메라위치계속고정
                    this.main_camera.transform.localRotation = Quaternion.Euler(9.73f, 0, 0);//회전값고정
                    this.main_camera_script.Current_camera_rotation_y = 0.0f;//회전값고정
                    this.Witch_field2.transform.localPosition = new Vector3(10.86f, 0.983f, -0.99f);
                    this.Witch_field2.transform.localRotation = Quaternion.Euler(0, 180, 0);//회전값고정 


                    //그레텔 생성
                    this.Gretel_Script.enabled = false;
                    this.Gretel.GetComponent<CharacterController>().enabled = false;
                    this.Gretel_Script.gameObject.transform.position = new Vector3(20.42f, 0.551f, -1.294f);
                    this.Gretel.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정
                    this.Gretel_Script.gameObject.GetComponent<BoxCollider>().enabled = true;//컬라이더on
                   

                    //핸젤 생성
                    this.Hansel.transform.parent = null;
                    this.Hansel.GetComponent<CharacterController>().enabled = false;
                    this.Hansel.transform.localPosition = new Vector3(22.0f, 0.16f, -1.53f);
                    this.Hansel.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정   
                    this.hansel_script.gameObject.GetComponent<BoxCollider>().enabled = true;//컬라이더on
                }                          
            }
            else
            {
                opacity_point = 0.0f;
                fade_in_enable = false;
                black_screen.SetActive(false);
                gameover = true; //게임오버플래그 true -> 페이드인끝난후실행
                this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능 -> 페이드인끝난후 실행

                this.Witch_field2.GetComponent<witch_script>().enabled = true;//navmeshagent해제전 오류방지 다시on
                this.Witch_field2.GetComponent<NavMeshAgent>().enabled = true;//초기화

                if (game_Management.stage_num == 14f)
                {
                    this.Gretel.GetComponent<CharacterController>().enabled = true;
                    this.Gretel_Script.enabled = true;
                    this.Hansel.GetComponent<CharacterController>().enabled = true;
                    this.hansel_script.in_elevator = false;
                }

            }

        }
        this.black_screen_m.color = new Color(0.0f, 0.0f, 0.0f, opacity_point);

        //스테이지1 게임오버시 세이브지점 
        if (game_Management.stage_num >= 10)
        {
            if ((Gretel_Script.gameover_be_caught == true || Gretel_Script.gameover_be_locked == true || Gretel_Script.gameover_fall == true)
                && gameover == true)
            {
                StartCoroutine("Game_Over");
                StartCoroutine("fade_out");

                //한번만실행
                gameover = false;
            }
        }

        //스테이지1-2 게임오버시 세이브지점2
        if (game_Management.stage_num >= 14)
        {
            if (((Gretel_Script.gameover_fall2 == true || Gretel_Script.gameover_be_caught2 == true || hansel_script.gameover_be_caught2_hansel             
                ))
                && gameover == true)
            { 
                StartCoroutine("Game_Over2");
                StartCoroutine("fade_out");

                //한번만실행
                gameover = false;
            }
        }

    }

    IEnumerator Game_Over()//게임오버코루틴
    {
        yield return new WaitForSeconds(4.0f);

        //새장초기화
        if (Gretel_Script.gameover_be_locked == true)//게임오버 플래그 초기화전에실행
        {
            Gretel_Script.cage.transform.parent.GetComponent<cage_Script>().hold = false;
            Gretel_Script.cage.transform.parent.GetComponent<cage_Script>().rock_on = true;
        }
        for (int i = 0; i <= 36; i++)
        {
           // this.eyes_effect[i].GetComponent<cage_Script>().fin = true;
        }

        //게임오버 플래그false
        Gretel_Script.gameover_be_caught = false;
        Gretel_Script.gameover_be_locked = false;
        Gretel_Script.gameover_fall = false;
        Gretel_Script.gameover_be_caught2 = false;

        //그레텔 생성
        this.Gretel_Script.gameObject.transform.position = new Vector3(-10.3f, 0.551f, -7.275f);
        this.Gretel.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정

      

        //마녀초기화
        StartCoroutine("Game_Over_delay1");
        Witch_Script.found_enable = false;
        Witch_Script.found_delay_timer = 0.0f;
        Witch_Script.searching_delay_timer = 0.0f;

        //소울초기화
        this.Soul.transform.localRotation = Quaternion.Euler(0, 0, 0);//회전값고정

        //감시의눈 초기화
        this.eyes_chaser_script.timer = 0.0f;
        this.eyes_chaser_script.transform.GetChild(0).gameObject.SetActive(false);

        //열쇠초기화
        this.key.transform.parent = null;
        this.key.transform.position = new Vector3(16.14f, 6.852f, -4.197f);
        this.key.transform.localRotation = Quaternion.Euler(-152.879f, -126.72f, -32.26398f);//회전값고정
        this.key.transform.GetComponent<BoxCollider>().enabled = true;//컬라이더온

        //새장초기화

        //데스카운트
        this.death_count++;

        //ui초기화
        this.UI_script.follow_hansel.SetActive(false);
        this.UI_script.chase_eyes_warning.SetActive(false);

        //나머지 스테이지관련 false
        this.game_Management.stage_num_1_11 = false;
        this.game_Management.stage_num_1_12 = false;
        this.game_Management.stage_num_1_13 = false;

        //쿠키를이미 변환시킨 후 게임오버 됬을경우 
        if (game_Management.stage_num >= 12)
        {
            /*cookie.transform.localPosition = new Vector3(-0.000f, 3.821f, 0.210f);
            cookie.transform.localRotation = Quaternion.Euler(90, 0, 0);
            cookie.SetActive(false);
            fake_cookie1.SetActive(true);
            fake_cookie2.SetActive(true);
            fake_cookie3.SetActive(true);
            snack_ground.SetActive(true);*/
            game_Management.stage_num_1_10 = false;
            game_Management.stage_num_1_12 = true;
            game_Management.stage_num = 12;
        }
        else
        {
            //1-10스테이지로
            game_Management.stage_num_1_10 = true;
            game_Management.stage_num = 10;
        }
        
    }

    IEnumerator Game_Over2()//게임오버코루틴
    {
        yield return new WaitForSeconds(4.0f);


        //1-10스테이지로
        game_Management.stage_num_1_14 = true;
        game_Management.stage_num = 14;

        //게임오버 플래그false
        Gretel_Script.gameover_be_caught = false;
        Gretel_Script.gameover_be_locked = false;
        Gretel_Script.gameover_fall = false;
        Gretel_Script.gameover_be_caught2 = false;
        Gretel_Script.gameover_fall2 = false;
        hansel_script.gameover_be_caught2_hansel = false;

        //그레텔 생성
        this.Gretel_Script.enabled = false;
        this.Gretel.GetComponent<CharacterController>().enabled = false;
        this.Gretel_Script.gameObject.transform.position = new Vector3(20.42f, 0.551f, -1.294f);
        this.Gretel.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정
        this.Gretel_Script.gameObject.GetComponent<BoxCollider>().enabled = true;//컬라이더on
        this.Gretel.GetComponent<CharacterController>().enabled = true;
        this.Gretel_Script.enabled = true;

        //핸젤 생성
        this.Hansel.transform.parent = null;
        this.Hansel.GetComponent<CharacterController>().enabled = false;
        this.hansel_script.solo = false;
        this.hansel_script.hold_hands_X = false;
        this.Hansel.transform.localPosition = new Vector3(22.0f, 0.16f, -1.53f);
        this.Hansel.transform.localRotation = Quaternion.Euler(0, 90, 0);//회전값고정
        this.hansel_script.gameObject.GetComponent<BoxCollider>().enabled = true;//컬라이더on
        this.Hansel.GetComponent<CharacterController>().enabled = true;
        this.hansel_script.in_elevator = false;

        //엘리베이터 초기화
        this.elevator.transform.position = new Vector3(35.97f, 0.712f, 3.517f);
        this.elevator.transform.localRotation = Quaternion.Euler(0, -90, 0);//회전값고정
        this.clock_work_script.arrive_x = false;
        this.clock_work_script.arrive_z = false;
        this.clock_work_script.enable_timer = 0.0f;

        //시계바늘 - 조커
        this.clock_work_script.kill = 0;

        //카메라 초기화
        this.main_camera.GetComponent<main_camera>().enabled = false;
        this.main_camera.transform.localPosition = new Vector3(1.36f, 0.9f, -3.76f);
        this.main_camera.transform.localRotation = Quaternion.Euler(9.73f, 0, 0);//회전값고정
        this.main_camera.GetComponent<main_camera>().enabled = true;
        this.main_camera_script.e_mode = false;
        this.main_camera_script.w_mode = false;
        this.main_camera_script.n_mode = false;
        this.main_camera_script.s_mode = false;
        this.main_camera_script.clock_tower_mode = false;
        this.main_camera_script.clock_lever_mode_control = false;
        this.main_camera_script.bridge2_mode = false;
        this.main_camera_script.ground_mode = true;
        this.main_camera_script.ray_e = false;
        this.main_camera_script.ray_w = false;
        this.main_camera_script.ray_s = false;
        this.main_camera_script.ray_n = false;
        this.main_camera_script.Current_camera_rotation_y = 0.0f;
        this.main_camera_script.vertical_freeze = false;

        //마녀초기화
        this.Witch_field2.GetComponent<witch_script>().enabled = false;//navmeshagent해제전 오류방지
        this.Witch_field2.GetComponent<NavMeshAgent>().enabled = false;//좌표변경위해
        this.Witch_field2.transform.localPosition = new Vector3(10.86f, 0.983f, -0.99f);
        this.Witch_field2.transform.localRotation = Quaternion.Euler(0, 180, 0);//회전값고정
        this.Witch_field2_Script.freeze_timer = 0.0f;
        this.Witch_field2_Script.freeze = true;//마녀정지상태
        this.Witch_field2_Script.target_main = true;//플래그초기화
        this.Witch_field2_Script.target_enable = true;//플래그초기화
        this.Witch_field2_Script.game_over = false; //플래그초기화

        //데스카운트
        this.death_count++;

        //나머지 스테이지관련 false
    }

    IEnumerator Game_Over_delay1()//게임오버코루틴,딜레이필요
    {
        yield return new WaitForSeconds(2.0f);
        Witch_Script.next_step = eyes_pattern.STEP.NORMAL;
    }

    IEnumerator fade_out()//페이드아웃
    {
        yield return new WaitForSeconds(2.0f);
        black_screen.SetActive(true);
        fade_out_enable = true;
    }

    IEnumerator fade_in()//페이드인
    {
        yield return new WaitForSeconds(1.5f);
        fade_in_enable = true;
    }
}