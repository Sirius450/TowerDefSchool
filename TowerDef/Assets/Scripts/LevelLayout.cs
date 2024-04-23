using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelLayout : MonoBehaviour
{
    GameManager GM;
    GameTile[,] gameTiles;


    // const pour la taille de la map
    const int Min = 0;
    const int MaxHauteur = 10;
    const int MaxLargeur = 16;

    //variable
    private int Hauteur;
    private int Largeur;
    private string[] FichierMap;

    int ligne = 0;
    public char[][] tableauCarte;
    bool mapValide = false;

    //Spwaner info
    private char spwan = 'S';
    private int spwanX = 0;
    private int spwanY = 0;

    //Fin info
    private char fin = 'F';
    private int finX = 0;
    private int finY = 0;



    #region Gettter\Setter
    //getter et setter pour Hauteur
    public int hauteur
    {
        get { return hauteur; }
        set
        {
            if (value < Min)   // si la valeur est negatif
            { Console.WriteLine("la hauter de la carte ne peut pas etre negatif"); }
            else
            {
                if (value > MaxHauteur) //si la valeur est superieur au max de la hauteur
                {
                    Console.WriteLine($"la hauter de la carte ne peut pas etre superieur a {MaxHauteur}");
                }
                else
                { Hauteur = value; }
            }
        }
    }

    //getter et setter pour Largeur
    public int largeur
    {
        get { return largeur; }
        set
        {
            if (value < Min)   // si la valeur est negatif
            { Console.WriteLine("la largeur de la carte ne peut pas etre negatif"); }
            else
            {
                if (value > MaxLargeur) //si la valeur est superieur au max de la hauteur
                {
                    Console.WriteLine($"la largeur de la carte ne peut pas etre superieur a {MaxLargeur}");
                }
                else
                { Largeur = value; }
            }
        }
    }

    //getter et setter pour les nom des map
    public string[] fichierMap
    {
        get
        { return FichierMap; }
        private set
        { FichierMap = value; }
    }
    #endregion

    private void Awake()
    {
        GM = GetComponent<GameManager>();
        gameTiles = new GameTile[MaxLargeur, MaxHauteur];
    }

    //constructeur
    public LevelLayout()
    {
        //si la tableau n'a pas ete creer, le faire
        if (FichierMap == null)
        {
            //obtenir le chemin du dossier
            string dossier = Directory.GetCurrentDirectory();

            // cherche les map dans le dossier 
            FichierMap = Directory.GetFiles(dossier, "*.map");

            //boucle pour garder juste le nom des map
            for (int i = 0; i < FichierMap.Length; i++)
            {
                //varible pour garder unique le nom des map
                string map = "\\";

                //obtenir lindex du dernier '\'
                int index = FichierMap[i].LastIndexOf(map);

                //soustrait tout les reste sauf le nom de la map
                string NewNomMap = FichierMap[i].Substring(index + 1);

                //ajuste de nouveau nom
                FichierMap[i] = NewNomMap;
            }
        }
    }

    //methode pour charger les cartes
    public void ChargerCarte(string mapName)
    {
        //reset les valeur
        ligne = 0;
        mapValide = false;
        if (tableauCarte == null)
        { }
        else
        { tableauCarte = null; }

        //permet de lire un fichier et le mettre dans un array
        tableauCarte = new char[][] { };

        //obtenir le chemin du dossier
        string dossier = Directory.GetCurrentDirectory();

        //charger fichier
        //lie tout le fichier ligne par ligne
        foreach (string ligneFichier in System.IO.File.ReadAllLines(mapName))
        {
            //creer un tableau temp pour les lignes 
            char[] ligneFichierTabelau = ligneFichier.ToCharArray();

            //cange la taille du tableauCarte et met la premiere ligne dans la tableau
            Array.Resize(ref tableauCarte, tableauCarte.Length + 1);
            tableauCarte[tableauCarte.GetUpperBound(0)] = ligneFichierTabelau;


            //boucle attraver la ligne pour voir si trouve objet important
            for (int i = 0; i < ligneFichier.Length - 1; i++)
            {
                // si trouve le Spwan
                if (tableauCarte[ligne][i] == spwan)
                {
                    GM.spawnTile = gameTiles[i, ligne];
                    var spawnPosition = new Vector3(i, ligne, 0);
                    var tile = Instantiate(GM.gameTilePrefab, spawnPosition, Quaternion.identity);
                    gameTiles[i, ligne] = tile.GetComponent<GameTile>();
                }

                //si trouve la fin
                if (tableauCarte[ligne][i] == fin)
                {
                    GM.endTile = gameTiles[i, ligne];
                }
                //trouve un mur
                if (tableauCarte[ligne][i] == 'X')
                {

                }
            }

            //ajoute une ligne a la fin du loading d'une ligne
            ligne++;

            //for (int x = 0; x < ColCount; x++)
            //{
            //    for (int y = 0; y < RowCount; y++)
            //    {
            //        var spawnPosition = new Vector3(x, y, 0);
            //        var tile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
            //        gameTiles[x, y] = tile.GetComponent<GameTile>();
            //        gameTiles[x, y].GM = this;
            //        gameTiles[x, y].X = x;
            //        gameTiles[x, y].Y = y;
            //        if ((x + y) % 2 == 0)
            //        {
            //            gameTiles[x, y].TurnGrey();
            //        }
            //    }
            //}

        }
    }
}

public class Map
{
    char[,] GreatWamSect79 = new char[,]
{
    {' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X'}, //1
    {' ', ' ', ' ', ' ', ' ', 'X', 'X', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', 'X'}, //2
    {' ', 'X', ' ', ' ', ' ', 'X', 'X', ' ', 'X', ' ', ' ', 'X', ' ', ' ', ' ', 'X'}, //3
    {' ', ' ', 'X', ' ', ' ', ' ', 'X', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //4
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', 'F'}, //5
    {' ', 'X', ' ', ' ', ' ', ' ', 'X', ' ', ' ', 'X', ' ', ' ', 'X', ' ', ' ', ' '}, //6
    {' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', 'X'}, //7
    {'S', ' ', ' ', ' ', ' ', 'X', 'X', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', 'X'}, //8
    {' ', 'X', ' ', ' ', ' ', 'X', 'X', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', 'X'}, //9
    {' ', ' ', ' ', ' ', ' ', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X'}  //10
  //  1    2    3    4    5    6    7   8     9    10   11  12    13   14   15   16
};

    char[,] NuclearWinter = new char[,]
{
    {'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X'}, //1
    {' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', ' ', ' '}, //2
    {'S', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'F'}, //3
    {'X', 'X', ' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', 'X', 'X', ' ', ' ', ' '}, //4
    {'X', 'X', ' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' ', 'X', 'X'}, //5
    {'X', 'X', ' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' ', 'X', 'X'}, //6
    {'X', 'X', ' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', 'X', 'X', ' ', 'X', 'X'}, //7
    {'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X'}, //8
    {'X', 'X', ' ', ' ', ' ', ' ', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' ', 'X', 'X'}, //9
    {'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X'}  //10
  //  1    2    3    4    5    6    7   8     9    10   11  12    13   14   15   16
};

    char[,] HepburnMineField = new char[,]
{
    {'X', 'X', ' ', ' ', ' ', ' ', ' ', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //1
    {'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //2
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', 'X', ' ', ' ', ' '}, //3
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X', ' ', ' '}, //4
    {'S', ' ', ' ', ' ', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'F'}, //5
    {' ', ' ', ' ', ' ', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //6
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' '}, //7
    {'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' '}, //8
    {'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //9
    {'X', 'X', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}  //10
  //  1    2    3    4    5    6    7   8     9    10   11  12    13   14   15   16
};

    char[,] heatedSkirmish = new char[,]
{
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' '}, //1
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' '}, //2
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' '}, //3
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' '}, //4
    {'S', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'F'}, //5
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', 'X', 'X', 'X', ' ', ' ', ' ', ' ', ' '}, //6
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //7
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //8
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //9
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}  //10
  //  1    2    3    4    5    6    7   8     9    10   11  12    13   14   15   16
};

    char[,] NoMansLand = new char[,]
{
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //1
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //2
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //3
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //4
    {'S', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'F'}, //5
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //6
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //7
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //8
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}, //9
    {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'X', ' ', ' ', ' ', ' ', ' ', ' ', ' '}  //10
  //  1    2    3    4    5    6    7   8     9    10   11  12    13   14   15   16
};






}
