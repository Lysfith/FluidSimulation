using FluidSimulation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluidSimulation
{
    public class FluidSimulation
    {
        private Cell[,] _map;
        private Cell[,] _newMap;

        public FluidSimulation(Cell[,] map)
        {
            _map = map;

        }

        public void Update(double time)
        {
            _newMap = new Cell[_map.GetLength(0), _map.GetLength(1)];

            var listCellsWithWater = new List<Cell>();

            for (int line = 0; line < _map.GetLength(1); line++)
            {
                for (int col = 0; col < _map.GetLength(0); col++)
                {
                    var cell = _map[line, col];

                    UpdateCell(cell);
                }
            }

            _map = _newMap;
        }

        private void UpdateCell(Cell cell, float water = 0)
        {
            Cell newCell = _newMap[cell.Y, cell.X];

            if(newCell == null)
            {
                newCell = new Cell()
                {
                    X = cell.X,
                    Y = cell.Y,
                    Altitude = cell.Altitude,
                    Water = cell.Water + water
                };
            }
            else
            {
                newCell.Water += water;
            }

            if(newCell.Water < 0.1f)
            {
                newCell.Water = 0;
            }

            if (water == 0 && cell.Water > 0)
            {
                int cptWater = 0;
                bool top = false;
                bool bottom = false;
                bool left = false;
                bool right = false;

                Cell cellTop = null;
                Cell cellBottom = null;
                Cell cellLeft = null;
                Cell cellRight = null;

                //Top
                if (cell.Y > 0)
                {
                    cellTop = _map[cell.Y - 1, cell.X];

                    if(cellTop.Water < cell.Water)
                    {
                        top = true;
                        cptWater++;
                    }
                }

                //Bottom
                if (cell.Y < _map.GetLength(0)-1)
                {
                    cellBottom = _map[cell.Y + 1, cell.X];

                    if (cellBottom.Water < cell.Water)
                    {
                        bottom = true;
                        cptWater++;
                    }
                }

                //Left
                if (cell.X > 0)
                {
                    cellLeft = _map[cell.Y, cell.X - 1];

                    if (cellLeft.Water < cell.Water)
                    {
                        left = true;
                        cptWater++;
                    }
                }

                //Right
                if (cell.X < _map.GetLength(1)-1)
                {
                    cellRight = _map[cell.Y, cell.X + 1];

                    if (cellRight.Water < cell.Water)
                    {
                        right = true;
                        cptWater++;
                    }
                }

                if(cptWater > 0)
                {
                    if(top)
                    {
                        UpdateCell(cellTop, cell.Water / (cptWater + 1f));
                    }
                    if (bottom)
                    {
                        UpdateCell(cellBottom, cell.Water / (cptWater + 1f));
                    }
                    if (left)
                    {
                        UpdateCell(cellLeft, cell.Water / (cptWater + 1f));
                    }
                    if (right)
                    {
                        UpdateCell(cellRight, cell.Water / (cptWater + 1f));
                    }

                    newCell.Water = cell.Water / (cptWater + 1f);
                }

                if (newCell.Water < 0.1f)
                {
                    newCell.Water = 0;
                }

                //int cellsInferior = cellsAdjacents.Where(x => x.Altitude < cell.Altitude).Count();
                //int cellsEqualsNoWater = cellsAdjacents.Where(x => x.Altitude == cell.Altitude && x.Water == 0).Count();

                //foreach (var cellAdjacent in cellsAdjacents)
                //{
                //    if(cellAdjacent.Altitude < cell.Altitude)
                //    {
                //        cellAdjacent.Water += cell.Water / (cellsInferior + cellsEqualsNoWater);
                //    }
                //    if (cellAdjacent.Altitude == cell.Altitude && cellAdjacent.Water == 0)
                //    {
                //        cellAdjacent.Water += (cell.Water / (cellsInferior + cellsEqualsNoWater)) +  / cellsEqualsNoWater;
                //    }
                //}
            }

            _newMap[cell.Y, cell.X] = newCell;
        }

        public void AddWater(int x, int y, float water)
        {
            _map[y, x].Water += water;
        }

        public Cell[,] GetMap()
        {
            return _map;
        }
    }
}
