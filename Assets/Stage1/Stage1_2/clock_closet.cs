using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_closet : MonoBehaviour
{
    //벽장
    public GameObject closet1;
    public GameObject closet2;
    public GameObject closet3;
    public GameObject none_closet1;
    public GameObject none_closet2;
    public GameObject none_closet3;

    //애니메이터
    Animator clock_closet_animator1;
    Animator clock_closet_animator2;
    Animator clock_closet_animator3;

    public GameObject main_player;
    main_player_1 Gretel_Script;//그레텔 스크립트   
    public GameObject main_camera;//메인카메라
    main_camera main_camera_script;//메인카메라 스크립트   
    public GameObject management;
    game_management game_Management;//게임관리 스크립트

    //활성화
    public bool enable = true;
    //애니메이션 플래그
    bool open;
    bool close;

    //이동판정 컬라이더
    public GameObject closet_collider1;
    public GameObject closet_collider2;
    public GameObject closet_collider3;

    //진입불가 컬라이더
    public GameObject closet_wallcollider1;
    public GameObject closet_wallcollider2;
    public GameObject closet_wallcollider3;

    //충돌
    public bool t_start;//텔레포트시작
    public bool t_ing;//텔레포트 도중
    public bool t_fine;//텔레포트끝
    public bool t_fine2;//텔레포트끝
    public bool t_length;//텔레포트길이

    // Start is called before the first frame update
    void Start()
    {
        this.clock_closet_animator1 = this.closet1.GetComponent<Animator>();
        this.clock_closet_animator2 = this.closet2.GetComponent<Animator>();
        this.clock_closet_animator3 = this.closet3.GetComponent<Animator>();
        this.main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        this.Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트     
        this.game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        //1-16 비활성화
        if (this.game_Management.stage_num > 15)
        {
            open = false;
            close = true;
            t_fine2 = false;
            t_length = false;//텔레포트끝
        }

        if (enable == true)
        {
            StartCoroutine("open_closet");
            enable = false;
        }
        if (this.t_start == true)
        {
            //이동불가
            this.Gretel_Script.next_step = main_player_1.STEP.NO_ACTION;
            this.Gretel_Script.step = main_player_1.STEP.NO_ACTION;

            // this.open = false;
            // this.close = true;
            t_length = true;//텔레포트시작

            this.t_ing = true;//텔레포트도중
            //E
            if (main_camera_script.e_mode == true)
            {
                int r = Random.Range(0, 2);
                if (r == 0)//E->W
                { 
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(24.111f, 0.6815f, 35.635f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, 270f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                else //E->N
                {
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(36.69f, 0.6815f, 40.185f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                this.t_start = false;//한번만실행
            }
            else if (main_camera_script.w_mode == true) //W
            {
                int r = Random.Range(0, 2);
                if (r == 0)//W->E
                {
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(41.213f, 0.6815f, 26.558f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, -270f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                else //W->N
                {
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(36.69f, 0.6815f, 40.185f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                this.t_start = false;//한번만실행
            }
            else //N
            {
                int r = Random.Range(0, 2);
                if (r == 0)//N->E
                {
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(41.213f, 0.6815f, 26.558f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, -270f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                else //N->W
                {
                    this.Gretel_Script.enabled = false;//스크립트해제
                    this.main_player.GetComponent<CharacterController>().enabled = false;//캐릭터 컨트롤러 해제
                    this.main_player.transform.position = new Vector3(24.111f, 0.6815f, 35.635f);//이동
                    this.main_player.transform.eulerAngles = new Vector3(0f, 270f, 0f);
                    this.main_player.GetComponent<CharacterController>().enabled = true;//캐릭터 컨트롤러 활성화
                    this.Gretel_Script.enabled = true;//스크립트해제
                }
                this.t_start = false;//한번만실행
            }
          
        }

        //텔레포트종료시
        if (t_fine == true)
        {
            open = true;
            close = false;
            t_fine = false;            

            this.none_closet1.SetActive(true);
            this.none_closet2.SetActive(true);
            this.none_closet3.SetActive(true);
            //이동불가해제
            this.Gretel_Script.next_step = main_player_1.STEP.NORMAL;
            this.Gretel_Script.step = main_player_1.STEP.NORMAL;
        }
        if (t_fine2 == true)
        {
            open = false;
            close = true;
            t_fine2 = false;
            t_length = false;//텔레포트끝
            StartCoroutine("open_closet");//일정시간후 텔레포트 벽장 활성화

            //문닫히는동안 컬라이더를 이용해 비활성화
            this.closet_wallcollider1.SetActive(true);
            this.closet_wallcollider2.SetActive(true);
            this.closet_wallcollider3.SetActive(true);

        }

        //H&G상태일때 이동불가
        if (this.Gretel_Script.step == main_player_1.STEP.H_G_NORMAL || this.Gretel_Script.step == main_player_1.STEP.H_G_WALK)
        {
            this.closet_collider1.gameObject.tag = "Untagged";
            this.closet_collider2.gameObject.tag = "Untagged";
            this.closet_collider3.gameObject.tag = "Untagged";
        }
        else
        {
            this.closet_collider1.gameObject.tag = "clock_closet";
            this.closet_collider2.gameObject.tag = "clock_closet";
            this.closet_collider3.gameObject.tag = "clock_closet";
        }

        this.clock_closet_animator1.SetBool("OPEN", this.open);
        this.clock_closet_animator2.SetBool("OPEN", this.open);
        this.clock_closet_animator3.SetBool("OPEN", this.open);
        this.clock_closet_animator1.SetBool("CLOSE", this.close);
        this.clock_closet_animator2.SetBool("CLOSE", this.close);
        this.clock_closet_animator3.SetBool("CLOSE", this.close);
    }

    IEnumerator open_closet()//활성화
    {
        yield return new WaitForSeconds(10.0f);//테스트용
        this.open = true;
        this.close = false;

        //문열리는동안 컬라이더를 이용한 비활성화 -> 활성화
        this.closet_wallcollider1.SetActive(false);
        this.closet_wallcollider2.SetActive(false);
        this.closet_wallcollider3.SetActive(false);

        yield return new WaitForSeconds(1.0f);//문열림시작후 딜레이
        //컬라이더 활성화
        this.closet_collider1.SetActive(true);
        this.closet_collider2.SetActive(true);
        this.closet_collider3.SetActive(true);


    }
}
