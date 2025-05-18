using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameState;

public class BoardManager : MonoBehaviour
{
    private Board board_;

    public Board board{
        get{
            return board_;
        }

        set{
            board_ = value;
            CreateTilesAccordingToBoard();
        }
    }

    private BoardState boardState_;

    public BoardState boardState{
        get{
            return boardState_;
        }

        set{
            boardState_ = value;
            UpdateAccordingToBoardState();
        }
    }

    TileManager[] tileManagers;

    List<EntityManager> entityManagers = new List<EntityManager>();

    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    public float tileSizeX;

    [SerializeField]
    public float tileSizeZ;

    [SerializeField]
    public float entityY;

    public EntityManagerEvent entitySelectedEvent = new EntityManagerEvent();

    public TileManagerEvent tileSelectedEvent = new TileManagerEvent();

    public EntityManagerEvent entityClickedEvent = new EntityManagerEvent();

    public TileManagerEvent tileClickedEvent = new TileManagerEvent();
    
    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        
    }

    void UpdateAccordingToBoardState()
    {
        
    }

    void OnTileSelected(TileManager tileManager){
        tileSelectedEvent.Invoke(tileManager);
    }

    void OnEntitySelected(EntityManager entityManager){
        entitySelectedEvent.Invoke(entityManager);
    }

    void OnTileClicked(TileManager tileManager){
        tileClickedEvent.Invoke(tileManager);
    }

    void OnEntityClicked(EntityManager entityManager){
        entityClickedEvent.Invoke(entityManager);
    }


    void CreateTilesAccordingToBoard(){

        if(tileManagers != null){
            return;
        }
        
        tileManagers = new TileManager[board.tiles.Length];

        foreach(Tile tile in board.tiles){
            if(tileManagers[tile.num] == null){
                tileManagers[tile.num] = CreateTileManagerAt(new Vector3(
                    tileSizeX * tile.gridX,
                    0f,
                    tileSizeZ * tile.gridY));
            }
            tileManagers[tile.num].tile = tile;

        }
    }

    void DestroyAllTiles(){

    }

    TileManager CreateTileManagerAt(Vector3 pos){
        var tileInstance = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
        var tileManager = tileInstance.GetComponent<TileManager>();
        tileManager.selectedEvent.AddListener(OnTileSelected);
        tileManager.clickedEvent.AddListener(OnTileClicked);
        return tileManager;
    }

    public void SpawnEntity(GameObject entityPrefab, Entity entity){
        var entityInstance = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity, transform);
        var entityManager = entityInstance.GetComponent<EntityManager>();
        if(entityManager == null){
            Debug.LogError("Entity prefab has no EntityManager attached.");
            Destroy(entityInstance);
            return;
        }

        entityManager.entity = entity;
        entityManager.selectedEvent.AddListener(OnEntitySelected);
        entityManager.clickedEvent.AddListener(OnEntityClicked);

        AddEntity(entityManager);
    }

    public void AddEntity(EntityManager entityManager){
        entityManagers.Add(entityManager);
        //entityManager.entity.player.entities.Add(entityManager.entity);
    }

    public void RemoveEntity(EntityManager entityManager){
        entityManagers.Remove(entityManager);
        board.entities.Remove(entityManager.entity);
    }

    public EntityManager GetEntityManagerFromEntity(Entity entity){
        foreach (var entityManager in entityManagers){
            if(entityManager.entity == entity){
                return entityManager;
            }
        }

        return null;
    }

    public TileManager GetTileManagerFromTile(Tile tile){
        if(tile == Tile.noTile){
            return null;
        }
        if(tile.num >= 0 && tile.num < tileManagers.Length){
            return tileManagers[tile.num];
        }

        return null;
    }

    public void DisplayTilesUIInfo(Tile[] tiles){
        foreach(var tile in tiles){
            var tileManager = GetTileManagerFromTile(tile);
            if(tileManager != null){
                tileManager.displayInfoUI = true;
            }
        }
    }

    public void ResetAllTileLayerDisplayUIInfo(){
        foreach (var tileManager in tileManagers){
            tileManager.displayInfoUI = false;
        }
    }

    public void ResetAllTileLayer(){
        foreach (var tileManager in tileManagers){
            GameManager.SetGameLayerRecursive(tileManager.gameObject, LayerMask.NameToLayer("Tile"));
        }
    }
}
