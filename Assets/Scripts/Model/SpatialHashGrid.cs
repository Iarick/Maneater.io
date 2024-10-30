using System.Collections.Generic;
using UnityEngine;


public class SpatialHashGrid : ICreatureObserver
{
    private readonly Dictionary<Vector2Int, List<Creature>> grid = new Dictionary<Vector2Int, List<Creature>>();
    private readonly float cellSize;

    public SpatialHashGrid(float cellSize)
    {
        this.cellSize = cellSize;
    }
    private void OnLifeEventHandler(Creature creature)
    {
        Vector2Int cellPosition = GetCellPosition(creature.transform.position);
        if(cellPosition != creature.cellPosition)
        {
            Remove(creature, creature.cellPosition);
            Insert(creature, cellPosition);
            creature.cellPosition = cellPosition;
            CheckForEnemiesInCell(cellPosition);
        }
    }
    private void CheckForEnemiesInCell(Vector2Int cellPosition)
    {
        // Получаем список существ в текущей ячейке и всех соседних ячейках
        List<Creature> creaturesInCurrentAndNeighborCells = new List<Creature>();
        var neighborPositions = GetNeighborPositions(cellPosition);
        foreach (var position in neighborPositions)
        {
            if (grid.TryGetValue(position, out List<Creature> creaturesInCell))
            {
                creaturesInCurrentAndNeighborCells.AddRange(creaturesInCell);
            }
        }

        // Проверка наличия существ с разными id в одной ячейке
        var distinctIds = new HashSet<int>();
        foreach (var creature in creaturesInCurrentAndNeighborCells)
        {
            distinctIds.Add(creature.id);
        }

        // Если в ячейке более одного уникального id, вызываем EnemyDetected
        if (distinctIds.Count > 1)
        {
            foreach (var creature in creaturesInCurrentAndNeighborCells)
            {
                List<Creature> enemies = creaturesInCurrentAndNeighborCells.FindAll(c => c.id != creature.id);
                if (enemies.Count > 0)
                {
                    if (creature != null)
                    {
                        creature.EnemyDetected(enemies);
                    }
                }
            }
        }
    }

    private void Insert(Creature obj, Vector2Int cellPosition)
    {
        // Помещаем существо только в текущую ячейку
        if (!grid.ContainsKey(cellPosition))
        {
            grid[cellPosition] = new List<Creature>();
        }
        grid[cellPosition].Add(obj);
    }

    private void Remove(Creature obj, Vector2Int cellPosition)
    {
        // Удаляем существо только из текущей ячейки
        if (grid.ContainsKey(cellPosition))
        {
            grid[cellPosition].Remove(obj);
        }
    }

    private Vector2Int GetCellPosition(Vector3 position)
    {
        int cellX = Mathf.FloorToInt(position.x / cellSize);
        int cellY = Mathf.FloorToInt(position.z / cellSize);
        return new Vector2Int(cellX, cellY);
    }
    private List<Vector2Int> GetNeighborPositions(Vector2Int cellPosition)
    {
        List<Vector2Int> neighborPositions = new List<Vector2Int>();
    
        for (int x = cellPosition.x - 1; x <= cellPosition.x + 1; x++)
        {
            for (int y = cellPosition.y - 1; y <= cellPosition.y + 1; y++)
            {
                neighborPositions.Add(new Vector2Int(x, y));
            }
        }
    
        return neighborPositions;
    }

    public void AddObserverTo(Creature creature)
    {
        creature.OnLife += OnLifeEventHandler;
        creature.OnDestroyed += () => Remove(creature, creature.cellPosition);
    }
    public List<Creature> GetAllCreatures()
    {
        List<Creature> creatures = new List<Creature>();
        foreach (var creatureList in grid.Values)
        {
            creatures.AddRange(creatureList);
        }
        return creatures;
    }
}
