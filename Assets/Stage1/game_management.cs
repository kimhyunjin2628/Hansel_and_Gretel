using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//navmaesh사용

public class game_management : MonoBehaviour
{
    // Start is called before the first frame update
    //스테이지1
    public int stage_num = 0;



    //스크립트참조
    public GameObject stage1;
    public GameObject camera_position_GameObject;
    public GameObject main_camera_GameObject;
    public GameObject Soul_GameObject;//핸젤소울
    public GameObject trap;//핸젤용함정
    public GameObject Hansel;//핸젤
    public GameObject key_stage1;//스테이지1열쇠

    stage1_animation stage1_animation;//스테이지1 애니메이션 스크립트
    main_player_1 Gretel_Script;//그레텔스크립트
    camera_position camera_position;//카메라포지션 스크립트                               
    public GameObject main_camera;//메인카메라
    main_camera main_camera_script;//메인카메라 스크립트
    Soul_Script soul_script;//핸젤영혼스크립트
    Hansel_Script hansel_script;//핸젤

    //필드이동
    public bool field_change1 = true;
    public bool field_change2 = false;

    //마녀소환
    public GameObject witch_stage1;
    public GameObject Witch;
    witch_script Witch_script;//마녀스크립트
    bool witch_ins = true;
    public bool witch_trigger_stage1_19 = false;
    public GameObject witch_hand1;
    public GameObject witch_hand2;
    public GameObject eyesEffect;

    //플레이타임
    public float play_time;

    //핸젤이동
    public bool hansel_move_1_21;
    public bool hansel_move_1_22;

    //시계
    public GameObject clock_tower;
    clock_Script clock_tower_script;
    public GameObject clock_effect;
    public GameObject clock_ring_effect;//이펙트 프리팹

    //카메라스크립트에서 스테이지별 좌표 초기화에 사용할 bool변수
    public bool stage_num_1_1 = false;
    public bool stage_num_1_2 = false;
    public bool stage_num_1_3 = false;
    public bool stage_num_1_4 = false;
    public bool stage_num_1_5 = false;
    public bool stage_num_1_6 = false;
    public bool stage_num_1_7 = false;
    public bool stage_num_1_8 = false;
    public bool stage_num_1_9 = false;
    public bool stage_num_1_10 = false;
    public bool stage_num_1_11 = false;
    public bool stage_num_1_12 = false;
    public bool stage_num_1_13 = false;
    public bool stage_num_1_14 = false;
    public bool stage_num_1_15 = false;
    public bool stage_num_1_16 = false;
    public bool stage_num_1_17 = false;
    public bool stage_num_1_18 = false;
    public bool stage_num_1_19 = false;
    public bool stage_num_1_20 = false;
    public bool stage_num_1_21 = false;
    public bool stage_num_1_22 = false;

    //스테이지간 딜레이
    //bool stage_num_1_3_delay = false;
    //bool stage_num_1_4_delat = false;

    //bridge3 입구
    public GameObject wire;
    public GameObject siren1;//와이어 출입 경고
    public GameObject siren2;//와이어 출입 경고
    public Material green_led;//사이렌 재질
    Animator wire_animator;

    //좌표알림커서
    public GameObject pointer;

    //batches최소화용
    public GameObject cage;
    public GameObject fence;

    public int i = 0;
    public enum STEP //동작
    {
        STAGE0, //0 
        STAGE1, //1
        STAGE2  //2
    };
    public STEP step = STEP.STAGE0;//현재상태
    public STEP next_step;//다음상태

    void Start()
    {
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        camera_position = camera_position_GameObject.GetComponent<camera_position>();//카메라스크립트
        main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        soul_script = Soul_GameObject.GetComponent<Soul_Script>();//핸젤영혼스크립트
        hansel_script = Hansel.GetComponent<Hansel_Script>();//핸젤스크립트
        stage1_animation = GameObject.FindGameObjectWithTag("stage1").GetComponent<stage1_animation>();//스테이지1 애니메이션 스크립트
        this.key_stage1.SetActive(false);
        clock_tower_script = clock_tower.GetComponent<clock_Script>();//시계 스크립트
        Witch_script = Witch.GetComponent<witch_script>();//마녀스크립트
        wire_animator = wire.GetComponent<Animator>();//철조망 입구문 스크립트
    }

    // Update is called once per frame
    void Update()
    {
        //플레이타임
        play_time += Time.deltaTime;

        switch (step)
        {
            case STEP.STAGE1:
                /* if (stage_num == 1)
                 {
                    // StartCoroutine("stage1_1_start");
                 }
                 else if (stage_num == 2)
                 {

                 }
                 else if (stage_num == 3)
                 {
                     StartCoroutine("stage1_3_start");
                 }*/
                if (stage_num == 9)
                {
                    this.Gretel_Script.gameObject.transform.position = new Vector3(-10.151f, this.Gretel_Script.gameObject.transform.position.y, -5.897f);
                }

                if (stage_num == 14 && this.field_change2 == true && this.witch_ins == true)//마녀생성!
                {
                    //기존마녀 false
                    this.witch_stage1.SetActive(false);

                    Debug.Log("마녀삭제");

                    //추적용마녀 true
                    this.Witch.SetActive(true);
                    witch_ins = false;
                }

                if (stage_num == 14 && this.hansel_script.gameover_be_caught2_hansel == true)//핸젤 마녀에게 잡힘 gameover 판정 메인카메라 위치고정
                {
                    //메인카메라 위치 고정
                    this.main_camera_script.enabled = false;
                    this.main_camera.transform.localPosition = new Vector3(-0.26f, 0.85f, -3.22f);
                    this.main_camera.transform.localEulerAngles = new Vector3(9.73f, 0, 0);
                }

                //Debug.Log(Vector3.Distance(this.siren1.transform.position, this.Gretel_Script.transform.position));
                if (stage_num >= 19 && Vector3.Distance(this.siren1.transform.position, this.Gretel_Script.transform.position) < 4f) 
                {
                    Debug.Log(Vector3.Distance(this.siren1.transform.position, this.Gretel_Script.transform.position));
                    this.witch_trigger_stage1_19 = true;
                    //핸젤 solo
                    this.hansel_script.solo = true;
                    this.hansel_script.hold_hands_X = true;
                    this.Hansel.transform.parent = null;
                    this.hansel_script.next_step = Hansel_Script.STEP.NORMAL;
                }
                
                break;
        }

        //스테이지1 필드구분
        if (this.step == STEP.STAGE1)
        {
            //한번만실행되는 코드
            if (Gretel_Script.gameObject.transform.position.x < 18f && this.field_change1 == false && this.field_change2 == true)
            {
                Gretel_Script.field1 = true;
                Gretel_Script.field2 = false;
                this.field_change1 = true;
                this.field_change2 = false;
                camera_position.V_flag_f = true;//메라좌표 재설정

                //batches최적화용 오브젝트 풀링
                cage.SetActive(true);
                fence.SetActive(true);
            }
            else if (Gretel_Script.gameObject.transform.position.x >= 18f && this.field_change2 == false && this.field_change1 == true)
            {
                Gretel_Script.field1 = false;
                Gretel_Script.field2 = true;
                this.field_change2 = true;
                this.field_change1 = false;
                camera_position.V_flag_f = true;//카메라좌표 재설정

                //batches최적화용 오브젝트 풀링
                cage.SetActive(false);
                fence.SetActive(false);
            }
        }


        //전환 if문은 딱 한번만 실행
        //1-1 전환 (마녀등장애니메이션)
        if (Gretel_Script.save1 == true)
        {
            StartCoroutine("stage1_1");
            Gretel_Script.save1 = false;
            stage_num_1_1 = true;//다음스테이지 true
        }
        //1-2전환(영혼뷰카메라전환)
        if (stage1_animation.animation_fin == true)//stage1_animation 스크립트 애니메이션끝날때(timer측정)
        {
            stage_num++;//1->2
            stage_num_1_1 = false;//전애니메이션 false
            camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설청
            stage_num_1_2 = true;//다음스테이지 true

            stage1_animation.animation_fin = false;//한번만실행을위해
        }
        //1-3전환 (핸젤영혼이동)
        if (camera_position.stage_num_1_2_fin == true)//카메라영혼뷰 이동 끝나면 시작, 1.5초딜레이존재
        {
            StartCoroutine("stage1_3_start");

            camera_position.stage_num_1_2_fin = false;//한번만실행을위해
        }
        //1-4전환(핸젤영혼회전밑이동)
        if (camera_position.stage_num_1_3_fin == true)//카메로영혼뷰 이동끝나면 시작, 1.0초 딜레이 존재
        {
            StartCoroutine("stage1_4_start");
            camera_position.stage_num_1_3_fin = false;//한번만실행을위해
        }
        //1_5전환(소울+그레텔)
        if (Gretel_Script.pointer_1_4 == true)//1-4스테이지 좌표에 닿았을때
        {
            StartCoroutine("stage1_5_start");
            pointer.GetComponent<BoxCollider>().enabled = false;//포인터컬라이더off
            Gretel_Script.pointer_1_4 = false;//한번만실행을위해

        }
        //1-6전환 그레텔이동가능
        if (soul_script.arrive_to_gretel == true)//소울이 그레텔에게 도착했을때
        {
            StartCoroutine("stage1_6_pointer");//일정시간뒤 좌표알리미생성
            stage_num++;//5->6
            stage_num_1_5 = false;//전애니메이션false
            stage_num_1_6 = true;//다음스테이지true
            Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능

            //포인터삭제
            pointer.SetActive(false);

            soul_script.arrive_to_gretel = false;//한번만 실행을 위해
        }
        //1-7전환 감옥파괴,과자획득
        if (Gretel_Script.pointer_1_6 == true)//1-6스테이지 좌표에 닿았을때
        {
            stage_num++;//6->7
            stage_num_1_6 = false;//전애니메이션false
            stage_num_1_7 = true;//다음스테이지true
            soul_script.V_flag = true;//스테이지 바뀐후 소울좌표 재설정
            camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            Gretel_Script.pointer_1_6 = false;//한번만 실행을 위해
        }
        //1-8전환 감옥탈출,그레텔이동가능
        if (this.stage_num_1_7 == true && soul_script.fire_fin == true)
        {
            stage_num++;//7->8
            stage_num_1_7 = false;//전애니메이션false
            stage_num_1_8 = true;//다음스테이지true
            //camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능
            this.pointer.SetActive(false);

            soul_script.fire_fin = false;//한번만 실행을 위해,다음공격을위해 초기화
        }

        //1-9전환 헨젤 새장에 갇힘
        if (this.stage_num_1_8 == true && this.hansel_script.snack_eat == true)
        {
            StartCoroutine("stage1_9_Trap");//1-9스테이지 딜레이후 실행

            this.hansel_script.snack_eat = false;//한번만 실행을 위해,다음공격을위해 초기화
        }

        //1-10전환 -> 1-9 코루틴에서

        //1-11전환 쿠키발견
        if (this.stage_num_1_10 && this.Gretel_Script.snack_ground == true)
        {
            stage_num++;//10->11
            stage_num_1_10 = false;//전애니메이션false
            stage_num_1_11 = true;//다음스테이지true
            camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            // Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;//그레텔 이동불가능
            this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;

            soul_script.V_flag = true;//스테이지 바뀐후 소울좌표 재설정

            eyesEffect.SetActive(false);//공격중지

            this.Gretel_Script.snack_ground = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-12전환 쿠키변환완료
        if (this.stage_num_1_11 == true && soul_script.fire_fin == true)
        {
            stage_num++;//11->12
            stage_num_1_11 = false;//전애니메이션false
            stage_num_1_12 = true;//다음스테이지true
                                  // camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능

            //헨젤컬라이더 크기 증가(nack용)
            //컬라이더크기 커짐
            this.Hansel.GetComponent<BoxCollider>().center = new Vector3(0.829f, 4.92f, -2.09f);
            this.Hansel.GetComponent<BoxCollider>().size = new Vector3(40f, 10.8f, 40f);


            soul_script.fire_fin = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-13전환 쿠키획득
        if (this.stage_num_1_12 == true && this.hansel_script.snack_eat == true)
        {
            stage_num++;//12->13
            stage_num_1_12 = false;//전애니메이션false
            stage_num_1_13 = true;//다음스테이지true
            //camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설

            this.hansel_script.snack_eat = false;//한번만 실행을 위해 초기화
        }
        //1-14전환 쿠키획득후 필드이동
        if (this.stage_num_1_13 == true && hansel_script.feild_change == true)
        {
            stage_num++;//13->14
            stage_num_1_13 = false;//전애니메이션false
            stage_num_1_14 = true;//다음스테이지true
            //camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정

            //핸젤 컬라이더 크기변경
            this.Hansel.GetComponent<BoxCollider>().center = new Vector3(-0.206f, 3.280f, -0.545f);
            this.Hansel.GetComponent<BoxCollider>().size = new Vector3(5.257f, 11.247f, 5.358f);

            this.hansel_script.feild_change = false;//한번만 실행을 위해 초기화
        }

        //1-15전환 쿠키획득후 필드이동
        if (this.stage_num_1_14 == true && clock_tower_script.clock_cross == true)
        {
            stage_num++;//14->15
            stage_num_1_14 = false;//전애니메이션false
            stage_num_1_15 = true;//다음스테이지true

            //메인플레이어
            Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            Gretel_Script.step = main_player_1.STEP.NO_ACTION;

            //메인카메라
            main_camera_GameObject.GetComponent<main_camera>().enabled = false;//스크립트false
            main_camera_GameObject.transform.localPosition = new Vector3(-0.07f, 4.46f, -9.21f);
            main_camera_GameObject.transform.localEulerAngles = new Vector3(9.73f, 0f, 0f);

            //카메라포지션
            camera_position_GameObject.GetComponent<camera_position>().enabled = false;//스크립트false
            camera_position_GameObject.transform.position = new Vector3(32.582f, 4.04f, 11.28f);

            //마녀
            Witch_script.freeze_timer = 21.0f;
            Witch_script.freeze = true;
            witch_hand1.GetComponent<Collider>().enabled = false;
            witch_hand2.GetComponent<Collider>().enabled = false;

            //시계이펙트
            clock_effect.SetActive(true);
            StartCoroutine("inst_clock_effect");
            //다음스테이지 코루틴
            StartCoroutine("stage1_16_start");
            this.clock_tower_script.clock_cross = false;//한번만 실행을 위해 초기화
        }

        //1-17전환 다이아몬드 케이크 발견
        if (this.stage_num_1_16 && this.Gretel_Script.snack_ground == true)
        {
            stage_num++;//16->17
            stage_num_1_16 = false;//전애니메이션false
            stage_num_1_17 = true;//다음스테이지true
            camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            // Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;//그레텔 이동불가능
            this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;


            this.soul_script.fire = true;//이동없이 바로 fire
            this.soul_script.normal = false;
            this.soul_script.walk = false;
            this.soul_script.jump_g = false;

            this.Gretel_Script.snack_ground = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-18전환 케이크 변환완료
        if (this.stage_num_1_17 == true && soul_script.fire_fin == true)
        {
            stage_num++;//17->18
            stage_num_1_17 = false;//전애니메이션false
            stage_num_1_18 = true;//다음스테이지true

            Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능
            Gretel_Script.step = main_player_1.STEP.NORMAL;//그레텔 이동가능

            //헨젤컬라이더 크기 증가(snack용)
            //컬라이더크기 커짐
            this.Hansel.GetComponent<BoxCollider>().center = new Vector3(0.829f, 4.92f, -2.09f);
            this.Hansel.GetComponent<BoxCollider>().size = new Vector3(40f, 10.8f, 40f);


            soul_script.fire_fin = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-19전환 케이크획득,사탕주머니획득가능
        if (this.stage_num_1_18 == true && this.hansel_script.snack_eat == true)
        {
            stage_num++;//18->19
            stage_num_1_18 = false;//전애니메이션false
            stage_num_1_19 = true;//다음스테이지true

            //핸젤솔로모드 해제
            hansel_script.hold_hands_X = false;
            hansel_script.solo = false;

            //다리3 
            wire_animator.SetBool("OPEN", true);
            this.siren1.GetComponent<MeshRenderer>().material = green_led;
            this.siren2.GetComponent<MeshRenderer>().material = green_led;

            this.hansel_script.snack_eat = false;//한번만 실행을 위해 초기화
        }
        //1-20전환 사탕주머니 발견
        if (this.stage_num_1_19 && this.Gretel_Script.snack_ground == true)
        {
            stage_num++;//19->20
            stage_num_1_19 = false;//전애니메이션false
            stage_num_1_20 = true;//다음스테이지true
            camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정
            this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;

            soul_script.V_flag = true;//스테이지 바뀐후 소울좌표 재설정

            this.soul_script.fire = true;//이동없이 바로 fire
            this.soul_script.normal = false;
            this.soul_script.walk = false;
            this.soul_script.jump_g = false;

            this.Gretel_Script.snack_ground = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-21전환 사탕주머니 변환완료
        if (this.stage_num_1_20 == true && soul_script.fire_fin == true)
        {
            stage_num++;//20->21
            stage_num_1_20 = false;//전애니메이션false
            stage_num_1_21 = true;//다음스테이지true

            Gretel_Script.next_step = main_player_1.STEP.NORMAL;//그레텔 이동가능
            Gretel_Script.step = main_player_1.STEP.NORMAL;//그레텔 이동가능

            //헨젤컬라이더 크기 증가(snack용)
            //컬라이더크기 커짐
            this.Hansel.GetComponent<BoxCollider>().center = new Vector3(0.829f, 4.92f, -2.09f);
            this.Hansel.GetComponent<BoxCollider>().size = new Vector3(40f, 10.8f, 40f);

            //핸젤이동
            this.Hansel.GetComponent<NavMeshAgent>().enabled = true;
            this.hansel_move_1_21 = true;

            soul_script.fire_fin = false;//한번만 실행을 위해,다음공격을위해 초기화
        }
        //1-22전환 사탕주머니획득
        if (this.stage_num_1_21== true && this.hansel_script.snack_eat == true)
        {
            stage_num++;//21->22
            stage_num_1_21 = false;//전애니메이션false
            stage_num_1_22 = true;//다음스테이지true

            //핸젤솔로모드 해제
            hansel_script.hold_hands_X = false;
            hansel_script.solo = false;

            //핸젤 이동
            this.hansel_move_1_22 = true;

            this.hansel_script.snack_eat = false;//한번만 실행을 위해 초기화
        }

        //동작관리
        this.step = this.next_step;
        
    }

    IEnumerator stage1_1()//3.5초후 스테이지진행
    {
        yield return new WaitForSeconds(3.5f);
        stage_num++;//0->1
        this.next_step = STEP.STAGE1;//스테이지1시작
        StartCoroutine("stage1_1_start");
    }

    IEnumerator stage1_1_start()//2초후애니메이션 재생
    {
        yield return new WaitForSeconds(3.0f);
        stage1_animation.stage1_1 = true; //스테이지1애니메이션 시작 -> 마녀등장
    }

    IEnumerator stage1_3_start()//1.5초후 스테이지진행
    {
        yield return new WaitForSeconds(1.5f);

        stage_num++;//2->3
        stage_num_1_2 = false;//전애니메이션false
        soul_script.V_flag = true;//스테이지 바뀐후 소울좌표 재설청
        stage_num_1_3 = true;//다음스테이지true
        camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설청

        // stage_num_1_3_delay = true;
    }
    IEnumerator stage1_4_start()//1.0초후 스테이지진행
    {
        yield return new WaitForSeconds(1.0f);

        stage_num++;//3->4
        stage_num_1_3 = false;//전애니메이션false
        soul_script.V_flag = true;//스테이지 바뀐후  소울좌표 재설청
        stage_num_1_4 = true;//다음스테이지true
        camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설청

        //좌표알리미 소환
        this.pointer.SetActive(true);
        pointer.transform.position = new Vector3(-12.16f, -0.069f, -1.865f);

        // stage_num_1_4_delay = true;
    }

    IEnumerator stage1_5_start()//2.5초후 스테이지진행
    {
        yield return new WaitForSeconds(2.5f);

        stage_num++;//4->5
        stage_num_1_4 = false;//전애니메이션false
        stage_num_1_5 = true;//다음스테이지true
        soul_script.V_flag = true;//스테이지 바뀐후  소울좌표 재설청

        // stage_num_1_3_delay = true;
    }
    IEnumerator stage1_6_pointer()//4.5초후 스테이지진행
    {
        yield return new WaitForSeconds(4.5f);

        //좌표알리미 소환
        this.pointer.SetActive(true);
        pointer.GetComponent<BoxCollider>().enabled = true;
        pointer.transform.position = new Vector3(-13.284f, -0.069f, -6.386f);

        // stage_num_1_3_delay = true;
    }

    IEnumerator stage1_9_Trap()//3초후 스테이지진행 , 1-10스테이지 자동으로 넘어감
    {
        yield return new WaitForSeconds(3f);
        stage_num++;//8->9
        stage_num_1_8 = false;//전애니메이션false
        stage_num_1_9 = true;//다음스테이지true
        camera_position.V_flag = true;//스테이지 바뀐후 카메라좌표 재설정

        //그레텔 비활성화 + 행동불가
        this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
        this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;
        this.Gretel_Script.gameObject.transform.position = new Vector3(-10.151f, this.Gretel_Script.gameObject.transform.position.y, -5.897f);

        yield return new WaitForSeconds(9.0f);
        this.trap.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        //1-10스테이지
        stage_num++;//9->10
        stage_num_1_9 = false;//전애니메이션false
        stage_num_1_10 = true;//다음스테이지true
        this.key_stage1.gameObject.SetActive(true);//열쇠활성화

        this.hansel_script.snack_point_before = this.hansel_script.snack_point;
        this.hansel_script.snack_point = 4;//UI변경
        //그레텔 활성화
        this.Gretel_Script.gameObject.SetActive(true);
        this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;

    }

    IEnumerator stage1_16_start()//6.0초후 스테이지진행
    {
        yield return new WaitForSeconds(7.5f);

        stage_num++;//15->16
        stage_num_1_15 = false;//전애니메이션false
        stage_num_1_16 = true;//다음스테이지true

        //그레텔
        this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
        this.Gretel_Script.step = main_player_1.STEP.NORMAL;

        //메인카메라
        main_camera_GameObject.transform.localPosition = new Vector3(0.3f, 4.46f, 8.52f);
        main_camera_GameObject.transform.localEulerAngles = new Vector3(9.73f, 180f, 0f);
        main_camera_GameObject.GetComponent<main_camera>().enabled = true;//스크립트온

        //카메라포지션
        camera_position_GameObject.GetComponent<camera_position>().enabled = true;//스크립트온

    }
    IEnumerator inst_clock_effect()//일정주기로 이펙트추가생성
    {
        yield return new WaitForSeconds(0.3f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(74.882f, -175.46f, -126.02f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(40.964f, -74.255f, -65.453f);
        clock_ring_effect.transform.localScale = new Vector3(51.72433f, 51.72433f, 51.72433f);
        yield return new WaitForSeconds(0.3f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(72.08144f, -6.62576f, -107.853f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(80.87901f, 112.805f, 100.528f);
        clock_ring_effect.transform.localScale = new Vector3(33.04047f, 33.04047f, 33.04047f);
        yield return new WaitForSeconds(0.3f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(112.5334f, 26.29438f, 150.02f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(-0.487f, 112.09f, -123.969f);
        clock_ring_effect.transform.localScale = new Vector3(33.04047f, 33.04047f, 33.04047f);
        yield return new WaitForSeconds(0.3f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(-112.844f,  26.29438f, 26.6364f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(-0.487f, 348.169f, 236.031f);
        clock_ring_effect.transform.localScale = new Vector3(33.04047f, 33.04047f, 33.04047f);    
        yield return new WaitForSeconds(0.3f);
        Instantiate(clock_ring_effect);
        clock_ring_effect.transform.localPosition = new Vector3(-75.62f, 25.79f, 127.44f);
        clock_ring_effect.transform.localEulerAngles = new Vector3(-33.69f, 394.287f, 240.759f);
        clock_ring_effect.transform.localScale = new Vector3(33.04047f, 33.04047f, 33.04047f);
    }
}
