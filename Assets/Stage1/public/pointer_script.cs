using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer_script : MonoBehaviour
{

    //그레텔 스크립트
    main_player_1 Gretel_Script;
    //game_management game_Management;//게임관리 스크립트
    // Start is called before the first frame update
    void Start()
    {
        Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
       // game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Gretel_Script.pointer_1_4 == true || this.Gretel_Script.pointer_1_6 == true)
        {
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
