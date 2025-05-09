using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameEffect;
using GameLogic.UserAction;


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

    [SerializeField]
    Transform mainCameraTransform;

    [SerializeField]
    Transform player1CameraTransform;

    [SerializeField]
    Transform player2CameraTransform;

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
            playerManagers[i].cardHoverEnterEvent.AddListener(OnCardHoverEnter);
            playerManagers[i].cardHoverExitEvent.AddListener(OnCardHoverExit);
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

        var startingTile1 = boardManager.board.GetTileAt(2, 2);
        var direction1 = Direction.East;
        var hero1 = new Hero(Game.currentGame.players[0], scriptableHero1, startingTile1, direction1);
        hero1.effects.Add(new MoveToChangeTileTypeEffect(hero1, TileType.Nature));

        playerManagers[0].player.TryToSpawnEntity(hero1);

        animationManager.SpawnEntity(hero1);

        var startingTile2 = boardManager.board.GetTileAt(2, 3);
        var direction2 = Direction.South;
        var hero2 = new Hero(Game.currentGame.players[1], scriptableHero2, startingTile2, direction2);
        //hero2.effects.Add(new MoveToChangeTileTypeEffect(hero2, TileType.CurseSource));

        playerManagers[1].player.TryToSpawnEntity(hero2);

        animationManager.SpawnEntity(hero2);

        entityInfoUI.weaponUsedUnityEvent.AddListener(OnWeaponUsed);
        entityInfoUI.effectHoverEnterEvent.AddListener(OnEffectHoverEnter);
        entityInfoUI.effectHoverExitEvent.AddListener(OnEffectHoverExit);
        entityInfoUI.weaponHoverEnterEvent.AddListener(OnWeaponHoverEnter);
        entityInfoUI.weaponHoverExitEvent.AddListener(OnWeaponHoverExit);

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

        GameLogic.GameAction.Action action = Game.currentGame.DequeueDepiledActionQueue();

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
        
        entityWasClickedThisFrame = true;
        EntityManager.UnselectEveryEntity();
        TileManager.UnselectEveryTile();
        entityManager.selected = entityManager.hovered;
        
    }

    private void OnWeaponUsed(){
        if(currentEntitySelected != null){
            currentEntitySelected.TryToAttack();
        }
    }

    void OnTileClicked(TileManager tileManager){
        if(blockInputs){
            return;
        }

        tileWasClickedThisFrame = true;
        var didMove = false;

        if(currentEntitySelected != null){
            if(currentEntitySelected.entity.player == Game.currentGame.currentPlayer){
                didMove = currentEntitySelected.TryToMove(tileManager.tile);
                if(!didMove){
                    var direction = DirectionsExtensions.FromCoordinateDifference(
                        tileManager.tile.gridX - currentEntitySelected.entity.currentTile.gridX,
                        tileManager.tile.gridY - currentEntitySelected.entity.currentTile.gridY);
                    currentEntitySelected.TryToChangeDirection(direction);
                }
            }
        }

        else{
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            tileManager.selected = tileManager.hovered;
        }
    }


    public void OnEndTurnPressed(){
        Game.currentGame.ReceiveUserAction(new EndTurnUserAction(Game.currentGame.currentPlayer.playerNum));
        
        if(Game.currentGame.currentPlayer.playerNum == 1){
            mainCameraTransform.position = player1CameraTransform.position;
            mainCameraTransform.rotation = player1CameraTransform.rotation;
        }
        else if(Game.currentGame.currentPlayer.playerNum == 2){
            mainCameraTransform.position = player2CameraTransform.position;
            mainCameraTransform.rotation = player2CameraTransform.rotation;
        }
        else{

        }

        EntityManager.UnselectEveryEntity();
        boardManager.GetEntityManagerFromEntity(Game.currentGame.currentPlayer.hero).selected = true;
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

    private void OnEffectHoverEnter(Effect effect)
    {
        effect.GetTilesAndEntitiesAffected(out Entity[] entities, out Tile[] tiles);
        boardManager.DisplayTilesUIInfo(tiles);
    }

    private void OnEffectHoverExit(Effect effect)
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    private void OnCardHoverEnter(Card card){
        card.activableEffect.GetTilesAndEntitiesAffected(out Entity[] entities, out Tile[] tiles);
        boardManager.DisplayTilesUIInfo(tiles);
    }

    private void OnCardHoverExit(Card card){
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    private void OnWeaponHoverEnter()
    {

        if(entityInfoUI == null){
            return;
        }
        if(entityInfoUI.entityManager == null){
            return;
        }

        if(entityInfoUI.entityManager.entity == Entity.noEntity){
            return;
        }

        entityInfoUI.entityManager.entity.GetTilesAndEntitiesAffectedByAtk(out Entity[] entities, out Tile[] tiles);
        boardManager.DisplayTilesUIInfo(tiles);
    }

    private void OnWeaponHoverExit()
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }
}
