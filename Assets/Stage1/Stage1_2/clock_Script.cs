using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock_Script : MonoBehaviour
{
    public GameObject[] clock_spade = new GameObject[4];
    public GameObject[] clock_heart = new GameObject[4];
    public GameObject[] clock_joker = new GameObject[4];

    public GameObject angle_spade;// 시곗바늘각도
    public GameObject angle_heart;// 시곗바늘각도
    float joker_current_angle_x = 90.0f;//시곗바늘각도

    public GameObject Gretel;//그레텔
    public GameObject main_camera;//메인카메라
    main_camera main_camera_script;//메인카메라 스크립트

    public GameObject clock_work;//시계태엽
    clock_work_script clock_work_script;//시계태엽 스크립트

    public GameObject Witch;

    //스테이지 클리어
    public bool clock_cross = false;
    public bool clock_freeze = false;
    public GameObject diamond_box;//스낵3변환가능
    public GameObject snack_ground;//스낵 발견 판정

    // Start is called before the first frame update
    void Start()
    {
        this.main_camera_script = main_camera.GetComponent<main_camera>();
        clock_work_script = clock_work.GetComponent<clock_work_script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clock_freeze == false)//시계활성화
        {
            if (main_camera_script.clock_tower_mode == true)
            {
                //스페이드회전
                for (int i = 0; i < 4; i++)
                {
                    this.clock_spade[i].transform.Rotate(0, 360f * Time.deltaTime, 0);
                    this.angle_spade.transform.LookAt(Gretel.transform.position);
                    //this.clock_diamond.transform.localRotation =  Quaternion.Euler(0.0f, angle_diamond.transform.localEulerAngles.y, 0.0f);//회전값고정           
                }
                this.clock_spade[0].transform.eulerAngles = new Vector3(angle_spade.transform.localEulerAngles.y - 90f, 90f, -90f);
                this.clock_spade[1].transform.eulerAngles = new Vector3(angle_spade.transform.localEulerAngles.y - 90f, 180f, -90f);
                this.clock_spade[2].transform.eulerAngles = new Vector3(angle_spade.transform.localEulerAngles.y - 90f, -90f, -90f);
                this.clock_spade[3].transform.eulerAngles = new Vector3(angle_spade.transform.localEulerAngles.y - 90f, 0f, -90f);

                //하트회전
                for (int i = 0; i < 4; i++)
                {
                    this.clock_heart[i].transform.Rotate(0, 360f * Time.deltaTime, 0);
                    this.angle_heart.transform.LookAt(Witch.transform.position);
                    //this.clock_diamond.transform.localRotation =  Quaternion.Euler(0.0f, angle_diamond.transform.localEulerAngles.y, 0.0f);//회전값고정           
                }
                this.clock_heart[0].transform.eulerAngles = new Vector3(angle_heart.transform.localEulerAngles.y - 90f, 90f, -90f);
                this.clock_heart[1].transform.eulerAngles = new Vector3(angle_heart.transform.localEulerAngles.y - 90f, 180f, -90f);
                this.clock_heart[2].transform.eulerAngles = new Vector3(angle_heart.transform.localEulerAngles.y - 90f, -90f, -90f);
                this.clock_heart[3].transform.eulerAngles = new Vector3(angle_heart.transform.localEulerAngles.y - 90f, 0f, -90f);
            }
            else
            {
                //시곗바늘초기화
                this.clock_spade[0].transform.eulerAngles = new Vector3(90f, 90f, -90f);
                this.clock_spade[1].transform.eulerAngles = new Vector3(90f, 180f, -90f);
                this.clock_spade[2].transform.eulerAngles = new Vector3(90f, -90f, -90f);
                this.clock_spade[3].transform.eulerAngles = new Vector3(90f, 0f, -90f);

                this.clock_heart[0].transform.eulerAngles = new Vector3(90f, 90f, -90f);
                this.clock_heart[1].transform.eulerAngles = new Vector3(90f, 180f, -90f);
                this.clock_heart[2].transform.eulerAngles = new Vector3(90f, -90f, -90f);
                this.clock_heart[3].transform.eulerAngles = new Vector3(90f, 0f, -90f);
            }

            //조커회전
            if (clock_work_script.kill >= 18)
            {
                clock_work_script.kill = 18;
            }
            if ((90 - (clock_work_script.kill * 5.0f)) <= joker_current_angle_x)
            {
                joker_current_angle_x -= Time.deltaTime * 5.0f;
            }
            else
            {
                joker_current_angle_x = 90f - (clock_work_script.kill * 5.0f);
            }
            this.clock_joker[0].transform.eulerAngles = new Vector3(joker_current_angle_x, 90f, -90f);
            this.clock_joker[1].transform.eulerAngles = new Vector3(joker_current_angle_x, 180f, -90f);
            this.clock_joker[2].transform.eulerAngles = new Vector3(joker_current_angle_x, -90f, -90f);
            this.clock_joker[3].transform.eulerAngles = new Vector3(joker_current_angle_x, 0f, -90f);

            //시계패턴종료 - 클리어
            if (this.angle_spade.transform.localEulerAngles.y >= 0.0f && this.angle_spade.transform.localEulerAngles.y <= 6.0f
                 && this.angle_heart.transform.localEulerAngles.y >= 177.5f && this.angle_heart.transform.localEulerAngles.y <= 182.5f
                 && this.clock_work_script.kill == 18)
            {
                clock_cross = true;
                clock_freeze = true;

                //시계바늘 고정
                this.clock_spade[0].transform.eulerAngles = new Vector3(-90f, 90f, -90f);
                this.clock_spade[1].transform.eulerAngles = new Vector3(-90f, 180f, -90f);
                this.clock_spade[2].transform.eulerAngles = new Vector3(-90f, -90f, -90f);
                this.clock_spade[3].transform.eulerAngles = new Vector3(-90f, 0f, -90f);

                this.clock_heart[0].transform.eulerAngles = new Vector3(90f, 90f, -90f);
                this.clock_heart[1].transform.eulerAngles = new Vector3(90f, 180f, -90f);
                this.clock_heart[2].transform.eulerAngles = new Vector3(90f, -90f, -90f);
                this.clock_heart[3].transform.eulerAngles = new Vector3(90f, 0f, -90f);

                /*this.clock_joker[0].transform.eulerAngles = new Vector3(180f, 90f, -90f);
                this.clock_joker[1].transform.eulerAngles = new Vector3(180, 180f, -90f);
                this.clock_joker[2].transform.eulerAngles = new Vector3(180, -90f, -90f);
                this.clock_joker[3].transform.eulerAngles = new Vector3(180, 0f, -90f);*/

                //다이아몬드 보물상자 생성
                this.diamond_box.SetActive(true);
                //판정영역 컬라이더 생성
                this.snack_ground.SetActive(true);
            }
        }//end of frreeze
    }//end of Update
}
