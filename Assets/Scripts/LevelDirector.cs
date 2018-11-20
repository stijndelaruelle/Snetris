using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    //Should become a pool
    [SerializeField]
    private Snake m_SnakePrefab;
    private List<Snake> m_Snakes;

    [SerializeField]
    private LevelGrid m_Grid;

    private void Awake()
    {
        m_Snakes = new List<Snake>();
    }

    private void Start()
    {
        SpawnSnake(1, 2, 2);
    }

    private void Update()
    {
        //Grow cheats
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            foreach (Snake snake in m_Snakes)
                snake.Grow(Direction.North);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            foreach (Snake snake in m_Snakes)
                snake.Grow(Direction.East);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            foreach (Snake snake in m_Snakes)
                snake.Grow(Direction.South);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            foreach (Snake snake in m_Snakes)
                snake.Grow(Direction.West);
        }
    }

    public void SpawnSnake(int x, int y, int size)
    {
        //Create a new snake
        Snake snake = GameObject.Instantiate<Snake>(m_SnakePrefab);
        snake.Initialize(m_Grid, new Vector2i(x, y), Direction.North);

        //Make the snake grow!
        for (int i = 1; i < size; ++i)
        {
            snake.Grow(Direction.South);
        }

        m_Snakes.Add(snake);
        m_Player.SetSnake(snake);
    }
}
