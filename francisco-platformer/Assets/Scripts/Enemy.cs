using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D enemyCharacter;
    [SerializeField] private float movespeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyCharacter = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingRight())
        {
            enemyCharacter.velocity = new Vector2(movespeed, 0);
        }
        else
        {
            enemyCharacter.velocity = new Vector2(-movespeed, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Reverse the current direction (Scale) of the X-Axis
        transform.localScale = new Vector2(-(Mathf.Sign(enemyCharacter.velocity.x)), 1f);
    }

    bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }
}
