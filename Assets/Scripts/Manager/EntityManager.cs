using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class EntityManagerEvent : UnityEvent<EntityManager>
{
}

[RequireComponent(typeof(Highlight))]
public class EntityManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    GameObject entityInfoCanvasPrefab;

    GameObject entityInfoCanvasInstance;

    private Entity entity_;

    public Entity entity{
        get{
            return entity_;
        }

        set{
            entity_ = value;
            UpdateAccordingToEntity();
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

    public TileManager goalTileManager;

    BoardManager boardManager;

    public EntityManagerEvent selectedEvent = new EntityManagerEvent();

    public EntityManagerEvent clickedEvent = new EntityManagerEvent();
    
    void Awake(){
        boardManager = GetComponentInParent<BoardManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
        selected = false;
        //entityInfoCanvasInstance = Instantiate(entityInfoCanvasPrefab, transform);
    }

    // Update is called once per frame
    void Update(){
        
        if(entityInfoCanvasInstance != null){
            entityInfoCanvasInstance.SetActive(hovered || selected);
            if(entity != null){
                entityInfoCanvasInstance.GetComponentInChildren<TextMeshProUGUI>().text = entity.name;
                entityInfoCanvasInstance.GetComponentInChildren<HealthUIDisplay>().health = entity.health;
            }
        }

        if(goalTileManager != null){
            transform.rotation = Quaternion.Euler(0f, entity.direction.ToAngle(), 0f);

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

    void UpdateAccordingToEntity(){
        
        transform.position = new Vector3(
            entity.currentTile.gridX * boardManager.tileSizeX, 
            boardManager.entityY,  
            entity.currentTile.gridY * boardManager.tileSizeZ);

        transform.rotation = Quaternion.Euler(0f, entity.direction.ToAngle(), 0f);
    }

    public bool TryToMove(Tile tile){


        if(!entity.CanMove(tile)){
            return false;
        }
        
        entity.player.TryToCreatePlayerUseMovementAction(tile.Distance(entity.currentTile), out PlayerUseMovementAction useMovementAction);
        entity.TryToCreateEntityMoveAction(tile, useMovementAction, out EntityMoveAction entityMoveAction);
        if(entityMoveAction.wasPerformed){
            //UpdateAccordingToEntity();
        }

        return entityMoveAction.wasPerformed;

    }
}
