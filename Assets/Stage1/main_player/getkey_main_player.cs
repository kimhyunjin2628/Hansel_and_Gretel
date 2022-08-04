using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getkey_main_player : MonoBehaviour
{
    CharacterController Gretel_cc;
    Animator Gretel_animator;


    public enum STEP //동작
    {
        NORMAL, //0 기본
        WALK, //1 걷기
        RUN, //2 뛰기
        NUM //3 동작수
    };
    public STEP step = STEP.NORMAL;//현재상태
    public STEP next_step;//다음상태

    //이동 - 포지션(position)
    float move_speed = 5f;//이동속도
    float move_dev = 10.0f; //이동속도관련

    //이동 - 로테이션(rotation)
    float rotate_speed = 360.0f;//회전속도
    public bool rotate_enable = false;
    public bool r1 = false;//회전변수1
    public bool r2 = false;//회전변수2

    //애니메이션 플래그
    public bool walk = false;
    public bool run = false;
    public bool normal = false;


    // Start is called before the first frame update
    void Start()
    {
        Gretel_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
        Gretel_animator = GetComponentInChildren<Animator>();//애니메이션 컨트롤러
    }

    // Update is called once per frame
    void Update()
    {
        //캐릭터 이동

        //이동1. 전진
        if (Input.GetKey(KeyCode.RightArrow))
         {
             //회전
             if (this.transform.localEulerAngles.y != 90.0f) // 오른쪽회전
             {
                 rotate_enable = true;//회전중
                 if (this.transform.localEulerAngles.y > 90.0f && this.transform.localEulerAngles.y < 270f)
                 {
                     this.transform.Rotate(0, -rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y < 90.0f) // 회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 90.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }
                 else if (this.transform.localEulerAngles.y >= 270.0f || this.transform.localEulerAngles.y < 90.0f) //왼쪽회전
                 {
                     rotate_enable = true;//회전중
                     this.transform.Rotate(0, rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y < 90.0f) //회전완료처리
                     {
                         r1 = true;//회전 추가연산을 위한 변수
                     }
                     if (r1 == true)
                     {
                         if (this.transform.localEulerAngles.y > 90.0f)
                         {
                             this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 90.0f, this.transform.localRotation.z));//로테이션 위치 고정
                             r1 = false;
                             rotate_enable = false; //회전종료
                         }
                     }
                 }
             }
             else
             {
                 rotate_enable = false; //회전종료
             }


             if (Input.GetKey(KeyCode.LeftShift))
             {
                 move_dev = 10.0f;

                 run = true;
                 walk = false;
                 normal = false;

                 this.next_step = STEP.RUN;
             }
             else
             {
                 move_dev = 16.0f;

                 walk = true;
                 run = false;
                 normal = false;

                 this.next_step = STEP.WALK;
             }

             if (rotate_enable == false)//회전중일 경우는 포지션(position)변경 X
             {
                 Gretel_cc.Move(((transform.forward * Time.deltaTime * move_speed).normalized) / move_dev);//이동
             }
         }
         //이동2. 후진
         else if (Input.GetKey(KeyCode.LeftArrow))
         {
             //회전
             if (this.transform.localEulerAngles.y != 270.0f) // 오른쪽회전
             {
                 rotate_enable = true; //회전중
                 if (this.transform.localEulerAngles.y > 270.0f || this.transform.localEulerAngles.y < 90f)
                 {
                     this.transform.Rotate(0, -rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y > 270.0f) // 회전완료처리
                     {
                         r2 = true; //회전 추가연산을 위한 변수
                     }
                     if (r2 == true)
                     {
                         if (this.transform.localEulerAngles.y < 270.0f)
                         {
                             this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 270.0f, this.transform.localRotation.z));//로테이션 위치 고정
                             r2 = false;
                             rotate_enable = false; //회전종료
                         }
                     }
                 }
                 else if (this.transform.localEulerAngles.y < 270.0f && this.transform.localEulerAngles.y >= 90.0f) //왼쪽회전
                 {
                     rotate_enable = true; //회전중
                     this.transform.Rotate(0, rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y > 270.0f) //회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 270.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }


             }
             else
             {
                 rotate_enable = false; //회전종료
             }


             if (Input.GetKey(KeyCode.LeftShift))
             {
                 move_dev = 10.0f;

                 run = true;
                 walk = false;
                 normal = false;

                 this.next_step = STEP.RUN;
             }
             else
             {
                 move_dev = 16.0f;

                 walk = true;
                 run = false;
                 normal = false;

                 this.next_step = STEP.WALK;
             }

             if (rotate_enable == false)//회전중일 경우는 포지션(position)변경 X
             {
                 Gretel_cc.Move(((transform.forward * Time.deltaTime * move_speed).normalized) / move_dev);//이동
             }
         }
         //이동3 윗방향이동
         else if (Input.GetKey(KeyCode.UpArrow))
         {
             //회전
             if (this.transform.localEulerAngles.y != 0.0f) // 오른쪽회전
             {
                 rotate_enable = true; //회전중
                 if (this.transform.localEulerAngles.y < 360.0f && this.transform.localEulerAngles.y >= 180f)
                 {
                     this.transform.Rotate(0, rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y < 90.0f) // 회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 0.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }
                 else if (this.transform.localEulerAngles.y > 0.0f && this.transform.localEulerAngles.y < 180.0f) //왼쪽회전
                 {
                     rotate_enable = true; //회전중
                     this.transform.Rotate(0, -rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y > 270.0f) //회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 0.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }


             }
             else
             {
                 rotate_enable = false; //회전종료
             }

             if (Input.GetKey(KeyCode.LeftShift))
             {
                 move_dev = 10.0f;

                 run = true;
                 walk = false;
                 normal = false;

                 this.next_step = STEP.RUN;
             }
             else
             {
                 move_dev = 16.0f;

                 walk = true;
                 run = false;
                 normal = false;

                 this.next_step = STEP.WALK;
             }

             if (rotate_enable == false)//회전중일 경우는 포지션(position)변경 X
             {
                 Gretel_cc.Move(((transform.forward * Time.deltaTime * move_speed).normalized) / move_dev);//이동
             }
         }
         //이동4 아래방향이동
         else if (Input.GetKey(KeyCode.DownArrow))
         {
             //회전
             if (this.transform.localEulerAngles.y != 180.0f) // 오른쪽회전
             {
                 rotate_enable = true; //회전중
                 if (this.transform.localEulerAngles.y >= 0.0f && this.transform.localEulerAngles.y < 180f)
                 {
                     this.transform.Rotate(0, rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y >= 180.0f) // 회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 180.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }
                 else if (this.transform.localEulerAngles.y > 180.0f && this.transform.localEulerAngles.y < 360.0f) //왼쪽회전
                 {
                     rotate_enable = true; //회전중
                     this.transform.Rotate(0, -rotate_speed, 0);//회전속도에 따른 회전처리

                     if (this.transform.localEulerAngles.y <= 180.0f) //회전완료처리
                     {
                         this.transform.localRotation = Quaternion.Euler(new Vector3(this.transform.localRotation.x, 180.0f, this.transform.localRotation.z));//로테이션 위치 고정
                         rotate_enable = false; //회전종료
                     }
                 }


             }
             else
             {
                 rotate_enable = false; //회전종료
             }

             if (Input.GetKey(KeyCode.LeftShift))
             {
                 move_dev = 10.0f;

                 run = true;
                 walk = false;
                 normal = false;

                 this.next_step = STEP.RUN;
             }
             else
             {
                 move_dev = 16.0f;

                 walk = true;
                 run = false;
                 normal = false;

                 this.next_step = STEP.WALK;
             }

             if (rotate_enable == false)//회전중일 경우는 포지션(position)변경 X
             {
                 Gretel_cc.Move(((transform.forward * Time.deltaTime * move_speed).normalized) / move_dev);//이동
             }
         }
         //기본상태
         else
         {
             walk = false;
             run = false;
             normal = true;

             //회전변수 초기화
             r1 = false;
             r2 = false;
             rotate_enable = false; //회전종료
         }

        //애니메이션 관리
        Gretel_animator.SetBool("WALK", walk);
        Gretel_animator.SetBool("RUN", run);
        Gretel_animator.SetBool("NORMAL", normal);
        //동작관리
        this.step = this.next_step;
    }
}
