using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] float TotalNPC = 0;
    [SerializeField] float EnragedNPC = 0;
    [SerializeField] float EnragedPercentage = 0;
    bool TotalNPCCounted = false;
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
        if (Time.time > lasttimeSinceChange + MinTimeSinceChange)
        {
            if (EnragedPercentage >= MildEnragedPercentage)
            {
                if (EnragedPercentage >= StrongEnragedPercentage)
                {
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(StrongEnragedOSTName, FadeDuration);
                    TimeSinceLastChange();

                }
                else
                {
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(MildEnragedlOSTName, FadeDuration);
                    TimeSinceLastChange();
                }
            }
            
            else
            {
                GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().PlayMusicWithFade(NeutralOSTName, FadeDuration);
                TimeSinceLastChange();
            }
        }

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

        EnragedNPC = 0;
        foreach (Continent continent in Continents)
        {
            EnragedNPC += continent._enragedCount;
            EnragedPercentage = (EnragedNPC / TotalNPC) * 100;
        }
        
        
        
    }

    private void TimeSinceLastChange()
    {
        lasttimeSinceChange = Time.time;
    }
}
