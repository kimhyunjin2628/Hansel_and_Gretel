using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snack_script : MonoBehaviour
{
    public GameObject[] fake_snack = new GameObject[5];//과자로변하기전
    public GameObject[] snack = new GameObject[5];//과자,스테이지별
    public GameObject snack2;//과자2

    public GameObject Soul_GameObject;//핸젤소울
    public GameObject management;          
    game_management game_Management;//게임관리 스크립트
    Soul_Script soul_script;//핸젤영혼스크립트
    public GameObject magic_particle;//과자아이템파티클

    //스테이지별 소환

    bool timer_flag = true;
    bool snack_falg = true;
    float timer = 0.0f;
    int index = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        soul_script = Soul_GameObject.GetComponent<Soul_Script>();//헨젤영혼스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (soul_script.explosion == true && game_Management.stage_num_1_7 == true && timer_flag == true)
        {
            if (timer >= 1.0f)//1초딜레이
            {
                fake_snack[index].SetActive(false);
                snack[index].SetActive(true);

                timer = 0.0f;//타이머초기화
                index++;
                timer_flag = false;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if (game_Management.stage_num_1_8 == true && snack_falg == true)
        {        
            StartCoroutine("snack_item");

            //플래그초기화
            snack_falg = false;
            timer_flag = true;
        }

        if (soul_script.explosion == true && game_Management.stage_num_1_11 == true && timer_flag == true)
        {
            index = 1;
            if (timer >= 1.0f)//1초딜레이
            {
                snack[index].SetActive(true);
                fake_snack[index].SetActive(false);
                fake_snack[++index].SetActive(false);
                fake_snack[++index].SetActive(false);

                timer = 0.0f;//타이머초기화
                index++;
                timer_flag = false;
                snack_falg = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        if (game_Management.stage_num_1_11 == true && snack_falg == true)
        {
            StartCoroutine("snack_item2");

            //플래그초기화
            snack_falg = false;
            timer_flag = true;
        }

        if (soul_script.explosion == true && game_Management.stage_num_1_17 == true && timer_flag == true)
        {
            index = 2;
            if (timer >= 1.0f)//1초딜레이
            {
                snack[index].SetActive(true);
                fake_snack[index + 2].SetActive(false);

                timer = 0.0f;//타이머초기화
                index++;
                timer_flag = false;
                snack_falg = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        if (game_Management.stage_num_1_17== true && snack_falg == true)
        {
            StartCoroutine("snack_item3");

            //플래그초기화
            snack_falg = false;
            timer_flag = true;
        }

        if (soul_script.explosion == true && game_Management.stage_num_1_20 == true && timer_flag == true)
        {
            index = 3;
            if (timer >= 1.0f)//1초딜레이
            {
                snack[index].SetActive(true);
                fake_snack[index + 2].SetActive(false);

                timer = 0.0f;//타이머초기화
                index++;
                timer_flag = false;
                snack_falg = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        if (game_Management.stage_num_1_20 == true && snack_falg == true)
        {
            StartCoroutine("snack_item4");

            //플래그초기화
            snack_falg = false;
            timer_flag = true;
        }
    }

    IEnumerator snack_item()
    {
        yield return new WaitForSeconds(2.0f);
        snack2.SetActive(true);
        this.magic_particle.SetActive(true);
        this.magic_particle.transform.position = new Vector3(-10.736f, 0.022f, -6.325f);
    }

    IEnumerator snack_item2()
    {
        yield return new WaitForSeconds(3.0f);
        this.magic_particle.SetActive(true);
        this.magic_particle.transform.position = new Vector3(1.82f, 8.325f, -5.772f);
    }

    IEnumerator snack_item3()
    {
        yield return new WaitForSeconds(4.0f);
        this.magic_particle.SetActive(true);
        this.magic_particle.transform.position = new Vector3(22.704f, 0.022f, 30.5f);
    }

    IEnumerator snack_item4()
    {
        yield return new WaitForSeconds(6.5f);
        this.magic_particle.SetActive(true);
        this.magic_particle.transform.position = new Vector3(-27.378f, -12.63f, 8.945f);
    }
}


