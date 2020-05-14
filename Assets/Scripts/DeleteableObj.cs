using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteableObj : MonoBehaviour
{
    void FixedUpdate()
    {
        /// este script se encarga de eliminar el objeto que lo este usando al final de la ronda
        if (PlayerManager.Instance.DeleteProps == true) {
            Destroy(gameObject);
            //Debug.Log("DeletedProp");
        }
    }
}
