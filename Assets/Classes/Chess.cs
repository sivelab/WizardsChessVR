using UnityEngine;
using System.Collections;

public class Chess {
	public enum Team {White, Black};
	public enum Piece {Pawn, Rook, Bishop, Queen, King, Knight};

	public struct TeamPiece{
		public Team team;
		public Piece piece;
	}

	private struct Position{
		public int x;
		public int y;
	}

	private bool canMove(Position from, Position to){
		return false;
	}

}
