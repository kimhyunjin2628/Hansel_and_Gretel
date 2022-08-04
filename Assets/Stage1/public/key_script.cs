using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key_script : MonoBehaviour
{
    // Start is called before the first frame update
    float timer = 0.0f;
    float y_position;//y축고정
    public main_player_1 Gretel_Script;

    public GameObject Game_over;
    game_over Game_over_script;//게임오버스크립트

    //게임매니지먼트
    public GameObject management;
    game_management game_Management;

    void Start()
    {
        y_position = this.transform.position.y;//y축정보저장
        Game_over_script = Game_over.GetComponent<game_over>();

        game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어와 연동
        if (Gretel_Script.get_key == false)
        {
            timer += Time.deltaTime;
            if (timer >= 1.6f)
            {
                timer = 0.0f;
                this.transform.position = new Vector3(this.transform.position.x, y_position, this.transform.position.z);
            }

            if (timer >= 0.8f)
            {
                this.transform.Translate(0, 0.1f * Time.deltaTime, 0);
            }
            else
            {
                this.transform.Translate(0, -0.1f * Time.deltaTime, 0);
            }
        }
        if (Gretel_Script.on_rock == true)
        {    
            this.transform.parent = null;

            this.transform.position = new Vector3(16.14f, 6.852f, -4.197f);
            this.transform.localRotation = Quaternion.Euler(-152.879f, -126.72f, -32.26398f);//회전값고정

            this.GetComponent<BoxCollider>().enabled = true;
            //플래그초기화
            Gretel_Script.on_rock = false;
        }

        //키삭제
        if (this.game_Management.stage_num >= 12)
        {
            this.gameObject.SetActive(false);
        }


    }
}
