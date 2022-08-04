using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beelze_bub_shadow_script : MonoBehaviour
{
    //애니메이터
    Animator bub_animator;

    //메인플레이어
    public GameObject Gretel;
    main_player_1 Gretel_Script;//그레텔 스크립트   

    //게임매니저
    public GameObject management;
    game_management game_Management;

    //메인카메라
    public GameObject main_camera;
    main_camera main_camera_script;

    //눈회전
    public float rotation_y = 0.0f;
    float constant = 0.5f;//회전값 상수

    //애니메이션 플래그
    bool open = false;
    bool close = false;
    // Start is called before the first frame update
    void Start()
    {
        bub_animator = this.GetComponent<Animator>();//벨제부브 애니메이터
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        main_camera_script = main_camera.GetComponent<main_camera>();//메인카메라 스크립트
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (this.game_Management.stage_num > 14f && this.main_camera_script.bridge3_mode == true && //플레이어 가까이 올시 눈뜸
             this.Gretel.transform.position.x < -17f)
        {
            //애니메이션플래그
            this.open = true;
        }

        if (this.open == true)//눈뜬상태
        {
            if (this.Gretel.transform.position.x < -17f)
            {
                this.rotation_y = this.Gretel.transform.position.x * 3.3f - 295f;//플레이어 회전값에대한 눈 회전값
                if (this.rotation_y < -377f)
                {
                    this.rotation_y = -377f;
                }
            }
            else
            {
                //애니메이션플래그
                this.close = true;
            }
      
            this.transform.localEulerAngles = new Vector3(15.0f,rotation_y,0.0f);
        }

        if (this.close == true)
        {
            if (this.rotation_y > -351)
            {
                this.rotation_y -= 30.0f * Time.deltaTime;
            }
            else
            {
                this.rotation_y = -351f;
            }

            if (this.Gretel.transform.position.x < -17f)
            {
                this.close = false;
            }
        }

        //애니메이션플래그
        this.bub_animator.SetBool("OPEN",this.open);
        this.bub_animator.SetBool("CLOSE", this.close);
    }

}
