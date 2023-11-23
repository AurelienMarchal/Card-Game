using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Game game_;

    public Game game{
        get{
            return game_;
        }

        set{
            game_ = value;
            UpdateAccordingToGame();
        }
    }
    

    [SerializeField]
    BoardManager boardManager;

    [SerializeField]
    GameObject heroPrefab;

    PlayerManager[] playerManagers;

    [SerializeField]
    int boardHeight;

    [SerializeField]
    int boardWidth;

    EntityManager currentEntitySelected;

    TileManager currentTileSelected;

    
    // Start is called before the first frame update
    void Start()
    {
        game = new Game(2, boardHeight, boardWidth);

        currentTileSelected = null;
        currentEntitySelected = null;

        boardManager.entitySelectedEvent.AddListener(OnEntitySelected);
        boardManager.tileSelectedEvent.AddListener(OnTileSelected);

        var health = new Health(new Heart[]{new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red)});
        var startingTile = boardManager.board.GetTileAt(4, 4);
        var direction = Direction.East;

        boardManager.SpawnEntity(heroPrefab, new Hero("Hero 1", startingTile, health, game.players[0], 1, direction));

        var health2 = new Health(new Heart[]{new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red)});
        var startingTile2 = boardManager.board.GetTileAt(4, 5);
        var direction2 = Direction.West;

        boardManager.SpawnEntity(heroPrefab, new Hero("Hero 2", startingTile2, health2, game.players[1], 1, direction2));

        game.StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEntitySelected(EntityManager entityManager){
        if(currentEntitySelected != null){
            if(currentEntitySelected.entity.player == game.currentPlayer){
                currentEntitySelected.entity.TryToAttack(entityManager.entity);
            }
        }

        currentEntitySelected = entityManager;
    }

    void OnTileSelected(TileManager tileManager){
        currentTileSelected = tileManager;
        if(currentEntitySelected == null){
            return;
        }

        if(currentEntitySelected.entity.player != game.currentPlayer){
            return;
        }

        currentEntitySelected.TryToMove(currentTileSelected);
    }

    void UpdateAccordingToGame(){

        boardManager.board = game.board;

    }

    public void OnEndTurnPressed(){
        game.EndPlayerTurn();
    }
}
