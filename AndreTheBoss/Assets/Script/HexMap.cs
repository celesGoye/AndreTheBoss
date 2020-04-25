using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{

    //public Material plainMat;
	private Texture[] plainTex;
	private List<Material> plainMat=new List<Material>();
	public Material myMat;

    public int mapWidth = 40;
    public int mapHeight = 30;

    [Range(0, 100)]
    public int swampFactor = 20;
    [Range(0, 100)]
    public int mountFactor = 20;
    [Range(0, 100)]
    public int forestFactor = 20;

    [Range(0, 100)]
    public int thornsSpawnRate = 1;
    [Range(0, 100)]
    public int stoneSpawnRate = 1;

    public HexTypeInfo hexPrefab_forest;
    public HexTypeInfo hexPrefab_swamp;
    public HexTypeInfo hexPrefab_mountain;
    public Indicator indicatorPrefab;
    public Obstacle obstaclePrefab;
    public HexCell hexCellPrefab;

    private HexCell[] cells;
    private HexType[] hexTypes;
    private Indicator[] indicators;

    private List<HexCell> currentRoutes;
    private List<HexCell> reachableCells;
    private List<HexCell> attackableCells;
    private List<HexCell> hiddenCells;

    public int revealRadius = 2;

    public void OnEnable()
    {
        
    }

    public void GenerateCells()
    {
        cells = new HexCell[mapHeight * mapWidth];
        hexTypes = new HexType[mapHeight * mapWidth];
        indicators = new Indicator[mapHeight * mapWidth];
        currentRoutes = new List<HexCell>();
        reachableCells = new List<HexCell>();
        attackableCells = new List<HexCell>();
        hiddenCells = new List<HexCell>();
		CreateMat();
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                HexCell cell = cells[x + z * mapWidth] = Instantiate<HexCell>(hexCellPrefab);
                CreateCell(cell, x, z);
                GenerateHexType(cell, x, z);
                CreateAttachIndicator(cell, x, z);
                GenerateObstacles(cell);
            }
        }

        ConnectNeighbours();
    }

    private void ConnectNeighbours()
    {
        // Setting neighbours
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (x < mapWidth - 1)
                    cells[x + z * mapWidth].SetNeighbour(HexDirection.E, cells[x + z * mapWidth + 1]);
                if (z % 2 == 1 && z < mapHeight - 1)
                {
                    if (x < mapWidth - 1)
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth]);
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth + 1]);
                    }
                    else
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth]);
                    }

                }
                else if (z % 2 == 0 && z < mapHeight - 1)
                {
                    if (x > 0)
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth]);
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NW, cells[x + z * mapWidth + mapWidth - 1]);
                    }
                    else
                    {
                        cells[x + z * mapWidth].SetNeighbour(HexDirection.NE, cells[x + z * mapWidth + mapWidth]);
                    }
                }
            }
        }
    }

    /*
     * Fog of Hexs
     */
    public void HideCells()
    {
        foreach(HexCell cell in hiddenCells)
        {
            cell.gameObject.SetActive(false);
        }
    }

    public void RevealCell(HexCell cell)
    {
        cell.gameObject.SetActive(true);
    }

    public void RevealCellsFrom(HexCell cell)
    {
        Queue<HexCell> revealCells = new Queue<HexCell>();
        revealCells.Enqueue(cell);

        while(revealCells.Count > 0)
        {
            HexCell cellToReveal = revealCells.Dequeue();
            RevealCell(cellToReveal);
            hiddenCells.Remove(cellToReveal);
            for (HexDirection dir = (HexDirection)0; dir <= HexDirection.NW; dir++)
            {
                HexCell nextCell = cellToReveal.GetNeighbour(dir);
                if(nextCell != null && nextCell.DistanceTo(cell) <= revealRadius)
                {
                    if(hiddenCells.Contains(nextCell))
                        revealCells.Enqueue(nextCell);
                }
            }
        }

    }

    public void UpdateHideCells()
    {
        // stub
    }
	
	public void CreateMat()
	{
		plainTex=Resources.LoadAll<Texture>("Environment/GroundTex");
		for(int i=0;i<plainTex.Length;i++)
		{
			Material mat=new Material(Shader.Find("Custom/HexCell"));
			mat.SetTexture("_MainTex",plainTex[i]);
			plainMat.Add(mat);
		}
	}

    private void CreateCell(HexCell cell, int x, int z)
    {
		int ran = Random.Range(0, plainMat.Count);
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.InnerRadius * 2);
        position.y = 0;
        position.z = z * (HexMetrics.OuterRadius * 1.5f);

        cell.InitHexCell();
        cell.GenerateMesh();
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.SetMaterial(plainMat[ran]);
		Debug.Log("cell created");
        cell.coordinate = HexCoordinate.FromOffsetCoordinate(x, z);
        hiddenCells.Add(cell);
    }

    private void GenerateHexType(HexCell cell, int x, int z)
    {
        HexType type = GetRandomHexType();
        hexTypes[x + z * mapWidth] = cell.hexType = type;
        
        if (type != HexType.Plain)
        {
			HexTypeInfo gm =new HexTypeInfo();
			switch(type)
			{
				case HexType.Forest:
					gm = Instantiate(hexPrefab_forest);
					break;
				case HexType.Swamp:
					gm = Instantiate(hexPrefab_swamp);
					break;
				case HexType.Mountain:
					gm = Instantiate(hexPrefab_mountain);
					break;
				
			}
            gm.ChangeType(type);
            gm.gameObject.transform.SetParent(cell.transform);
            gm.gameObject.transform.localPosition = Vector3.zero;
        }
    }

    private void GenerateObstacles(HexCell cell)
    {
        if (cell.hexType == HexType.Mountain)
            return;

        float probability = (float)Random.Range(1, 100);
        if(probability < thornsSpawnRate)
        {
            Obstacle obstacle = Instantiate<Obstacle>(obstaclePrefab);
            cell.obstacle = obstacle;
            obstacle.transform.SetParent(cell.transform);
            obstacle.transform.localPosition = Vector3.zero;
            obstacle.SetType(ObstacleType.Thorns);
        }
        else if(probability < thornsSpawnRate + stoneSpawnRate)
        {
            Obstacle obstacle = Instantiate<Obstacle>(obstaclePrefab);
            cell.obstacle = obstacle;
            obstacle.transform.SetParent(cell.transform);
            obstacle.transform.localPosition = Vector3.zero;
            obstacle.SetType(ObstacleType.Stones);
        }
    }

    private void CreateAttachIndicator(HexCell cell, int x, int z)
    {
        Indicator indicator = cell.indicator = indicators[x + z * mapWidth] = Instantiate(indicatorPrefab);
        indicator.transform.SetParent(cell.transform);
        indicator.SetColor(Indicator.EndColor);
        indicator.transform.localPosition = Vector3.zero;
        indicator.gameObject.SetActive(false);
    }

    public HexType GetRandomHexType()
    {
        float probability = (float)Random.Range(1, 100);
        
        if (probability < swampFactor)
            return HexType.Swamp;
        else if (probability < swampFactor + mountFactor)
            return HexType.Mountain;
        else if (probability < swampFactor + mountFactor + forestFactor)
            return HexType.Forest;
        return HexType.Plain;
    }

    public Vector3 GetCenterPoint()
    {
        Vector3 point;
        point.x = HexMetrics.InnerRadius * 2 * (mapWidth / 2);
        point.y = 0;
        point.z = HexMetrics.OuterRadius * 1.5f * (mapHeight / 2);
        return point;
    }

    private HexCell cellSelected;

    public void SelectHex(Vector3 point)
    {
        UnselectHex();
        HexCoordinate hexCoord = HexCoordinate.FromPosition(transform.InverseTransformPoint(point));
        int index = hexCoord.X + hexCoord.Z * mapWidth + hexCoord.Z / 2;
        cellSelected = cells[index];
        cellSelected.indicator.gameObject.SetActive(true);
        cellSelected.indicator.SetColor(Indicator.StartColor);
    }

    public void UnselectHex()
    {
        if(cellSelected != null)
        {
            cellSelected.indicator.gameObject.SetActive(false);
        }
    }

    public Rect GetBorder()
    {
        Rect borders = new Rect();
        borders.xMin = 0f;
        borders.xMax = HexMetrics.InnerRadius * 2f * mapWidth;
        borders.yMin = 0f;
        borders.yMax = HexMetrics.OuterRadius * 1.5f * mapHeight;
        return borders;
    }

    public void FindPath(HexCell fromCell, HexCell toCell)
    {
        if (fromCell == toCell || !toCell.CanbeDestination())
            return;

        List<HexCell> cellToFind = new List<HexCell>();
        currentRoutes.Clear();

        // Set distance to max value
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
            cells[i].prevCell = null;
        }

        fromCell.Distance = 0;
        cellToFind.Add(fromCell);
        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);
            if (cell == toCell)
                break;

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance;
                    if (nextCell.hexType == HexType.Mountain)
                        continue;
                    else if (nextCell.hexType == HexType.Plain)
                        distance = cell.Distance + 1;
                    else if (nextCell.hexType == HexType.Forest || nextCell.hexType == HexType.Swamp)
                        distance = cell.Distance + 2;


                    nextCell.heuristicDistance = nextCell.DistanceTo(toCell);
                    if (nextCell.Distance == int.MaxValue)
                    {
                        distance += nextCell.heuristicDistance;
                        nextCell.Distance = distance;
                        cellToFind.Add(nextCell);
                        nextCell.prevCell = cell;
                    }
                    else if (nextCell.Distance > distance + nextCell.heuristicDistance)
                    {
                        nextCell.Distance = distance + nextCell.heuristicDistance;
                        nextCell.prevCell = cell;
                    }
                }
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        currentRoutes.Clear();
        if (toCell.prevCell != null)
        {
            HexCell prev = toCell;
            while(prev != fromCell)
            {
                currentRoutes.Insert(0, prev);
                prev = prev.prevCell;
            }
        }
    }

    public void ShowPath(HexCell fromCell, HexCell toCell)
    {
        HideIndicator();
        if (fromCell == null || toCell == null || fromCell == toCell)
            return;
        fromCell.indicator.gameObject.SetActive(true);
        fromCell.indicator.SetColor(Indicator.StartColor);
        toCell.indicator.gameObject.SetActive(true);
        toCell.indicator.SetColor(Indicator.EndColor);

        HexCell prev = toCell.prevCell;
        while(prev != fromCell && prev != null)
        {
            prev.indicator.gameObject.SetActive(true);
            prev.indicator.SetColor(Indicator.RouteColor);
            prev = prev.prevCell;
        }
    }

    public void FindReachableCells(HexCell startCell, int maxDistance)
    {
        List<HexCell> cellToFind = new List<HexCell>();
        
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        startCell.Distance = 0;
        cellToFind.Add(startCell);
        reachableCells.Clear();

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance;
                    if (!nextCell.CanbeDestination())
                        continue;
                    else if (nextCell.hexType == HexType.Plain)
                        distance = cell.Distance + 1;
                    else if (nextCell.hexType == HexType.Forest || nextCell.hexType == HexType.Swamp)
                        distance = cell.Distance + 2;

                    if (nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        if(nextCell.Distance < maxDistance+1)
                            cellToFind.Add(nextCell);
                    }
                    else if (nextCell.Distance > distance)
                    {
                        if (nextCell.Distance > distance)
                        {
                            nextCell.Distance = distance;
                        }
                    }
                    if (cell.Distance <= maxDistance && cell != startCell && cell.hexType != HexType.Mountain && cell.obstacle == null)
                        reachableCells.Add(cell);
                }
            }
            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        reachableCells.Remove(startCell);
    }

    public void ShowReachableCells()
    {
        HideIndicator();
        for (int i = 0; i < reachableCells.Count; i++)
        {
            reachableCells[i].indicator.gameObject.SetActive(true);
            reachableCells[i].indicator.SetColor(Indicator.StartColor);
        }
    }

    public void ProbeAttackTarget(HexCell startCell)
    {
        if (startCell.pawn == null)
            return;

        List<HexCell> cellToFind = new List<HexCell>();

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        Debug.Log("Attack range: " + startCell.pawn.AttackRange);

        startCell.Distance = 0;
        cellToFind.Add(startCell);
        reachableCells.Clear();

        while (cellToFind.Count > 0)
        {
            HexCell cell = cellToFind[0];
            cellToFind.RemoveAt(0);

            for (HexDirection dir = (HexDirection)0; dir <= (HexDirection)5; dir++)
            {
                HexCell nextCell = cell.GetNeighbour(dir);
                if (nextCell != null)
                {
                    int distance = cell.Distance + 1;
                    if(nextCell.Distance == int.MaxValue)
                    {
                        nextCell.Distance = distance;
                        cellToFind.Add(nextCell);
                    }
                }
            }
            if (cell.Distance <= startCell.pawn.AttackRange)
                reachableCells.Add(cell);

            cellToFind.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        }

        reachableCells.Remove(startCell);

        attackableCells.Clear();
        foreach (HexCell cell in reachableCells)
        {
            if (cell.CanbeAttackTargetOf(startCell))
                attackableCells.Add(cell);
        }
    }

    public void ShowAttackCandidates()
    {
        HideIndicator();
        
        for (int i = 0; i < attackableCells.Count; i++)
        {
            attackableCells[i].indicator.gameObject.SetActive(true);
            attackableCells[i].indicator.SetColor(Indicator.AttackColor);
        }
        Debug.Log("Candidates: " + attackableCells.Count);
    }

    public bool IsReachable(HexCell cell)
    {
        return reachableCells.Contains(cell);
    }


    public List<HexCell> GetCurrentRoutes()
    {
        return currentRoutes;
    }

    public void HideIndicator()
    {
        for (int i = 0; i < cells.Length; i++)
            cells[i].indicator.gameObject.SetActive(false);
    }

    public HexCell GetCellFromPosition(Vector3 point)
    {
        HexCoordinate hexCoord = HexCoordinate.FromPosition(transform.InverseTransformPoint(point));
        int index = hexCoord.X + hexCoord.Z * mapWidth + hexCoord.Z / 2;
        return cells[index];
    }

    public HexCell GetRandomCellToSpawn()
    {
        // make it centered
        int ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
        int ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
        HexCell cell = cells[ranX + ranY * mapWidth];
        while (!cell.CanbeDestination())
        {
            ranX = Random.Range(mapWidth / 4, mapWidth / 4 * 3);
            ranY = Random.Range(mapHeight / 4, mapHeight / 4 * 3);
            cell = cells[ranX + ranY * mapWidth];
        }
            return cell;
    }

}
