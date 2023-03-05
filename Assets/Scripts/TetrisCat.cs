using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisCat : MonoBehaviour
{
    Rigidbody2D rb;
    bool isDraggable = true;
    List<BoxCollider2D> colliders;

    Vector3 initialPosition = Vector3.zero;

    public event System.Action OnFreedom;

    private void Start() {
        initialPosition = transform.localPosition;
        rb = GetComponent<Rigidbody2D>();
        colliders = new List<BoxCollider2D>(GetComponents<BoxCollider2D>());
        GameManager.onGamePause += OnPause;
        GameManager.onGameUnPause += OnUnPause;
        GameManager.onGameStart += DestroyCat;
    }

    private void Update() {
        if(!isDraggable && Height() < -5){
            GameManager.DeadCat();
            DestroyCat();
        }
    }

    void DestroyCat(){
        GameManager.freeCats.Remove(this);
        GameManager.onGamePause -= OnPause;
        GameManager.onGameUnPause -= OnUnPause;
        GameManager.onGameStart -= DestroyCat;
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(Vector3.up * Lowth() + Vector3.right * transform.position.x, Vector3.up * Height() + Vector3.right * transform.position.x);
    }

    private Vector3 offset = Vector3.zero;

    public float Height(){
        float ans = transform.position.y;
        foreach(BoxCollider2D collider in colliders){
            ans = Mathf.Max(collider.bounds.center.y + collider.bounds.extents.y, ans);
        }
        return ans;
    }
    public float Lowth(){
        float ans = transform.position.y;
        foreach(BoxCollider2D collider in colliders){
            ans = Mathf.Min(collider.bounds.center.y - collider.bounds.extents.y, ans);
        }
        return ans;
    }

    public float getSpeed(){
        return rb.velocity.magnitude;
    }

    void Freedom(){
        transform.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        transform.parent = null;
        foreach(Collider2D collider in GetComponents<Collider2D>()){
            collider.isTrigger = false;
        }
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        rb.gravityScale = 1;
        isDraggable = false;
        OnFreedom?.Invoke();
    }

    void OnUnPause(){
        rb.simulated = true;
    }

    void OnPause(){
        if(isDraggable) transform.localPosition = initialPosition;
        rb.simulated = false;
    }

    private void OnMouseUp() {
        if(GameManager.currentState != GameState.Game) return;
        if(!isDraggable) return;
        if(Lowth() >= Camera.main.transform.position.y-2.75){
            Freedom();
        }else{
            if(Vector3.Distance(initialPosition, transform.localPosition) < 0.5f){
                transform.Rotate(0, 0, -90);
            }
            transform.localPosition = initialPosition;    
        }
    }

    private void OnMouseDown()
    {
        if(GameManager.currentState != GameState.Game) return;
        if(!isDraggable) return;
        foreach(Collider2D collider in GetComponents<Collider2D>()){
            collider.isTrigger = true;
        }
        rb.gravityScale = 0;
        offset = Camera.main.transform.position +  gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }

    private void OnMouseDrag()
    {
        if(GameManager.currentState != GameState.Game) return;
        if(!isDraggable) return;
        Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
        transform.localPosition = Camera.main.ScreenToWorldPoint(newPosition) + offset - Camera.main.transform.position;
    }

    bool freeCat = false;
    private void OnCollisionStay2D(Collision2D other) {
        if(GameManager.currentState != GameState.Game) return;
        if(!freeCat){
            GameManager.freeCats.Add(this);
            freeCat = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(GameManager.currentState != GameState.Game) return;
        //has 5percent chance of meowing when collided
        if(Random.Range(0, 100) < 5) {
            AudioManager.Meow();
        }
    }

}
