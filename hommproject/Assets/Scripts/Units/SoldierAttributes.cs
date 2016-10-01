using UnityEngine;
using System.Collections;

public class SoldierAttributes : AttributesBase {

	private int soldierHealth = 10;
	private int soldierDamage = 2;
	private int soldierSpeed = 4;
	private int soldierRange = 1;

	void Awake(){
		if(tag.Equals("NPC")) { soldierSpeed = 2; }
		base.maxHealth = soldierHealth;
		base.damage = soldierDamage;
		base.speed = soldierSpeed;
		base.attackRange = soldierRange;
	}

	void Start(){
		base.currentHealth = maxHealth;
	}
}
