using UnityEngine;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("Components")] private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private RigidbodyConstraints2D _rigidbodyConstraints2D;

        [Header("Animation Values")] private static readonly int WalkingX = Animator.StringToHash("walkingX");
        private static readonly int WalkingY = Animator.StringToHash("walkingY");

        [Header("Classes")] private BuildingController _buildingController;

        [Header("Movement Values")] [SerializeField]
        private float moveSpeed;

        private void Awake()
        {
            _buildingController = GetComponent<BuildingController>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!_buildingController.builderMode)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.None;
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                Walk();

                PlayWalkingAnimation();

                DetermineDirection();
            }
            else
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                _animator.SetFloat(WalkingX, 0f);
                _animator.SetFloat(WalkingY, 0f);
            }
        }

        private void Walk()
        {
            var velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, _rigidbody.velocity.y);
            _rigidbody.velocity = velocity;
            _rigidbody.velocity = new Vector2(velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
        }

        private void PlayWalkingAnimation()
        {
            _animator.SetFloat(WalkingY, _rigidbody.velocity.y);

            if (_rigidbody.velocity.x < 0)
            {
                _animator.SetFloat(WalkingX, -_rigidbody.velocity.x);
            }
            else
            {
                _animator.SetFloat(WalkingX, _rigidbody.velocity.x);
            }
        }

        private bool DetermineDirection()
        {
            return _spriteRenderer.flipX = _rigidbody.velocity.x switch
            {
                < 0 => true,
                0 => _spriteRenderer.flipX,
                _ => false
            };
        }
    }
}