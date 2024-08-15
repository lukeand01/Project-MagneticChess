using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSystemComponent_2D
{
    //should this be a component? or just a class
    //this 

    Rigidbody2D _rb;
    Transform _body;

    float jumpForce;


    //BUFFER is for the player to be able to press jump before getting to the ground and the game recognizing it still
    float buffer_Current;
    float buffer_Total;

    //HOLD JUMP is for how long the player can hold the jump
    float holdJump_Current;
    float holdJump_Total;

    //JUMP QUANTITY is the amount of jumps you have before topuching the ground
    int jumpQuantity_Total;
    int jumpQuantity_Current;

    //COYOTE is the amount of time you can be off the ground and still jump
    float coyoteTime_Current;
    float coyoteTime_Total;

    LayerMask _groundLayer;

    BoxCollider2D _boxCollider;

    public JumpSystemComponent_2D(Transform _body, Rigidbody2D _rb, float jumpForce, float buffer_Total, float holdJump_Total, int jumpQuantity_Total,
        float coyoteTime_Total, int[] groundLayerIndexArray, BoxCollider2D _boxCollider)
    {
        //everything should be here.
        this._body = _body;
        this._rb = _rb;
        this.jumpForce = jumpForce;
        this.buffer_Total = buffer_Total;
        this.holdJump_Total = holdJump_Total;
        this.jumpQuantity_Total = jumpQuantity_Total;
        this.coyoteTime_Total = coyoteTime_Total;

        foreach (var item in groundLayerIndexArray)
        {
            _groundLayer |= (1 << item);
        }

        this._boxCollider = _boxCollider;
    }



    #region BASE FUNCTIONS FOR JUMPING

    public void JumpStart()
    {

        if (!canJump)
        {
            Debug.Log("buffer");
            buffer_Current = buffer_Total;
            return;
        }

        jumpQuantity_Current++;

        isHoldingJump = true;
        canHold = true;
        canRelease = true;

        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(_body.up * jumpForce, ForceMode2D.Impulse);
    }
    public void JumpHold()
    {
        if (!canHold) return;

        holdJump_Current += Time.fixedDeltaTime;

        if (holdJump_Current > holdJump_Total)
        {
            //if its more it needs to start falling.

            StartFalling();
            return;
        }

        if (holdJump_Current > holdJump_Total * 0.7f)
        {
            //removes gravity.

            _rb.AddForce(_body.up * 0.08f, ForceMode2D.Force);
            return;
        }
    }
    public void JumpRelease()
    {
        if (!canRelease)
        {
            return;
        }

        StartFalling();
    }


    #endregion

    void StartFalling()
    {


        if (_rb.velocity.y > 0)
        {
            _rb.velocity = new Vector3(0, -1f, 0);
        }

        canRelease = false;
        canHold = false;
        isHoldingJump = false;

        return;
    }

    public void ProcessUpdateFixedFunctions()
    {
        isGrounded = IsGrounded();
        canJump = CanJump();


        HandleCoyote();
        HandleGroundedLogic();

    }

    void HandleGroundedLogic()
    {
        //whatever happens when its grounded or when its no longer grounded.
        if (isGrounded)
        {
            jumpQuantity_Current = 0;
            coyoteTime_Current = 0;
            holdJump_Current = 0;
            jumpQuantity_Current = 0;

            if (buffer_Current > 0)
            {
                
                JumpStart();
            }
            buffer_Current = 0;
        }
        else
        {
            HandleCoyote();
        }

    }

    #region JUMP QUANTITIES

    public void SetNewJumpQuantity(int newJumpQuantity)
    {
        jumpQuantity_Total = newJumpQuantity;
    }

    public int GetJumpQuantity { get { return jumpQuantity_Total; } }

    #endregion

    #region COYOTE
    void HandleCoyote()
    {
        if (coyoteTime_Total > coyoteTime_Current)
        {
            coyoteTime_Current += Time.fixedDeltaTime;
        }

    }
    void HandleBufferLogic()
    {
        if (buffer_Current > 0)
        {
            buffer_Current -= Time.fixedDeltaTime;
        }
    }

    #endregion

    #region BOOLEAN

    bool canJump;
    bool canRelease;
    bool canHold;
    bool isHoldingJump;
    bool isGrounded;
    bool CanJump()
    {
        //can jump if you have more 

        if (jumpQuantity_Current >= jumpQuantity_Total)
        {
            //Debug.Log("1");
            return false;
        }


        if (isGrounded)
        {
            //Debug.Log("2");
            return true;
        }
        else
        {
            //Debug.Log("3");
            if (!isHoldingJump && coyoteTime_Current < coyoteTime_Total)
            {
                //Debug.Log("4");
                return true;
            }
            else
            {
                return false;
            }
        }




    }

    bool IsGrounded()
    {

        //use a box as reference to find a especifc type of layer.
        Vector2 direction = _body.right; // The direction of the cast
        Vector2 origin = (Vector2)_body.position + _boxCollider.offset; // Starting point of the cast
        Vector2 size = _boxCollider.size; // Size of the box cast

        // Perform the box cast
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, direction, 0, _groundLayer);

        return hit.collider != null;
    }

    #endregion

}
