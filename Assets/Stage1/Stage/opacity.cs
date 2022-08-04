using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opacity : MonoBehaviour
{
    public GameObject management;
    game_management game_Management;//게임관리 스크립트

    public Material scnack1;
    public Material scnack2;

    float opacity_point = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        this.scnack1.color = new Color(0.9f, 0.6f, 0.35f, opacity_point);
        this.scnack2.color = new Color(0.1f, 0.1f, 0.1f, opacity_point);

        if (game_Management.stage_num_1_8 == true)
        {
            if (opacity_point > 0.8f)
            {
                opacity_point -= 0.15f * Time.deltaTime;
            }
            else if (opacity_point <= 0.8f && opacity_point > 0)
            {
                opacity_point -= 0.5f * Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(false);
                opacity_point = 1.0f;
            }
        }
    }
}
