using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    ScriptableHero scriptableHero1;

    [SerializeField]
    ScriptableHero scriptableHero2;

    [SerializeField]
    BoardManager boardManager;

    [SerializeField]
    AnimationManager animationManager;

    [SerializeField]
    PlayerManager[] playerManagers;

    bool blockInputs;

    [SerializeField]
    int boardHeight;

    [SerializeField]
    int boardWidth;

    EntityManager currentEntitySelected;

    EntityManager lastEntitySelected;

    TileManager currentTileSelected;
    
    [SerializeField]
    ManaUIDisplay manaUIDisplay;

    [SerializeField]
    EntityInfoUI entityInfoUI;

    [SerializeField]
    TextMeshProUGUI playerTextMesh;

    [SerializeField]
    CameraFollowingSelectedEntity cameraFollowingSelectedEntity;

    bool entityWasClickedThisFrame;

    bool tileWasClickedThisFrame;

    int UILayer;
    

    
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
        lastEntitySelected = null;

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;

        UILayer = LayerMask.NameToLayer("UI");

        boardManager.entitySelectedEvent.AddListener(OnEntitySelected);
        boardManager.tileSelectedEvent.AddListener(OnTileSelected);
        boardManager.entityClickedEvent.AddListener(OnEntityClicked);
        boardManager.tileClickedEvent.AddListener(OnTileClicked);

        var startingTile1 = boardManager.board.GetTileAt(4, 4);
        var direction1 = Direction.East;
        var hero1 = new Hero(Game.currentGame.players[0], scriptableHero1, startingTile1, direction1);
        hero1.effects.Add(new MoveToChangeTileTypeEffect(hero1, TileType.Nature));

        animationManager.SpawnEntity(hero1);

        var startingTile2 = boardManager.board.GetTileAt(4, 5);
        var direction2 = Direction.West;
        var hero2 = new Hero(Game.currentGame.players[1], scriptableHero2, startingTile2, direction2);
        //hero2.effects.Add(new MoveToChangeTileTypeEffect(hero2, TileType.CurseSource));

        animationManager.SpawnEntity(hero2);

        Game.currentGame.PileAction(new StartGameAction());
    }

    // Update is called once per frame
    void Update()
    {

        
        if(Game.currentGame.depiledActionQueue.Count > 0){
            blockInputs = true;
            DequeueDepiledActionQueue();
        }


        else{
            blockInputs = animationManager.animationPlaying;
        }

        if(blockInputs){
            /*
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            currentTileSelected = null;
            */
        }
        else{

            if(Mouse.current.leftButton.wasPressedThisFrame){
                if(!IsPointerOverUIElement()){
                    if(!entityWasClickedThisFrame && !tileWasClickedThisFrame){
                        EntityManager.UnselectEveryEntity();
                        TileManager.UnselectEveryTile();
                        currentEntitySelected = null;
                        currentTileSelected = null;
                    }
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame){
                if(!IsPointerOverUIElement()){
                    EntityManager.UnselectEveryEntity();
                    TileManager.UnselectEveryTile();
                    currentEntitySelected = null;
                    currentTileSelected = null;
                }
            }
        }

        if(Game.currentGame.currentPlayer != null){
            manaUIDisplay.player = Game.currentGame.currentPlayer;
            playerTextMesh.text = Game.currentGame.currentPlayer.ToString();
        }

        else{
            playerTextMesh.text = "";
            if(manaUIDisplay.player!= null){
                manaUIDisplay.player = null;
            }
        }
        
        if(currentEntitySelected != null){
            boardManager.ResetAllTileLayer();
            var tileManager = boardManager.GetTileManagerFromTile(currentEntitySelected.entity.currentTile);
            SetGameLayerRecursive(tileManager.gameObject, LayerMask.NameToLayer("UICamera"));
        }

        entityInfoUI.gameObject.SetActive(currentEntitySelected != null);

        if(lastEntitySelected != currentEntitySelected){
            entityInfoUI.entityManager = currentEntitySelected;
        }

        lastEntitySelected = currentEntitySelected;

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;
    }

    private void DequeueDepiledActionQueue(){

        if(animationManager.animationPlaying){
            return;
        }

        Action action = Game.currentGame.DequeueDepiledActionQueue();

        while(!action.wasCancelled && !action.wasPerformed && Game.currentGame.depiledActionQueue.Count > 0){
            action = Game.currentGame.DequeueDepiledActionQueue();
        }

        if(action.wasCancelled){
            Debug.Log($"Playing cancel animation for action {action}");
        }
    
        else if(action.wasPerformed){
            Debug.Log($"Playing perform animation for action {action}");
            animationManager.PlayAnimationForAction(action);
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

        if(currentEntitySelected != null){
            if(currentEntitySelected.entity.player == Game.currentGame.currentPlayer){
                currentEntitySelected.entity.TryToCreateEntityAttackAction(entityManager.entity, out _);
            }
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
        Game.currentGame.PileAction(new EndPlayerTurnAction(Game.currentGame.currentPlayer));
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static void SetGameLayerRecursive(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetGameLayerRecursive(child.gameObject, layer);
        }
    }
}
