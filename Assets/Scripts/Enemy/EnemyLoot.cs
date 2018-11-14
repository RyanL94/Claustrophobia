using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour {

    [System.Serializable]
    public class DropCurrency
    {
        public string name;
        public GameObject item;
        public int dropRate;
    }

    public List<DropCurrency> LootTable = new List<DropCurrency>();
    public int dropChance;

	public void CalculateDropRate()
    {
        int rngesus = Random.Range(0, 101);

        if (rngesus > dropChance)
        {
            return;
        }

        if (rngesus <= dropChance)
        {
            int itemChance = 0;

            for (int i = 0; i < LootTable.Count; i++)
            {
                itemChance += LootTable[i].dropRate;
            }

            int ran = Random.Range(0, itemChance);

            for (int i = 0; i<LootTable.Count; i++)
            {
                if (ran <= LootTable[i].dropRate)
                {
                    var item = LootTable[i].item;
                    Instantiate(item, transform.position + item.transform.position, item.transform.rotation);
                    return;
                }
                ran -= LootTable[i].dropRate;

            }
        }
    }

    void OnDestroy()
    {
        CalculateDropRate();
    }
}
