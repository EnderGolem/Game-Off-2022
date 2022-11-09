using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSecretPlaceLogic : MonoBehaviour
{
    [Tooltip("Интервал между итерациями исчезновения тайла")]
    [SerializeField]
    protected float interval;
    [Tooltip("Интенсивность исчезновения")]
    [SerializeField]
    protected float intensity;
    private float curTime = 0;
    private Tilemap map;
    //Множество координат тайлов, подлежащих удалению
    private HashSet<Vector3Int> cells = new HashSet<Vector3Int>();
    private void Awake()
    {
        map = GetComponent<Tilemap>();
        map.color = Color.white;
    }
    //При столкновении игрока с тайлом удаляем этот тайл и все связанные с ним
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            var  pos = map.WorldToCell(collision.transform.position);
            AddCell(new Vector3Int(pos.x+1, pos.y));
            AddCell(new Vector3Int(pos.x-1, pos.y));
            AddCell(new Vector3Int(pos.x, pos.y+1));
            AddCell(new Vector3Int(pos.x, pos.y-1));
        }
    }
    private void Update()
    {
        if (cells.Count>0)
        {
            if (curTime <= 0)
            {
                curTime = interval;
                foreach (var cell in cells)
                {
                    var old = map.GetColor(cell);
                    map.SetColor(cell, new Color(old.r, old.g, old.b, old.a - intensity));
                    if (old.a - intensity <= 0)
                        map.SetTile(cell, null);
                }
                cells.RemoveWhere(x => map.GetTile(x) == null);
            }
            else { curTime -= Time.deltaTime; }
        }
    }
    //Добавление новых тайлов в список на удаление
    private void AddCell(Vector3Int cell)
    {
        if (map.GetTile(cell) !=null)
        {
            cells.Add(cell);
            map.SetTile(cell, map.GetTile(cell));
            map.SetTileFlags(cell, TileFlags.None);
            var v1 = new Vector3Int(cell.x + 1, cell.y);
            var v2 = new Vector3Int(cell.x - 1, cell.y);
            var v3 = new Vector3Int(cell.x, cell.y + 1);
            var v4 = new Vector3Int(cell.x, cell.y - 1);

            if (!cells.Contains(v1)) AddCell(v1);
            if (!cells.Contains(v2)) AddCell(v2);
            if (!cells.Contains(v3)) AddCell(v3);
            if (!cells.Contains(v4)) AddCell(v4);
        }
    }
}
