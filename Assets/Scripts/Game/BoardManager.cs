using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using UnityChess;
using UnityEngine;
using static UnityChess.SquareUtil;
using System.Linq;

public class BoardManager : MonoBehaviourSingleton<BoardManager> {
	private readonly GameObject[] allSquaresGO = new GameObject[64];
	private Dictionary<Square, GameObject> positionMap;
	private const float BoardPlaneSideLength = 14f; // measured from corner square center to corner square center, on same side.
	private const float BoardPlaneSideHalfLength = BoardPlaneSideLength * 0.5f;
	private const float BoardHeight = 1.6f;
	private readonly System.Random rng = new System.Random();

	private void Awake() {
		GameManager.NewGameStartedEvent += OnNewGameStarted;
		GameManager.NewGame960StartedEvent += OnNewGame960Started;
		GameManager.GameResetToHalfMoveEvent += OnGameResetToHalfMove;
		
		positionMap = new Dictionary<Square, GameObject>(64);
		Transform boardTransform = transform;
		Vector3 boardPosition = boardTransform.position;
		
		for (int file = 1; file <= 8; file++) {
			for (int rank = 1; rank <= 8; rank++) {
				GameObject squareGO = new GameObject(SquareToString(file, rank)) {
					transform = {
						position = new Vector3(boardPosition.x + FileOrRankToSidePosition(file), boardPosition.y + BoardHeight, boardPosition.z + FileOrRankToSidePosition(rank)),
						parent = boardTransform
					},
					tag = "Square"
				};

				positionMap.Add(new Square(file, rank), squareGO);
				allSquaresGO[(file - 1) * 8 + (rank - 1)] = squareGO;
			}
		}
	}

	private void OnNewGameStarted() {
		ClearBoard();
		
		foreach ((Square square, Piece piece) in GameManager.Instance.CurrentPieces) {
			CreateAndPlacePieceGO(piece, square);
		}

		EnsureOnlyPiecesOfSideAreEnabled(GameManager.Instance.SideToMove);
	}

    private void OnNewGame960Started()
    {
        ClearBoard();

		/*List<(Square, Piece)> testPieces = GameManager.Instance.CurrentPieces;


		// King Is placed between the two rooks 
		// first rook can be placed 1-6
		// second rook is placed from the first rook  + 1 to end
		// king is now placed inbetween the two rooks. 

		List<(Square, Piece)> pieces960White = new List<(Square, Piece)>();
		List<(Square, Piece)> pieces960Black = new List<(Square, Piece)>();
		List<int> takenSpaces = new List<int>();
		int firstRook = UnityEngine.Random.Range(1, 7);
		takenSpaces.Add(firstRook);
		pieces960White.Add((new Square(firstRook,1), new Rook(Side.White)));
		pieces960Black.Add((new Square(firstRook,8), new Rook(Side.Black)));
		int secondRook = UnityEngine.Random.Range(firstRook + 2, 9);
        pieces960White.Add((new Square(secondRook,1), new Rook(Side.White)));
        pieces960Black.Add((new Square(secondRook,8), new Rook(Side.Black)));
        takenSpaces.Add(secondRook);
		int king = UnityEngine.Random.Range(firstRook + 1, secondRook);
        pieces960White.Add((new Square(king,1), new King(Side.White)));
        pieces960Black.Add((new Square(king,8), new King(Side.Black)));
        takenSpaces.Add(king);
		int firstBishop;
		bool isPlaced = false;
        while (!isPlaced)
		{
			int temp = UnityEngine.Random.Range(1, 9);
			
			if (takenSpaces.Contains(temp) == false)
			{
                firstBishop = temp; 
				isPlaced = true;
                pieces960White.Add((new Square(firstBishop, 1), new Bishop(Side.White)));
                pieces960Black.Add((new Square(firstBishop, 8), new Bishop(Side.Black)));
                takenSpaces.Add(temp);
				
            }
				
        }
		isPlaced = false;
		int secondBishop;
        while (!isPlaced)
        {
            int temp = UnityEngine.Random.Range(1, 9);
            if (takenSpaces.Contains(temp) == false)
            {
                secondBishop = temp;
                isPlaced = true;
                pieces960White.Add((new Square(secondBishop, 1), new Bishop(Side.White)));
                pieces960Black.Add((new Square(secondBishop, 8), new Bishop(Side.Black)));
                takenSpaces.Add(temp);

            }
        }
		isPlaced = false;
		int queen;
        while (!isPlaced)
        {
            int temp = UnityEngine.Random.Range(1, 9);
            if (takenSpaces.Contains(temp) == false)
            {
                queen = temp;
                isPlaced = true;
                pieces960White.Add((new Square(queen, 1), new Queen(Side.White)));
                pieces960Black.Add((new Square(queen, 8), new Queen(Side.Black)));
                takenSpaces.Add(temp);

            }
        }
		isPlaced = false;
		int firstKnight;
        while (!isPlaced)
        {
            int temp = UnityEngine.Random.Range(1, 9);
            if (takenSpaces.Contains(temp) == false)
            {
                firstKnight = temp;
                isPlaced = true;
                pieces960White.Add((new Square(firstKnight, 1), new Knight(Side.White)));
                pieces960Black.Add((new Square(firstKnight, 8), new Knight(Side.Black)));
                takenSpaces.Add(temp);

            }
        }
		isPlaced = false;

        int secondKnight;
        while (!isPlaced)
        {
            int temp = UnityEngine.Random.Range(1, 9);
            if (takenSpaces.Contains(temp) == false)
            {
                secondKnight = temp;
                isPlaced = true;
                pieces960White.Add((new Square(secondKnight, 1), new Knight(Side.White)));
                pieces960Black.Add((new Square(secondKnight, 8), new Knight(Side.Black)));
                takenSpaces.Add(temp);

            }
        }

		pieces960White = pieces960White.OrderBy(bruh => bruh.Item1.Rank).ToList();
		pieces960Black = pieces960Black.OrderBy(bruh => bruh.Item1.Rank).ToList();
		*//*
		foreach(var b in pieces960White)
		{
			UnityEngine.Debug.Log(b.Item1.ToString());
			UnityEngine.Debug.Log(b.Item2.ToString());
		}
		*//*

		int i = 0;
		int white = 0;
		int black = 0;
		bool check = true;

		
		
		while(i < 32)
		{
			UnityEngine.Debug.Log(i.ToString());
			UnityEngine.Debug.Log(white.ToString());
			UnityEngine.Debug.Log(black.ToString());
			if (check == true) 
			{
				testPieces[i] = pieces960White[white];
				white++;
				i+= 3;
				check = false;
			}
			else if (check == false) 
			{
                testPieces[i] = pieces960Black[black];
                black++;
                i++;
				check = true;
            }

		}


*/

		foreach ((Square square, Piece piece) in GameManager.Instance.CurrentPieces)
		{
			CreateAndPlacePieceGO(piece, square);
			UnityEngine.Debug.Log(square.ToString() + " | " + piece.ToString());
        }

		

		


        EnsureOnlyPiecesOfSideAreEnabled(GameManager.Instance.SideToMove);
    }

    private void OnGameResetToHalfMove() {
		ClearBoard();

		foreach ((Square square, Piece piece) in GameManager.Instance.CurrentPieces) {
			CreateAndPlacePieceGO(piece, square);
		}

		GameManager.Instance.HalfMoveTimeline.TryGetCurrent(out HalfMove latestHalfMove);
		if (latestHalfMove.CausedCheckmate || latestHalfMove.CausedStalemate) SetActiveAllPieces(false);
		else EnsureOnlyPiecesOfSideAreEnabled(GameManager.Instance.SideToMove);
	}

	public void CastleRook(Square rookPosition, Square endSquare) {
		GameObject rookGO = GetPieceGOAtPosition(rookPosition);
		rookGO.transform.parent = GetSquareGOByPosition(endSquare).transform;
		rookGO.transform.localPosition = Vector3.zero;
	}

	public void CreateAndPlacePieceGO(Piece piece, Square position) {
		string modelName = $"{piece.Owner} {piece.GetType().Name}";
		GameObject pieceGO = Instantiate(
			Resources.Load("PieceSets/Marble/" + modelName) as GameObject,
			positionMap[position].transform
		);

		/*if (!(piece is Knight) && !(piece is King)) {
			pieceGO.transform.Rotate(0f, (float) rng.NextDouble() * 360f, 0f);
		}*/
	}

	public void GetSquareGOsWithinRadius(List<GameObject> squareGOs, Vector3 positionWS, float radius) {
		float radiusSqr = radius * radius;
		foreach (GameObject squareGO in allSquaresGO) {
			if ((squareGO.transform.position - positionWS).sqrMagnitude < radiusSqr)
				squareGOs.Add(squareGO);
		}
	}

	public void SetActiveAllPieces(bool active) {
		VisualPiece[] visualPiece = GetComponentsInChildren<VisualPiece>(true);
		foreach (VisualPiece pieceBehaviour in visualPiece) pieceBehaviour.enabled = active;
	}

	public void EnsureOnlyPiecesOfSideAreEnabled(Side side) {
		VisualPiece[] visualPiece = GetComponentsInChildren<VisualPiece>(true);
		foreach (VisualPiece pieceBehaviour in visualPiece) {
			Piece piece = GameManager.Instance.CurrentBoard[pieceBehaviour.CurrentSquare];
			
			pieceBehaviour.enabled = pieceBehaviour.PieceColor == side
			                         && GameManager.Instance.HasLegalMoves(piece);
		}
	}

	public void TryDestroyVisualPiece(Square position) {
		VisualPiece visualPiece = positionMap[position].GetComponentInChildren<VisualPiece>();
		if (visualPiece != null) DestroyImmediate(visualPiece.gameObject);
	}
	
	public GameObject GetPieceGOAtPosition(Square position) {
		GameObject square = GetSquareGOByPosition(position);
		return square.transform.childCount == 0 ? null : square.transform.GetChild(0).gameObject;
	}
	
	private static float FileOrRankToSidePosition(int index) {
		float t = (index - 1) / 7f;
		return Mathf.Lerp(-BoardPlaneSideHalfLength, BoardPlaneSideHalfLength, t);
	}
	
	private void ClearBoard() {
		VisualPiece[] visualPiece = GetComponentsInChildren<VisualPiece>(true);

		foreach (VisualPiece pieceBehaviour in visualPiece) {
			DestroyImmediate(pieceBehaviour.gameObject);
		}
	}

	public GameObject GetSquareGOByPosition(Square position) => Array.Find(allSquaresGO, go => go.name == SquareToString(position));
}