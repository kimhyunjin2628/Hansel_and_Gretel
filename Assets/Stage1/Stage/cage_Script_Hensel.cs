using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cage_Script_Hensel : MonoBehaviour
{

    public bool trap_on = false;
    bool start = false;

    GameObject cage;//새장
    public GameObject key;//열쇠
    public main_player_1 Gretel_Script;//그레텔스크립트
    public Hansel_Script Hansel_Script;//핸젤스크립트
    bool trap_up_active;//trap_up 코루틴 한번만 실행
    bool trap_remove = false; //트랩 제거
    public bool rock_on;//케이지 스크립트용 rock_on 

    // Start is called before the first frame update
    void Start()
    {
        trap_on = false;
        start = false;
        cage = this.transform.GetChild(2).gameObject;//새장
        trap_up_active = true;
     //   Gretel_Script = GameObject.FindGameObjectWithTag("Player").GetComponent<main_player_1>();//그레텔스크립트
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cage);
        if (start == false)
        {
            StartCoroutine("trap_down");
            start = true;
        }

        if (trap_on == true)//새장 하강
        {
            if (this.cage.transform.localPosition.y > -3.4f)
            {
                this.cage.transform.Translate(Vector3.down * 9f * Time.deltaTime);
            }
            else
            {
                this.cage.transform.localPosition = new Vector3(0.14452f, -3.4f, -0.156f);
                trap_on = false;
            }

        }

        if (rock_on == true && trap_up_active == true)//트랩해제 직후
        {
            this.key.SetActive(true);
            StartCoroutine("trap_up");
            Hansel_Script.chaos = false;
            trap_up_active = false;
        }

        if (trap_remove == true)//트랩제거
        {
            if (this.cage.transform.localPosition.y < 8.0f)
            {
                this.cage.transform.Translate(Vector3.up * 9f * Time.deltaTime);
            }
            else
            {
                trap_remove = false;
                Destroy(this.gameObject);
                
            }
        }
    }

    IEnumerator trap_down()//새장밑으로
    {
        yield return new WaitForSeconds(1.5f);
        trap_on = true;
    }


    IEnumerator trap_up()//새장위로
    {
        yield return new WaitForSeconds(1.5f);
        Hansel_Script.hold_hands_X = false;
        trap_remove = true;
    }

}
