using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

using System.IO;
using Newtonsoft.Json;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameEffect;
using GameLogic.UserAction;
using GameLogic.GameState;
using System;





public class GameManager : MonoBehaviour
{

    private GameState gameState_;


    public GameState gameState
    {
        get
        {
            return gameState_;
        }

        set
        {
            gameState_ = value;
            UpdateAccordingToGameState();
        }

    }

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
    TextMeshProUGUI turnTextMesh;

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

    bool gameStateHasChanged;

    // Start is called before the first frame update
    void Start()
    {

        Game.currentGame.SetUpGame(playerManagers.Length, boardHeight, boardWidth);
        //Game.currentGame.board = Game.currentGame.board;



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

        //Testing
        var startingTile1 = Game.currentGame.board.GetTileAt(2, 2);
        var direction1 = Direction.East;
        var hero1 = new Hero(Game.currentGame.players[0], scriptableHero1, startingTile1, direction1);
        hero1.effects.Add(new MoveToChangeTileTypeEffect(hero1, TileType.Nature));
        hero1.num = 0;
        Game.currentGame.players[0].entities.Add(hero1);
        Game.currentGame.players[0].hero = hero1;

        //playerManagers[0].player.TryToSpawnEntity(hero1);

        //animationManager.SpawnEntity(hero1);

        var startingTile2 = Game.currentGame.board.GetTileAt(2, 3);
        var direction2 = Direction.South;
        var hero2 = new Hero(Game.currentGame.players[1], scriptableHero2, startingTile2, direction2);
        //hero2.effects.Add(new MoveToChangeTileTypeEffect(hero2, TileType.CurseSource));
        hero2.num = 0;
        Game.currentGame.players[1].entities.Add(hero2);
        Game.currentGame.players[1].hero = hero2;


        //playerManagers[1].player.TryToSpawnEntity(hero2);

        //animationManager.SpawnEntity(hero2);

        // A enlever
        for (var i = 0; i < playerManagers.Length; i++)
        {
            playerManagers[i].playerState = Game.currentGame.players[i].ToPlayerState();
            //playerManagers[i].cardHoverEnterEvent.AddListener(OnCardHoverEnter);
            //playerManagers[i].cardHoverExitEvent.AddListener(OnCardHoverExit);
        }

        entityInfoUI.weaponUsedUnityEvent.AddListener(OnWeaponUsed);
        entityInfoUI.effectHoverEnterEvent.AddListener(OnEffectHoverEnter);
        entityInfoUI.effectHoverExitEvent.AddListener(OnEffectHoverExit);
        //entityInfoUI.weaponHoverEnterEvent.AddListener(OnWeaponHoverEnter);
        //entityInfoUI.weaponHoverExitEvent.AddListener(OnWeaponHoverExit);

        Game.currentGame.StartGame();
        gameState = Game.currentGame.ToGameState();
        UpdateVisuals();


    }

    // Update is called once per frame
    void Update()
    {


        if (Game.currentGame.depiledActionQueue.Count > 0)
        {
            blockInputs = true;
            gameStateHasChanged = true;
            //test
            ActionState actionState = Game.currentGame.DequeueDepiledActionQueueAndGetActionState();

            if (actionState != null)
            {
                string serializedActionState = JsonConvert.SerializeObject(actionState);
                Debug.Log("Serialized ActionState :" + serializedActionState);
                var deserializedActionState = JsonConvert.DeserializeObject<ActionState>(serializedActionState);
                Debug.Log("Deserialized ActionState :" + deserializedActionState);

                

                if (deserializedActionState != null)
                {
                    Debug.Log("Playing animation for " + serializedActionState);
                    animationManager.PlayAnimationForActionState(deserializedActionState);
                }
                
            }
        }


        else
        {
            blockInputs = animationManager.animationPlaying;
        }

        if (blockInputs)
        {
            boardManager.ResetAllTileLayerDisplayUIInfo();
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            currentTileSelected = null;
            
        }
        else
        {

            //Testing
            if (gameStateHasChanged)
            {
                
                string jsonGameState = JsonConvert.SerializeObject(Game.currentGame.ToGameState(), Formatting.Indented);

                // File path (write to persistent data path)
                string filePath = Path.Combine(Application.persistentDataPath, "LastGameState.json");

                Debug.Log($"Writing GameState to {filePath}");
                // Write to file
                File.WriteAllText(filePath, jsonGameState);

                gameStateHasChanged = false;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (!IsPointerOverUIElement())
                {
                    if (!entityWasClickedThisFrame && !tileWasClickedThisFrame)
                    {
                        EntityManager.UnselectEveryEntity();
                        TileManager.UnselectEveryTile();
                        currentEntitySelected = null;
                        currentTileSelected = null;
                    }
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (!IsPointerOverUIElement())
                {
                    EntityManager.UnselectEveryEntity();
                    TileManager.UnselectEveryTile();
                    currentEntitySelected = null;
                    currentTileSelected = null;
                }
            }
        }

        

        if (currentEntitySelected != null)
        {
            boardManager.ResetAllTileLayer();
            var tileManager = boardManager.GetTileManagerFromTileNum(currentEntitySelected.entityState.currentTileNum);
            SetGameLayerRecursive(tileManager.gameObject, LayerMask.NameToLayer("UICamera"));
        }

        entityInfoUI.gameObject.SetActive(currentEntitySelected != null);

        if (lastEntitySelected != currentEntitySelected)
        {
            entityInfoUI.entityManager = currentEntitySelected;
        }

        lastEntitySelected = currentEntitySelected;

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;
        
        
    }

    void UpdateAccordingToGameState()
    {

        if (gameState == null)
        {
            return;
        }

        boardManager.boardState = gameState.boardState;

        if (gameState.playerStates == null)
        {
            return;
        }


        // Pour l'instant les playerManagers sont créés avant 

        foreach (PlayerState playerState in gameState.playerStates)
        {
            
            if (playerState.playerNum >= 0 &&
                playerState.playerNum < playerManagers.Length &&
                playerManagers[playerState.playerNum] != null)
            {
                playerManagers[playerState.playerNum].playerState = playerState;
            }
        }

    }

    public void UpdateVisuals()
    {
        UpdatePlayerText();
        UpdateTurnText();
        boardManager.UpdateVisuals();
        foreach (var playerManager in playerManagers)
        {
            playerManager.UpdateVisuals();
        }
    }

    public void UpdatePlayerText()
    {
        
        if (gameState == null)
        {
            playerTextMesh.text = "";
            return;
        }

        //Debug.Log("Updating player text to : " + "Player " + gameState.currentPlayerNum.ToString());
        playerTextMesh.text = "Player " + gameState.currentPlayerNum.ToString();
    }

    public void UpdateTurnText()
    {
        
        if (gameState == null)
        {
            turnTextMesh.text = "";
            return;
        }

        //Debug.Log("Updating player text to : " + "Player " + gameState.currentPlayerNum.ToString());
        turnTextMesh.text = "Turn " + gameState.turn;
    }


    public void SendUserAction(UserAction userAction)
    {
        Debug.Log($"Sending UserAction : {userAction}");
        var result = Game.currentGame.ReceiveUserAction(userAction);
        if (!result)
        {
            Debug.LogWarning($"Unable to perform {userAction}");
        }
        else
        {
            Debug.Log("Updating Game State");
            //Visual changes need to be applied only if an animation won't do it. 
            gameState = Game.currentGame.ToGameState();
        }
    }

    public PlayerManager GetPlayerManagerFromPlayerNum(uint playerNum)
    {

        if (playerManagers == null)
        {
            return null;
        }

        if (playerNum < 0 || playerNum > playerManagers.Length - 1)
        {
            return null;
        }

        return playerManagers[playerNum];
    }

    public EntityManager GetEntityManagerFromPlayernumAndEntityNum(uint playerNum, uint entityNum)
    {
        var playerManager = GetPlayerManagerFromPlayerNum(playerNum);

        if (playerManager == null)
        {
            return null;
        }

        return playerManager.GetEntityManagerFromEntityNum(entityNum);
    }

    /*
    [Obsolete]
    private void DequeueDepiledActionQueue()
    {

        if (animationManager.animationPlaying)
        {
            return;
        }

        //GameLogic.GameAction.Action action = Game.currentGame.DequeueDepiledActionQueue();

        while (!action.wasCancelled && !action.wasPerformed && Game.currentGame.depiledActionQueue.Count > 0)
        {
            //action = Game.currentGame.DequeueDepiledActionQueue();
        }

        if (action.wasCancelled)
        {
            Debug.Log($"Playing cancel animation for action {action}");
        }

        else if (action.wasPerformed)
        {
            Debug.Log($"Playing perform animation for action {action}");
            animationManager.PlayAnimationForAction(action);
        }

    }
    */


    void OnEntitySelected(EntityManager entityManager)
    {
        currentEntitySelected = entityManager;
        
        boardManager.ResetAllTileLayerDisplayUIInfo();
        if (entityManager.entityState.playerNum == gameState.currentPlayerNum)
        {
            boardManager.DisplayTilesUIInfo(entityManager.entityState.tileNumsToMoveTo.ToArray());
        }
    }

    void OnTileSelected(TileManager tileManager)
    {
        currentTileSelected = tileManager;
        if (currentEntitySelected == null)
        {
            
            return;
        }
        else
        {
            
        }
    }

    void OnEntityClicked(EntityManager entityManager)
    {
        if (blockInputs)
        {
            return;
        }

        entityWasClickedThisFrame = true;
        EntityManager.UnselectEveryEntity();
        TileManager.UnselectEveryTile();
        entityManager.selected = entityManager.hovered;

    }

    private void OnWeaponUsed()
    {
        if (currentEntitySelected == null)
        {
            return;
        }
        SendUserAction(new AtkWithEntityUserAction(currentEntitySelected.entityState.playerNum, currentEntitySelected.entityState.num));
    }

    void OnTileClicked(TileManager tileManager)
    {
        if (blockInputs)
        {
            return;
        }

        tileWasClickedThisFrame = true;


        if (currentEntitySelected != null)
        {
            //TODO: Check if it's the right player

            var canMove = currentEntitySelected.CanMove(tileManager);
            if (canMove)
            {
                var entityManagerCurrentTileState = boardManager.GetTileStateFromTileNum(currentEntitySelected.entityState.currentTileNum);
                if (entityManagerCurrentTileState != null)
                {
                    var direction = DirectionsExtensions.FromCoordinateDifference(
                        tileManager.tileState.gridX - entityManagerCurrentTileState.gridX,
                        tileManager.tileState.gridY - entityManagerCurrentTileState.gridY);
                    SendUserAction(new MoveEntityUserAction(
                        currentEntitySelected.entityState.playerNum,
                        currentEntitySelected.entityState.num,
                        direction));
                }
            }
            
            /*
            if (currentEntitySelected.entity.player == Game.currentGame.currentPlayer)
            {
                didMove = currentEntitySelected.TryToMove(tileManager.tile);
                if (!didMove)
                {
                    var direction = DirectionsExtensions.FromCoordinateDifference(
                        tileManager.tile.gridX - currentEntitySelected.entity.currentTile.gridX,
                        tileManager.tile.gridY - currentEntitySelected.entity.currentTile.gridY);
                    currentEntitySelected.TryToChangeDirection(direction);
                }
            }
            */
        }

        else
        {
            EntityManager.UnselectEveryEntity();
            TileManager.UnselectEveryTile();
            currentEntitySelected = null;
            tileManager.selected = tileManager.hovered;
        }
    }

    
    public void OnEndTurnPressed()
    {
        SendUserAction(new EndTurnUserAction(Game.currentGame.currentPlayer.playerNum));

        /*
        //temp et plutot playerNum == 0 ou playerNum == 1
        if (Game.currentGame.currentPlayer.playerNum == 1)
        {
            mainCameraTransform.position = player1CameraTransform.position;
            mainCameraTransform.rotation = player1CameraTransform.rotation;
        }
        else if (Game.currentGame.currentPlayer.playerNum == 2)
        {
            mainCameraTransform.position = player2CameraTransform.position;
            mainCameraTransform.rotation = player2CameraTransform.rotation;
        }
        else
        {

        }
        */

        EntityManager.UnselectEveryEntity();
        //boardManager.GetEntityManagerFromEntity(Game.currentGame.currentPlayer.hero).selected = true;
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


    private void OnEffectHoverEnter(EffectState effectState)
    {
        boardManager.DisplayTilesUIInfo(effectState.tilesAffectedNums.ToArray());
    }

    
    private void OnEffectHoverExit(EffectState effect)
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    [Obsolete]
    private void OnCardHoverEnter(Card card)
    {
        card.activableEffect.GetTilesAndEntitiesAffected(out Entity[] entities, out Tile[] tiles);
        boardManager.DisplayTilesUIInfo(tiles);
    }

    [Obsolete]
    private void OnCardHoverExit(Card card)
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    [Obsolete]
    private void OnWeaponHoverEnter()
    {

        if (entityInfoUI == null)
        {
            return;
        }
        if (entityInfoUI.entityManager == null)
        {
            return;
        }

        if (entityInfoUI.entityManager.entity == Entity.noEntity)
        {
            return;
        }

        entityInfoUI.entityManager.entity.GetTilesAndEntitiesAffectedByAtk(out Entity[] entities, out Tile[] tiles);
        boardManager.DisplayTilesUIInfo(tiles);
    }

    [Obsolete]
    private void OnWeaponHoverExit()
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }
    

}
