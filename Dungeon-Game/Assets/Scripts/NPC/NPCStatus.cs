using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatus : BaseStatus
{

    private GameObject miniBar, myMiniBar = null;

    protected override void SetUp()
    {
        base.SetUp();
        miniBar = (GameObject)Resources.Load("Prefabs/MiniHealthBar", typeof(GameObject));
    }

    protected override void UIUpdate()
    {
        base.UIUpdate();
        if (myMiniBar != null) myMiniBar.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 1f);
    }

    protected override void DamageUpdate()
    {
        if (myMiniBar != null) Destroy(myMiniBar);
        myMiniBar = Instantiate(miniBar, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 1f), Quaternion.identity);
        myMiniBar.GetComponent<MiniBar>().multiplier = (float)HP / MaxHp;
    }
}
