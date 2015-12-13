
using UnityEngine;
public static class Collision2DExtensions
{
    public static bool IsWithPlayer(this Collision2D collision)
    {
        if(collision == null)
        {
            throw new UnityException("collision null");
        }

        var playerController = collision.gameObject.GetComponent<PlayerController>();
        return playerController != null;
    }
}

