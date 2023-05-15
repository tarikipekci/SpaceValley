using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public bool builderMode;
    public GameObject selectedConstruction;
    private bool _showedUp;
    private GameObject _spawnItem;
    private Color _originalColor;
    private Collider2D _collider2D;
    public bool _canBuild;
    private float gridSize = 0.64f;
    [Header("Classes")] public static BuildingController instance;
    public BuildingDeskController BuildingDeskController;

    private void Awake()
    {
        instance = this;
    }

    private bool CheckCollisions(Collider2D moveCollider, Vector2 direction, float distance)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            ContactFilter2D filter = new ContactFilter2D();

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
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hitX = Mathf.Round(worldPos.x / gridSize) * gridSize;
            var hitY = Mathf.Round(worldPos.y / gridSize) * gridSize;

            CreateBluePrintOfObject(worldPos, hitX, hitY);

            if (_canBuild)
            {
                _spawnItem.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                _spawnItem.GetComponent<SpriteRenderer>().color = Color.red;
            }

            PlaceObject(hitX, hitY);
            IsItAvailableToBuild();
        }
    }

    private void CreateBluePrintOfObject(Vector2 worldPos, float hitX, float hitY)
    {
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
        if (CheckCollisions(_collider2D, _spawnItem.transform.position, 10f))
        {
            _canBuild = false;
        }
        else
        {
            _canBuild = true;
        }
    }

    private void PlaceObject(float hitX, float hitY)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canBuild)
        {
            builderMode = false;
            _spawnItem.transform.position = new Vector2(hitX, hitY);
            _spawnItem.GetComponent<SpriteRenderer>().color = _originalColor;
            _collider2D.isTrigger = false;
            BuildingDeskController.CloseBuildingDesk();
        }
    }
}