using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverHandler : MonoBehaviour
{

    public Action<Magnetic> eventAMagneticCollided; //we call from the magnetic itself that collided.
    public void OnAMagneticPieceCollided(Magnetic magneticPiece) => eventAMagneticCollided?.Invoke(magneticPiece);



}
