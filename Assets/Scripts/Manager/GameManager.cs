using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    BoardManager boardManager;

    [SerializeField]
    GameObject heroPrefab;

    [SerializeField]
    PlayerManager[] playerManagers;

    bool blockInputs;

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
        
        Game.currentGame.SetUpGame(playerManagers.Length, boardHeight, boardWidth);
        boardManager.board = Game.currentGame.board;

        for(var i = 0;  i < playerManagers.Length; i++){
            playerManagers[i].player = Game.currentGame.players[i];
        }

        blockInputs = false;

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
        hero1.effects.Add(new MoveToChangeTileTypeEffect(hero1, TileType.Nature));

        boardManager.SpawnEntity(heroPrefab, hero1);

        var health2 = new Health(new Heart[]{new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red), new Heart(HeartType.Red, HeartType.Red)});
        var startingTile2 = boardManager.board.GetTileAt(4, 5);
        var direction2 = Direction.West;
        var hero2 = new Hero("Hero 2", startingTile2, health2, Game.currentGame.players[1], 1, direction2);
        hero2.effects.Add(new MoveToChangeTileTypeEffect(hero2, TileType.Cursed));

        boardManager.SpawnEntity(heroPrefab, hero2);

        Game.currentGame.StartGame();
    }

    // Update is called once per frame
    void Update()
    {

        
        if(Game.currentGame.depiledActionQueue.Count > 0){
            blockInputs = true;
            DequeueDepiledActionQueue();
        }


        else{
            //if no animation
            blockInputs = false;
        }

        if(blockInputs){
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            currentTileSelected = null;
        }
        else{
            
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

    private void DequeueDepiledActionQueue(){
        Action action = Game.currentGame.DequeueDepiledActionQueue();
        while(!action.wasCancelled && !action.wasPerformed && Game.currentGame.depiledActionQueue.Count > 0){
            action = Game.currentGame.DequeueDepiledActionQueue();
        }

        if(action.wasCancelled){
            Debug.Log($"Playing cancel animation for action {action}");
        }
    
        else if(action.wasPerformed){
            Debug.Log($"Playing perform animation for action {action}");
        }
        
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
        if(blockInputs){
            return;
        }

        entityWasClickedThisFrame = true;
        EntityManager.UnselectEveryEntity();
        TileManager.UnselectEveryTile();
        entityManager.selected = entityManager.hovered;
        
    }

    void OnTileClicked(TileManager tileManager){
        if(blockInputs){
            return;
        }

        tileWasClickedThisFrame = true;

        if(currentEntitySelected == null){
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            tileManager.selected = tileManager.hovered;
        }
        else{
            if(currentEntitySelected.entity.player == Game.currentGame.currentPlayer){
                var didMove = currentEntitySelected.TryToMove(tileManager.tile);
                if(didMove){
                    
                }
            }
        }
    }


    public void OnEndTurnPressed(){
        Game.currentGame.EndPlayerTurn();
    }
}
