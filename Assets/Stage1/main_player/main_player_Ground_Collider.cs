using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_player_Ground_Collider : MonoBehaviour
{
    main_player_1 main_player_1;
    CharacterController Ground_cc;

    public bool _is_ground = false;



    // Start is called before the first frame update
    void Start()
    {
        main_player_1 = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();
        Ground_cc = this.GetComponent<CharacterController>();//캐릭터 컨트롤러
    }

    // Update is called once per frame
    void Update()
    {
        //Ground_cc.Move(Vector3.forward * Time.deltaTime);//이동처리
        //Ground_cc.Move(Vector3.back * Time.deltaTime);//이동처리
        Ground_cc.Move(Vector3.down * 3.0f * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)//땅에 닿은 직후
    {
        Debug.Log("닿음");
        if (hit.collider.tag == "Ground")
        {
            _is_ground = true;
        }//end of if
    }//end of OnControllerColliderHit*/

}
