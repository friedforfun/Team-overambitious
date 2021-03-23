using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : BaseStatus
{
    // NPC unique status stuff here

    private void npcDamage()
    {
        if (myMiniBar != null) Destroy(myMiniBar);
        myMiniBar = Instantiate(miniBar, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 1f), Quaternion.identity);
        myMiniBar.GetComponent<MiniBar>().multiplier = (float)HP / MaxHp;
    }
}
