using UnityEngine;

public class SoundTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Play sound: Pickup");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlaySound("SFX_Pickup");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Play sound: Drop");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlaySound("SFX_Drop");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Play music: MildAngry");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade("OST_Neutral", 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Play music: MildAngry");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade("OST_MildAngry", 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Play music: MildAngry");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade("OST_StrongAngry", 0.5f);
        }

    }
}
