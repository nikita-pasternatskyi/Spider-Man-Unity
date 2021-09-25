using UnityEngine;

public abstract class State
{
    public abstract void Exit();
    public abstract void Enter();
    public abstract void FixedUpdate();
    public abstract void LogicUpdate();

    public virtual void OnCollisionExit2D(Collision2D collision) { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }

    public virtual void OnTriggerEnter2D(Collider2D collider) { }

    public virtual void OnTriggerExit2D(Collider2D collider) { }
}