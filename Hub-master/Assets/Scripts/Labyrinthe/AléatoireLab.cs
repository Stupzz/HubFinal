using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AléatoireLab : MonoBehaviour {


	public Tile tuileMur;
	public Tile tuileDepart;
	public Tile tuileArrive;
	public Tile tuileSol;
	public Tilemap maMap;
	public Tilemap sol;
	public int LargeurLabyrinthe;
	public int HauteurLabyrinthe;
	int NbCasesAZero;
	int d,x,y=0;

	void Propagation (int [,] M, int Px, int Py, int valeurARemplacer, int valeurAPlacer){
		if (M [Px, Py] == valeurARemplacer) {
			M [Px, Py] = valeurAPlacer;
			if (M[Px,Py]==0& Px %2 !=0 & Py %2 != 0){
				NbCasesAZero++;
			}
			Propagation (M, Px, Py + 1, valeurARemplacer, valeurAPlacer);
			Propagation (M, Px, Py - 1, valeurARemplacer, valeurAPlacer);
			Propagation (M, Px + 1, Py, valeurARemplacer, valeurAPlacer);
			Propagation (M, Px - 1, Py, valeurARemplacer, valeurAPlacer);
		}
	}

	void GenererCoordonnées (int [,]M, int Gx, int Gy){
		
	}

	// Use this for initialization
	void Start () {
		int[,] Matrice = new int[LargeurLabyrinthe, HauteurLabyrinthe];
		int[,] MatriceDouble = new int[LargeurLabyrinthe, (HauteurLabyrinthe*2)+2];

		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < (HauteurLabyrinthe*2)+2; j++){
				MatriceDouble [i, j] = -1;
				} 
			}
		int k = 0;
		for (int i = 0; i < LargeurLabyrinthe; i++) 
		{
			for (int j = 0; j < HauteurLabyrinthe; j++) 
			{
				if (i % 2 != 0 & j % 2 != 0) {
					Matrice [i, j] = k;
					k++;
				} else 
				{
					Matrice [i, j] = -1;
				}
			}
		}
		NbCasesAZero = 1;

		while (NbCasesAZero < (LargeurLabyrinthe/2)*(HauteurLabyrinthe/2))
			{
				//Prendre au hasard (x, y) tel que :
				//L[x][y] = −1 et (x impair ou y impair)

				
			//GenererCoordonnées (Matrice,x, y);
			bool CoordsPasPret = true;
			while (CoordsPasPret)
			{
				x =  Random.Range(0,LargeurLabyrinthe-1);
				y =  Random.Range(0,HauteurLabyrinthe-1);
				if ((Matrice[x,y]==-1)&((x%2 != 0)^(y%2!=0))&(x!=0&y!=0))
				{
					CoordsPasPret=false;
				}
			}
				
			if (x % 2 != 0) {
				d = Matrice [x, y - 1] - Matrice [x, y + 1];
				if (d > 0) {
					Matrice [x,y] = Matrice [x,y + 1];
					Propagation (Matrice, x, y - 1, Matrice [x, y - 1], Matrice [x,y + 1]);
				} else if (d < 0) {
					Matrice [x,y] = Matrice [x,y - 1];
					Propagation (Matrice, x, y + 1, Matrice [x, y + 1], Matrice [x,y - 1]);
				}
			} else {
			d = Matrice [x-1, y] - Matrice [x+1, y];
				if (d > 0) {
					Matrice [x,y] = Matrice [x+1,y];
					Propagation (Matrice, x-1, y, Matrice [x-1, y], Matrice [x+1,y]);
				} else if (d < 0) {
					Matrice [x,y] = Matrice [x-1,y];
					Propagation (Matrice, x+1, y, Matrice [x+1, y], Matrice [x-1,y]);
			}
			}
		}




		/*
		//placement des murs depuis matrice solo 
		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < HauteurLabyrinthe; j++){
				if (Matrice [i, j] == -1) {
					maMap.SetTile (new Vector3Int (i, j, 0), tuileMur);
				} 
			}
		}
		*/
		//placement du sol
		for (int i = 0; i < LargeurLabyrinthe; i++){
				for (int j = 0; j < HauteurLabyrinthe*2+2; j++){
					sol.SetTile (new Vector3Int (i, j, 0), tuileSol);
				}
			}






		//copie de la matrice;
		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < HauteurLabyrinthe; j++){
				MatriceDouble [i, j] = Matrice [i, j];
				MatriceDouble [LargeurLabyrinthe-1-i, HauteurLabyrinthe*2+1-j] = Matrice [i, j];
			}
		}



		//Set les cases entre les deux boards a -2 
		for (int i = 0; i < LargeurLabyrinthe; i++) {
			MatriceDouble [i, HauteurLabyrinthe + 1] = -2;
			MatriceDouble [i, HauteurLabyrinthe] = -2;
		}


		//placement des murs depuis la matrice double;
		for (int i = 0; i < LargeurLabyrinthe; i++){
			for (int j = 0; j < (HauteurLabyrinthe*2)+2; j++){
				if (MatriceDouble [i, j] == -1) {
					maMap.SetTile (new Vector3Int (i, j, 0), tuileMur);
				} 
			}
		}

		//arrivée et sortie set en brut
		sol.SetTile (new Vector3Int (1, HauteurLabyrinthe-2, 0), tuileDepart);
		sol.SetTile (new Vector3Int (LargeurLabyrinthe-2, 1, 0), tuileArrive);

		sol.SetTile (new Vector3Int (1, HauteurLabyrinthe*2, 0), tuileArrive);
		sol.SetTile (new Vector3Int (LargeurLabyrinthe-2, HauteurLabyrinthe+3, 0), tuileDepart );



    }

    // Update is called once per frame
    void Update () {

    }
}
