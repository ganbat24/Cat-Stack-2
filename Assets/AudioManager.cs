using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> meows;

    public static AudioManager instance;
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    public static void Meow(){
        AudioSource tmp = Instantiate(instance.source, instance.transform);
        tmp.clip = randomMeow();
        tmp.Play();
        Destroy(tmp.gameObject, 1f);
    }

    static AudioClip randomMeow(){
        return instance.meows[Random.Range(0, instance.meows.Count)];
    }
}
