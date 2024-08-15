using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using UnityEngine;


public class Magnetic : MonoBehaviour
{
    //THIS IS EVERY PIECE. 
    

    [Separator("GRAPHIC")]
    [SerializeField] GameObject graphicHolder; //this holds the actual graphical aprt of the magnetic.
    [SerializeField] GameObject[] graphicArray;

    [Separator("GRAVITY VARIABLES")]
    [SerializeField] float pullForce;
    [SerializeField] float pullRange;

    [field:SerializeField] public bool shouldBePulled { get; private set; }//this is false if it already touched a magnetic, or if its in the player´s hand.
    bool canBeInteracted;

    public void ControlCollider(bool control)
    {
         myCollider.isTrigger = !control;      
    }

    BoxCollider myCollider;


    LayerMask targetLayer;

    Rigidbody rb;
    [field: SerializeField] GameObject targetMagnetic;

    [SerializeField] bool pullAtStart;

    Vector3 originalPos; 



    private void Start()
    {
        if(pullAtStart)
        {
            SetTable();
        }
    }

    public void SetHand(Vector3 pos)
    {
        transform.position = pos;   

        shouldBePulled = false;
        canBeInteracted = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        SetOriginalPos(pos);
    }
    public void SetTable()
    {
        shouldBePulled = true;
        canBeInteracted = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        GameHandler.instance._singlePlayer.RemoveMagnet(this);
        GameHandler.instance._singlePlayer.CheckIfShouldWin();

    }

    void SetOriginalPos(Vector3 pos)
    {
        originalPos = pos;
    }
    /// <summary>
    ///This is used to change what kind of graphic the magnetic uses. like square or cicle.
    /// </summary>
    public void SetGraphic(MagnetPieceGraphicType _graphicType)
    {
        Debug.Log("graphic type " +  graphicArray.Length);

        for (int i = 0; i < graphicArray.Length; i++)
        {
            var item = graphicArray[i];
            if(i == (int)_graphicType)
            {
                Debug.Log("turned this on");
                item.SetActive(true);
            }
            else
            {
                Debug.Log("turned this off");
                item.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        targetLayer |= (1 << gameObject.layer);
        gameObject.name = "Magnetic Piece; ID: " + Guid.NewGuid().ToString();
        
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {

        if (!shouldBePulled) return;


        if(targetMagnetic != null)
        {
            MoveTowardsTargetMagneticPiece();
        }
        else
        {
            LookForMagneticPiecesInRange();
        }

    }

    void MoveTowardsTargetMagneticPiece()
    {

        //Debug.Log("target  " + targetMagnetic.transform.position);
        //Debug.Log("own " + transform.position);

        Vector3 pullDir = targetMagnetic.transform.position - transform.position;
        float distance = pullDir.magnitude;
        float reducedForce = pullForce * 0.01f;
        Vector3 force = pullDir * reducedForce;
        //Debug.Log("PullDir " + pullDir);
        //Debug.Log("Force " + force);
        rb.AddForce(force, ForceMode.Force);

    }

    void LookForMagneticPiecesInRange()
    {

        RaycastHit[] rayArray = Physics.SphereCastAll(transform.position, pullRange, transform.up, 100, targetLayer);



        for (int i = 0; i < rayArray.Length; i++)
        {
            if (rayArray[i].collider.gameObject.name != gameObject.name)
            {
              
                Magnetic _magnetic = rayArray[i].collider.GetComponent<Magnetic>();

                if (_magnetic == null) continue;

                if (_magnetic.shouldBePulled)
                {
                    targetMagnetic = rayArray[i].collider.gameObject;
                }


            }
        }


        
    }

    public Magnetic TryToGetMagnetPiece()
    {
        if(canBeInteracted)
        {
            return this;
        }

        return null;
    }

    public void PositionToMove(Vector3 pos)
    {
        //it should go up first, then it should move tot arget.
        canBeInteracted = false;
        StartCoroutine(PositionToMoveProcess(pos));

    }
    public void ReturnToOriginalPosition()
    {
        canBeInteracted = false;
        StartCoroutine(PositionToMoveProcess(originalPos));
    }

    IEnumerator PositionToMoveProcess(Vector3 pos)
    {
        float timer_1 = 0.5f;
        transform.DOMoveY(transform.position.y + 0.2f, timer_1);
        yield return new WaitForSeconds(timer_1);
        float timer_2 = 0.5f;
        transform.DOMove(pos, timer_2);
        yield return new WaitForSeconds(timer_2);

        GameHandler.instance._singlePlayer.AddMagnet(this);

        shouldBePulled = false;
        canBeInteracted = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        targetMagnetic = null;
        transform.DOKill();
        StopAllCoroutines();
    }


    //why this behavior?
    //once it returns it should
    //now we need a list to tell if we won
    //and be able to call the timer 

    private void OnCollisionEnter(Collision collision)
    {
        if (!shouldBePulled) return;
        if (collision.gameObject.layer != gameObject.layer) return;



        shouldBePulled = false;

        GameHandler.instance._observer.OnAMagneticPieceCollided(this);
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

}

public enum MagnetPieceGraphicType
{
    Square = 0,
    Circle = 1,
    Star = 2
}