using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes sure we can grow
public class Snake : MonoBehaviour
{
    [SerializeField]
    private float m_MoveSpeed;
    private float m_MoveUpdateTimer;

    [SerializeField]
    private SnakePart m_SnakePartPrefab;
    private SnakePart m_Head; //Double Linked list
    public SnakePart Head
    {
        get { return m_Head; }
    }

    private bool m_IsAlive = true;

    public void Initialize(LevelGrid grid, Vector2i gridPosition, Direction direction)
    {
        m_IsAlive = true;

        m_Head = GameObject.Instantiate<SnakePart>(m_SnakePartPrefab);
        m_Head.Initialize(grid, gridPosition, null, direction);

        m_Head.SnakeCollisionEvent += OnSnakeCollision;
        m_Head.TetrisCollisionEvent += OnTetrisCollision;
    }

    private void OnDestroy()
    {
        if (m_Head != null)
        {
            m_Head.SnakeCollisionEvent -= OnSnakeCollision;
            m_Head.TetrisCollisionEvent -= OnTetrisCollision;
        }
    }

    private void Update()
    {
        if (!m_IsAlive)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        //Move timer
        if (m_MoveUpdateTimer > m_MoveSpeed)
        {
            m_Head.Move();
            m_MoveUpdateTimer -= m_MoveSpeed;
        }

        m_MoveUpdateTimer += Time.deltaTime;
    }

    public void SetDirection(Direction direction)
    {
        if (m_Head != null)
            m_Head.Direction = direction;
    }

    public SnakePart Grow(Direction direction)
    {
        if (m_Head != null)
            return m_Head.Grow(direction);

        return null;
    }

    private void OnSnakeCollision(LevelGridObject otherObject)
    {
        //For now we don't check if it's a pickup or anything else we just turn into a tetris block!
        if (m_Head != null)
            m_Head.ChangeToTetrisState();

        Debug.Log("Snake hit something!");
    }

    private void OnTetrisCollision()
    {
        m_IsAlive = false;
        Debug.Log("Snake died as a tetris block!");
    }
}
