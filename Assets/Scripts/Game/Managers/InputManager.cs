using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public GameManager gameManager;

    public Func<bool> IsInputLocked;

    public Action OnActionDenided;
    public Action OnSelected;
    public Action OnMoveOrder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*
    // Update is called once per frame
    void Update()
    {
        // Check if input locked by UI
        if (EventSystem.current?.IsPointerOverGameObject() ?? false)
        {
            return;
        }
        
        if (IsInputLocked?.Invoke() ?? false)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            TryTileClick();
        }
        else
        {
            TryTileHoover();
        }
        
    }
    

    private HexTile TileAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 100f, gameManager.mapManager.GroundLayer))
        {
            return hit.collider.GetComponentInParent<HexTile>();
        }
        return null;
    }

    private void TryTileClick()
    {
        HexTile tile = TileAtMouse();
        if (tile != null)
        {
            TileSelect(tile);
        }
    }

    private void TryTileHoover()
    {
        HexTile tile = TileAtMouse();
        HexTile lastSelected = gameManager.mapManager.lastSelectedTile;
        
        if(tile == lastSelected || tile == lastTileHoover)
            return;

        lastTileHoover = tile;
        
        if (!lastSelected)
        {
            TileHoover(tile);
        }
        else if(lastSelected && tile)
        {
            TilePathHoover(tile);
        }
    }

    void TileSelect(HexTile tile)
    {
        if (!tile)
        {
            gameManager.mapManager.SetSelectedTile(null);
            return;
        }

        HexTile lastSelected = gameManager.mapManager.lastSelectedTile;
        
        if (!lastSelected && tile.HasTeam())
        {
            gameManager.mapManager.SetSelectedTile(tile);
            OnSelected?.Invoke();
        }
        else if (lastSelected && tile != lastSelected)
        {
            gameManager.mapManager.SetFinalPath(tile);
            OnMoveOrder?.Invoke();
        }
        else if (lastSelected && tile == lastSelected && !tile.Excavated())
        {
            gameManager.mapManager.DeselectAll();
            gameManager.TurnDig(tile);
        }
        else
        {
            OnActionDenided.Invoke();
        }
    }
    
    void TileHoover(HexTile tile)
    {
        gameManager.mapManager.SetHooverTile(tile);
    }
    
    void TilePathHoover(HexTile tile)
    {
        gameManager.mapManager.SetPathHooverTile(tile);
    }*/
}
