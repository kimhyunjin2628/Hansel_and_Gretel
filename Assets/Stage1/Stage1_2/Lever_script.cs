using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_script : MonoBehaviour
{
    Animator lever_animator;//라벨애니메이터
    Animator shower_animator;//샤워애니메이터
    Animator closet_animator;//벽장애니메이터
    Animator closet_animator2;//벽장애니메이터

    //애니메이션 플래그
    bool on = false;//레버 on -> 가동중지
    bool off = false;//레버 off -> 재가동

    //벽장 애니메이션 플래그
    bool open = false;
    bool close = false;

    //게임매니지먼트
    public GameObject management;
    game_management game_Management;

    //카메라포지션
    public GameObject camera_position;//카메라포지션
    camera_position camera_position_script;//카메라포지션 스크립트

    //핸젤과 그레텔
    GameObject gretel;
    GameObject hansel;
    public GameObject lever;
    public GameObject lever_position;//레버 인식 위치
    public GameObject water_drop;//물방울
    public GameObject Shower;//샤워기
    public GameObject hanging_ground;//매달려있는 지형
    public GameObject cookie;//쿠키

    //벽장
    public GameObject closet;
    public GameObject closet2;
    public GameObject closet_collider;
    public GameObject closet2_collider;
    public bool teleport;
    public bool teleport2;

    //재질
    public Material green_red;
    //타이머
    float water_drop_timer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        lever_animator = this.GetComponent<Animator>();
        shower_animator = this.Shower.GetComponent<Animator>();
        closet_animator = this.closet.GetComponent<Animator>();
        closet_animator2 = this.closet2.GetComponent<Animator>();

        gretel = GameObject.FindGameObjectWithTag("Player");
        hansel = GameObject.FindGameObjectWithTag("Player2");
        this.water_drop.transform.localPosition = new Vector3(-0.48f, 11.7f, 3.819f);
        game_Management = management.GetComponent<game_management>();//게임관리스크립트

        camera_position_script = camera_position.GetComponent<camera_position>();//카메라포지션스크립트

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.lever_position.transform.position, gretel.transform.position) < 0.8f ||
            Vector3.Distance(this.lever_position.transform.position, hansel.transform.position) < 1.0f)
        {
            this.on = true;
            this.off = false;
            this.lever.GetComponent<MeshRenderer>().materials[0].color = Color.red;
            this.green_red.color = Color.red;
        }
        else
        {
            this.off = true;
            this.on = false;
            this.lever.GetComponent<MeshRenderer>().materials[0].color = Color.green;
            this.green_red.color = Color.green;
        }


        if (on == false)//레버가동중(그린)
        {
            if (water_drop_timer > 1.0)
            {
                water_drop.SetActive(true);
            }
            else
            {
                water_drop_timer += Time.deltaTime;
            }
            if (water_drop.transform.localPosition.y > 0.349f)
            {             
                this.water_drop.transform.Translate(Vector3.back * 0.5f * Time.deltaTime);
            }
            else
            {
                this.water_drop.transform.localPosition = new Vector3(-0.48f, 1.308f, 3.997f);
            }
           

            hanging_ground.GetComponent<BoxCollider>().enabled = false;

            //벽장
            this.open = false;
            this.close = true;
            this.closet_collider.SetActive(false);//컬라이더off
            this.closet2_collider.SetActive(false);//컬라이더off
            this.closet.GetComponent<BoxCollider>().enabled = true;//컬라이더on

        }
        else //(레드)
        {
            this.water_drop.SetActive(false);

            water_drop_timer = 0.0f;
            hanging_ground.GetComponent<BoxCollider>().enabled = true;//1-11로넘어갈수있음
        
        
            //벽장
            this.open = true;
            this.closet_collider.SetActive(true);//컬라이더on
            //this.closet2_collider.SetActive(true);//컬라이더on -> none_closet태그로 main_player스크립트에서 처리
            this.closet.GetComponent<BoxCollider>().enabled = false;//컬라이더off
        }


        if (teleport == true)//텔레포트직후
        {
            //텔레포트
            this.gretel.GetComponent<main_player_1>().enabled = false;
            this.gretel.GetComponent<CharacterController>().enabled = false;
            this.gretel.transform.position = new Vector3(4.261f, 9.1249f, 0.446f);
            this.gretel.transform.eulerAngles = new Vector3(0,-180,0);
            this.gretel.GetComponent<CharacterController>().enabled = true;
            this.gretel.GetComponent<main_player_1>().enabled = true;
            //벽장1컬라이더ON
            this.closet_collider.SetActive(true);

            //애니메이션플래그
            this.open = false;
            this.close = true;

            //플래그초기화
            teleport = false;
        }

        if (teleport2 == true)//텔레포트직후2
        {
            //텔레포트
            this.gretel.GetComponent<main_player_1>().enabled = false;
            this.gretel.GetComponent<CharacterController>().enabled = false;
            this.gretel.transform.position = new Vector3(3.88f, 0.551f, -4.36f);
            this.gretel.GetComponent<CharacterController>().enabled = true;
            this.gretel.GetComponent<main_player_1>().enabled = true;
            //벽장2컬라이더ON
            //this.closet2_collider.SetActive(true); -> none_closet태그로 main_player스크립트에서 처리

            //애니메이션플래그
            this.open = false;
            this.close = true;

            //플래그초기화
            teleport2 = false;
        }

        if (this.camera_position_script.teleport_comp == true)//텔레포트완료
        {
            open = true;
            close = false;
            //플래그초기화
            this.camera_position_script.teleport_comp = false;
        }

        lever_animator.SetBool("ON",on);
        lever_animator.SetBool("OFF", off);
        shower_animator.SetBool("ON",on);
        shower_animator.SetBool("OFF", off);
        closet_animator.SetBool("OPEN", open);
        closet_animator.SetBool("CLOSE",close);
        closet_animator2.SetBool("OPEN", open);
        closet_animator2.SetBool("CLOSE", close);
    }
}
