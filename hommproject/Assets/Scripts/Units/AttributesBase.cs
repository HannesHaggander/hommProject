using UnityEngine;
using System.Collections;

public class AttributesBase : MonoBehaviour {

	public int maxHealth = 1;
	public int currentHealth = 1;
	public int damage = 1;
	public int speed = 1;
	public int attackRange = 1;
	
	public int combatId = -1;
	
	public bool isMyTurn = false;

	AStarPathfinding pathfinding = null;

	void Update(){
		if(isMyTurn){
			if(Input.GetKeyDown(KeyCode.J)){
				getPath();
			}
			if(Input.GetKeyDown(KeyCode.P)){
				followPath();
			}
			if(Input.GetKeyDown(KeyCode.A)){
				print("trying to attack");
				Collider[] cols = Physics.OverlapBox(transform.position, new Vector3(2,2,2), Quaternion.identity, 1 << LayerMask.NameToLayer("Unit"));
				if(cols.Length > 0){
					foreach(Collider c in cols){
						if(c == gameObject){
							print("dont hit yourself stupid");
						} 
						else if(inAttackRange(c.gameObject)){
							print(gameObject.name + " dealt damage to " + c.gameObject.name);
							DealDamage(c.gameObject);
						}						
					}
				}
			}
		}
	}

	protected void setAttributes(int maxHealth, int damage, int speed, int attackRange){
		this.maxHealth = maxHealth;
		this.damage = damage;
		this.speed = speed;
		this.attackRange = attackRange;
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
			print("currentHealth: " + currentHealth + " after damage " + (currentHealth - dmgValue));
			currentHealth -= dmgValue;
			isDead();
		}
	}

	public virtual void isDead(){
		if(currentHealth <= 0){
			print(transform.root.name + " is dead");
			GameObject combatManager = GameObject.FindGameObjectWithTag("CombatManager");
			if(combatManager != null){
				combatManager.GetComponent<CombatManager>().RemoveUnitFromList(gameObject);
				
			}
			Destroy(gameObject);
		}
	}

	protected bool inAttackRange(GameObject target){
		return (Vector3.Distance(transform.transform.position, target.transform.position) >= attackRange);
	
	}

	public int getSpeed(){
		return speed;
	}

	public void SetMyTurn(bool b){
		isMyTurn = b;
	}

	public void getPath(){
		if(pathfinding == null){
			pathfinding = GetComponent<AStarPathfinding>();
		}
		if(isMyTurn){
			pathfinding.CalculateEntirePath();
			pathfinding.printPath(pathfinding.entirePath);
		} 
		else {
			foreach(Transform t in transform){
				GameObject.Destroy(t.gameObject);
			}
		}
	}

	public void followPath(){
		if(!isMyTurn){ return; }
		for(int i = 0; i < speed; i++){
			if(i > pathfinding.entirePath.Count-1){
				break;
			}
			transform.position = (Vector3) pathfinding.entirePath[i];
		}
	}
}
