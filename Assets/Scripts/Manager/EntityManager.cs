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
    GameObject entityNameCanvasPrefab;

    GameObject entityNameCanvasInstance;

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
        private set{
            selected_ = value;

            if(selected){
                selectedEvent.Invoke(this);
            }
            
            var highlight = GetComponent<Highlight>();
            if(highlight != null){
                highlight.ToggleHighlight(selected, true);
            }
        }
    }

    private bool hovered_;

    public bool hovered{
        get{
            return hovered_;
        }
        private set{
            hovered_ = value;
            if(!selected){
                var highlight = GetComponent<Highlight>();
                if(highlight != null){
                    highlight.ToggleHighlight(hovered);
                }
            }
        }
    }

    public EntityManagerEvent selectedEvent = new EntityManagerEvent();
    
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
        selected = false;
        entityNameCanvasInstance = Instantiate(entityNameCanvasPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(entityNameCanvasInstance != null){
            entityNameCanvasInstance.SetActive(hovered);
            if(entity != null){
                entityNameCanvasInstance.GetComponentInChildren<TextMeshProUGUI>().text = entity.name;
            }
        }
    }

    void OnMouseDown(){
        UnselectEveryEntity();
        selected = hovered;
    }

    
    void OnMouseOver(){
        hovered = true;
    }

    void OnMouseExit(){
        hovered = false;
    }

    void UnselectEveryEntity(){
        foreach(GameObject entity in GameObject.FindGameObjectsWithTag("Entity")){
            var entityManager = entity.GetComponent<EntityManager>();
            if(entityManager != null){
                entityManager.selected = false;
            }
        }
    }

    void UpdateAccordingToEntity(){
        var boardManager = GetComponentInParent<BoardManager>();
        if(boardManager == null){
            return;
        }
        transform.position = new Vector3(
            entity.currentTile.gridX * boardManager.tileSizeX, 
            boardManager.entityY,  
            entity.currentTile.gridY * boardManager.tileSizeZ);

        transform.rotation = Quaternion.Euler(0f, entity.direction.ToAngle(), 0f);
    }

    public bool TryToMove(TileManager tileManager){

        var result = entity.TryToMove(tileManager.tile);
        if(result){
            UpdateAccordingToEntity();
        }

        return result;

    }
}
