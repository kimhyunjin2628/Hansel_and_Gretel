using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul_Weight : MonoBehaviour
{
    Animator soul_weight_animator;//소울애니메이터
    CharacterController soul_weight_cc;//소울캐릭터컨트롤러

    public GameObject soul;
    //애니메이션플래그
    bool attack;
    float attack_timer;
    //추 컬라이더
    public GameObject weight_collider;

    // Start is called before the first frame update
    void Start()
    {
        soul_weight_animator = this.soul.GetComponent<Animator>();
        soul_weight_cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack == false)
        {
            //이동
            Vector3 direcction = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical")); //(S모드)
                                                                                                           //회전
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                this.transform.localRotation = Quaternion.Euler(0, 90, 0);

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                this.transform.localRotation = Quaternion.Euler(0, -90, 0);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.D))
            {
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);

            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.A))
            {
                this.transform.localRotation = Quaternion.Euler(0, -180, 0);
            }

            soul_weight_cc.Move(((direcction * 1f * Time.deltaTime).normalized) / 20f);//이동처리 

            //공격
            if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                attack = true;
                StartCoroutine("attack_fine");
            }
        }
        else
        {
            if (attack_timer <= 1.2f)
            {
                attack_timer += Time.deltaTime;
            }
            else
            {
                attack_timer = 0.0f;
                attack = false;
            }
        }
        //애니메이션관리
        soul_weight_animator.SetBool("ATTACK",attack);
    }

    IEnumerator attack_fine()//어택 공격판정 잔상 삭제
    {
        yield return new WaitForSeconds(0.6f);
        this.weight_collider.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.8f);
        this.weight_collider.GetComponent<BoxCollider>().enabled = true;
    }
}
