using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public bool moverMode;
    private float gridSize;
    private Camera _camera;
    private GameObject movingObject;
    private bool _showedUp;
    private Color _originalColor;
    private Collider2D _collider2D;
    private bool _canBuild;
    private bool isTrigger;

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

    private void Awake()
    {
        gridSize = 0.32f;
        _camera = Camera.main;
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
                    var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Vector2.zero);

                    if (hit.collider != null)
                    {
                        movingObject = hit.collider.gameObject;
                        isTrigger = hit.collider;
                        _collider2D = hit.collider;
                    }
                }

                CreateBlueprintOfMovingObject(hitX, hitY);
                IsItAvailableToReplace();
            }
        }
    }

    private void CreateBlueprintOfMovingObject(float hitX, float hitY)
    {
        Cursor.visible = false;
        if (_showedUp == false)
        {
            _showedUp = true;
        }

        _collider2D.isTrigger = true;
        movingObject.transform.position = new Vector2(hitX, hitY);
        movingObject.GetComponent<SpriteRenderer>().color = _canBuild ? Color.green : Color.red;
        ReplaceTheSelectedObject(hitX, hitY);
    }

    public void TurnOnMoverMode()
    {
        moverMode = true;
    }

    private void IsItAvailableToReplace()
    {
        _canBuild = !CheckCollisions(_collider2D, movingObject.transform.position, 10f);
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
                _collider2D.isTrigger = isTrigger;
                //_buildingDeskController.CloseBuildingDesk();
                Cursor.visible = true;
                _showedUp = false;
                movingObject = null;
            }
        }
    }
}