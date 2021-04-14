using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")] 
    [SerializeField][Range(2.0f, 15.0f)] private float _speed = 4.0f;
    [SerializeField][Range(0.2f, 1.0f)] private float _laserOffset;

    [Header("Position Data")] 
    private Transform _playerSpawnPosition;
    private Transform _leftPlayerBorder;
    private Transform _rightPlayerBorder;
    private Transform _topPlayerBorder;
    private Transform _bottomPlayerBorder;

    [Header("Game Objects")] 
    [SerializeField] private GameObject _laserPrefab;

    [Header("Laser Cooldown System")]
    [SerializeField] [Range(0.1f, 1.5f)] private float _cooldownTime = 0.25f;
    private float _nextFire = 0.0f;

    // ========================================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
        ResetSpawnPosition();
    }

    private void Update()
    {
        Movement();
        PlayerBorders();
        InstantiateLaser();
    }

    private void FindGameObjects()
    {
        _playerSpawnPosition = GameObject.Find("PlayerSpawnPosition").transform;
        _leftPlayerBorder = GameObject.Find("PlayerBorder_left").transform;
        _rightPlayerBorder = GameObject.Find("PlayerBorder_right").transform;
        _topPlayerBorder = GameObject.Find("PlayerBorder_top").transform;
        _bottomPlayerBorder = GameObject.Find("PlayerBorder_bottom").transform;
    }

    private void NullChecking()
    {
        if (_playerSpawnPosition == null)
        {
            Debug.LogError("'_playerSpawnPosition' is NULL! Have you named the GameObject 'PlayerSpawnPosition'?");
        }

        if (_topPlayerBorder == null)
        {
            Debug.LogError("'_topPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_top'?");
        }

        if (_rightPlayerBorder == null)
        {
            Debug.LogError("'_rightPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_right'?");
        }

        if (_leftPlayerBorder == null)
        {
            Debug.LogError("'_leftPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_left'?");
        }

        if (_bottomPlayerBorder == null)
        {
            Debug.LogError("'_bottomPlayerBorder' is NULL! Have you named the GameObject 'PlayerBorder_bottom'?");
        }
    }

    private void ResetSpawnPosition()
    {
        transform.position = GameObject.Find("PlayerSpawnPosition").transform.position;
    }

    private void Movement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalMovement, verticalMovement, 0) * (_speed * Time.deltaTime));
    }

    private void PlayerBorders()
    {
        var position = transform.position;

        // left border
        if (position.x < _leftPlayerBorder.transform.position.x)
            position = new Vector3(_rightPlayerBorder.transform.position.x, transform.position.y, 0);

        // right border
        if (position.x > _rightPlayerBorder.transform.position.x)
            position = new Vector3(_leftPlayerBorder.transform.position.x, transform.position.y, 0);

        // y-restriction clamping
        position = new Vector3(position.x, Mathf.Clamp(position.y, _bottomPlayerBorder.transform.position.y,
            _topPlayerBorder.transform.position.y), 0);
        transform.position = position;
    }

    private void InstantiateLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            // Cooldown System
            _nextFire = _cooldownTime + Time.time;

            Instantiate(_laserPrefab, transform.position + (_laserPrefab.transform.up * _laserOffset),
                Quaternion.identity);
        }
    }
}
