Game of Life
============
Implementation of [Conwey's Game of Life](http://www.google.fi) to practise WPF, NUnit, etc.

![ScreenShot](https://raw.github.com/jums/game-of-life/master/screenshot.png)

Rules
-----
A world is a x * y grid of cells that are dead or alive.

On each evolution/round all the cells of the world is evaluated for the following rules.

1. Any live cell with fewer than two live neighbours dies, as if caused by under-population.
2. Any live cell with two or three live neighbours lives on to the next generation.
3. Any live cell with more than three live neighbours dies, as if by overcrowding.
4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

TODO / Ideas
------------
* Drawing of the initial state.
* Option to change the grid scale / world size.
* Wackier colors.
* Different life colors based on the persitence of a cell. (requires evaluation of the future world in advance)
* Show random seed
* Input custom random seed.
