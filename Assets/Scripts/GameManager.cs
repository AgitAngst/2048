using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using TMPro;
using YG;
using Lean.Touch;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private float _travelTime = 0.25f;
    [SerializeField] private int _winCondition = 2048;
    [SerializeField] private GameObject _winScreen, _loseScreen;
    [SerializeField] private TextMeshProUGUI _textCurrentScore;
    [SerializeField] public TextMeshProUGUI _textHighScore;
    [SerializeField] public TextMeshProUGUI[] _textHighScoresDupli;
    [SerializeField] public SaveSytem saveSytem;

    private int _currentScore = 0;
    [HideInInspector] public int _highScore = 0;

    private List<Node> _nodes;
    private List<Block> _blocks;
    private GameState _state;
    private int _round;

    private Vector2 test;
    private BlockType GetBlockTypeByValue(int value) => _types.First(t => t.Value == value);
    void Start()
    {
        ChangeState(GameState.GenerateLevel);
        LoadScore();
    }
    private void OnEnable()
    {
       LeanTouch.OnFingerSwipe += SwipeHandle;
    }

    private void OnDisable()
    {
       LeanTouch.OnFingerSwipe -= SwipeHandle;
    }
    private void Update()
    {
        if (_state != GameState.WaitingInputs) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) Shift(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow)) Shift(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow)) Shift(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow)) Shift(Vector2.down);

        if (Input.GetKeyDown(KeyCode.A)) Shift(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D)) Shift(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W)) Shift(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S)) Shift(Vector2.down);

    }

    private void SwipeHandle(LeanFinger finger)
    {
        if (finger == null) return;

    }

    public void Reset()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void SaveScore()
    {
        saveSytem.SaveSettings();
    }
    public void LoadScore()
    {
       saveSytem.LoadSettings();
    }

    public void ResetScore()
    {
        YandexGame.ResetSaveProgress();
    }

    private void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlocks:
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInputs:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                _winScreen.SetActive(true);
                SaveScore();
                break;
            case GameState.Lose:
                _loseScreen.SetActive(true);
                SaveScore();
                foreach (var item in _textHighScoresDupli)
                {
                    item.text = _highScore.ToString();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    void GenerateGrid()
    {
        _round = 0;
        _nodes = new List<Node>();
        _blocks = new List<Block>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _nodes.Add(node);
            }
        }

        var center = new Vector2((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, center, quaternion.identity);
        board.size = new Vector2(_width, _height);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10f);
        ChangeState(GameState.SpawningBlocks);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => Random.value).ToList();
        foreach (var node in freeNodes.Take(amount))
        {
            SpawnBlock(node, Random.value > 0.8f ? 4:2);
        }

        if (freeNodes.Count() == 0)
        {
            //Lost
            ChangeState(GameState.Lose);
            return;
        }
        ChangeState(_blocks.Any(b=>b.Value == _winCondition) ? GameState.Win : GameState.WaitingInputs);
    }

    void SpawnBlock(Node node,int value)
    {
        var block = Instantiate(_blockPrefab, node.Pos,Quaternion.identity);
        block.Init(GetBlockTypeByValue(value));
        block.SetBlock(node);
        _blocks.Add(block);

    }
    void Shift(Vector2 dir)
    {
        ChangeState(GameState.Moving);
        
        var orderedBlocks = _blocks.OrderBy(b => b.Pos.x)
            .ThenBy(b => b.Pos.y).ToList();
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();
        AudioManager.Instance.PlayEffects(AudioManager.Instance.blockMovement, true);

        foreach (var block in orderedBlocks)
        {
            var next = block.Node;
            do
            {
                block.SetBlock(next);
                var possibleNode = GetNodeAtPosition(next.Pos + dir);

                if (possibleNode != null)
                {
                    //Node is present
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Value))
                    {
                        block.MergeBlock(possibleNode.OccupiedBlock);
                        AudioManager.Instance.PlayEffects(AudioManager.Instance.blockMerge, true);
                    }
                    else if (possibleNode.OccupiedBlock == null) next = possibleNode;
                    //None hit? End do while loop
                }
            } while (next != block.Node);

            
        }

        var sequence = DOTween.Sequence();

        foreach (var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.Pos : block.Node.Pos;
            sequence.Insert(0, block.transform.DOMove(movePoint, _travelTime));
        }

        sequence.OnComplete(() =>
        {
            foreach (var block in orderedBlocks.Where(b => b.MergingBlock != null))
            {
                MergeBlocks(block.MergingBlock,block);
            }
            
            ChangeState(GameState.SpawningBlocks);
        });

    }

    public void ShiftUP()
    {
        Shift(Vector2.up);
    }
    public void ShiftDOWN()
    {
        Shift(Vector2.down);
    }
    public void ShiftLEFT()
    {
        Shift(Vector2.left);
    }
    public void ShiftRIGHT()
    {
        Shift(Vector2.right);
    }
    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        SpawnBlock(baseBlock.Node, baseBlock.Value *2);
        _currentScore += (baseBlock.Value * 2);
        _textCurrentScore.text = _currentScore.ToString();
        if (_currentScore >= _highScore)
        {
            _highScore = _currentScore;
            _textHighScore.text = _highScore.ToString();
        }
        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
    }

    void RemoveBlock(Block block)
    {
        _blocks.Remove(block);
        Destroy(block.gameObject);
    }
    Node GetNodeAtPosition(Vector2 pos)
    {
        return _nodes.FirstOrDefault(n => n.Pos == pos);
    }
}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}

public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInputs,
    Moving,
    Win,
    Lose
}

