using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] LineRenderer line;
    [SerializeField] float lineDuration = 0.05f;
    [SerializeField] float lineRange = 600f;
    [SerializeField] WorldBehaviors worldBehaviors;

    public bool ShootGun()
    {
        if (Physics.Raycast(spawnPoint.position, transform.forward, out RaycastHit hit, lineRange))
        {
            line.enabled = true;
            line.SetPosition(0, spawnPoint.position);
            line.SetPosition(1, hit.point);

            if (hit.transform.gameObject.tag == "Monster")
            {
                StartCoroutine(ShootLine());
                worldBehaviors.spawnedMonsterList.Remove(hit.transform.parent.GetComponent<MonsterController>());
                Destroy(hit.transform.parent.gameObject);
                return true;
            }
            else if (hit.collider.gameObject.tag == "Wall")
            {
                StartCoroutine(ShootLine());
                return false;
            }
        }
        StartCoroutine(ShootLine());
        return false;
    }

    private IEnumerator ShootLine()
    {
        yield return new WaitForSeconds(lineDuration);
        line.enabled = false;
    }
}
