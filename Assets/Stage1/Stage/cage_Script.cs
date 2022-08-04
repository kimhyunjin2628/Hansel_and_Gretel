using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cage_Script : MonoBehaviour
{
    Animator eye_animator;

    public GameObject key;//열쇠
    public GameObject rock1;//자물쇠
    public GameObject rock2;//자물쇠
    public main_player_1 Gretel_Script;//그레텔스크립트
    public Hansel_Script Hansel_Script;//핸젤스크립트
    public bool trap_up_active = true;//trap_up 코루틴 한번만 실행
    bool trap_remove = false; //트랩 제거
    public bool rock_on;//케이지 스크립트용 rock_on 
    

    bool trap_on = false;
    bool start = false;
    public bool fin = false;
    bool remove = false;
    public bool hold = false;

    //오브젝트 사라지지않고 남아있을경우 
    public float kill_trap_timer =0.0f;

    GameObject cage;
    // Start is called before the first frame update
    void Start()
    {
        trap_on = false;
        start = false;
        fin = false;
        rock_on = false;
        cage = this.transform.GetChild(2).gameObject;//새장
        eye_animator = GetComponent<Animator>();//애니메이션 컨트롤러

        Debug.Log("시작");
    }

    // Update is called once per frame
    void Update()
    {
        //오브젝트 사라지지않고 남아있을경우 
        if (kill_trap_timer > 10.0f)
        {
            if (this.rock1.activeSelf == false)
            {
                this.fin = true;
                this.hold = false;
            }
            kill_trap_timer = 0.0f;
        }
        else
        {
            kill_trap_timer += Time.deltaTime;
        }

        if (start == false)
        {
            StartCoroutine("trap_down");
            this.cage.GetComponent<BoxCollider>().enabled = true;
            start = true;
            
        }

        if (trap_on == true)//새장 하강
        {
            if (this.cage.transform.position.y >= -0.7f)
            {
                this.cage.transform.Translate(Vector3.down * 15f * Time.deltaTime);
            }
            else
            {
                this.cage.transform.position = new Vector3(this.cage.transform.position.x, -0.7f, this.cage.transform.position.z);
                this.cage.tag = "Untagged";//장애물판정 해제
                trap_on = false;
            }
           
        }
        
        if (fin == true)//삭제
        {
            fin = false;//플래그초기화
            if (hold == false)
            {
                remove = true;//삭제애니메이션재생
                StartCoroutine("remove_Coroutine");//삭제코루틴호출
            }
        }


        if (remove == true)
        {
            this.cage.GetComponent<BoxCollider>().enabled = false;
            this.cage.transform.Translate(Vector3.down * 5f * Time.deltaTime);//새장하강

        }


        //열쇠로 트랩해제

        if (rock_on == true && trap_up_active == true)//트랩해제 직후
        {
            this.key.SetActive(true);
            StartCoroutine("trap_up");
            trap_up_active = false;

            //컬라이더 생성
            BoxCollider[] c = this.transform.GetChild(2).transform.GetChild(3).GetComponents<BoxCollider>();
            c[0].enabled = true;
            c[1].enabled = true;
            c[2].enabled = true;
            c[3].enabled = true;
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
                this.cage.transform.position = new Vector3(this.cage.transform.position.x, 12.3f, this.cage.transform.position.z);//원위치
                this.cage.GetComponent<Collider>().enabled = true;//컬라이더 on
                start = false;//플래그초기화
                remove = false;//플래그초기화
                trap_up_active = true;//플래그초기화
                this.cage.tag = "Enemy_Trap";//장애물판정 초기화
                this.enabled = false;//스크립트off

                //플래그초기화
                trap_on = false;
                fin = false;
                rock_on = false;

                this.gameObject.SetActive(false);

            }
        }

        //애니메이션 관리
        eye_animator.SetBool("REMOVE", remove);
    }//end of Update


    IEnumerator trap_down()//새장밑으로
    {
        yield return new WaitForSeconds(1.5f);
        trap_on = true;
        StartCoroutine("fin_Coroutine");

    }
    IEnumerator fin_Coroutine()//오브젝트유지시간
    {
        yield return new WaitForSeconds(3.0f);
        fin = true;
    }

    IEnumerator remove_Coroutine()//오브젝트삭제
    {
        yield return new WaitForSeconds(1.0f);
        start = false;//플래그초기화
        this.cage.transform.position = new Vector3(this.cage.transform.position.x, 12.3f, this.cage.transform.position.z);
        remove = false;
        this.cage.tag = "Enemy_Trap";//장애물판정 초기화
        this.enabled = false;//스크립트off

        //플래그초기화
        trap_on = false;
        fin = false;
        rock_on = false;
        kill_trap_timer = 0.0f;

        this.gameObject.SetActive(false);
    }

    IEnumerator trap_up()//새장위로
    {
        yield return new WaitForSeconds(1.5f);
        //트랩제거후 동작 헨젤원상복귀,컬라이더해제
        Hansel_Script.hold_hands_X = false;
        Hansel_Script.solo = false;
        Hansel_Script.GetComponent<BoxCollider>().enabled = true;
        Hansel_Script.chaos = false;
        //트랩제거후 트랩원상복귀
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.rock1.SetActive(false);
        this.rock2.SetActive(false);
        this.key.SetActive(false);
        rock_on = false;

        Debug.Log("트랩원상복귀");


        trap_remove = true;
    }
}
