using UnityEngine;
using UnityEngine.SceneManagement;
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
    bool _gameEnabled;

    public AudioClip CountDownSound;
    public AudioClip GoSound;
    public AudioClip GoalSound;
    public AudioClip WinSound;
    public AudioClip RespawnSound;
    AudioSource _audioSource;


    // Use this for initialization
    void Start () {
        _audioSource = GetComponent<AudioSource>();
        SetupGame();
    }

    public void SetupGame()
    {
        _puck = (GameObject) GameObject.Instantiate(Puck, PuckStart, Quaternion.identity);
        _player1 = (GameObject) GameObject.Instantiate(Player1Object, Player1Start, Quaternion.identity);
        _player2 = (GameObject) GameObject.Instantiate(Player2Object, Player2Start, Quaternion.identity);

        SetPlayersEnabledState(false);
        
        _gameEnabled = false;
        
        StopCoroutine("SpawnPlayer");
        StartCoroutine(GameStartCountdown());
    }

    // countdown, then enable players and game
    IEnumerator GameStartCountdown()
    {
        _audioSource.clip = CountDownSound;
        _audioSource.Play();
        InfoText.text = "3";
        yield return new WaitForSeconds(1);

        _audioSource.Play();
        InfoText.text = "2";
        yield return new WaitForSeconds(1);

        _audioSource.Play();
        InfoText.text = "1";
        yield return new WaitForSeconds(1);

        _audioSource.clip = GoSound;
        _audioSource.Play();
        InfoText.text = "";
        SetPlayersEnabledState(true);
        _gameEnabled = true;        
    }

    void SetPlayersEnabledState(bool state)
    {
        var player1Controller = _player1.GetComponent<PlayerController>();
        player1Controller.PlayerEnabled = state;

        var player2Controller = _player2.GetComponent<PlayerController>();
        player2Controller.PlayerEnabled = state;
    }

    public void RespawnPlayer(int playerNumber)
    {
        if (_gameEnabled)
        { 
            StartCoroutine(SpawnPlayer(playerNumber, RespawnWaitTime));
        }
    }
    
    IEnumerator SpawnPlayer(int playerNumber, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _audioSource.PlayOneShot(RespawnSound);
        if(playerNumber == 1)
        {
            _player1 = (GameObject) GameObject.Instantiate(Player1Object, RestrictToNegativeX(Random.insideUnitCircle) * SpawnRadius, Quaternion.identity);
            var player1Controller = _player1.GetComponent<PlayerController>();
            player1Controller.PlayerEnabled = true;
        }
        if(playerNumber == 2)
        {
            _player2 = (GameObject)GameObject.Instantiate(Player2Object, RestrictToPositiveX(Random.insideUnitCircle) * SpawnRadius, Quaternion.identity);
            var player2Controller = _player2.GetComponent<PlayerController>();
            player2Controller.PlayerEnabled = true;
        }
    }

    Vector3 RestrictToNegativeX(Vector3 vector)
    {
        if(vector.x>0)
        {
            return new Vector3(-vector.x, vector.y, vector.z);
        }

        return vector;
    }

    Vector3 RestrictToPositiveX(Vector3 vector)
    {
        if (vector.x < 0)
        {
            return new Vector3(-vector.x, vector.y, vector.z);
        }

        return vector;
    }

    public void EndRound(RoundWinType winType)
    {
        _gameEnabled = false;
        if(Player1Score < 3 && Player2Score < 3)
        {
            StartCoroutine(ShowRoundEndText(winType));
        }
        else
        {
            StartCoroutine(ShowGameEndText());
        }
        
    }

    IEnumerator ShowRoundEndText(RoundWinType winType)
    {
        switch (winType)
        {
            case RoundWinType.Draw:
                InfoText.text = "DRAW";
                break;
            case RoundWinType.Player1:
                InfoText.text = "POINT RED";
                break;
            case RoundWinType.Player2:
                InfoText.text = "POINT BLUE";
                break;
            default:
                break;
        }

        _audioSource.PlayOneShot(GoalSound);

        yield return new WaitForSeconds(2);
        InfoText.text = "";

        if (_puck != null)
        {
            Destroy(_puck);
        }
        if (_player1 != null)
        {
            Destroy(_player1);
        }
        if (_player2 != null)
        {
            Destroy(_player2);
        }

        ClearFoods();

        SetupGame();
    }

    IEnumerator ShowGameEndText()
    {
        if(Player1Score == 3)
        {
            InfoText.text = "RED WINS!";
        }
        else
        {
            InfoText.text = "BLUE WINS!";
        }

        _audioSource.PlayOneShot(WinSound);

        yield return new WaitForSeconds(3);
        InfoText.text = "";
        
        SceneManager.LoadScene("Menu");
    }

    

    // Update is called once per frame
    void Update () {
        if(_gameEnabled)
        {
            _foodSpawnTimer += Time.deltaTime;
            if (_foodSpawnTimer >= FoodSpawnTime)
            {
                SpawnFood();
                _foodSpawnTimer = 0;
            }
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
