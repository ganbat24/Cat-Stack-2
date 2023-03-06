using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource mainTheme;
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> meows;
    [SerializeField] AudioClip rewardClip;
    [SerializeField] AudioClip sploshClip;
    static bool isMute = false;

    public static AudioManager instance;
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    public static void ToggleMute(){
        isMute = !isMute;
        instance.mainTheme.mute = isMute;
    }

    public static void Meow(){
        AudioSource tmp = Instantiate(instance.source, instance.transform);
        tmp.clip = randomMeow();
        tmp.Play();
        //will not change mid play
        tmp.mute = isMute;
        Destroy(tmp.gameObject, 1f);
    }

    public static void Reward(){
        AudioSource tmp = Instantiate(instance.source, instance.transform);
        tmp.clip = instance.rewardClip;
        tmp.Play();
        //will not change mid play
        tmp.mute = isMute;
        Destroy(tmp.gameObject, 1f);
    }

    public static void Splosh(){
        AudioSource tmp = Instantiate(instance.source, instance.transform);
        tmp.clip = instance.sploshClip;
        tmp.Play();
        //will not change mid play
        tmp.mute = isMute;
        Destroy(tmp.gameObject, 1f);
    }

    static AudioClip randomMeow(){
        return instance.meows[Random.Range(0, instance.meows.Count)];
    }
}
