using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeAnimator : MonoBehaviour
{
    ForgePlayer forgePlayer;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        forgePlayer = GetComponent<ForgePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!forgePlayer.networkObject.IsOwner)
        {
            anim.SetFloat("GroundDistance"  , forgePlayer.networkObject.GroundDistance);
            anim.SetFloat("InputHorizontal" , forgePlayer.networkObject.InputHorizontal);
            anim.SetFloat("InputVertical"   , forgePlayer.networkObject.InputVertical);
            anim.SetFloat("InputMagnitude"  , forgePlayer.networkObject.InputMagnitude);
            anim.SetBool("IsGrounded"       , forgePlayer.networkObject.IsGrounded);
            anim.SetBool("IsStrafing"       , forgePlayer.networkObject.IsStrafing);
            anim.SetBool("IsSprinting"      , forgePlayer.networkObject.IsSprinting);
            return;
        }

        forgePlayer.networkObject.GroundDistance    = anim.GetFloat("GroundDistance");
        forgePlayer.networkObject.InputHorizontal   = anim.GetFloat("InputHorizontal");
        forgePlayer.networkObject.InputVertical     = anim.GetFloat("InputVertical");
        forgePlayer.networkObject.InputMagnitude    = anim.GetFloat("InputMagnitude");
        forgePlayer.networkObject.IsGrounded        = anim.GetBool("IsGrounded");
        forgePlayer.networkObject.IsStrafing        = anim.GetBool("IsStrafing");
        forgePlayer.networkObject.IsSprinting       = anim.GetBool("IsSprinting");
    }
}
