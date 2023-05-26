using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject selectedConstruction;
    public GameObject _spawnItem;
    
    [Header("Components")]
    private Collider2D _collider2D;
    private Color _originalColor;
    
    [Header("Variables")]
    public bool builderMode;
    private bool _showedUp;
    public bool _canBuild;
    private float gridSize = 0.32f;
    
    [Header("Classes")] public static BuildingController instance;
    public BuildingDeskController BuildingDeskController;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        instance = this;
    }

    private bool CheckCollisions(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            var hits = new RaycastHit2D[10];
            var filter = new ContactFilter2D();

            var numHits = moveCollider.Cast(direction, filter, hits, distance);

            for (var i = 0; i < numHits; i++)
            {
                if (hits[i].collider.enabled && hits[i].collider.tag != "Room")
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    private void Update()
    {
        if (builderMode)
        {
            Vector2 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            var hitX = Mathf.Round(worldPos.x / gridSize) * gridSize;
            var hitY = Mathf.Round(worldPos.y / gridSize) * gridSize;

            CreateBluePrintOfObject(worldPos, hitX, hitY);

            _spawnItem.GetComponent<SpriteRenderer>().color = _canBuild ? Color.green : Color.red;

            IsItAvailableToBuild();
            PlaceObject(hitX, hitY);
        }
    }

    private void CreateBluePrintOfObject(Vector2 worldPos, float hitX, float hitY)
    {
        Cursor.visible = false;
        if (_showedUp == false)
        {
            _spawnItem = Instantiate(selectedConstruction, worldPos, selectedConstruction.transform.rotation);
            _showedUp = true;
            _originalColor = _spawnItem.GetComponent<SpriteRenderer>().color;
        }


        _spawnItem.transform.position = new Vector2(hitX, hitY);
        _collider2D = _spawnItem.GetComponent<Collider2D>();
        _collider2D.isTrigger = true;
    }

    private void IsItAvailableToBuild()
    {
        if (selectedConstruction.GetComponent<Collider2D>().isTrigger == false)
        {
            _canBuild = !CheckCollisions(_collider2D, _spawnItem.transform.position, 10f);
        }
    }

    private void PlaceObject(float hitX, float hitY)
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0) || !_canBuild) return;
        builderMode = false;
        _spawnItem.transform.position = new Vector2(hitX, hitY);
        _spawnItem.GetComponent<SpriteRenderer>().color = _originalColor;
        _collider2D.isTrigger = selectedConstruction.GetComponent<Collider2D>().isTrigger;

        BuildingDeskController.CloseBuildingDesk();
        Cursor.visible = true;
        _showedUp = false;
    }
}