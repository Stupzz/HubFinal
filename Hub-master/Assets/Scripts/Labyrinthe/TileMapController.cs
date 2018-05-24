using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour {

	public Tile tuile;
	public Tilemap maMap;
	public int LargeurLabyrinthe;
	public int HauteurLabyrinthe;


	// Use this for initialization
	void Start () {

		Debug.Log ("Tout est ok");

		//Création de la matrice qui va definir les murs:
		//1 => mur incassable
		//0 => sol
		int[,] Matrice = new int[LargeurLabyrinthe, HauteurLabyrinthe];

		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < HauteurLabyrinthe; j++){
				if (i==0 | j==0| i == LargeurLabyrinthe-1| j== HauteurLabyrinthe-1){
					Matrice[i,j]=1;
				}else{
					Matrice[i,j]=0;
				}
			}
		}
		//Parcour de la matrice, set des tuiles en fornction du contenu de la matrice
		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < HauteurLabyrinthe; j++){
				if (Matrice[i,j]==1){
					maMap.SetTile (new Vector3Int(i,j,0), tuile);
				}
			}
		}
		if (HauteurLabyrinthe>6)
		{
			if (HauteurLabyrinthe % 2 == 0) {
				for (int a = 0; a < LargeurLabyrinthe; a++) {
					maMap.SetTile (new Vector3Int (a, (HauteurLabyrinthe/2) + 1, 0), tuile);
					maMap.SetTile (new Vector3Int (a, (HauteurLabyrinthe/2) - 2, 0), tuile);
				}
			} else 
			{
				for (int b = 0; b < LargeurLabyrinthe; b++) {
					maMap.SetTile (new Vector3Int (b, (HauteurLabyrinthe/2) + 1, 0), tuile);
					maMap.SetTile (new Vector3Int (b, (HauteurLabyrinthe/2) - 1, 0), tuile);
				}
			}
		}

    }

    // Update is called once per frame
    void Update () {

    }
}