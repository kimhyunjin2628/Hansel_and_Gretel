using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snack_item : MonoBehaviour
{
    float timer = 0.0f;
    float y_position;//y축고정

    public bool remove_snack_item;
    //게임매니저
    public GameObject management;
    game_management game_Management;
    // Start is called before the first frame update
    void Start()
    {
        y_position = this.transform.position.y;//y축정보저장
        game_Management = management.GetComponent<game_management>();//게임관리스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (game_Management.stage_num_1_8 == true)
        {
            timer += Time.deltaTime;
            if (timer >= 1.0f)
            {
                timer = 0.0f;
                this.transform.position = new Vector3(this.transform.position.x, y_position, this.transform.position.z);
            }

            if (timer >= 0.5f)
            {
                this.transform.Translate(0, 0, 0.25f * Time.deltaTime);
            }
            else
            {
                this.transform.Translate(0, 0, -0.25f * Time.deltaTime);
            }
        }

        if (remove_snack_item == true)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "cookie_position_change")
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y,2.5f);
        }
    }
}

