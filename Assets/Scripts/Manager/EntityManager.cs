using UnityEngine;
using UnityEngine.Events;
using TMPro;

using GameLogic;
using GameLogic.GameAction;
using GameLogic.GameState;
using System;

[System.Serializable]
public class EntityManagerEvent : UnityEvent<EntityManager>
{
}

[RequireComponent(typeof(Highlight))]
public class EntityManager : MonoBehaviour
{
    [SerializeField]
    float yOffset;

    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject entityInfoCanvasPrefab;

    GameObject entityInfoCanvasInstance;

    [Obsolete]
    private Entity entity_;

    [Obsolete]
    public Entity entity
    {
        get
        {
            return entity_;
        }

        set
        {
            entity_ = value;
            UpdateAccordingToEntity();
        }
    }

    private EntityState entityState_;

    public EntityState entityState{
        get{
            return entityState_;
        }

        set{
            entityState_ = value;
            UpdateAccordingToEntityState();
        }
    }

    private bool selected_;

    public bool selected{
        get{
            return selected_;
        }
        set{
            selected_ = value;

            if(selected){
                selectedEvent.Invoke(this);
                GameManager.SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("UICamera"));
            }
            else{
                GameManager.SetGameLayerRecursive(gameObject, LayerMask.NameToLayer("Entity"));
            }
            /*
            var highlight = GetComponent<Highlight>();
            if(highlight != null){
                highlight.ToggleHighlight(selected, true);
            }
            */
        }
    }

    private bool hovered_;

    public bool hovered{
        get{
            return hovered_;
        }
        private set{
            hovered_ = value;
            
            var highlight = GetComponent<Highlight>();
            if(highlight != null){
                highlight.ToggleHighlight(hovered);
            }
            
        }
    }

    [SerializeField]
    float walkingSpeed;

    [SerializeField]
    float timeToDislayInfoUI = 1f;

    float infoUITimer = 0f;

    [HideInInspector]
    public TileManager goalTileManager;

    BoardManager boardManager;

    [HideInInspector]
    public EntityManagerEvent selectedEvent = new EntityManagerEvent();

    [HideInInspector]
    public EntityManagerEvent clickedEvent = new EntityManagerEvent();
    
    void Awake(){
        boardManager = GetComponentInParent<BoardManager>();
        entityInfoCanvasInstance = Instantiate(entityInfoCanvasPrefab, transform);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
        selected = false;
        ResetInfoUITimer();
    }

    // Update is called once per frame
    void Update(){
        
        if(entityInfoCanvasInstance != null){
            entityInfoCanvasInstance.SetActive(hovered || infoUITimer > 0f);
        }

        if(goalTileManager != null){
            //transform.rotation = Quaternion.Euler(0f, entity.direction.ToAngle(), 0f);

            float step = walkingSpeed * Time.deltaTime;
            
            var actualGoal = new Vector3(goalTileManager.transform.position.x, boardManager.entityY, goalTileManager.transform.position.z);
            if(Vector3.Distance(transform.position, actualGoal) < step){
                transform.position = actualGoal;
                goalTileManager = null;
            }
            else{
                transform.position += (actualGoal - transform.position).normalized * step;
            }
        }

        animator.SetBool("isWalking", goalTileManager != null);

        if(infoUITimer > 0f){
            infoUITimer = Mathf.Max(infoUITimer - Time.deltaTime, 0f);
        }

    }

    void OnMouseDown(){
        clickedEvent.Invoke(this);
    }

    void OnMouseOver(){
        hovered = true;
    }

    void OnMouseExit(){
        hovered = false;
    }

    public static void UnselectEveryEntity(){
        foreach(GameObject entity in GameObject.FindGameObjectsWithTag("Entity")){
            var entityManager = entity.GetComponent<EntityManager>();
            if(entityManager != null){
                entityManager.selected = false;
            }
        }
    }

    private void UpdateAccordingToEntityState()
    {
        //A voir a cause des animations
        //UpdatePositionAccordingToEntityState();
        //UpdateRotationAccordingToEntityState();
        UpdateHealthUIDisplay();
        UpdateMovementUIDisplay();
        UpdateNameUIDisplay();
    }

    [Obsolete]
    void UpdateAccordingToEntity(){
        
        UpdatePositionAccordingToEntity();
        UpdateRotationAccordingToEntity();

        UpdateHealthUIDisplay();
        UpdateMovementUIDisplay();
        UpdateNameUIDisplay();
    }

    [Obsolete]
    public void UpdatePositionAccordingToEntity()
    {
        transform.position = new Vector3(
            entity.currentTile.gridX * boardManager.tileSizeX,
            boardManager.entityY + yOffset,
            entity.currentTile.gridY * boardManager.tileSizeZ);

    }

    [Obsolete]
    public void UpdateRotationAccordingToEntity()
    {
        transform.rotation = Quaternion.Euler(0f, entity.direction.ToAngle(), 0f);
    }

    [Obsolete]
    public bool TryToMove(Tile tile)
    {

        if (!entity.CanMoveByChangingDirection(tile))
        {
            return false;
        }

        entity.TryToCreateEntityUseMovementAction(
            tile.Distance(entity.currentTile) * entity.costToMove.mouvementCost,
            out EntityUseMovementAction useMovementAction);

        entity.TryToCreateEntityMoveAction(tile, useMovementAction, out EntityMoveAction entityMoveAction);

        return entityMoveAction.wasPerformed;

    }

    [Obsolete]
    public bool TryToChangeDirection(Direction direction)
    {

        entity.TryToCreateEntityChangeDirectionAction(direction, null, out EntityChangeDirectionAction entityChangeDirectionAction);
        return entityChangeDirectionAction.wasPerformed;

    }

    [Obsolete]
    public bool TryToAttack(Entity entity)
    {

        if (!this.entity.CanAttackByChangingDirection(entity))
        {
            return false;
        }

        this.entity.TryToCreateEntityUseMovementAction(this.entity.costToAtk.mouvementCost, out EntityUseMovementAction entityUseMovementAction);
        this.entity.TryToCreateEntityPayHeartCostAction(this.entity.costToAtk.heartCost, out EntityPayHeartCostAction entityPayHeartCostAction);

        if (!entityPayHeartCostAction.wasPerformed || !entityUseMovementAction.wasPerformed)
        {
            return false;
        }

        this.entity.TryToCreateEntityAttackAction(entity, out EntityAttackAction entityAttackAction, entityPayHeartCostAction);

        return entityAttackAction.wasPerformed;
    }

    [Obsolete]
    public bool TryToAttack()
    {

        entity.GetTilesAndEntitiesAffectedByAtk(
            out Entity[] attackedEntities,
            out Tile[] _
            );

        if (attackedEntities.Length < 1)
        {
            return false;
        }

        return TryToAttack(attackedEntities[0]);
    }

    public void UpdateHealthUIDisplay(){
        var healthUIDisplay = entityInfoCanvasInstance.GetComponentInChildren<HealthUIDisplay>();
        if(healthUIDisplay!=null){
            healthUIDisplay.healthState = entityState.healthState;
            ResetInfoUITimer();
        }
    }

    public void UpdateMovementUIDisplay(){
        var movementUIDisplay = entityInfoCanvasInstance.GetComponentInChildren<MovementUIDisplay>();
        if(movementUIDisplay!=null){
            movementUIDisplay.entityState = entityState;
            ResetInfoUITimer();
        }
    }

    public void UpdateNameUIDisplay(){
        var nameUIDisplay = entityInfoCanvasInstance.GetComponentInChildren<TextMeshProUGUI>();
        if(nameUIDisplay!=null){
            nameUIDisplay.text = entityState.name;
            ResetInfoUITimer();
        }
    }

    void ResetInfoUITimer(){
        infoUITimer = timeToDislayInfoUI;
    }
}
