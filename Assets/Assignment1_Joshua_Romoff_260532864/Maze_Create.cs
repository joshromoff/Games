using UnityEngine;

using System.Collections;
using System.Collections.Generic;
public class Maze_Create : MonoBehaviour {


	public GameObject floor;
	public GameObject Room3x3;
	public GameObject hole;
	public GameObject sWall;
	public GameObject nWall;
	public GameObject eWall;
	public GameObject wWall;
	public GameObject BlueRoom;
	public GameObject RedRoom;
	public GameObject GreenRoom;
	public GameObject ExitRoom;
	public GameObject StartRoom;
	public GameObject RedBall;
	public GameObject GreenBall;
	public GameObject BlueBall;
	public GameObject PurpleBall;
	public GameObject BlueWall;
	public GameObject GreenWall;
	public GameObject RedWall;
	public GameObject PurpleWall;
	Vector3 start = new Vector3(0f,-3f,0f);
	float depth = -20f; //the depth of our dungeon

	//struct that will be our representation of a spot in our grid
	//it keeps track of which direction it connects with
	//and its position
	public struct Cell{
		public int N;
		public int E;
		public int S;
		public int W;
		public int connected;
		public int x;
		public int y;
		public int finalPath;
		public Cell(int x1, int y1){
			x = x1;
			y = y1;
			N=E=S=W=connected=finalPath = 0;
		}
	}
	static int size = 30;
	bool [] array = new bool[size*size];
	Cell[,] grid = new Cell[size,size];
	List<Cell> frontier = new List<Cell>();
	Dictionary<int, Vector2> rooms = new Dictionary<int,Vector2>();

	Dictionary<int, int> wallRooms = new Dictionary<int,int>();
	Dictionary<int, int> ballRooms = new Dictionary<int,int>();
	// Use this for initialization
	void Start () {

		//randomly creats our ball wall combination such that the maze is solvable
		CreateBallWallCombo ();
		//places the rooms and projectile/walls
		CreateRooms ();
		//we need to ensure a path from our 3rd secondary room to the end room
		Vector2 room3 = rooms [3];
		Cell EndPathStart = new Cell((int)room3.x + 1, (int)room3.y + 5);

		Cell EndPathEnd = new Cell(28, 27);
		createPath (EndPathStart,EndPathEnd );
		//run prims to connect the grid
		prims ();
		//finally draw the grid
		CreateGrid ();


	}
	void CreateGrid(){
				//i is x, j is z
		for (int i = 0; i < size; i++) {
			for (int j = 0; j<size; j++) {
				int x = (int)rooms[1].x;
				int y = (int)rooms[1].y;
				//we always need floors and ceilings

				if(grid[i,j].connected != 2 ){
					Instantiate (floor, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					Instantiate (floor, new Vector3 (i*2, depth+4, j*2), Quaternion.identity);
					//if we are on the left boundary build a west wall
					if(i==0)Instantiate (wWall, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					//if we are on the right boundary build an east wall
					if(i == size-1) Instantiate (eWall, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					//otherwise we build an east wall if our cell isnt connected to its east and were not in a rom
					else if(grid[i,j].E ==0 &&grid[i+1,j].connected != 2) Instantiate (eWall, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					//if we our at the bottom and not at the start build a south wall
					if(j==0 && i!=0)Instantiate (sWall, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					//if we are at the north boundary build a north wall
					if(j==size-1) Instantiate (nWall, new Vector3 (i*2, depth, j*2), Quaternion.identity);
					//otherwise build a north wall if were not in a room and we arent connected to the north
					else if(grid[i,j].N ==0 &&grid[i,j+1].connected != 2)Instantiate (nWall, new Vector3 (i*2, depth, j*2), Quaternion.identity); 
			
							}
						}
				}
		}

	void CreateRooms(){
		//each room is 3x3
		//they come in pairs, second room is locked.
		//at least 1 room must connect to start,consider that later
		//x and z coordinates are discrete from start to start +27
		//y will stay the same
		//pick bottom left corner randomly such that they dont collide
		//i played around with this quite a bit, each room has a range of options where it can be placed
		//build room 1A 3x3, add 3 to the z build room 1B

		Vector3 rnd1 = new Vector3 (Random.Range (3, size-15), depth, Random.Range (4, size-18));
		Vector3 rnd2 = new Vector3 (Random.Range (size-15, size-3), depth, Random.Range (size-21, size-15));
		while (Mathf.Abs (rnd1.x - rnd2.x ) <= 3 && Mathf.Abs (rnd1.z - rnd2.z ) <= 6) { //while room2 intersects with room1 calculate a new room
			rnd2 = new Vector3 (Random.Range (size-15, size-3), depth, Random.Range (size-21, size-15));
		}
		Vector3 rnd3 = new Vector3 (Random.Range (3, size-3), depth, Random.Range (size-15, size-9)); 
		//while room3 intersects with room1 or room2 calculate a new room
		while ((Mathf.Abs (rnd1.x - rnd3.x ) <= 3 && Mathf.Abs (rnd1.z - rnd3.z ) <= 6) || (Mathf.Abs (rnd2.x - rnd3.x ) <= 3 && Mathf.Abs (rnd2.z - rnd3.z ) <= 6) ) { 
			rnd3 = new Vector3 (Random.Range (3, size-3), depth, Random.Range (size-15, size-9));
				}
		//offset z axis for attached room
		Vector3 offset = new Vector3 (0, 0, 6);
		Vector3 scale = new Vector3 (2, 1, 2);
		Vector3 rnd1s = new Vector3(rnd1.x *2, rnd1.y,rnd1.z*2);
		Vector3 rnd2s = new Vector3(rnd2.x *2, rnd2.y,rnd2.z*2);
		Vector3 rnd3s = new Vector3(rnd3.x *2, rnd3.y,rnd3.z*2);
		//make the rooms
		Instantiate(Room3x3, rnd1s,Quaternion.identity );
		Instantiate(BlueRoom, (rnd1s+offset),Quaternion.identity );
		Instantiate(Room3x3, rnd2s,Quaternion.identity );
		Instantiate(RedRoom, (rnd2s+offset),Quaternion.identity );
		Instantiate(Room3x3, rnd3s,Quaternion.identity );
		Instantiate(GreenRoom, (rnd3s+offset),Quaternion.identity );
		Instantiate (StartRoom, new Vector3 (0, depth, 0), Quaternion.identity);
		Instantiate (ExitRoom, new Vector3 (54, depth, 54), Quaternion.identity);
		//add the room locations to a list
		rooms.Add(1,new Vector2(rnd1.x,rnd1.z));
		rooms.Add(2,new Vector2(rnd2.x,rnd2.z));
		rooms.Add(3,new Vector2(rnd3.x,rnd3.z));
		rooms.Add (4,new Vector2(27,27));


		//mark rooms as reachable only to there entrances
		markRoom (rnd1,6);
		markRoom (rnd2,6);
		markRoom (rnd3,6);
		//mark the last secondary room as not connected but reachable
		grid [(int)rnd1.x + 1, (int)rnd1.z + 5].connected = 0;
		//mark start
		markRoom (new Vector3(0,depth,0),3);
		//correct for start exits
		grid [1, 2].connected = 0;
		grid [2, 1].connected = 0;
		//mark finish
		markRoom (new Vector3(27,depth,27),3);
		//correct for there only being one entrance.
		//instantiate wallTriggers
		Dictionary<int, Vector3> rooms2 = new Dictionary<int,Vector3> ();
		rooms2.Add (0, new Vector3 (0, depth, -6));
		rooms2.Add (1, rnd1s);
		rooms2.Add (2, rnd2s);
		rooms2.Add (3, rnd3s);
		rooms2.Add (4, new Vector3 (54, depth, 48.5f));
		Vector3 offsetDoor = new Vector3 (3, .75f, 6);
		Vector3 offsetBall = new Vector3 (3, -.9f, 9);
		Instantiate (RedWall, rooms2 [wallRooms [1]]+offsetDoor, Quaternion.identity);
		Instantiate (BlueWall, rooms2 [wallRooms [2]]+offsetDoor, Quaternion.identity);
		Instantiate (GreenWall, rooms2 [wallRooms [3]]+offsetDoor, Quaternion.identity);
		Instantiate (PurpleWall, rooms2 [wallRooms [4]]+offsetDoor, Quaternion.identity);
		//instantiateProjectiles
		Instantiate (RedBall, rooms2 [ballRooms [1]]+offsetBall, Quaternion.identity);
		Instantiate (BlueBall, rooms2 [ballRooms [2]]+offsetBall, Quaternion.identity);
		Instantiate (GreenBall, rooms2 [ballRooms [3]]+offsetBall, Quaternion.identity);
		Instantiate (PurpleBall, rooms2 [ballRooms [4]]+offsetBall, Quaternion.identity);
	}

	void markRoom(Vector3 room, int size){
		for (int j = (int)room.z; j < (int)room.z+size; j++) {
			for (int i = (int)room.x; i <(int)room.x+3; i++) {
				
				//if were at a door set to 1 (connected), otherwise set to 2 (innaccessible)
				if((i == (int)room.x +1 && j ==(int) room.z) || (((i == (int)room.x) || (i == (int) room.x+2)) && j ==(int)room.z +1))
					grid[i,j].connected = 0;
				else grid[i,j].connected = 2;
			}
		} 
		}


	void prims(){
		Cell start = grid [1, 2]; //start at 1,2
		//mark the postion
		start.x = 1;
		start.y = 2;
		//connect it to the grid and find frontier
		connectAndFrontier (start);
		//while our frontier inst empty
		while (frontier.Count != 0) {
			//get a random cell from our frontier and remove it
			int rnd = Random.Range (0,frontier.Count);
			Cell cur = frontier[rnd];
			frontier.RemoveAt(rnd);
			//get its neighbours that are connected to the graph, pick one randomly
			List<Cell> neighbours = getNeighbours(cur);
			int rnd2 = Random.Range(0,neighbours.Count);
			Cell n = neighbours[rnd2];
			//connect our current frontier node
			connectAndFrontier(cur);
			markDirection(cur,n);// marks the walls that need to be broken between frontier and neighbour


				}


		}
	void connectAndFrontier(Cell cell){
		grid [cell.x, cell.y] = cell; //add it to the grid
		grid [cell.x, cell.y].connected = 1; //connect it

		//make potential cells, additions to the frontier
		Cell[] temp = new Cell[4]{new Cell(cell.x -1, cell.y),new Cell(cell.x +1, cell.y),new Cell(cell.x , cell.y-1),new Cell(cell.x, cell.y+1)};
		foreach(Cell cur in temp){
			if( cur.x >= 0 && cur.y >= 0 && cur.y < size && cur.x < size && grid[cur.x,cur.y].connected == 0 ){
				bool caught = false;
				//make sure the potential addition isnt already in the frontier
				foreach(Cell front in frontier){
					if(front.x == cur.x && front.y == cur.y){
						caught = true;}
						
				}
				if(!caught) frontier.Add (cur);
			}
		}
	}

	List<Cell> getNeighbours(Cell cell){
		List<Cell> n = new List<Cell>();
		//add potential neighbours
		Cell[] temp = new Cell[4]{new Cell(cell.x -1, cell.y),new Cell(cell.x +1, cell.y),new Cell(cell.x , cell.y-1),new Cell(cell.x, cell.y+1)};
		//make sure theyre valid and connected
		foreach(Cell cur in temp){
			if( cur.x >= 0 && cur.y >= 0 && cur.y < size && cur.x < size && grid[cur.x,cur.y].connected == 1 && grid [cur.x, cur.y].finalPath != 1){
				n.Add (cur);
			}
		}
		return n;
	}

	void markDirection(Cell cur, Cell prev){
		//mark the direction of the connection
		if (cur.x - prev.x == 1) {
						grid[cur.x,cur.y].W = 1;
						grid[prev.x,prev.y].E = 1;
				}
		else if (cur.x - prev.x == -1) {
			grid[cur.x,cur.y].E = 1;
			grid[prev.x,prev.y].W = 1;
		}
		else if (cur.y - prev.y == 1) {
			grid[cur.x,cur.y].S = 1;
			grid[prev.x,prev.y].N = 1;
		}
		else if (cur.y - prev.y == -1) {
			grid[cur.x,cur.y].N = 1;
			grid[prev.x,prev.y].S = 1;
		}
	}
	void createPath(Cell start, Cell end){
		Cell current = start;
		int difx = start.x - end.x; //- means east
		int dify = start.y - end.y; //- means north
		//booleans that determine definite direction.
		bool goEast = false;

		grid [current.x, current.y].connected = 1;
		grid [current.x, current.y].finalPath = 1;
		//while we havent found the end
		while (difx != 0 || dify != 0) {
			//go east untill we dont need to
			if (difx < 0 && grid [current.x + 1, current.y].connected !=2)
					goEast = true;//east

			//go east marking the path
			if (goEast) {
				
					grid [current.x, current.y].E = 1;
					grid [current.x + 1, current.y].W = 1;
					current = new Cell(current.x + 1, current.y);
					difx +=1;
					goEast = false;
					grid [current.x, current.y].connected = 1;
					grid [current.x, current.y].finalPath = 1;
				
			} 
			//then go north
			else {
					grid [current.x, current.y].N = 1;
					grid [current.x, current.y + 1].S = 1;
					current = new Cell(current.x, current.y + 1);
					dify += 1;
					grid [current.x, current.y].connected = 1;
					grid [current.x, current.y].finalPath = 1;
				
			} 

		}
	}
	void CreateBallWallCombo(){
		//key: 1 is red, 2 is blue, 3 is green, 4 is purple
		//value: 0 is start, 1 is room1, 2 is room2, 3 is room3,4 is room4
		List<int> ballWallColors = new List<int>(){1,2,3,4};
		int room = 4;//start at room 4
		//choose the final door color,add it to the dictionary
		int currentColorIndex = Random.Range (0, ballWallColors.Count);
		int currentBallColor = ballWallColors[currentColorIndex];
		
		//while we still have projectiles left to distribut
		while(ballWallColors.Count > 0){
			//add breakable wall color to room
			wallRooms.Add (currentBallColor,room);
			//add the same color ball to the room before it
			ballRooms.Add (currentBallColor,room-1);
			ballWallColors.RemoveAt (currentColorIndex);

			room -=1;
			//randomize the next door
			if(ballWallColors.Count>0){
			currentColorIndex = Random.Range (0, ballWallColors.Count);
			currentBallColor = ballWallColors[currentColorIndex];
			}
		}

	}
}




		


