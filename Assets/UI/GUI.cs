using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI : MonoBehaviour
{
    public GameObject Gamestart;
    Game_Start Gamestart_Script;
    public GameObject start_button;

    // Start is called before the first frame update
    void Start()
    {
        Gamestart_Script = Gamestart.GetComponent<Game_Start>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnGUI()
    {
        
    }

    public void start_button_Onclick()
    {
        Gamestart_Script.start_button_Onclick = true;
        start_button.SetActive(false);
    }
}
