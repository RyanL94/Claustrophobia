using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDrop : Drop {

	public int gold;

	protected override void Take() {
		base.Take();
		game.player.money += gold;
	}
}
