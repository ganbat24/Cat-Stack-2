using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisCat : MonoBehaviour
{
    Rigidbody2D rb;
    bool isDraggable = true;

    Vector3 initialPosition = Vector3.zero;

    public event System.Action OnFreedom;

    private void Start() {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private Vector3 offset = Vector3.zero;
    private float lastDownTime = 0f;

    private void OnMouseUp() {
        if(!isDraggable) return;
        if(transform.position.y >= -2){
            foreach(Collider2D collider in GetComponents<Collider2D>()){
                collider.isTrigger = false;
            }
            Destroy(gameObject.GetComponent<CircleCollider2D>());
            rb.gravityScale = 1;
            isDraggable = false;
            OnFreedom?.Invoke();
        }else{
            if(Time.time - lastDownTime < 0.1f){
                transform.Rotate(0, 0, -90);
            }
            transform.position = initialPosition;    
        }
        
    }

    private void OnMouseDown()
    {
        if(!isDraggable) return;
        lastDownTime = Time.time;
        foreach(Collider2D collider in GetComponents<Collider2D>()){
            collider.isTrigger = true;
        }
        rb.gravityScale = 0;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }

    private void OnMouseDrag()
    {
        if(!isDraggable) return;
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }
}
