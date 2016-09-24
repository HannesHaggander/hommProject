using UnityEngine;
using System.Collections;

public class AttributesBase : MonoBehaviour {

	protected int maxHealth = 1;
	protected int currentHealth = 1;
	protected int damage = 1;
	protected int speed = 1;
	protected int attackRange = 1;

	void Update(){

	}

	protected void DealDamage(GameObject target){
		/*if(target == null){
			print(target.transform.root.name + " is null");
			return;
		}
		if(target.GetComponent<AttributesBase>() == null){
			print(target.transform.root.name + " is missing an attribute class");
			return;
		}

		AttributesBase attributes = target.GetComponent<AttributesBase>();
		attributes.TakeDamage(damage);*/
		if(target != null){
			target.SendMessage("TakeDamage", damage);
		}
	}


	public void TakeDamage(int dmgValue){
		if(dmgValue >= 0){
			currentHealth -= dmgValue;
			isDead();
		}
	}

	public virtual void isDead(){
		if(currentHealth <= 0){
			print(transform.root.name + " is dead");
			Destroy(gameObject);
		}
	}

	protected bool inAttackRange(GameObject target){
		return (Vector3.Distance(transform.transform.position, target.transform.position) >= attackRange);
	
	}
}
