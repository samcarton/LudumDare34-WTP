using UnityEngine;
using System.Collections;

public class FoodController : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        var otherPlayerController = other.transform.GetComponent<PlayerController>();
        if (otherPlayerController)
        {            
            otherPlayerController.EatFood();            
            Destroy(gameObject);
        }
    }
}
