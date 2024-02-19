# FlappyBird

the control script for a [Working Flappy Bird Arcade Cabinet](https://steamcommunity.com/sharedfiles/filedetails/?id=3159105570)

Controls:
* Space to Flap

This uses some raster sprite intersections to do collisions with the pipes. also uses ignore color to create transparent pixels. the monospace font's transparent pixel is technically 3 chars. so to do raster image manipulation you need all the rows to be the same length. the raster sprite also does the math to convert from the pixel space at its scale to the screen space. and calculating intersections between two raster sprites.

Resources Used:
* Game Assets: [Flappy Bird Assets](https://github.com/samuelcust/flappy-bird-assets)
* Cabinet Design taken from: [Functioning Tetris blueprint](https://steamcommunity.com/sharedfiles/filedetails/?id=2906631630)
