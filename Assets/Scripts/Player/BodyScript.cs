using UnityEngine;

public class BodyScript : MonoBehaviour
{
    public PlayerMovementScript player;
    public int followIndex = 10;

    void Update()
    {
        if (player == null) return;

        int index = Mathf.Min(followIndex, player.PositionsCount - 1);

        Vector2 targetPos = player.GetPositionAt(index);

        transform.position = targetPos;

        if (index > 0)
        {
            Vector2 nextPos = player.GetPositionAt(index - 1);
            Vector2 direction = nextPos - targetPos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}

