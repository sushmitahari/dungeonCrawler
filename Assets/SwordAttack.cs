using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 3;
    Vector2 rightAttackOffset;
    
    // Start is called before the first frame update
    private void Start()
    {
        
        rightAttackOffset = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttackRight(){
        print("Attack right");
        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }
    public void AttackLeft(){
        print("Attack left");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }
    public void StopAttack(){
        swordCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other){
        print("Enter");
        if(other.tag == "Enemy"){
            //Deal damage to the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null){
                enemy.Health -= damage;
            }
        }
    }
}
