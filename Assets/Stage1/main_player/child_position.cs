using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class child_position : MonoBehaviour
{
    public GameObject soul_position;
    public GameObject key_position;
    public GameObject Hansel_position;
    public GameObject Key;
    public GameObject Hansel;
    public GameObject Soul;

    main_player_1 gretel_script;
    bool soul_arrive_y = false;
    bool soul_arrive_z = false;
    float soul_move_time = 0.0f;

    Soul_Script soul_script;//핸젤영혼스크립트

    // Start is called before the first frame update
    void Start()
    {
        this.gretel_script = this.GetComponent<main_player_1>();
        soul_script = Soul.GetComponent<Soul_Script>();//핸젤영혼스크립트
    }

    // Update is called once per frame
    void Update()
    {
        if (Key.transform.parent == this.gameObject.transform)
        {
            Key.transform.localPosition = key_position.transform.localPosition;
            //Key.transform.eulerAngles = new Vector3(-152.879f, -126.72f, -32.26398f);//회전값고정
            Key.transform.localRotation = Quaternion.Euler(-152.879f, -126.72f, -32.26398f);
        }
        if (Soul.transform.parent == this.gameObject.transform)
        {
            Soul.transform.localPosition = soul_position.transform.localPosition;
            Soul.transform.eulerAngles = this.transform.localEulerAngles;//회전값고정
        }
        if (Hansel.transform.parent == this.gameObject.transform)
        {
            Hansel.transform.localPosition = Hansel_position.transform.localPosition;
            //Hansel.transform.eulerAngles = new Vector3(this.transform.position.x, 3.96f, this.transform.position.z);//회전값고정
            Hansel.transform.localRotation = Quaternion.Euler(0f,3.96f,0f);
        }


        //올라갈시 소울좌표 이동
        if (gretel_script.step == main_player_1.STEP.HANG_JUMP || gretel_script.step == main_player_1.STEP.JUMP)
        {
            if (this.soul_move_time <= 0.5f)
            {
                this.soul_move_time += Time.deltaTime;
                this.soul_position.transform.Translate(Vector3.up * 0.5f * Time.deltaTime);
                this.soul_position.transform.Translate(Vector3.back * 0.5f * Time.deltaTime);
            }
            else
            {
                //애니메이션플래그
                this.soul_script.normal = true;
                this.soul_script.fire = false;
                this.soul_script.explosion = false;
                this.soul_script.jump_g = false;
            }
        }
        else
        {
            if (this.soul_position.transform.localPosition.y > -0.63f)
            {
                this.soul_position.transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
                //애니메이션플래그
                this.soul_script.normal2 = false;
                this.soul_script.walk2 = true;
            }
            else
            {
                this.soul_arrive_y = true;
            }

            if (this.soul_position.transform.localPosition.z < 1.6f)
            {
                this.soul_position.transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
            }
            else
            {
                this.soul_arrive_z = true;
            }

            if(this.soul_arrive_z == true && this.soul_arrive_y == true)
            {
                this.soul_position.transform.localPosition = new Vector3(-0.9f, -0.63f, 1.6f);
                this.soul_arrive_y = false;
                this.soul_arrive_z = false;

                //애니메이션플래그
                this.soul_script.walk2 = false;
                this.soul_script.normal2 = true;

                //플래그초기화
                this.soul_move_time  = 0.0f;
            }
        }

    }//end of Update
}
