using UnityEngine;
using System.Collections;

public class ClearSight : MonoBehaviour
{
    public float DistanceToPlayer = 5.0f;
    void Update()
    {
        RaycastHit[] hits;
        // you can also use CapsuleCastAll()
        // TODO: setup your layermask it improve performance and filter your hits.
        hits = Physics.RaycastAll(transform.position, transform.forward, DistanceToPlayer,1<<1);
        foreach(RaycastHit hit in hits)
        {
			Debug.Log("hit detected"+hit.collider.gameObject.name);
            Renderer R = hit.collider.renderer;
            if (R == null)
                continue; // no renderer attached? go to next hit
            // TODO: maybe implement here a check for GOs that should not be affected like the player

            AutoTransparent AT = R.GetComponent<AutoTransparent>();
            if (AT == null) // if no script is attached, attach one
            {
//				Debug.Log("AT entered");
                AT = R.gameObject.AddComponent<AutoTransparent>();
            }
            AT.BeTransparent(); // get called every frame to reset the falloff
        }
    }
}