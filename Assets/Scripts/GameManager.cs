using Assets.Scripts.BoardGeneration;
using Assets.Scripts.BoardGeneration.Interfaces;
using Assets.Scripts.InputHandler;
using Assets.Scripts.InputHandler.Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int BoardWidth;
    [SerializeField] int BoardHeight;
    [SerializeField] int TotalMines;

    private IBoard _board;
    private IInputHandler _inputHandler;
    private bool gameover;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _board = GetComponentInChildren<Board>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Start() => NewGame();

    private void NewGame()
    {
        gameover = false;

        Camera.main.transform.position = new Vector3(BoardWidth / 2f, BoardHeight / 2f, -10f);
        
        _board.PrepareBoard(BoardWidth, BoardHeight, TotalMines);
    }

    private void Update()
    {
        if (_inputHandler.GetRestartInput())
            NewGame();
        else if (!gameover)
            if (_inputHandler.GetRightMouseButtomDown())
                _board.Flag();
            else if (_inputHandler.GetLeftMouseButtomDown())
                _board.Reveal();
    }
}