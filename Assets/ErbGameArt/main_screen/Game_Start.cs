using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//씬전환 네임스페이스

public class Game_Start : MonoBehaviour
{
    bool game_start = false;

    //게임시작버튼
    public bool start_button_Onclick = false;

    //메인타이틀
    public GameObject main_title;
    Animator main_title_animator;
    bool start = false;//애니메이션플래그

    //페이드아웃
    public Material black_screen_m;
    public GameObject black_screen;
    float opacity_point = 0.0f;
    public bool fade_out_enable = false;
    // Start is called before the first frame update
    void Start()
    {
        main_title_animator = main_title.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //카메라이동
        if (start_button_Onclick == true)
        {
            game_start = true;
            start = true;
            StartCoroutine("fade_out");
            start_button_Onclick = false;//플래그초기화
        }
        if (game_start == true)
        {
            this.transform.Translate(Vector3.forward * 40.0f * Time.deltaTime);


            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            

            if (this.transform.position.y > -16f && this.transform.position.z > 160f)
            {
                main_title.SetActive(false);
                this.transform.Translate(Vector3.down * 140.0f * Time.deltaTime);
            }
        }



        //페이드아웃

        if (fade_out_enable == true)//페이드아웃
        {
            if (opacity_point < 1)
            {
                opacity_point += 0.5f * Time.deltaTime;
            }
            else
            {
                opacity_point = 1.0f;

                //플래그초기화
                fade_out_enable = false;
                game_start = false;
                StartCoroutine("scene_change");
            }
        }
        this.black_screen_m.color = new Color(0.0f, 0.0f, 0.0f, opacity_point);


        //애니메이션관리
        this.main_title_animator.SetBool("START",start);
    }


    IEnumerator fade_out()//페이드아웃
    {
        yield return new WaitForSeconds(4.5f);
        black_screen.SetActive(true);
        fade_out_enable = true;
    }

    IEnumerator scene_change()//씬체인지
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Stage1");
    }
}
