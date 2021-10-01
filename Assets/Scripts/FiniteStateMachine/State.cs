using UnityEngine;

public abstract class State
{
    public virtual void Exit() { }
    public virtual void Enter() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }

    public virtual void OnCollisionExit2D(Collision2D collision) { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }

    public virtual void OnTriggerEnter2D(Collider2D collider) { }

    public virtual void OnTriggerExit2D(Collider2D collider) { }
}
