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

    private enum UIState
    {
        Default, AnimationPlaying, EntitySelected, TileSelected, CardSelected
    }

    UIState uiState_;

    UIState uiState
    {
        get { return uiState_; }
        set
        {
            uiState_ = value;
            Debug.Log($"New UIState : {uiState}");
            switch (value)
            {
                case UIState.Default:
                case UIState.AnimationPlaying:
                    entityInfoUI.gameObject.SetActive(false);
                    boardManager.ResetAllTileLayerDisplayUIInfo();
                    boardManager.ResetAllTileLayer();
                    ResetAllEntityLayer();
                    EntityManager.UnselectEveryEntity();
                    TileManager.UnselectEveryTile();
                    currentEntitySelected = null;
                    currentTileSelected = null;
                    break;

                case UIState.EntitySelected:
                    TileManager.UnselectEveryTile();
                    currentTileSelected = null;
                    entityInfoUI.gameObject.SetActive(true);
                    boardManager.ResetAllTileLayer();
                    var tileManager = boardManager.GetTileManagerFromTileNum(currentEntitySelected.entityState.currentTileNum);
                    SetGameLayerRecursive(tileManager.gameObject, LayerMask.NameToLayer("UICamera"));
                    boardManager.ResetAllTileLayerDisplayUIInfo();
                    if (currentEntitySelected.entityState.playerNum == gameState.currentPlayerNum)
                    {
                        boardManager.DisplayTilesUIInfo(currentEntitySelected.entityState.tileNumsToMoveTo.ToArray());
                    }

                    entityInfoUI.entityManager = currentEntitySelected;
                    break;

                case UIState.TileSelected:
                    EntityManager.UnselectEveryEntity();
                    ResetAllEntityLayer();
                    currentEntitySelected = null;
                    boardManager.ResetAllTileLayerDisplayUIInfo();
                    entityInfoUI.gameObject.SetActive(false);
                    break;

                case UIState.CardSelected:
                    EntityManager.UnselectEveryEntity();
                    TileManager.UnselectEveryTile();
                    currentEntitySelected = null;
                    currentTileSelected = null;
                    boardManager.ResetAllTileLayerDisplayUIInfo();
                    boardManager.ResetAllTileLayer();
                    ResetAllEntityLayer();
                    entityInfoUI.gameObject.SetActive(false);
                    boardManager.DisplayTilesUIInfo(currentCardSelected.cardState.possibleTileTargets.ToArray());
                    //same with entity
                    break;

                default: break;
            }
            
        }
    }

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
    BoardManager boardManager;

    [SerializeField]
    AnimationManager animationManager;

    [SerializeField]
    PlayerManager[] playerManagers;

    [SerializeField]
    int boardHeight;

    [SerializeField]
    int boardWidth;

    EntityManager currentEntitySelected;

    TileManager currentTileSelected;

    EntityManager currentEntityHoveredWhileHoldingCard;

    TileManager currentTileHoveredWhileHoldingCard;

    CardManager currentCardSelected;

    [SerializeField]
    public ManaUIDisplay manaUIDisplay;

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

    [SerializeField]
    LayerMask layerMaskEntity;

    [SerializeField]
    LayerMask layerMaskTile;

    bool entityWasClickedThisFrame;

    bool tileWasClickedThisFrame;

    int UILayer;


    // Start is called before the first frame update
    void Start()
    {

        Game.currentGame.SetUpGame(playerManagers.Length, boardHeight, boardWidth);
        //Game.currentGame.board = Game.currentGame.board;

        currentTileSelected = null;
        currentEntitySelected = null;
        currentCardSelected = null;
        currentEntityHoveredWhileHoldingCard = null;
        currentTileHoveredWhileHoldingCard = null;

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;

        uiState = UIState.Default;

        UILayer = LayerMask.NameToLayer("UI");

        boardManager.entitySelectedEvent.AddListener(OnEntitySelected);
        boardManager.entityMouseDownEvent.AddListener(OnEntityMouseDown);
        boardManager.entityMouseUpEvent.AddListener(OnEntityMouseUp);

        boardManager.tileSelectedEvent.AddListener(OnTileSelected);
        boardManager.tileMouseDownEvent.AddListener(OnTileMouseDown);
        boardManager.tileMouseUpEvent.AddListener(OnTileMouseUpEvent);



        // --------- Testing ---------
        var startingTile1 = Game.currentGame.board.GetTileAt(2, 2);
        var direction1 = Direction.East;
        var hero1 = new Hero(
            Game.currentGame.players[0],
            EntityModel.MageHero,
            "Mage",
            startingTile1,
            new Health(new HeartType[] { HeartType.Red, HeartType.Red, HeartType.Red }),
            3,
            new List<EntityEffect> {new MoveToChangeTileTypeEffect(null, TileType.Nature)},
            direction1);
        
        hero1.num = 0;
        Game.currentGame.players[0].entities.Add(hero1);
        Game.currentGame.players[0].hero = hero1;

        //playerManagers[0].player.TryToSpawnEntity(hero1);

        //animationManager.SpawnEntity(hero1);

        var startingTile2 = Game.currentGame.board.GetTileAt(2, 3);
        var direction2 = Direction.South;
        var hero2 = new Hero(
            Game.currentGame.players[1],
            EntityModel.MageHero,
            "Mage",
            startingTile2,
            new Health(new HeartType[] { HeartType.Red, HeartType.Red, HeartType.Red }),
            3,
            new List<EntityEffect> {new MoveToChangeTileTypeEffect(null, TileType.Cursed)},
            direction2);
        //hero2.effects.Add(new MoveToChangeTileTypeEffect(hero2, TileType.CurseSource));
        hero2.num = 0;
        Game.currentGame.players[1].entities.Add(hero2);
        Game.currentGame.players[1].hero = hero2;


        //playerManagers[1].player.TryToSpawnEntity(hero2);

        //animationManager.SpawnEntity(hero2);

        // A enlever
        for (var i = 0; i < playerManagers.Length; i++)
        {
            var playerState = Game.currentGame.players[i].ToPlayerState();
            playerManagers[i].playerState = playerState;
            playerManagers[i].cardSelectedEvent.AddListener((cardManager) => OnCardSelected(cardManager, playerState.playerNum));
            playerManagers[i].cardMouseDownEvent.AddListener((cardManager) => OnCardMouseDown(cardManager, playerState.playerNum));
            playerManagers[i].cardMouseUpEvent.AddListener((cardManager) => OnCardMouseUp(cardManager, playerState.playerNum));
            playerManagers[i].cardHoverEnterEvent.AddListener((cardManager) => OnCardHoverEnter(cardManager, playerState.playerNum));
            playerManagers[i].cardHoverExitEvent.AddListener((cardManager) => OnCardHoverExit(cardManager, playerState.playerNum));
        }

        // ------------------

        entityInfoUI.weaponUsedUnityEvent.AddListener(OnWeaponUsed);
        entityInfoUI.effectHoverEnterEvent.AddListener(OnEffectHoverEnter);
        entityInfoUI.effectHoverExitEvent.AddListener(OnEffectHoverExit);
        //entityInfoUI.weaponHoverEnterEvent.AddListener(OnWeaponHoverEnter);
        //entityInfoUI.weaponHoverExitEvent.AddListener(OnWeaponHoverExit);

        Game.currentGame.PileAction(new StartGameAction());
        gameState = Game.currentGame.ToGameState();
        UpdateVisuals();


    }

    

    // Update is called once per frame
    void Update()
    {
        //UI state machine
        switch (uiState)
        {
            case UIState.Default:

                break;

            case UIState.AnimationPlaying:
                if (!animationManager.animationPlaying && Game.currentGame.actionStatesToSendQueue.Count == 0)
                {
                    uiState = UIState.Default;
                    OnAnimationsFinished();
                }
                break;

            case UIState.EntitySelected:
            case UIState.TileSelected:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (!IsPointerOverUIElement())
                    {
                        if (!entityWasClickedThisFrame && !tileWasClickedThisFrame)
                        {
                            uiState = UIState.Default;
                        }
                    }
                }

                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    if (!IsPointerOverUIElement())
                    {
                        uiState = UIState.Default;
                    }
                }
                break;

            case UIState.CardSelected:
                if (!Mouse.current.leftButton.isPressed)
                {
                    TryToPlayeCarddWithTargets(
                        0,
                        currentCardSelected.positionInHand,
                        currentCardSelected.cardState,
                        currentEntityHoveredWhileHoldingCard == null ? null : currentEntityHoveredWhileHoldingCard.entityState,
                        currentTileHoveredWhileHoldingCard == null ? null : currentTileHoveredWhileHoldingCard.tileState);
                    currentCardSelected.selected = false;
                    currentCardSelected.hoveringOver = false;
                    currentCardSelected = null;
                    
                    uiState = UIState.Default;
                    
                }
                //Temp
                foreach (var playerManager in playerManagers)
                {
                    playerManager.handManager.UpdateCardsPosition();
                }
                break;

            default:
                break;
        }


        if (Game.currentGame.actionStatesToSendQueue.Count > 0 && !animationManager.animationPlaying /*temp*/)
        {
            //test
            ActionState actionState = Game.currentGame.DequeueActionStateToSendQueue();
            string serializedActionState = JsonConvert.SerializeObject(actionState);
            Debug.Log("Serialized ActionState :" + serializedActionState);
            var deserializedActionState = JsonConvert.DeserializeObject<ActionState>(serializedActionState);
            Debug.Log("Deserialized ActionState :" + deserializedActionState);


            if (uiState != UIState.AnimationPlaying)
            {
                uiState = UIState.AnimationPlaying;
            }
            
            HandleActionState(deserializedActionState);
            
        }

        entityWasClickedThisFrame = false;
        tileWasClickedThisFrame = false;

    }

    

    void FixedUpdate()
    {
        switch (uiState)
        {
            case UIState.CardSelected:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitEntity;
                RaycastHit hitTile;
                Debug.DrawRay(ray.origin, ray.direction * 20, Color.white);
                EntityManager.UnhoverEveryEntity();
                TileManager.UnhoverEveryTile();
                var carHoveringOver = false;
                currentEntityHoveredWhileHoldingCard = null;
                currentTileHoveredWhileHoldingCard = null;
                if (Physics.Raycast(ray, out hitEntity, 1000f, layerMaskEntity))
                {
                    if (hitEntity.collider != null)
                    {
                        var entityManager = hitEntity.collider.gameObject.GetComponent<EntityManager>();
                        if (entityManager != null)
                        {
                            carHoveringOver = true;
                            entityManager.OnMouseOver();
                            currentEntityHoveredWhileHoldingCard = entityManager;
                        }
                    }
                }
                else
                {

                    if (Physics.Raycast(ray, out hitTile, 1000f, layerMaskTile))
                    {
                        if (hitTile.collider != null)
                        {
                            var tileManager = hitTile.collider.gameObject.GetComponent<TileManager>();
                            if (tileManager != null)
                            {
                                carHoveringOver = true;
                                tileManager.OnMouseOver();
                                currentTileHoveredWhileHoldingCard = tileManager;
                            }
                        }
                    }
                }

                if (currentCardSelected != null)
                {
                    currentCardSelected.hoveringOver = carHoveringOver;
                }
                
                break;
            default: break;
        }
    }

    void HandleActionState(ActionState actionState)
    {
        if (actionState == null)
        {
            return;
        }

        Debug.Log("Playing animation for " + JsonConvert.SerializeObject(actionState));
        animationManager.HandleActionState(actionState);
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

        manaUIDisplay.playerState = gameState.playerStates[(int)gameState.currentPlayerNum];

    }

    public void UpdateVisuals()
    {
        UpdatePlayerText();
        UpdateTurnText();
        boardManager.UpdateVisuals();
        manaUIDisplay.UpdateVisuals();
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

    public void OnAnimationsFinished()
    {

        Debug.Log("--------- Animation finished ---------");
        Debug.Log("Updating according To Game State");

        gameState = Game.currentGame.ToGameState();
        //UpdateVisuals();


        string jsonGameState = JsonConvert.SerializeObject(gameState, Formatting.Indented);

        // File path (write to persistent data path)
        string filePath = Path.Combine(Application.persistentDataPath, "LastGameState.json");

        Debug.Log($"Writing GameState to {filePath}");
        // Write to file
        File.WriteAllText(filePath, jsonGameState);
    }


    public void SendUserAction(UserAction userAction)
    {
        Debug.Log($"Sending UserAction : {JsonConvert.SerializeObject(userAction)}");
        var result = Game.currentGame.ReceiveUserAction(userAction);
        if (!result)
        {
            Debug.LogWarning($"Unable to perform {userAction}");
        }
        else
        {
            Debug.Log("Updating Game State");
            //Visual changes need to be applied only if an animation won't do it. 
            //gameState = Game.currentGame.ToGameState();
        }
    }


    private bool TryToPlayeCarddWithTargets(uint playerNum, int cardPositionInHand, CardState cardState, EntityState targetEntityState, TileState targetTileState)
    {
        if (!cardState.needsEntityTarget && !cardState.needsTileTarget)
        {
            SendUserAction(new PlayCardFromHandUserAction(playerNum, cardPositionInHand, 0, 0, 0));
        }
        else if (cardState.needsEntityTarget)
        {

        }
        else if (cardState.needsTileTarget && targetTileState != null)
        {
            SendUserAction(new PlayCardFromHandUserAction(playerNum, cardPositionInHand, 0, 0, tileTargetNum: (int)targetTileState.num));
        }

        return false;
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

        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }

        ResetAllEntityLayer();

        currentEntitySelected = entityManager;
        uiState = UIState.EntitySelected;

    }

    void OnEntityMouseDown(EntityManager entityManager)
    {
        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }

        entityWasClickedThisFrame = true;
        EntityManager.UnselectEveryEntity();
        entityManager.selected = entityManager.hovered;

    }

    private void OnEntityMouseUp(EntityManager entityManager)
    {
        if (uiState != UIState.CardSelected)
        {
            return;
        }
    }

    void OnTileSelected(TileManager tileManager)
    {
        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }

        boardManager.ResetAllTileLayer();

        currentTileSelected = tileManager;
        uiState = UIState.TileSelected;
    }

    void OnTileMouseDown(TileManager tileManager)
    {
        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }

        tileWasClickedThisFrame = true;


        if (uiState == UIState.EntitySelected)
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
            TileManager.UnselectEveryTile();
            tileManager.selected = tileManager.hovered;
        }
    }


    private void OnTileMouseUpEvent(TileManager tileManager)
    {
        if (uiState != UIState.CardSelected)
        {
            return;
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

    private void OnCardSelected(CardManager cardManager, uint playerNum)
    {
        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }

        currentCardSelected = cardManager;
        uiState = UIState.CardSelected;
    }


    private void OnCardMouseDown(CardManager cardManager, uint playerNum)
    {
        if (uiState == UIState.AnimationPlaying)
        {
            return;
        }


        CardManager.UnselectEveryCard();
        cardManager.selected = true;
    }

    private void OnCardMouseUp(CardManager cardManager, uint playerNum)
    {
        if (!cardManager.selected)
        {
            return;
        }

        if (!cardManager.cardState.needsEntityTarget && !cardManager.cardState.needsTileTarget)
        {
            //And current mouse pos is in the right area
            SendUserAction(new PlayCardFromHandUserAction(playerNum, cardManager.positionInHand, 0, 0, 0));
        }
        else if (cardManager.cardState.needsEntityTarget)
        {

        }
        else if (cardManager.cardState.needsTileTarget)
        {

        }

        

    }

    private void OnCardHoverEnter(CardManager cardManager, uint playerNum)
    {
        //card.activableEffect.GetTilesAndEntitiesAffected(out Entity[] entities, out Tile[] tiles);
        //boardManager.DisplayTilesUIInfo(tiles);
    }

    private void OnCardHoverExit(CardManager cardManager, uint playerNum)
    {
        //boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    [Obsolete]
    private void OnCardHoverEnter(Card card)
    {
        //card.activableEffect.GetTilesAndEntitiesAffected(out Entity[] entities, out Tile[] tiles);
        //boardManager.DisplayTilesUIInfo(tiles);
    }

    [Obsolete]
    private void OnCardHoverExit(Card card)
    {
        boardManager.ResetAllTileLayerDisplayUIInfo();
    }

    private void OnWeaponUsed()
    {
        if (uiState != UIState.EntitySelected)
        {
            return;
        }
        SendUserAction(new AtkWithEntityUserAction(currentEntitySelected.entityState.playerNum, currentEntitySelected.entityState.num));
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

    public void ResetAllEntityLayer()
    {
        if (playerManagers == null)
        {
            return;
        }

        foreach (var playerManager in playerManagers)
        {
            playerManager.ResetAllEntityLayer();
        }
    }
    

}
