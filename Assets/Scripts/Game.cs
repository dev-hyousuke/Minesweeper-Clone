using Assets.Scripts.BoardGeneration.Interfaces;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] int BoardWidth;
    [SerializeField] int BoardHeight;

    private IBoard _board;
    private bool gameover;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _board = GetComponentInChildren<Board>();
    }

    private void Start() => NewGame();

    private void NewGame()
    {
        gameover = false;

        _board.PrepareBoard(BoardWidth, BoardHeight);

        Camera.main.transform.position = new Vector3(BoardWidth / 2f, BoardHeight / 2f, -10f);
        //_board.Draw(state);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            NewGame();
        else if (!gameover)
            if (Input.GetMouseButtonDown(1))
                _board.Flag();
            else if (Input.GetMouseButtonDown(0))
                _board.Reveal();
    }
}