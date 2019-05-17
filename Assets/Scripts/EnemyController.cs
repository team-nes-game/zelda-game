﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    // +-------------+
    // | Public      |
    // +-------------+
    public int MAX_HEALTH;
    public int DAMAGE;
    public float SPEED;
    public Transform TARGET;
    public float MAX_DISTANCE;
    public float MIN_DISTANCE;

    // +-----------+
    // | Private   |
    // +-----------+
    private int CUR_HEALTH;
    private Rigidbody2D SELF;
    private Animator ANIMATOR;
    private float DIST_TO_TAR;
    private float LAST_X, LAST_Y;

    // Start is called before the first frame update
    void Start() {
        SELF        = GetComponent<Rigidbody2D>();
        CUR_HEALTH  = MAX_HEALTH;
        TARGET      = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        ANIMATOR    = GetComponent<Animator>();
        DIST_TO_TAR = Vector2.Distance(transform.position, TARGET.position);
    }

    // Update is called once per frame
    void Update() {
        DIST_TO_TAR = Vector2.Distance(transform.position, TARGET.position);
    }

    void LateUpdate() {
        MoveEnemy();
        Animate(TARGET.position - transform.position);
    }

    void MoveEnemy() {
        if (DIST_TO_TAR > MIN_DISTANCE && DIST_TO_TAR < MAX_DISTANCE) {
            transform.position = Vector2.MoveTowards(transform.position, TARGET.position, SPEED * Time.deltaTime);
            return;
        }

        DIST_TO_TAR = MIN_DISTANCE;
    }

    // This is the same animation from the player, just modified to add the functionality
    // of having to animate more stuff for the enemy
    private void Animate(Vector3 direction) {
        // Idle animation
        if (Vector2.Distance(transform.position, TARGET.position) > MAX_DISTANCE) {
            ANIMATOR.SetFloat("lastX", LAST_X);
            ANIMATOR.SetFloat("lastY", LAST_Y);
            ANIMATOR.SetBool("Moving", false);
            ANIMATOR.SetBool("Attacking", false);
        }
        else if (DIST_TO_TAR == MIN_DISTANCE && DIST_TO_TAR < MAX_DISTANCE) {
            ANIMATOR.SetFloat("lastX", TARGET.position.x - transform.position.x);
            ANIMATOR.SetFloat("lastY", TARGET.position.y - transform.position.y);
            ANIMATOR.SetBool("Moving", false);
            ANIMATOR.SetBool("Attacking", true);
        }
        // Running animation where we set up which direction
        // we were last facing in order to maintain consistent
        // idle animations
        else {
            LAST_X = direction.x;
            LAST_Y = direction.y;
            ANIMATOR.SetBool("Moving", true);
            ANIMATOR.SetBool("Attacking", false);
        }

        ANIMATOR.SetFloat("moveX", direction.x);
        ANIMATOR.SetFloat("moveY", direction.y);
    }

    public void TakeDamage(int damage) {
        CUR_HEALTH -= damage;
        Debug.Log("taking damage");
    }
}
