using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    // Can also add enter/exit functions so that transitioning states dont need to be coupled - pass the player to different states rather than pass states to player
    void Enter(Player player);
    void Execute(Player player);
}

public class Standing: IPlayerState
{
    public void Enter(Player player)
    {
        Debug.Log("Enter Standing");
        player.currentState = this;
        player.transform.localScale = new Vector3(1, 2, 1);
    }
    public void Execute(Player player)
    {
        // Put all key checks into State functions - this gets run every frame the state is active - no need for switch
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Transition to Ducking
            Ducking duck = new Ducking();
            duck.Enter(player);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            // Transition to Ducking
            Sprinting sprint = new Sprinting();
            sprint.Enter(player);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jumping jump = new Jumping();
            jump.Enter(player);
        }
    }

}

public class Ducking : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Ducking");
        player.currentState = this;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.transform.localScale = new Vector3(1, 1, 1);
    }
    public void Execute(Player player)
    {
       // Debug.Log("Ducking");
        // Put all key checks into State functions - this gets run every frame the state is active - no need for switch
        if (Input.GetKeyUp(KeyCode.S))
        {
            // Transition to standing - no need to toggle
            Standing standing = new Standing();
            standing.Enter(player);
        }
    }
}

public class Sprinting : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Sprinting");
        player.currentState = this;
    }
    public void Execute(Player player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(player.speed, 0, 0);
        // Put all key checks into State functions - this gets run every frame the state is active - no need for switch
        if (Input.GetKeyUp(KeyCode.W))
        {
            Standing standing = new Standing();
            standing.Enter(player);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LongJump lJump = new LongJump();
            lJump.Enter(player);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Sliding slide = new Sliding();
            slide.Enter(player);
        }
    }
}

public class LongJump : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Long jump");
        player.currentState = this;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        //rb.transform.localScale = new Vector3(1, 2.25f, 1);
        rb.velocity = new Vector3(player.speed * 1.35f, player.jumpHeight/1.5f,0);
    }
    public void Execute(Player player)
    {
        // To Transition out you can raycast - Make the cast distance really small (.55 past center position so .05f from ground
        if (Physics.Raycast(player.transform.position, Vector3.down, 1f))
        {
            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("Hit Ground");
                Sprinting sprint = new Sprinting();
                sprint.Enter(player);
            } else
            {
                Debug.Log("Hit Ground");
                Standing standing = new Standing();
                standing.Enter(player);
            }
            
        }
    }
}
public class Sliding : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Slide");
        player.currentState = this;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.transform.localScale = new Vector3(1, 1, 1);
        rb.velocity = new Vector3(player.speed * 2.25f, 0 , 0);
    }
    public void Execute(Player player)
    {
        // To Transition out you can raycast - Make the cast distance really small (.55 past center position so .05f from ground
        if (Input.GetKeyUp(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                Sprinting sprint = new Sprinting();
                sprint.Enter(player);
            }
            else
            {
                Standing standing = new Standing();
                standing.Enter(player);
            }
        }
    }
}
public class Diving : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Diving");
        player.currentState = this;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.transform.localScale = new Vector3(1, 2.25f, 1);
        rb.velocity = new Vector3(0, -player.jumpHeight/1.5f, 0);
    }
    public void Execute(Player player)
    {
        // To Transition out you can raycast - Make the cast distance really small (.55 past center position so .05f from ground
        if (Physics.Raycast(player.transform.position, Vector3.down, 1.3f))
        {
            Standing stand = new Standing();
            stand.Enter(player);
        }
    }
}

public class Jumping : IPlayerState
{
    public void Enter(Player player)
    {
        // Avoids the need for Player to communicate after start
        Debug.Log("Enter Jumping");
        player.currentState = this;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        //rb.transform.localScale = new Vector3(1, 2.25f, 1);
        rb.velocity = new Vector3(0, player.jumpHeight, 0);
    }
    public void Execute(Player player)
    {
        // To Transition out you can raycast - Make the cast distance really small (.55 past center position so .05f from ground
        if (Physics.Raycast(player.transform.position, Vector3.down, 1f))
        {
            Debug.Log("Hit Ground");
            Standing stand = new Standing();
            stand.Enter(player);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Diving dive = new Diving();
            dive.Enter(player);
        }
    }
}


