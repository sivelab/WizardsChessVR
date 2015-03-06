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
                Black,
                White,
        };

        enum SquareStatus
        {
                Empty,
                Black,
                White,
                BlackKing,
                WhiteKing,
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
        public GameObject WhitePawnPrefab;
        public GameObject BlackPawnPrefab;

        private GameObject[,] squares = new GameObject[8, 8];
        private GameObject[,] pieces = new GameObject[8, 8];
        private SquareStatus[,] statuses = new SquareStatus[8, 8];
        private Side turn;
        private Dictionary<NetworkPlayer, Side> playerSides = new Dictionary<NetworkPlayer, Side> ();
        
        
        void Start ()
        {
                for (int x = 0; x < 8; x++) {
                        for (int z = 0; z < 8; z++) {
                                GameObject square = GameObject.CreatePrimitive (PrimitiveType.Quad);
                                squares [x, z] = square;
                                square.transform.eulerAngles = new Vector3 (90, 0, 0);
                                square.transform.parent = transform;
                                square.transform.localPosition = new Vector3 (x, 0, z);
                                setSquareState (x, z, State.Normal);
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
                setSquareState (Selectx, Selectz, State.Normal);
                Selectx = x;
                Selectz = z;
                setSquareState (Selectx, Selectz, State.Selected);
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
                                statuses [x, z] = SquareStatus.Empty;
                                GameObject piece = pieces [x, z];
                                if (piece != null) {
                                        Destroy (piece);
                                        pieces [x, z] = null;
                                }
                        }

                }
                for (int x = 0; x < 3; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                statuses [x, z] = SquareStatus.White;
                                GameObject piece = (GameObject)Instantiate (WhitePawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 180, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
                for (int x = 5; x < 8; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                statuses [x, z] = SquareStatus.Black;
                                GameObject piece = (GameObject)Instantiate (BlackPawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 0, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
                turn = Side.Black;
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
                setSquareState (Hoverx, Hoverz, State.Normal);
                Hoverx = x;
                Hoverz = z;
                if (Input.GetButtonDown ("Fire1")) {
                        if (Network.isServer) {
                                ClientSelect (x, z, Network.player);
                        } else {
                                board.networkView.RPC ("ClientSelect", RPCMode.Server, x, z);
                        }
                }
                setSquareState (x, z, State.Hover);
                setSquareState (Selectx, Selectz, State.Selected);
                squares [0, 0].renderer.material.color = Color.blue;
        }

        enum State
        {
                Normal,
                Hover,
                Selected}
        ;
        private void setSquareState (int x, int z, State state)
        {
                if (x == -1) {
                        return;
                }
                if (state == State.Normal) {
                        if ((x + z) % 2 == 0) {
                                squares [x, z].renderer.material = evenSquares;
                        } else {
                                squares [x, z].renderer.material = oddSquares;
                        }
                } else if (state == State.Hover) {
                        if (x != Selectx || z != Selectz) {
                                squares [x, z].renderer.material = hover;
                        }
                } else {
                        squares [x, z].renderer.material = select;
                }
                squares [x, z].renderer.material.SetTextureScale ("Base", new Vector2 (8, 8));
        }
}
