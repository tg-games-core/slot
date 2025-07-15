using System;
using Core;
using Project.Bounce.Settings;
using UnityEngine;

namespace Project.Bounce
{
    public class PlinkoDice : PooledBehaviour
    {
        private const float FallThreshold = -5f;
        
        public event Action Bounced;

        [SerializeField]
        private DiceSettings _diceSettings;
        
        [SerializeField, Header("Настройки кубика")]
        private Color _normalColor = Color.white;
        
        [SerializeField]
        private Color _fallingColor = Color.red;

        private bool _isFalling;
        
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            if (_rigidbody != null)
            {
                _rigidbody.mass = _diceSettings.Mass;
                _rigidbody.linearDamping = _diceSettings.LinearDrag;
                _rigidbody.angularDamping = _diceSettings.AngularDrag;
                _rigidbody.gravityScale = _diceSettings.GravityScale;
                _rigidbody.freezeRotation = _diceSettings.FreezeRotation;

                _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                if (!_diceSettings.FreezeRotation)
                {
                    _rigidbody.angularVelocity =
                        UnityEngine.Random.Range(-_diceSettings.RotationSpeed, _diceSettings.RotationSpeed);
                }
            }

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = _normalColor;
            }
        }

        private void Update()
        {
            if (_rigidbody != null && _rigidbody.linearVelocity.magnitude > _diceSettings.MaxSpeed)
            {
                _rigidbody.linearVelocity = _rigidbody.linearVelocity.normalized * _diceSettings.MaxSpeed;
            }

            CheckFallingState();

            if (!_diceSettings.FreezeRotation && _rigidbody != null)
            {
                transform.Rotate(0, 0, _diceSettings.RotationSpeed * Time.deltaTime);
            }
        }

        private void CheckFallingState()
        {
            bool wasFalling = _isFalling;
            _isFalling = transform.position.y < FallThreshold;

            if (_isFalling != wasFalling && _spriteRenderer != null)
            {
                _spriteRenderer.color = _isFalling ? _fallingColor : _normalColor;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_rigidbody != null)
            {
                Vector2 randomImpulse = new Vector2(
                    UnityEngine.Random.Range(-0.5f, 0.5f),
                    UnityEngine.Random.Range(0.1f, 0.3f)
                );
                _rigidbody.AddForce(randomImpulse, ForceMode2D.Impulse);
            }

            if (collision.gameObject.TryGetComponent(out CircleController _))
            {
                Bounced?.Invoke();
            }
        }
        
        public void DestroyDice()
        {
            Debug.Log("Уничтожаем кубик: " + gameObject.name);
        
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);

            Gizmos.color = Color.red;
            Vector3 fallLine = new Vector3(transform.position.x, FallThreshold, transform.position.z);
            Gizmos.DrawLine(fallLine + Vector3.left * 2, fallLine + Vector3.right * 2);
        }
    }
}