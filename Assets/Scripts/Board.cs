using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
        public Board ()
        {
                board = this;
        }

        public static Board board;
        public Material oddSquares;
        public Material evenSquares;
        public Material hover;
        public Material select;
        private GameObject[,] squares = new GameObject[8, 8];

        public GameObject[,] pieces = new GameObject[8, 8];

        public GameObject WhitePawnPrefab;
        public GameObject BlackPawnPrefab;

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
                NewGame ();

        }

        public void NewGame ()
        {
                for (int x = 0; x < 8; x++) {
                        for (int z = 0; z < 8; z++) {
                                GameObject piece = pieces [x, z];
                                if (piece != null) {
                                        Destroy (piece);
                                        pieces [x, z] = null;
                                }
                        }
                }
                for (int x = 0; x < 3; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                GameObject piece = (GameObject)Instantiate (WhitePawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 180, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
                for (int x = 5; x < 8; x++) {
                        for (int z = (x + 1) % 2; z < 8; z += 2) {
                                GameObject piece = (GameObject)Instantiate (BlackPawnPrefab);
                                pieces [x, z] = piece;
                                piece.transform.eulerAngles = new Vector3 (0, 0, 0);
                                piece.transform.parent = transform;
                                piece.transform.localPosition = new Vector3 (x, 0, z);
                        }
                }
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
                setSquareState (Selectx, Selectz, State.Normal);
                Hoverx = x;
                Hoverz = z;
                if (Input.GetButtonDown ("Fire1")) {
                        Selectx = x;
                        Selectz = z;
                }
                setSquareState (x, z, State.Hover);
                setSquareState (Selectx, Selectz, State.Selected);

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
                        squares [x, z].renderer.material = hover;
                } else {
                        squares [x, z].renderer.material = select;
                }
                squares [x, z].renderer.material.SetTextureScale ("Base", new Vector2 (8, 8));
        }
}
