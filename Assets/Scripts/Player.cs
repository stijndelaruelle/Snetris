using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int m_PlayerID;

    [SerializeField]
    private Snake m_Snake;

    private void Update()
    {
        if (m_Snake == null)
            return;

        if (Input.GetButtonDown("North_Player" + m_PlayerID))
            m_Snake.SetDirection(Direction.North);

        if (Input.GetButtonDown("South_Player" + m_PlayerID))
            m_Snake.SetDirection(Direction.South);

        if (Input.GetButtonDown("East_Player" + m_PlayerID))
            m_Snake.SetDirection(Direction.East);

        if (Input.GetButtonDown("West_Player" + m_PlayerID))
            m_Snake.SetDirection(Direction.West);
    }

    public void SetSnake(Snake snake)
    {
        m_Snake = snake;
    }
}
