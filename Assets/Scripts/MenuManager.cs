using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour {

    AudioSource _audioSource;
    bool _clicked;

    public AudioClip MenuSound;

    void Start()
    {
        _clicked = false;
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnLoadClicked()
    {
        if(_clicked == false)
        {
            _clicked = true;
            _audioSource.PlayOneShot(MenuSound);
            
            StartCoroutine(LoadGame(0.2f));
        }        
    }

    IEnumerator LoadGame(float soundTime)
    {
        yield return new WaitForSeconds(soundTime);
        SceneManager.LoadScene("mainscene");        
    }
}
