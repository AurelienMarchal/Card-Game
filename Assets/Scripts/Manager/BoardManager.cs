using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameLogic;
using GameLogic.GameState;
using System;

[Serializable]
public struct PrefabCorrespondingToEntityModel {
    public EntityModel entityModel;
    public GameObject prefab;
}

public class BoardManager : MonoBehaviour
{
    [Obsolete]
    private Board board_;

    [Obsolete]
    public Board board
    {
        get
        {
            return board_;
        }

        set
        {
            board_ = value;
            CreateTilesAccordingToBoard();
        }
    }

    private BoardState boardState_;

    public BoardState boardState
    {
        get
        {
            return boardState_;
        }

        set
        {
            boardState_ = value;
            UpdateAccordingToBoardState();
        }
    }

    [SerializeField]
    PrefabCorrespondingToEntityModel[] prefabCorrespondingToEntityModels;

    TileManager[] tileManagers;

    [Obsolete]
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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateAccordingToBoardState()
    {
        if (boardState == null)
        {
            return;
        }

        if (boardState.tileStates == null)
        {
            return;
        }

        //TODO: Check if tiles have changed and Remove/Add accordingly

        if (tileManagers == null || tileManagers.Length != boardState.tileStates.Count)
        {
            tileManagers = new TileManager[boardState.tileStates.Count];
        }

        foreach (TileState tileState in boardState.tileStates)
        {
            //Il faut etre sur que ca ne soit pas le cas avant ca ou avoir un attribut maxTileNum dans boardState 
            if (tileState.num >= 0 && tileState.num < tileManagers.Length)
            {
                if (tileManagers[tileState.num] == null)
                {
                    tileManagers[tileState.num] = CreateTileManagerAt(new Vector3(
                        tileSizeX * tileState.gridX,
                        0f,
                        tileSizeZ * tileState.gridY));
                }
                tileManagers[tileState.num].tileState = tileState;
            }
        }
    }

    void OnTileSelected(TileManager tileManager)
    {
        tileSelectedEvent.Invoke(tileManager);
    }

    void OnEntitySelected(EntityManager entityManager)
    {
        entitySelectedEvent.Invoke(entityManager);
    }

    void OnTileClicked(TileManager tileManager)
    {
        tileClickedEvent.Invoke(tileManager);
    }

    void OnEntityClicked(EntityManager entityManager)
    {
        entityClickedEvent.Invoke(entityManager);
    }

    [Obsolete]
    //A enlever  
    void CreateTilesAccordingToBoard()
    {

        if (tileManagers != null)
        {
            return;
        }

        tileManagers = new TileManager[board.tiles.Length];

        foreach (Tile tile in board.tiles)
        {
            if (tileManagers[tile.num] == null)
            {
                tileManagers[tile.num] = CreateTileManagerAt(new Vector3(
                    tileSizeX * tile.gridX,
                    0f,
                    tileSizeZ * tile.gridY));
            }
            tileManagers[tile.num].tile = tile;

        }
    }

    void DestroyAllTiles()
    {

    }

    TileManager CreateTileManagerAt(Vector3 pos)
    {
        var tileInstance = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
        var tileManager = tileInstance.GetComponent<TileManager>();
        tileManager.selectedEvent.AddListener(OnTileSelected);
        tileManager.clickedEvent.AddListener(OnTileClicked);
        return tileManager;
    }

    [Obsolete]
    public void SpawnEntity(GameObject entityPrefab, Entity entity)
    {

        var entityInstance = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity, transform);
        var entityManager = entityInstance.GetComponent<EntityManager>();
        if (entityManager == null)
        {
            Debug.LogError("Entity prefab has no EntityManager attached.");
            Destroy(entityInstance);
            return;
        }

        entityManager.entity = entity;
        entityManager.selectedEvent.AddListener(OnEntitySelected);
        entityManager.clickedEvent.AddListener(OnEntityClicked);

        AddEntity(entityManager);
    }

    public EntityManager SpawnEntity(EntityState entityState)
    {
        GameObject entityPrefab = null;
        foreach(PrefabCorrespondingToEntityModel prefabCorrespondingToEntityModel in prefabCorrespondingToEntityModels){
            if(prefabCorrespondingToEntityModel.entityModel == entityState.model){
                entityPrefab = prefabCorrespondingToEntityModel.prefab;
            }
        }

        if (entityPrefab == null)
        {

            Debug.LogError($"Not prefab corresponding to EntityModel {entityState.model}.");
            return null;
        }

        var entityInstance = Instantiate(entityPrefab, Vector3.zero, Quaternion.identity, transform);
        var entityManager = entityInstance.GetComponent<EntityManager>();
        if (entityManager == null)
        {
            Debug.LogError("Entity prefab has no EntityManager attached.");
            Destroy(entityInstance);
            return null;
        }

        entityManager.entityState = entityState;
        entityManager.selectedEvent.AddListener(OnEntitySelected);
        entityManager.clickedEvent.AddListener(OnEntityClicked);
        return entityManager;
    }

    [Obsolete]
    public void AddEntity(EntityManager entityManager)
    {
        entityManagers.Add(entityManager);
        //entityManager.entity.player.entities.Add(entityManager.entity);
    }

    [Obsolete]
    public void RemoveEntity(EntityManager entityManager)
    {
        entityManagers.Remove(entityManager);
        board.entities.Remove(entityManager.entity);
    }

    [Obsolete]
    public EntityManager GetEntityManagerFromEntity(Entity entity)
    {
        foreach (var entityManager in entityManagers)
        {
            if (entityManager.entity == entity)
            {
                return entityManager;
            }
        }

        return null;
    }

    public TileManager GetTileManagerFromTileNum(uint tileNum)
    {
        if (tileManagers == null)
        {
            return null;
        }

        if (tileNum < tileManagers.Length)
        {
            return tileManagers[tileNum];
        }

        return null;
    }

    public TileState GetTileStateFromTileNum(uint tileNum)
    {
        TileManager tileManager = GetTileManagerFromTileNum(tileNum);
        if (tileManager == null)
        {
            return null;
        }
        return tileManager.tileState;
    }

    [Obsolete]
    public void DisplayTilesUIInfo(Tile[] tiles)
    {
        /*
        foreach (var tile in tiles)
        {
            var tileManager = GetTileManagerFromTile(tile);
            if (tileManager != null)
            {
                tileManager.displayInfoUI = true;
            }
        }
        */
    }

    public void ResetAllTileLayerDisplayUIInfo()
    {
        foreach (var tileManager in tileManagers)
        {
            tileManager.displayInfoUI = false;
        }
    }

    public void ResetAllTileLayer()
    {
        foreach (var tileManager in tileManagers)
        {
            GameManager.SetGameLayerRecursive(tileManager.gameObject, LayerMask.NameToLayer("Tile"));
        }
    }
}
