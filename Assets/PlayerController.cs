using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
using System.Threading;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> collisions = new List<RaycastHit2D>();
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }
    // Update is called once per frame
    private void FixedUpdate(){
        //If we're able to move
        if (canMove){
            //If movement input is not 0, try to move
        if (movementInput != Vector2.zero){
            bool success = TryMove(movementInput);
            //If it is 0,
            if (!success){
                //It will try to move only on the x-axis
                success = TryMove(new Vector2(movementInput.x, 0));        
            }
            //If that also doesn't work
            if (!success){
                //It moves only on the y-axis
                success = TryMove(new Vector2(0, movementInput.y));
            }
            animator.SetBool("isMoving", success);
        }
        else{
            animator.SetBool("isMoving", false);
        }
        //Set direction of sprite to movement direction
        if (movementInput.x < 0){
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0){
            spriteRenderer.flipX = false;
        }
        }
        
    }
    private bool TryMove(Vector2 direction){
        if (direction != Vector2.zero){
            int count = rb.Cast(direction, movementFilter, collisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else{
                return false;
            }
        }
        else{
            //Can't move if there's no direction to move in
            return false;
        }
    }
    void OnFire(){
        animator.SetTrigger("swordAttack");
    }
    public void SwordAttack(){
        lockMovement();
        if (spriteRenderer.flipX == true){
            swordAttack.AttackLeft();
        }
        else{
            swordAttack.AttackRight();
        }
    }
    public void EndSwordAttack(){
        unlockMovement();
        swordAttack.StopAttack();
    }
    public void lockMovement(){
        canMove = false;
    }
    public void unlockMovement(){
        canMove = true;
    }
}
