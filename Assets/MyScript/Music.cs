using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	Object[] myMusic; // declare this as Object array
	AudioSource GameMusic;
	void Awake () {
        GameMusic = GetComponent<AudioSource>();
		myMusic = Resources.LoadAll("Audio",typeof(AudioClip));
		playRandomMusic ();
	}
	
	void Start (){
        GameMusic.Play(); 
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameMusic.isPlaying)
			playRandomMusic();
	}
	
	void playRandomMusic() {
        GameMusic.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        GameMusic.Play();
	}
}
