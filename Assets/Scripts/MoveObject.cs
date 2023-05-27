using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public bool moverMode;
    private float gridSize = 0.32f;
    private Camera _camera;
    private GameObject movingObject;
    private bool _showedUp;
    private Color _originalColor;
    private bool _canBuild;
    private BuildingDeskController[] _buildingDeskControllers;
    private bool objectSelected;

    private void Awake()
    {
        _camera = Camera.main;
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
        if (moverMode)
        {
            Vector2 worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            var hitX = Mathf.Round(worldPos.x / gridSize) * gridSize;
            var hitY = Mathf.Round(worldPos.y / gridSize) * gridSize;
            if (moverMode)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition),
                        Vector2.zero);

                    if (hit.collider != null)
                    {
                        objectSelected = true;
                        movingObject = hit.collider.gameObject;
                        Cursor.visible = false;
                    }
                }

                if (objectSelected)
                {
                    CreateBlueprintOfMovingObject(hitX, hitY);
                    if (_showedUp)
                    {
                        IsItAvailableToReplace();
                    }
                }
            }
        }
    }

    private void CreateBlueprintOfMovingObject(float hitX, float hitY)
    {
        if (_showedUp == false)
        {
            _showedUp = true;
        }

        movingObject.GetComponent<Collider2D>().isTrigger = true;
        movingObject.transform.position = new Vector3(hitX, hitY, 0f);
        movingObject.GetComponent<SpriteRenderer>().color = _canBuild ? Color.green : Color.red;
        ReplaceTheSelectedObject(hitX, hitY);
    }

    public void TurnOnMoverMode()
    {
        moverMode = true;
    }

    private void IsItAvailableToReplace()
    {
        _canBuild = !CheckCollisions(movingObject.GetComponent<Collider2D>(), movingObject.transform.position, 10f);
    }

    private void ReplaceTheSelectedObject(float hitX, float hitY)
    {
        if (_showedUp)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _canBuild)
            {
                moverMode = false;
                movingObject.transform.position = new Vector2(hitX, hitY);
                movingObject.GetComponent<SpriteRenderer>().color = Color.white;
                movingObject.GetComponent<Collider2D>().isTrigger = false;
                _buildingDeskControllers = FindObjectsOfType<BuildingDeskController>();
                foreach (var t in _buildingDeskControllers)
                {
                    if (t._opened)
                    {
                        t.CloseBuildingDesk();
                    }
                }

                Cursor.visible = true;
                _showedUp = false;
                movingObject = null;
                _canBuild = false;
                objectSelected = false;
            }
        }
    }
}