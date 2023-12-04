using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{

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

    [SerializeField]
    HealthUIDisplay healthUIDisplay;
    
    [SerializeField]
    MovementUIDisplay movementUIDisplay;

    [SerializeField]
    TextMeshProUGUI playerTextMesh;

    bool entityWasClickedThisFrame;

    bool tileWasClickedThisFrame;

    
    // Start is called before the first frame update
    void Start()
    {
        
        Game.currentGame.SetUpGame(2, boardHeight, boardWidth);
        boardManager.board = Game.currentGame.board;

        currentTileSelected = null;
        currentEntitySelected = null;

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;

        boardManager.entitySelectedEvent.AddListener(OnEntitySelected);
        boardManager.tileSelectedEvent.AddListener(OnTileSelected);
        boardManager.entityClickedEvent.AddListener(OnEntityClicked);
        boardManager.tileClickedEvent.AddListener(OnTileClicked);

        var health = new Health(new Heart[]{new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red)});
        var startingTile = boardManager.board.GetTileAt(4, 4);
        var direction = Direction.East;
        var hero1 = new Hero("Hero 1", startingTile, health, Game.currentGame.players[0], 1, direction);
        hero1.effects.Add(new TestEffect(hero1, TileType.Nature));

        boardManager.SpawnEntity(heroPrefab, hero1);

        var health2 = new Health(new Heart[]{new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red)});
        var startingTile2 = boardManager.board.GetTileAt(4, 5);
        var direction2 = Direction.West;
        var hero2 = new Hero("Hero 2", startingTile2, health2, Game.currentGame.players[1], 1, direction2);
        hero2.effects.Add(new TestEffect(hero2, TileType.Cursed));

        boardManager.SpawnEntity(heroPrefab, hero2);

        Game.currentGame.StartGame();
    }

    // Update is called once per frame
    void Update()
    {

        if(Mouse.current.leftButton.wasPressedThisFrame){
            if(!entityWasClickedThisFrame && !tileWasClickedThisFrame){
                EntityManager.UnselectEveryEntity();
                TileManager.UnselectEveryTile();
                currentEntitySelected = null;
                currentTileSelected = null;
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame){
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            currentTileSelected = null;
        }

        if(currentEntitySelected != null){
            healthUIDisplay.health = currentEntitySelected.entity.health;
        }

        else if(healthUIDisplay.health!=null){
            healthUIDisplay.health = null;
        }

        if(Game.currentGame.currentPlayer != null){
            movementUIDisplay.player = Game.currentGame.currentPlayer;
            playerTextMesh.text = Game.currentGame.currentPlayer.ToString();
        }

        else{
            playerTextMesh.text = "";
            if(movementUIDisplay.player!= null){
                movementUIDisplay.player = null;
            }
        }


        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;
    }

    void OnEntitySelected(EntityManager entityManager){

        currentEntitySelected = entityManager;
    }

    void OnTileSelected(TileManager tileManager){
        currentTileSelected = tileManager;
        if(currentEntitySelected == null){
            return;
        }
    }

    void OnEntityClicked(EntityManager entityManager){
        entityWasClickedThisFrame = true;

        if(currentEntitySelected == null){
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            entityManager.selected = entityManager.hovered;
        }
        else{
            if(currentEntitySelected.entity.player == Game.currentGame.currentPlayer){
                //currentEntitySelected.entity.TryToAttack(entityManager.entity);
            }
        }
    }

    void OnTileClicked(TileManager tileManager){
        tileWasClickedThisFrame = true;

        if(currentEntitySelected == null){
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            tileManager.selected = tileManager.hovered;
        }
        else{
            if(currentEntitySelected.entity.player == Game.currentGame.currentPlayer){
                var didMove = currentEntitySelected.TryToMove(tileManager);
                if(didMove){
                    Game.currentGame.OnEntityMoving(currentEntitySelected.entity);
                }
            }
        }
    }


    public void OnEndTurnPressed(){
        Game.currentGame.EndPlayerTurn();
    }
}
