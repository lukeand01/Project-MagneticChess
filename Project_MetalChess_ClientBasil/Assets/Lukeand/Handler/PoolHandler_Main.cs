using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler_Main : MonoBehaviour
{

    private void Awake()
    {
        MagnetPool_SetUp();
    }

    #region MAGNETIC PIECEE
    [SerializeField] Magnetic magnetTemplate;
    ObjectPool<Magnetic> pool_MagnetPiece;
    [SerializeField] Transform magneticPiece_container;

    public void ClearMagnets()
    {
        for (int i = 0; i < magneticPiece_container.childCount; i++)
        {
            magneticPiece_container.GetChild(i).gameObject.SetActive(false);                                                                                                        
        }
    }

    public Magnetic GetMagnet()
    {
        Magnetic newInventoryUnit = pool_MagnetPiece.Get();

        newInventoryUnit.gameObject.name = "MagneticPiece; ID: " + Guid.NewGuid().ToString();   
        newInventoryUnit.gameObject.SetActive(true);
        newInventoryUnit.transform.SetParent(magneticPiece_container);
        magneticPiece_container.name = "MagneticPiece: " + magneticPiece_container.childCount.ToString(); 


        return newInventoryUnit;
    }

    void MagnetPool_SetUp()
    {

        pool_MagnetPiece = new ObjectPool<Magnetic>(Magnet_Create, null, Magnet_ReturnToPool, defaultCapacity: 50);

    }

    void Magnet_ReturnToPool(Magnetic magneticPiece)
    {
        magneticPiece.gameObject.SetActive(false);
    }

    Magnetic Magnet_Create()
    {
        var bullet = Instantiate(magnetTemplate);
        return bullet;
    }

    public void InventoryUnit_Release(Magnetic magneticPiece)
    {
        pool_MagnetPiece.Release(magneticPiece);
    }
    #endregion

}
