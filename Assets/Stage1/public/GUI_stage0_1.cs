using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_stage0_1 : MonoBehaviour
{
    //게임매니저
    public GameObject management;
    game_management game_Management;

    //메인플레이어
    public GameObject Gretel;
    main_player_1 Gretel_Script;//그레텔 스크립트   

    public GameObject Hansel;
    Hansel_Script hansel_script;//핸젤 스크립트


   // public GameObject chase_eyes_warning;//감시의눈 경고이미지 -> eyes_pattern스크립트, eyes_chase스크립트 에서 사용
    public GameObject follow_hansel;//핸젤 파워업시 가이드 이미지
    public GameObject chase_eyes_warning;//감시의눈 경고 이미지

    public GameObject image;//핸젤이미지
    public Sprite[] tutorial_image = new Sprite[4];
    int index = 0;
    bool image_change_down = false;
    bool image_change_up = false;
    bool enable = true;

    // Start is called before the first frame update
    void Start()
    {
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        hansel_script = Hansel.GetComponent<Hansel_Script>();//핸젤스크립트

        StartCoroutine("tutorial1");
    }

    // Update is called once per frame
    void Update()
    {

        if (image_change_down == true)
        {
            if (image.GetComponent<RectTransform>().localPosition.y > 29.8f)
            {
                image.transform.Translate(Vector3.down * 300.0f * Time.deltaTime);
            }
            else
            {
                image.GetComponent<RectTransform>().localPosition = new Vector3(226f, 29.8f, 0f);
            }
        }

        if (image_change_up == true)
        {
            if (image.GetComponent<RectTransform>().localPosition.y < 373f)
            {
                image.transform.Translate(Vector3.up * 300.0f * Time.deltaTime);
            }
            else
            {
                image.GetComponent<RectTransform>().localPosition = new Vector3(226f, 373f, 0f);
            }
        }

        if (index >= 3)
        {
            index = 0;
            enable = false;
        }

        if (hansel_script.step == Hansel_Script.STEP.POWERUP || hansel_script.step == Hansel_Script.STEP.RUSH)
        {
            this.follow_hansel.SetActive(true);
        }
        else
        {
            this.follow_hansel.SetActive(false);
        }
    
        
    }

    IEnumerator tutorial1()//페이드인
    {
            yield return new WaitForSeconds(3.0f);
            this.image.GetComponent<Image>().sprite = tutorial_image[index];
            image.SetActive(true);
            image_change_down = true;
            yield return new WaitForSeconds(5.0f);
            image_change_down = false;
            yield return new WaitForSeconds(10.0f);
            image_change_up = true;
            yield return new WaitForSeconds(5.0f);
            image_change_up = false;
            image.SetActive(false);
            index++;
            if (enable == true && index < 3)
            {
                StartCoroutine("tutorial1");
            }
    }
}
