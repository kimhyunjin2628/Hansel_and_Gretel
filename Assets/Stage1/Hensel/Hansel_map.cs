using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hansel_map : MonoBehaviour
{
    public Texture[] wink_map = new Texture[25];

    public GameObject Hansel_FACE;
    main_player_1 Hansel_Script;//그레텔 스크립트           

    //눈깜빡임
    bool wink = true;
    int wink_index_current = 0;//현재인덱스
    int wink_index = 0;
    float wink_error_time = 0.0f;//일정시간 인덱스가 그대로면 코루틴다시실행

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (this.wink == true)//눈깜빡임시작
        {
            StartCoroutine("wink_cool");
            this.wink = false;//한번만 실행
        }
        //눈깜빡임 오류방지
        this.wink_index_current = this.wink_index;
        if (this.wink_index == this.wink_index_current)//인덱스변화X
        {
            this.wink_error_time += Time.deltaTime;
        }
        if (this.wink_error_time >= 3.0f)//실행중인 눈깜빡임관련 코루틴 X -> 재실행
        {
            this.wink = true;
            this.wink_error_time = 0.0f;
        }
    }

    IEnumerator wink_cool()//눈 깜빡임 주기
    {
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("wink_start");
        this.wink_error_time = 0.0f;//정상코루틴실행 판단
    }
    IEnumerator wink_start()//눈 깜빡임
    {
        yield return new WaitForSeconds(0.02f);
        this.wink_error_time = 0.0f;//정상코루틴실행 판단
        this.Hansel_FACE.GetComponent<SkinnedMeshRenderer>().material.mainTexture = this.wink_map[this.wink_index];
        if (wink_index < 23)
        {
            wink_index++;
            StartCoroutine("wink_start");
        }
        else
        {
            wink_index = 0;
            StartCoroutine("wink_cool");
        }
    }
}
