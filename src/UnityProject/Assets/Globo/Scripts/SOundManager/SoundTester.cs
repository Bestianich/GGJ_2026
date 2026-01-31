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
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlaySound("NPC_Grab");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Play sound: Drop");
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlaySound("NPC_Drop");
        }
    }
}
