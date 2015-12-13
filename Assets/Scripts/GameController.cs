using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public int Player1Score = 0;
    public int Player2Score = 0;
    public Text Player1ScoreText;
    public Text Player2ScoreText;
    public Text InfoText;

    float _foodSpawnTimer;
    public float FoodSpawnTime = 3.0f;
    public GameObject Food;
    public float SpawnRadius= 8.3f;

    public Vector3 Player1Start;
    public Vector3 Player2Start;
    public Vector3 PuckStart = Vector3.zero;
    public float RespawnWaitTime;
    public GameObject Player1Object;
    public GameObject Player2Object;
    public GameObject Puck;

    GameObject _player1;
    GameObject _player2;
    GameObject _puck;
    List<GameObject> _foods = new List<GameObject>();

    // Use this for initialization
    void Start () {
        SetupGame();
    }

    public void SetupGame()
    {
        _puck = (GameObject) GameObject.Instantiate(Puck, PuckStart, Quaternion.identity);
        _player1 = (GameObject) GameObject.Instantiate(Player1Object, Player1Start, Quaternion.identity);
        _player2 = (GameObject) GameObject.Instantiate(Player2Object, Player2Start, Quaternion.identity);
    }

    public void RespawnPlayer(int playerNumber)
    {        
        StartCoroutine(SpawnPlayer(playerNumber, RespawnWaitTime));
    }
    
    IEnumerator SpawnPlayer(int playerNumber, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(playerNumber == 1)
        {
            _player1 = (GameObject) GameObject.Instantiate(Player1Object, Random.insideUnitCircle * SpawnRadius, Quaternion.identity);
        }
        if(playerNumber == 2)
        {
            _player2 = (GameObject)GameObject.Instantiate(Player2Object, Random.insideUnitCircle * SpawnRadius, Quaternion.identity);
        }        
    }

    public void EndRound(RoundWinType winType)
    {
        StopAllCoroutines();
        if(_puck != null)
        {
            Destroy(_puck);
        }
        if(_player1 != null)
        {
            Destroy(_player1);
        }
        if(_player2 != null)
        {
            Destroy(_player2);
        }

        ClearFoods();

        SetupGame();
    }

    // Update is called once per frame
    void Update () {
        _foodSpawnTimer += Time.deltaTime;
        if(_foodSpawnTimer >= FoodSpawnTime)
        {
            SpawnFood();
            _foodSpawnTimer = 0;
        }
    }

    void SpawnFood()
    {
        _foods.Add((GameObject) GameObject.Instantiate(Food, Random.insideUnitCircle * SpawnRadius, Quaternion.identity));
    }

    void ClearFoods()
    {
        foreach(var food in _foods)
        {
            if(food != null)
            {
                Destroy(food);
            }
        }

        _foods.Clear();
    }

    public void GiveScoreToPlayer(int playerNumber)
    {
        if(playerNumber == 1)
        {
            Player1Score++;
        }
        if(playerNumber == 2)
        {
            Player2Score++;
        }

        UpdateHud();
    }
    
    void UpdateHud()
    {
        Player1ScoreText.text = Player1Score.ToString();
        Player2ScoreText.text = Player2Score.ToString();
    }
}
