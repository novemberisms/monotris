using System;
using System.Collections.Generic;

namespace MonoTris
{
    public class RandomGenerator
    {
        private Random _rng = new Random();
        private List<TetrominoShape> _shapeBag = new List<TetrominoShape>();

        public TetrominoShape GenerateShape()
        {
            if (_shapeBag.Count == 0)
            {
                FillShapeBag();
            }

            var result = _shapeBag[0];
            _shapeBag.RemoveAt(0);

            return result;
        }

        private void FillShapeBag()
        {
            // create a new list with a shallow copy of the shape database
            _shapeBag = new List<TetrominoShape>(TetrominoShapeDatabase.Shapes);
            
            // shuffle the list using the Fisher-Yates algorithm
            var n = _shapeBag.Count;

            while (n > 1)
            {
                n--;
                var q = _rng.Next(n + 1); 
                var value = _shapeBag[q];
                _shapeBag[q] = _shapeBag[n];
                _shapeBag[n] = value;
            }
        }
    }
}
