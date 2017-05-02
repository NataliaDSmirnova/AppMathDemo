using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    float t = 0;
    int cur_audio;
    public AudioSource audioSourceCur;
    public AudioClip[] myClips;
    // Use this for initialization
    void Start()
    {
        audioSourceCur = gameObject.AddComponent<AudioSource>();
        myClips = new AudioClip[3];
        myClips[0] = Resources.Load("breeze") as AudioClip;
        myClips[1] = Resources.Load("gomm") as AudioClip;
        myClips[2] = Resources.Load("prodigal") as AudioClip;

        cur_audio = 0;
        audioSourceCur.clip = myClips[cur_audio];
        //au_breeze.loop = true;
        audioSourceCur.volume = 0;
        audioSourceCur.Play();

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t < 2.0f)
        {
            FadeIn(0.5f);
        }
        if (t > 8.0f)
        {
            FadeOut(0.5f);
        }
        if (t > 10.0f)
        {
            ++cur_audio;
            cur_audio %= 4;
            if (cur_audio == 3)
            {
                audioSourceCur.Stop();
            }
            else
            {
                audioSourceCur.clip = myClips[cur_audio];
                audioSourceCur.volume = 0;
                audioSourceCur.Play();
            }
            
            t = 0.0f;
        }
    }

    void FadeOut(float coef)
    {
        audioSourceCur.volume -= Time.deltaTime * coef;
    }

    void FadeIn(float coef)
    {
        audioSourceCur.volume += Time.deltaTime * coef;
    }
}