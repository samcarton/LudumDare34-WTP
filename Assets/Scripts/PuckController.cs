using UnityEngine;
using System.Collections;

public class PuckController : MonoBehaviour {

    AudioSource _audioSource;
    public AudioClip HitSound;

	// Use this for initialization
	void Start () {
        _audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.IsWithPlayer())
        {
            _audioSource.PlayOneShot(HitSound);
        }
    }
}
