using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public IPlayerState currentState;
    public float jumpHeight = 20f;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        currentState = new Standing();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Execute(this);
    }
}
