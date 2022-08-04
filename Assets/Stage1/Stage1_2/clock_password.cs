using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_password : MonoBehaviour
{
    public GameObject main_player;
    main_player_1 Gretel_Script;//그레텔 스크립트   
    public GameObject main_camera;//메인카메라
    main_camera main_camera_script;//메인카메라 스크립트   

    public GameObject[] clock_gear = new GameObject[8];//톱니바퀴
    public GameObject[] clock_mark = new GameObject[5];//3
    public GameObject mark_input;//마크 입력기
    public GameObject num_input;//번호 입력기
    public GameObject wire_siren;//철조망 사이렌
    public GameObject wire;//철조망

    Animator wire_animator;
    //재질
    public Material green_led;

    //인덱스
    public int[] num_index = new int [6];
    int mark_index = 1;
    //카메라뷰
    public float view_point = 60.0f;
    //오브젝트회전 
    float rotate_angle_mark = 0.0f;
    float rotate_angle_num = 0.0f;
    bool still_rotate = false;

    // Start is called before the first frame update
    void Start()
    {
        this.main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        this.Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트  
        this.wire_animator = wire.GetComponent<Animator>();//철조망 애니메이터
    }

    // Update is called once per frame
    void Update()
    {
        //pass2 - mark
        if (this.Gretel_Script.clock_pass2 == true)
        {

            //패스워드 입력
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && this.rotate_angle_mark == 0f)
            {
                if (this.mark_index < 4)//인덱스1~4 반복
                {
                    this.mark_index++;
                }
                else
                {
                    this.mark_index = 1;
                }

                this.rotate_angle_mark = 90.0f;//회전시작

            }
        }
 
        //마크 입력기 회전
        if (this.rotate_angle_mark > 0f)
        {
            this.rotate_angle_mark -= 55.0f * Time.deltaTime;//회전값
            this.mark_input.transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);

            for (int i = 0; i <= 4; i++) //톱니바퀴회전
            {
                this.clock_gear[i].transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);
            }
            this.clock_gear[6].transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);
        }
        else
        {
            this.rotate_angle_mark = 0.0f;
            if (mark_index == 1)
            {
                this.mark_input.transform.localEulerAngles = new Vector3(-90f, 0f, 120f);
            }
            else if (mark_index == 2)
            {
                this.mark_input.transform.localEulerAngles = new Vector3(-90f, 0f, 210f);
            }
            else if (mark_index == 3)
            {
                this.mark_input.transform.localEulerAngles = new Vector3(-90f, 0f, 300f);
            }
            else if (mark_index == 4)
            {
                this.mark_input.transform.localEulerAngles = new Vector3(-90f, 0f, 30f);
            }
        }

        //pass - num
        if (this.Gretel_Script.clock_pass1 == true)
        {
            //패스워드 입력
            if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) && this.rotate_angle_num == 0f)
            {
                if (this.num_index[mark_index] < 12)//인덱스1~12 반복
                {
                    this.num_index[mark_index]++;
                }
                else
                {
                    this.num_index[mark_index] = 1;
                }

                this.rotate_angle_num = 30.0f;//회전시작

            }
        }

        //번호 입력기 회전
        if (this.rotate_angle_num > 0f)
        {
            this.rotate_angle_num -= 55.0f * Time.deltaTime;//회전값
            this.clock_mark[mark_index - 1].transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);//시계번호판 회전
            this.num_input.transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);//번호입력판 회전

            for (int i = 0; i <= 5; i++) //톱니바퀴회전
            {
                this.clock_gear[i].transform.Rotate(0f, 0f, 55.0f * Time.deltaTime);
            }
        }
        else
        {
            this.rotate_angle_num = 0.0f;
        
            this.clock_mark[mark_index - 1].transform.localEulerAngles = new Vector3(0f, -90f, 90f + ((num_index[mark_index]) * 30f));//시계번호판
            this.num_input.transform.localEulerAngles = new Vector3(-90f, 0f, ((num_index[mark_index]) * 30f));//번호입력판
        }

        //카메라뷰
        if (this.Gretel_Script.clock_pass2 == true || this.Gretel_Script.clock_pass1 == true)
        {
            //카메라 뷰
            if (this.view_point > 40f)
            {
                this.view_point -= 25.0f * Time.deltaTime;
            }
            else
            {
                this.view_point = 40.0f;
            }
            this.main_camera.GetComponent<Camera>().fieldOfView = view_point;
        }
        else
        {
            //카메라 뷰
            if (this.view_point < 80f)
            {
                this.view_point += 25.0f * Time.deltaTime;
            }
            else
            {
                this.view_point = 80.0f;
            }
            this.main_camera.GetComponent<Camera>().fieldOfView = view_point;
        }

        //클리어판정
        if (this.num_index[1] == 5 && this.num_index[2] == 2 && this.num_index[3] == 11 && this.num_index[4] == 8) 
        {
            this.wire_siren.GetComponent<MeshRenderer>().material = green_led;
            this.wire_animator.SetBool("OPEN", true);
        }
    }// end of update
}
