using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_weapon : MonoBehaviour
{
    public int attackDamage = 20;
	public int enragedAttackDamage = 40;

	public Collider2D[] col;

	public Collider2D[] SpecialAttackCol;
    public AttackData NormalAttack = new AttackData(true, true, 20, 25, 5f, 1f);
	public AttackData EnFlamedNormalAttack = new AttackData(true, false, 35, 45, 1f, 1f);

	public Vector3 attackOffset;
	public LayerMask attackMask;
	public PlayerController player;


	public bool SpecialAttack = false;
	public int attackNumber = 0;
	
	public int attackCombo = 0;
	private Animator animator;
	public Boss boss;
	private void Start() {
		animator = GetComponent<Animator>();
		SpecialAttack = false;
		attackCombo = 0;
		boss = GetComponent<Boss>();
	}
	private void Update() {
		attackNumber = animator.GetInteger("AttackType");
		if (attackCombo == 3)
		{
			attackCombo = 0;
			SpecialAttack = true;
		}
	}
    public void EnableAttackCollider()
    {
		
        col[attackNumber].enabled = true; 
		attackCombo++;
    }

   
    public void DisableAttackCollider()
    {
        col[attackNumber].enabled = false; 
    }

	public void SpecialAttackh1()
	{
		SpecialAttackCol[0].enabled = !SpecialAttackCol[0].enabled;
		
	}

	public void SpecialAttackh2()
	{
		SpecialAttackCol[1].enabled = !SpecialAttackCol[1].enabled;
		
	}

	public void play2ndhitSound(){

		if(boss.inFlamed){
			SFXManager.Instance.PlaySound3D("SwordSlashStrong" , animator.gameObject.transform.position);
		} else {
			SFXManager.Instance.PlaySound3D("SwordSlash" , animator.gameObject.transform.position);
		}
	}

}
