using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class save_point : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject candy;
    //public GameObject save;
    float timer = 0.0f;
    float y_position;//y축고정
    public GameObject management;//게임관리자


    //그레텔 스크립트
    main_player_1 Gretel_Script;
    game_management game_Management;//게임관리 스크립트

    void Start()
    {
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
        y_position = this.transform.position.y;//y축정보저장
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f)
        {
            timer = 0.0f;
            this.transform.position = new Vector3(this.transform.position.x, y_position,this.transform.position.z); 
        }

        if (timer>=0.5f)
        {
            this.transform.Translate(0, 0.18f * Time.deltaTime, 0);
        }
        else
        {
            this.transform.Translate(0, -0.18f * Time.deltaTime, 0);
        }

        if (Gretel_Script.save_point1 == true)//세이브포인트1 활성화
        {
            this.GetComponent<BoxCollider>().enabled = false;
            candy.SetActive(false);
            //save.SetActive(true);
        }
    }

}


