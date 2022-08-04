using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage1_animation : MonoBehaviour
{
    Animator stage1_animator;

    public GameObject[] witch = new GameObject[10];//애니메이션용 마녀
    public GameObject witch_object; //마녀
    public GameObject Soul;//애니메이션용 소울
    public GameObject drink;//애니메이션용 술병


    //핸젤
    Hansel_Script hansel_script;//헨젤스크립트

    //게임매니저
    public GameObject management;
    game_management game_Management;

    public bool stage1_1 = false; //스테이지 1-1 마녀등장
    public bool animation_fin = false;//애니메이션 종료플래그
    
    float timer = 0.0f;

    //문열기
    public bool door_open = false;//애니메이션 플래그
    bool door_open_delay = false;//핸젤rush모드 -> 딜레이후 문열림

    // Start is called before the first frame update
    void Start()
    {
        stage1_animator = GetComponent<Animator>();//애니메이션 컨트롤러
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        hansel_script = GameObject.FindGameObjectWithTag("Player2").GetComponent<Hansel_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stage1_1 == true)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 22f)
        {
            animation_fin = true;
            timer = 0;
            stage1_1 = false;
        }

        if (animation_fin == true)
        {
            this.Soul.SetActive(true);
        }
        if (this.game_Management.stage_num_1_3 == true)
        {
            this.Soul.SetActive(false);
        }

        if (this.game_Management.stage_num_1_4 == true)//애니메이션용 마녀삭제
        {
            for (int i = 0; i <= 8; i++)
            {
                this.witch[i].SetActive(false);
            }
            this.witch_object.SetActive(true);
        }

        if (timer >= 15f)
        {
            this.drink.SetActive(false);
        }


        //다음스테이지로 넘어가는 open_door
        if (hansel_script.step == Hansel_Script.STEP.RUSH && door_open_delay == false)
        {
            StartCoroutine("open");
            door_open_delay = true;//일정딜레이(코루틴)후 문열림, 코루틴 한번만 호출
        }

        //애니메이션 관리
        stage1_animator.SetBool("STAGE1_1", stage1_1);
        stage1_animator.SetBool("OPEN_DOOR", door_open);
    }

    IEnumerator open()//새장밑으로
    {
        yield return new WaitForSeconds(0.5f);
        door_open = true;

    }
}
