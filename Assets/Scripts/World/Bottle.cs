using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public List<Rigidbody> all = new List<Rigidbody>();

    public void Crash()
    {
        foreach(Rigidbody rb in all)
        {
            rb.isKinematic = false;
            //  da ovde nismo pozvali na false, sudari sa metkom i igracem ne bi imali uticaj na flase
            //  odleteli bi
        }
    }
}
