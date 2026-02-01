using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] int TotalNPC = 0;
    [SerializeField] int EnragedNPC = 0;
    [SerializeField] float EnragedPercentage = 0;
    [SerializeField] bool TotalNPCCounted = false;
    [Header("Audio Setup")]
    [SerializeField] float MildEnragedPercentage = 25;
    [SerializeField] float StrongEnragedPercentage = 50;
    [SerializeField] string NeutralOSTName;
    [SerializeField] string MildEnragedlOSTName;
    [SerializeField] string StrongEnragedOSTName;
    [SerializeField] float FadeDuration = 0.5f;
    private float lasttimeSinceChange = 0;
    [SerializeField] private float MinTimeSinceChange = 5;

    [Header("Continents")]
    [SerializeField] List<Continent> Continents;

    // Update is called once per frame
    void Update()
    {
        CountEnraged();
        PlayMusic();
    }

    private void PlayMusic()
    {
        Debug.Log("PlayMusic Called");
        if (Time.time > lasttimeSinceChange + MinTimeSinceChange)
        {
            Debug.Log("Music time change");
            if (EnragedPercentage >= MildEnragedPercentage)
            {
                if (EnragedPercentage >= MildEnragedPercentage)
                {
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(StrongEnragedOSTName, FadeDuration);

                }
                else
                {
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(MildEnragedlOSTName, FadeDuration);
                }
            }
            TimeSinceLastChange();
        }
        else { 
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(NeutralOSTName, FadeDuration); 
            TimeSinceLastChange(); }
    }

    private void CountEnraged()
    {
        if (TotalNPCCounted == false)
        {
            foreach (Continent continent in Continents)
            {
                TotalNPC += continent._entities.Count;
            }
            TotalNPCCounted = true;
        }


        foreach (Continent continent in Continents)
        {
            Debug.Log("Numero di NPC arrabbiati: " + continent._enragedCount);
            EnragedNPC += continent._enragedCount;
        }
        EnragedPercentage = EnragedNPC / TotalNPC * 100;
    }

    private void TimeSinceLastChange()
    {
        lasttimeSinceChange = Time.time;
    }
}
