using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstoryMoveSpaceship : MonoBehaviour
{
    private readonly float moveSpeed = 100.0f;
    private readonly float maxDistance = 2500.0f;

    private void OnEnable()
    {
        StartCoroutine(MoveSpaceship());
    }

    private IEnumerator MoveSpaceship()
    {
        while (true)
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
            
            // check if reached maximum distance
            if (transform.position.z > maxDistance)
            {
                gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
}
