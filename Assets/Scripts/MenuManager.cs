using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour {


    public void OnLoadClicked()
    {
        SceneManager.LoadScene("mainscene");
    }
}
