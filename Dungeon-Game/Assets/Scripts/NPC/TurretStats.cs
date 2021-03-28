using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStats : NPCStatus
{
    protected override void SetUp()
    {
        base.SetUp();
        OnDeath += () => { StartCoroutine(Death()); };
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
