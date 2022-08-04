using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hansel_speech_ballroon : MonoBehaviour
{
    public GameObject speech_ballroon;//말풍선 UI
    public GameObject pivot;//회전축
    public GameObject pivot_H;//회전축
    public GameObject main_camera;//메인카메라
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.pivot.transform.position = this.pivot_H.transform.position;
        this.pivot.transform.localEulerAngles = new Vector3(0f, this.main_camera.transform.localEulerAngles.y , 0f);
    }
}
