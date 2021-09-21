using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHandler : MonoBehaviour , ICollision,ITrigger
{

    [SerializeField]ColliderDetector colliderDetector;
    
    // 
    /// <summary>
    /// 
    /// </summary>
    
    // Start is called before the first frame update
    void Start()
    {
        colliderDetector = GetComponent<ColliderDetector>();
        C_Registrar();
    }

    void SetPositionToTop()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = new Vector3(10,1,10);
    } 
    
    IEnumerator SetPositionToTopWtihDelay()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Rigidbody>().isKinematic = true;
        transform.position = new Vector3(10,3,10);
    }

    public void C_Registrar()
    {
        colliderDetector.RegisterForCollision("Finish", collision => SetPositionToTop());
        colliderDetector.RegisterForCollision("Finish", collision => StartCoroutine(SetPositionToTopWtihDelay()));
    } 
    
    public void T_Registrar()
    {
        
    }

}
