using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAttack : BasicAttack
{

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform launchPoint;

    private GameObject[] players;

    // Line of sight layermask
    // 8 is Ground layer, 9 is Wall layer, 11 is enemy layers
    private LayerMask losMask = (1 << 8) | (1 << 9) | (1 << 11);

    public override void PerformAttack(Vector3 direction, float attackPower)
    {
        GameObject newProjectile = Instantiate(projectile, launchPoint.position, Quaternion.LookRotation(direction, Vector3.up));
        newProjectile.GetComponent<IProjectile>().Fire(direction, attackPower, gameObject, null);
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void Update()
    {
        foreach (GameObject player in players)
        {
            if ((player.transform.position - gameObject.transform.position).magnitude < 10f)
            {
                if (LineOfSightCheck(player))
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, player.transform.position - transform.position, 0.8f * Time.deltaTime, 0.0f));
                    //transform.LookAt(player.transform);
                    
                    if (Vector3.Dot(transform.forward, (player.transform.position - transform.position).normalized) >= 0.6f)
                        Attack(player.transform.position - gameObject.transform.position);
                }
            } 
            
        }
    }

    /// <summary>
    /// Check if other is in line of sight, uses layer mask for ground and wall check only (other hostiles will not block LoS with this check)
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool LineOfSightCheck(GameObject other)
    {
        Vector3 losCheckPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z); // point to check line of sight from
        Vector3 otherOffset = new Vector3(other.transform.position.x, other.transform.position.y + 0.9f, other.transform.position.z);
        Vector3 directionToOther = otherOffset - losCheckPoint;
        Debug.DrawRay(losCheckPoint, directionToOther, Color.cyan);
        RaycastHit hit;
        Ray los = new Ray(losCheckPoint, directionToOther);
        if (Physics.Raycast(los, out hit, losMask))
        {
            //Debug.Log($"Hit name: {hit.transform.name}");
            //Debug.Log($"Other name: {other.transform.name}");
            if (hit.transform.name == other.transform.name)
                return true;

        }
        return false;
    }

}
