using UnityEngine;
using System.Collections;

public class AttributesBase : MonoBehaviour {

	public int maxHealth = 1;
	public int currentHealth = 1;
	public int damage = 1;
	public int speed = 1;
	public int attackRange = 1;
	public bool isRange = false;

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
			if(Input.GetMouseButtonDown(0)){
				onTargetEnemy(MasterObject.me.Correctmousepos());
			}
		}
	}

	protected void setAttributes(int maxHealth, int damage, int speed, int attackRange){
		this.maxHealth = maxHealth;
		this.damage = damage;
		this.speed = speed;
		this.attackRange = attackRange;
	}

	protected virtual void AttackTarget(){
		print("STUB");
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
		if(!b){
			foreach(Transform t in transform){
				print("removing child: " + t.name);
				GameObject.Destroy(t.gameObject);
			}
		}
	}

	public void getPath(){
		if(pathfinding == null){
			pathfinding = GetComponent<AStarPathfinding>();
		}
		if(isMyTurn){
			pathfinding.CalculateEntirePath(MasterObject.me.Correctmousepos());
			pathfinding.printPath(pathfinding.entirePath);
		} 
		else {
			print("its not my turn");
			
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

	public void onTargetEnemy(Vector3 pos){
		Collider[] cols = Physics.OverlapBox(pos, new Vector3(0.5f,0.5f,0.5f), Quaternion.identity, 1 << LayerMask.NameToLayer("Unit"));
		if(cols.Length > 0){
			print(cols[0].name);
			Vector3 closestPathTile = getClosestAdjacentToTarget(cols[0].transform.position);
			if(closestPathTile == cols[0].transform.position){
				print("TAG:Attributesbase - error: did not find any empty tiles around targeted enemy");
			} 
			else {
				print("TAG:Attributesbase - moving to " + closestPathTile);
				if(!pathfinding){
					pathfinding = GetComponent<AStarPathfinding>();
				}
				ArrayList attackPath =  pathfinding.CalculateEntirePath(closestPathTile);
				pathfinding.printPath(attackPath);
			}			
		}		
	}

	private Vector3 getClosestAdjacentToTarget(Vector3 targetpos){
		Vector3 result = targetpos;
		float targetDistance = 0;
		float minDistance = 10000;

		for(int x = -1; x <= 1; x++){
			for(int y = -1; y <= 1; y++){
				if(!(x == 0 && y == 0)){ // self pos
					//print("checking x: " + (targetpos.x + x) + " y: " + (targetpos.y + y));

					Vector3 testDistanceVector = new Vector3(targetpos.x + x, targetpos.y + y, 0);
					Collider[] cols = Physics.OverlapBox(testDistanceVector, 
														new Vector3(0.1f,0.1f,0.1f), 
														Quaternion.identity, 
														1 << LayerMask.NameToLayer("Terrain") | 1 << LayerMask.NameToLayer("Units"));
					if(cols.Length == 0){
						targetDistance = Mathf.Round(Vector3.Distance(transform.position, testDistanceVector));
						if(targetDistance < minDistance){
							minDistance = targetDistance;
							result = testDistanceVector;
						}
					} else {
						foreach(Collider c in cols){
							print(c.name);
						}
					}
				} 
			}
		}
		return result;
	}
}
