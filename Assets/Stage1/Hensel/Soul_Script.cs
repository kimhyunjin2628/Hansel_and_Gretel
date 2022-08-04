using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul_Script : MonoBehaviour
{
    //애니메이션컨트롤러
    Animator Soul_animator;
    //캐릭터컨트롤러
    CharacterController Soul_cc;
    //게임매니저
    public GameObject management;
    game_management game_Management;
    //메인플레이어
    GameObject Gretel;
    main_player_1 Gretel_Script;//그레텔 스크립트     
    //매쉬정보
    public GameObject mesh1;
    public GameObject mesh2;
    public GameObject mesh3;
    public GameObject Point_Light; 

    //이동 - 로테이션(rotation)
    float rotate_speed = 320.0f;//회전속도
    //회전유무 
    bool rotation_flag = true;

    //좌표
    Vector3 destination;//목적좌표
    public bool start = false;
    public bool transform_start = false;
    float x_horizontal = 0.0f;
    float z_vertical = 0.0f;
    float y_height = 0.0f;
    bool x_arrive = false;
    bool y_arrive = false;
    bool z_arrive = false;
    //이동속도관련
    public float move_dev_xz;
    public float move_dev_y;

    //좌표초기화용
    public bool V_flag = true;

    //스테이지별 좌표 도착
    public bool arrive_to_gretel;//그레텔에게도착 1-4
    public bool arrive_to_fire_position_1_7;//1-7

    //화염이펙트,화염방출
    float fire_timer = 0.0f;
    public GameObject fire_particle;
    public Material Soul_material;
    float fire_color_G = 255f;
    Vector3 comback_vector;
    public bool fire_fin = false;//fire종료플래그 게임매니저에게 전달

    //애니메이션 플래그
    public bool normal = false;
    public bool walk = false;
    public bool fire = false;
    public bool explosion = false;
    public bool jump_g = false;
    public bool normal2 = false;
    public bool walk2 = false;


    // Start is called before the first frame update
    void Start()
    {
        Soul_animator = GetComponent<Animator>();//애니메이션 컨트롤러
        Soul_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        Gretel = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

         
        //좌표이동시작 1-3스테이지
        if (game_Management.stage_num_1_3 == true && V_flag == true)
        {
            //매쉬활성화
            this.mesh1.GetComponent<SkinnedMeshRenderer>().enabled = true;
            this.mesh2.GetComponent<SkinnedMeshRenderer>().enabled = true;
            this.mesh3.GetComponent<SkinnedMeshRenderer>().enabled = true;
            this.Point_Light.SetActive(true);

            //좌표이동초기화플래그
            destination = new Vector3(1f, 1f, -4f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true
            //이동속도
            move_dev_xz = 20f;
            move_dev_y = 120f;
        }
        //좌표이동시작 1-4스테이지
        if (game_Management.stage_num_1_4 == true && V_flag == true)
        {

            //좌표이동초기화플래그
            destination = new Vector3(-9.705f,1.001f, -3.016f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true

            //이동속도
            move_dev_xz = 20f;
            move_dev_y = 120f;
        }
        //좌표이동시작 1-5스테이지
        if (game_Management.stage_num_1_5 == true && V_flag == true)
        {

            //좌표이동초기화플래그
            destination = new Vector3(-11.861f, 0.365f, -1.434f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true
            //회전X
            rotation_flag = false;

            //이동속도
            move_dev_xz = 35f;
            move_dev_y = 120f;
        }
        //좌표이동 시작 1-7스테이지(*fire)
        if (game_Management.stage_num_1_7 == true && V_flag == true)
        {
            //좌표이동초기화플래그
            destination = new Vector3(-11.02f, 0.37f ,-6.26f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true
            this.transform.parent = null;//잠시 친자관계해제

            //*fire
            comback_vector = this.transform.position;//돌아갈위치저장

            //이동속도
            move_dev_xz = 120f;
            move_dev_y = 120f;
        }
        //1-10스테이지 (*fire)
        if(game_Management.stage_num_1_11 == true && V_flag == true)
        {


            //좌표이동초기화플래그
            destination = new Vector3(2.236f, 8.809f, -5.569f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true
            this.transform.parent = null;//잠시 친자관계해제

            //*fire
            comback_vector = this.transform.position;//돌아갈위치저장

            //이동속도
            move_dev_xz = 120f;
            move_dev_y = 120f;
        }
        //1-20스테이지 (*fire)
        if (game_Management.stage_num_1_20 == true && V_flag == true)
        {


            //좌표이동초기화플래그
            destination = new Vector3(-26.707f, -12.66f, 8.943f);
            start = true;//좌표스타트
            transform_start = true;//백터방향초기화
            V_flag = false;//다음 스테이지 변환때 true
            this.transform.parent = null;//잠시 친자관계해제

            //*fire
            comback_vector = this.transform.position;//돌아갈위치저장

            //이동속도
            move_dev_xz = 120f;
            move_dev_y = 120f;
        }



        //화염방출
        if (fire == true)//준비동작
        {
            this.Soul_material.color = new Color(255, fire_color_G, 0);//점점 빨간색으로
            this.fire_color_G -= 40f * Time.deltaTime;

            if (fire_timer >= 5.0f)//fire_ready지속시간
            {
                fire_timer = 0.0f;

                //애니메이션 플래그
                fire = false;
                explosion = true;
                
            }
            else
            {
                fire_timer += Time.deltaTime;
            }
        }
        if (explosion == true)//방출
        {
            this.fire_particle.SetActive(true);//이펙트방출  
            if (game_Management.stage_num_1_11 == true)
            {
                fire_particle.transform.position= new Vector3(1.75f, 8.406f, -5.757f);
            }
            else if (game_Management.stage_num_1_17 == true)
            {
                fire_particle.transform.position = new Vector3(22.449f, -0.103f, 30.531f);
            }
            else if (game_Management.stage_num_1_20 == true)
            {
                fire_particle.transform.position = new Vector3(-27.435f, -13.92f, 9.052f);
            }

            if (fire_timer >= 2.0f)//fire종료
            {
                this.fire_timer = 0.0f;
                this.fire_color_G = 255f;
                this.Soul_material.color = new Color(255, fire_color_G, 0);//다시노란색으로
                this.fire_particle.SetActive(false);


                    this.transform.position = comback_vector;//원래위치로 되돌아감
                    this.transform.parent = Gretel.transform;//친자관계 재설정

                fire_fin = true;//종료플래그

                //애니메이션플래그
                explosion = false;
                normal = true;
            }
            else
            {
                fire_timer += Time.deltaTime;

                    this.transform.Translate(Vector3.back * 0.35f * Time.deltaTime);

            }

        }

        //좌표까지 이동법
        if (start == true)//좌표이동시작
        {
            Debug.Log("도착지점 :" + this.destination + " 현재위치 :" + this.transform.position + " x:" + x_horizontal + " y:" + y_height
               + " z:" + z_vertical);
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

                //애니메이션플래그
                normal = false;
                jump_g = false;
                walk = true;
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

                //애니메이션플래그
                normal = true;
                walk = false;

                //1-5스테이지 소울 좌표(그레텔) 도착시
                if (game_Management.stage_num_1_5 == true)
                {
                    arrive_to_gretel = true;//도착완료
                    this.transform.localRotation = Quaternion.Euler(0, 90f, 0);
                    this.transform.parent = Gretel.transform;
                    rotation_flag = true;//회전 안했으므로 초기화
                }
                if (game_Management.stage_num_1_7 == true)
                {
                    arrive_to_fire_position_1_7 = true;
                    //애니메이션플래그
                    normal = false;
                    walk = false;
                    jump_g = false;
                    fire = true;
                }
                if (game_Management.stage_num_1_11 == true)
                {
                    //애니메이션플래그
                    normal = false;
                    walk = false;
                    jump_g = false;
                    fire = true;
                }
                if (game_Management.stage_num_1_20 == true)
                {
                    //애니메이션플래그
                    normal = false;
                    walk = false;
                    jump_g = false;
                    fire = true;
                }
            }          


            Vector3 direcction = new Vector3(x_horizontal, 0, z_vertical); //이동방향
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
                else
                {
                    this.transform.Rotate(0, -1f, 0);//회전속도에 따른 회전처리
                }

            }
            Soul_cc.Move(((direcction  * 10f *Time.deltaTime).normalized) / move_dev_xz);//이동처리
            Soul_cc.Move(((direction_y * 1f * Time.deltaTime).normalized) / move_dev_y);//이동처리

        }//end of start 좌표이동


        //그레텔 모션관련
        if (start== false && fire == false && explosion == false && this.game_Management.stage_num_1_11 == false && this.game_Management.stage_num >= 6)//좌표이동중엔 실행X
        {
            if (Gretel_Script.step == main_player_1.STEP.NORMAL || Gretel_Script.step == main_player_1.STEP.H_G_NORMAL || Gretel_Script.step == main_player_1.STEP.NO_ACTION)
            {
                //애니메이션 플래그
                normal = true;
                walk = false;
                fire = false;
                explosion = false;
                jump_g = false;
            }
            else if (Gretel_Script.step == main_player_1.STEP.WALK || Gretel_Script.step == main_player_1.STEP.RUN || Gretel_Script.step == main_player_1.STEP.H_G_WALK)
            {
                //애니메이션 플래그
                normal = false;
                walk = true;
                fire = false;
                explosion = false;
                jump_g = false;
            }
            else if (Gretel_Script.step == main_player_1.STEP.JUMP)
            {
                //애니메이션 플래그
                normal = false;
                walk = false;
                fire = false;
                explosion = false;
                jump_g = true;
            }
        }



        //애니메이션 관리
        Soul_animator.SetBool("NORMAL", normal);
        Soul_animator.SetBool("WALK", walk);
        Soul_animator.SetBool("FIRE", fire);
        Soul_animator.SetBool("EXPLOSION", explosion);
        Soul_animator.SetBool("JUMP_G", jump_g);
        Soul_animator.SetBool("NORMAL2", normal2);
        Soul_animator.SetBool("WALK2", walk2);
    }//end of Update
}
