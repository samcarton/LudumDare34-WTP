using UnityEngine;
using System.Collections;

public class ArenaController : MonoBehaviour {

    GameController _gameController;
    
    public float DividingLineX = 0.0f;
    public float DividingLineTolerance = 0.001f;

    // Use this for initialization
    void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void OnTriggerExit2D(Collider2D other)
    {        
        if (IsPuck(other))
        {
            var winType = DetermineWinner(other.transform);
            // destroy puck
            Destroy(other.gameObject);
            // reset pieces
            _gameController.EndRound(winType);
            return;
        }

        if(IsPlayer(other))
        {
            // remove "life?"
            // destroy the player
            Destroy(other.gameObject);
            // set for respawn
            RespawnPlayer(other.gameObject);
        }
    }

    bool IsPuck(Collider2D collider)
    {
        var puckController = collider.GetComponent<PuckController>();
        return puckController != null;
    }

    bool IsPlayer(Collider2D collider)
    {
        var playerController = collider.GetComponent<PlayerController>();
        return playerController != null;
    }
    
    RoundWinType DetermineWinner(Transform puckTransform)
    {
        var xCoordinate = puckTransform.position.x;

        if(xCoordinate >= LowerDrawBoundary && xCoordinate <= UpperDrawBoundary)
        {
            return RoundWinType.Draw;
        }
        else if(xCoordinate < LowerDrawBoundary)
        {
            _gameController.GiveScoreToPlayer(2);
            return RoundWinType.Player2;
        }
        else if(xCoordinate > UpperDrawBoundary)
        {
            _gameController.GiveScoreToPlayer(1);
            return RoundWinType.Player1;
        }

        return RoundWinType.Draw;
    }

    void RespawnPlayer(GameObject playerObject)
    {
        var playerController = playerObject.GetComponent<PlayerController>();
        _gameController.RespawnPlayer(playerController.PlayerNumber);
    }

    public float LowerDrawBoundary { get { return DividingLineX - DividingLineTolerance; } }
    public float UpperDrawBoundary { get { return DividingLineX + DividingLineTolerance; } }

        
    // Update is called once per frame
    void Update () {
    
    }
}
