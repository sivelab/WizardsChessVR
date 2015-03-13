using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(NetworkView))]
public class Board : MonoBehaviour
{
        public Board ()
        {
                board = this;
        }

        enum Side
        {
                None,
                Black,
                White,
        };

        enum SquarePiece
        {
                None,
                Pawn,
                King,
        };

        enum TurnStatus
        {
                Select,
                Move,
                ContinueMove,
        };

        public static Board board;
        public Material oddSquares;
        public Material evenSquares;
        public Material hover;
        public Material select;
        public Material highlight;
        public GameObject WhitePawnPrefab;
        public GameObject BlackPawnPrefab;

        private GameObject[,] squares = new GameObject[8, 8];
        private GameObject[,] pieces = new GameObject[8, 8];
        private Side[,] squareSide = new Side[8, 8];
        private SquarePiece[,] squarePiece = new SquarePiece[8, 8];
        private Side turn;
        private TurnStatus turnStatus;
        private Dictionary<NetworkPlayer, Side> playerSides = new Dictionary<NetworkPlayer, Side> ();
        
        
        void Start ()
        {
                networkView.group = 2;
                for (int x = 0; x < 8; x++) {
                        for (int z = 0; z < 8; z++) {
                                GameObject square = GameObject.CreatePrimitive (PrimitiveType.Quad);
                                squares [x, z] = square;
                                square.transform.eulerAngles = new Vector3 (90, 0, 0);
                                square.transform.parent = transform;
                                square.transform.localPosition = new Vector3 (x, 0, z);
                                square.transform.localScale = new Vector3 (1, 1, 1);
                        }
                }
                ClientAction ("NewGame", Network.player);
        }

        [RPC]
        public void ClientSelect (int x, int z, NetworkMessageInfo info)
        {
                ClientSelect (x, z, info.sender);
        }

        public void ClientSelect (int x, int z, NetworkPlayer source)
        {

                if (!playerSides.ContainsKey (source)) {
                        return;
                }
                if (playerSides [source] != turn) {
                        return;
                }

                networkView.RPC ("SelectSquare", RPCMode.AllBuffered, x, z);
        }

        [RPC] 
        public void SelectSquare (int x, int z)
        {
                if (x == -1)
                        return;
                if (turnStatus == TurnStatus.Select) {
                        if (squareSide [x, z] == turn) {
                                turnStatus = TurnStatus.Move;
                                Selectx = x;
                                Selectz = z;
                        }
                } else if (turnStatus == TurnStatus.Move && squareSide [x, z] == turn) {
                        Selectx = x;
                        Selectz = z;
                } else if (canMove (x, z, turnStatus == TurnStatus.ContinueMove)) {
                        bool wasJump = Selectx - x < -1 || Selectx - x > 1;
                        squareSide [x, z] = turn;
                        squarePiece [x, z] = squarePiece [Selectx, Selectz];
                        squareSide [Selectx, Selectz] = Side.None;
                        squarePiece [Selectx, Selectz] = SquarePiece.None;

                        pieces [Selectx, Selectz].transform.localPosition = new Vector3 (x, 0, z);
                        pieces [x, z] = pieces [Selectx, Selectz];
                        pieces [Selectx, Selectz] = null;

                        if (wasJump) {
                                int mx = (x + Selectx) / 2;
                                int mz = (z + Selectz) / 2;
                                Destroy (pieces [mx, mz]);
                                pieces [mx, mz] = null;
                                squareSide [mx, mz] = Side.None;
                                squarePiece [mx, mz] = SquarePiece.None;
                        }
                        Selectx = x;
                        Selectz = z;
                        if (turn == Side.Black && x == 0) {
                                squarePiece [x, z] = SquarePiece.King;
                                pieces [x, z].transform.localScale = new Vector3 (1, 2, 1);
                        }
                        if (turn == Side.White && x == 7) {
                                squarePiece [x, z] = SquarePiece.King;
                                pieces [x, z].transform.localScale = new Vector3 (1, 2, 1);
                        }

                        if (wasJump && (canMove (x + 2, z + 2, true) || canMove (x + 2, z - 2, true) || canMove (x - 2, z + 2, true) || canMove (x - 2, z - 2, true))) {
                                turnStatus = TurnStatus.ContinueMove;
                        } else {
                                turnStatus = TurnStatus.Select;
                                turn = otherSide (turn);
                                Selectx = -1;
                                Selectz = -1;
                        }
                } else if (x == Selectx && z == Selectz) {
                        //Must be TurnStatus.ContinueMove
                        turnStatus = TurnStatus.Select;
                        turn = otherSide (turn);
                        Selectx = -1;
                        Selectz = -1;
                }
        }

        [RPC]
        public void ClientAction (string action, NetworkMessageInfo info)
        {
                ClientAction (action, info.sender);
        }

        public void ClientAction (string action, NetworkPlayer source)
        {
                if (action == "NewGame") {
                        Network.RemoveRPCsInGroup (networkView.group);
                        networkView.RPC ("NewGame", RPCMode.AllBuffered);
                }
                if (action == "PlayBlack") {
                        playerSides [source] = Side.Black;
                }
                if (action == "PlayWhite") {
                        playerSides [source] = Side.White;
                }
        }


        [RPC]
        public void NewGame ()
        {

                for (int x = 0; x < 8; x++) {
                        for (int z = 0; z < 8; z++) {
                                squareSide [x, z] = Side.None;
                                squarePiece [x, z] = SquarePiece.None;
                                GameObject piece = pieces [x, z];
                                if (piece != null) {
                                        Destroy (piece);
                                        pieces [x, z] = null;
                                }
                        }

                }
                for (int x = 0; x < 3; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                squareSide [x, z] = Side.White;
                                squarePiece [x, z] = SquarePiece.Pawn;
                                GameObject piece = (GameObject)Instantiate (WhitePawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 180, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
                for (int x = 5; x < 8; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                squareSide [x, z] = Side.Black;
                                squarePiece [x, z] = SquarePiece.Pawn;
                                GameObject piece = (GameObject)Instantiate (BlackPawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 0, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
                turn = Side.Black;
                turnStatus = TurnStatus.Select;
        }
        
        private int Hoverx = -1;
        private int Hoverz = -1;
        private int Selectx = -1;
        private int Selectz = -1;
        public void Hover (int x, int z)
        {
                if (x < 0 || x >= 8 || z < 0 || z >= 8) {
                        x = -1;
                        z = -1;
                }
                Hoverx = x;
                Hoverz = z;

                if (Input.GetButtonDown ("Fire1")) {
                        if (Network.isServer) {
                                ClientSelect (x, z, Network.player);
                        } else {
                                board.networkView.RPC ("ClientSelect", RPCMode.Server, x, z);
                        }
                }
        }

        enum State
        {
                Normal,
                Hover,
                Selected}
        ;

        void Update ()
        {
                for (int x = 0; x < 8; x++) {
                        for (int z = 0; z < 8; z++) {
                                if (x == Selectx && z == Selectz) {
                                        squares [x, z].renderer.material = select;
                                } else if (x == Hoverx && z == Hoverz) {
                                        squares [x, z].renderer.material = hover;
                                } else if (turnStatus == TurnStatus.Select && squareSide [x, z] == turn) {
                                        squares [x, z].renderer.material = highlight;
                                } else if ((turnStatus == TurnStatus.Move || turnStatus == TurnStatus.ContinueMove) && canMove (x, z, turnStatus == TurnStatus.ContinueMove)) {
                                        squares [x, z].renderer.material = highlight;
                                } else if (turnStatus == TurnStatus.ContinueMove && x == Selectx && z == Selectz) {
                                        squares [x, z].renderer.material = highlight;
                                } else if ((x + z) % 2 == 0) {
                                        squares [x, z].renderer.material = evenSquares;
                                } else {
                                        squares [x, z].renderer.material = oddSquares;
                                }
                                //squares [x, z].renderer.material.SetTextureScale ("Base", new Vector2 (8, 8));
                        }
                }
        }

        bool canMove (int x, int z, bool jumpOnly)
        {
                if (x < 0 || x >= 8 || z < 0 || z >= 8)
                        return false;
                int dx = x - Selectx;
                int dz = z - Selectz;
             
                //Going wrong way
                if (squarePiece [Selectx, Selectz] == SquarePiece.Pawn && (
                        (dx > 0 && turn == Side.Black) ||
                        (dx < 0 && turn == Side.White)))
                        return false;

                //Going one step, just check move spot
                if (!jumpOnly && (dx == 1 || dx == -1) && (dz == 1 || dz == -1)) {
                        return squareSide [x, z] == Side.None;
                }

                //Going two steps, check move spot and jumping
                if ((dx == 2 || dx == -2) && (dz == 2 || dz == -2)) {
                        return squareSide [x, z] == Side.None && Enemy (turn, squareSide [Selectx + dx / 2, Selectz + dz / 2]);
                }

                return false;
        }

        bool Enemy (Side a, Side b)
        {
                return (a == Side.Black && b == Side.White) || (a == Side.White && b == Side.Black);
        }

        Side otherSide (Side side)
        {
                if (side == Side.Black)
                        return Side.White;
                if (side == Side.White)
                        return Side.Black;
                return Side.None;
        }

}
