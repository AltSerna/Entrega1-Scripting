using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting
{
    public class Node
    {
        public bool isExplored = false;
        public Node isExploredFrom;

        public int[,] position = new int[1, 2];
    }
    class SearchPathASG
    {
        bool _isExploring = true;
        Random random = new Random();
        public Node _startingPoint;
        public Node _endingPoint;
        Queue<Node> _queue = new Queue<Node>();
        List<Node> _path = new List<Node>();
        Node _searchingPoint;
        Dictionary<int[,], Node> _block = new Dictionary<int[,], Node>();
        public Node[,] globalMaze=new Node[5,5];
        public int contadorDeNodos = 0;

        public void GeneradorDeMaze()
        {
            Node[,] maze = new Node[5, 5];
            int randNum;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    maze[i, j] = new Node();
                    maze[i, j].position[0,0] = i;
                    maze[i, j].position[0,1] = j;
                    contadorDeNodos++;

                    if (j==1)
                    {
                        randNum = random.Next(0,2);
                        if(randNum==1)
                        {
                            maze[i, j] = null;
                        }
                        maze[4, 1] = new Node();
                        maze[4, 1].position[0,0] = i;
                        maze[4, 1].position[0,1] = j;
                        contadorDeNodos++;
                    }

                    if (j == 3)
                    {
                        randNum = random.Next(0, 2);
                        if (randNum == 1)
                        {
                            maze[i, j] = null;
                        }
                        maze[2, 3] = new Node();
                        maze[2, 3].position[0,0] = i;
                        maze[2, 3].position[0,1] = j;
                        contadorDeNodos++;
                    }
                }
            }

            StartAndEndPoints(maze);
            

        }

        public void StartAndEndPoints(Node[,] maze)
        {
            int randStart = random.Next(0, 4);
            int randEnd = random.Next(0, 4);

            _startingPoint = maze[randStart, 0];
            _endingPoint = maze[randEnd, 4];

            PrintMaze(maze);
        }

        public void PrintMaze(Node[,] readyMaze)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if(readyMaze[i,j]!=null)
                    {
                        if(readyMaze[i,j]==_startingPoint)
                        {
                            Console.Write("S");
                            if(j<4)
                            {
                                Console.Write("-");
                            }
                        }
                        else if(readyMaze[i,j]==_endingPoint)
                        {
                            Console.Write("F");
                            if (j < 4)
                            {
                                Console.Write("-");
                            }
                        }
                        else
                        {
                            Console.Write("O");
                            if (j < 4)
                            {
                                Console.Write("-");
                            }
                        }
                    }
                    else
                    {
                        Console.Write("I");
                        if(j<4)
                        {
                            Console.Write("-");
                        }
                    }
                }
                Console.WriteLine();
            }

            globalMaze = readyMaze;
            Path(readyMaze);
        }



        public List<Node> Path(Node[,] maze)
        {
            if(_path.Count==0)
            {
                LoadAllBlocks(maze);
                BFS();
                CreatePath();
            }
            return _path;
        }

        private void LoadAllBlocks(Node[,] maze)
        {
            Node[] nodes = new Node[contadorDeNodos];

            int contador = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    contador++;
                    nodes[contador] = maze[i, j];
                }
            }

            foreach (Node node in nodes)
            {
                int[,] gridPos = new int[1,2];
                //gridPos = node.position;

                if (_block.ContainsKey(gridPos))
                {
                    Console.WriteLine("2 Nodes present in same position. i.e nodes overlapped.");
                }
                else
                {
                    _block.Add(gridPos, node);        // Add the position of each node as key and the Node as the value
                }
            }
        }

        private void BFS()
        {
            _queue.Enqueue(_startingPoint);
            while (_queue.Count > 0 && _isExploring)
            {
                _searchingPoint = _queue.Dequeue();
                OnReachingEnd();
                ExploreNeighbourNodes();
            }
        }

        private void OnReachingEnd()
        {
            if (_searchingPoint == _endingPoint)
            {
                _isExploring = false;
            }
            else
            {
                _isExploring = true;
            }
        }

        private void ExploreNeighbourNodes()
        {
            if (!_isExploring) { return; }

            int coordX;
            int coordY;
            coordX = _searchingPoint.position[0, 0];
            coordY = _searchingPoint.position[0, 1];
            Node[] arrayNodes = new Node[4];
            int[,] neighbourPosition = new int[1,1];

            Node upNeighbour;
            if(coordX==0)
                upNeighbour = null;
            else
            {
                upNeighbour = globalMaze[coordX - 1, coordY];
                arrayNodes[0] = upNeighbour;
            }

            Node rightNeighbour;
            if (coordY == 4)
                rightNeighbour = null;
            else
            {
                rightNeighbour = globalMaze[coordX, coordY + 1];
                arrayNodes[1] = rightNeighbour;
            }

            Node leftNeighbour;
            if (coordY == 0)
                leftNeighbour = null;
            else
            {
                leftNeighbour = globalMaze[coordX, coordY - 1];
                arrayNodes[2] = leftNeighbour;
            }

            Node downNeighbour;
            if (coordX == 4)
                downNeighbour = null;
            else
            {
                downNeighbour = globalMaze[coordX + 1, coordY];
                arrayNodes[3] = downNeighbour;
            }

            if (_block.ContainsKey(rightNeighbour.position) && !(rightNeighbour is null))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
            {
                Node node = _block[rightNeighbour.position];

                if (!node.isExplored)
                {
                    _queue.Enqueue(node);                       // Enqueueing the node at this position
                    node.isExplored = true;
                    node.isExploredFrom = _searchingPoint;      // Set how we reached the neighbouring node i.e the previous node; for getting the path
                }
            }
            else return;

            if (_block.ContainsKey(upNeighbour.position) && !(upNeighbour.position is null))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
            {
                Node node = _block[upNeighbour.position];

                if (!node.isExplored)
                {
                    _queue.Enqueue(node);                       // Enqueueing the node at this position
                    node.isExplored = true;
                    node.isExploredFrom = _searchingPoint;      // Set how we reached the neighbouring node i.e the previous node; for getting the path
                }
            }
            else return;

            if (_block.ContainsKey(downNeighbour.position) && !(downNeighbour.position is null))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
            {
                Node node = _block[downNeighbour.position];

                if (!node.isExplored)
                {
                    _queue.Enqueue(node);                       // Enqueueing the node at this position
                    node.isExplored = true;
                    node.isExploredFrom = _searchingPoint;      // Set how we reached the neighbouring node i.e the previous node; for getting the path
                }
            }
            else return;

            if (_block.ContainsKey(leftNeighbour.position) && !(leftNeighbour.position is null))               // If the explore neighbour is present in the dictionary _block, which contians all the blocks with Node.cs attached
            {
                Node node = _block[leftNeighbour.position];

                if (!node.isExplored)
                {
                    _queue.Enqueue(node);                       // Enqueueing the node at this position
                    node.isExplored = true;
                    node.isExploredFrom = _searchingPoint;      // Set how we reached the neighbouring node i.e the previous node; for getting the path
                }
            }
            else return;

        }

        public void CreatePath()
        {
            SetPath(_endingPoint);
            Node previousNode = _endingPoint.isExploredFrom;

            while (previousNode != _startingPoint)
            {
                SetPath(previousNode);
                previousNode = previousNode.isExploredFrom;
            }

            SetPath(_startingPoint);
            _path.Reverse();

        }

        private void SetPath(Node node)
        {
            _path.Add(node);
        }

    }
}
