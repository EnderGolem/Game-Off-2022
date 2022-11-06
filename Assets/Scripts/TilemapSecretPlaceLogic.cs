using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSecretPlaceLogic : MonoBehaviour
{
    private Tilemap map;
    private void Awake()
    {
        map = GetComponent<Tilemap>();
    }
    //При столкновении игрока с тайлом удаляем этот тайл и все связанные с ним
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var  pos = map.WorldToCell(collision.transform.position);
            Clear(new Vector3Int(pos.x+1, pos.y));
            Clear(new Vector3Int(pos.x-1, pos.y));
            Clear(new Vector3Int(pos.x, pos.y+1));
            Clear(new Vector3Int(pos.x, pos.y-1));
        }
    }
    //Метод очистки карты от множества тайлов
    private void Clear(Vector3Int pos)
    {
        if (map.GetTile(pos)!=null)
        {
            map.SetTile(pos, null);
            Clear(new Vector3Int(pos.x + 1, pos.y));
            Clear(new Vector3Int(pos.x - 1, pos.y));
            Clear(new Vector3Int(pos.x, pos.y + 1));
            Clear(new Vector3Int(pos.x, pos.y - 1));
        }
    }
}
