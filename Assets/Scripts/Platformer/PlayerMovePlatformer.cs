using System.Collections.Generic;
using UnityEngine;

public class PlayerMovePlatformer : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D _rigidbody;
    private bool _isGrounded;
    private Collider2D _сollider;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _сollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        List<Collider2D> colliders = new();
        ContactFilter2D filter = new ContactFilter2D();
        _isGrounded = Physics2D.OverlapCollider(_сollider, filter, colliders) > 0;

        if (Input.GetKey(KeyCode.Space) && _isGrounded)
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
        if (Input.GetKey(KeyCode.LeftArrow))
            _rigidbody.linearVelocity = new Vector2(-moveSpeed, _rigidbody.linearVelocity.y);
        if (Input.GetKey(KeyCode.RightArrow))
            _rigidbody.linearVelocity = new Vector2(moveSpeed, _rigidbody.linearVelocity.y);
    }
}