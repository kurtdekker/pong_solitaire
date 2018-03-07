Unity3D Pong Solitaire - One Scene One Script (and a few assets)

This repository is super-simple Pong solitaire (one paddle, one ball, three walls).

by Kurt Dekker - http://www.plbm.com/ - http://www.twitter.com/kurtdekker

Unity 5.2.2f1 - kicked off November 29, 2015
Unity 5.5.1f1 - updated March 7, 2018

Let's do it in one scene with one script (and a few assets).

Let's break down the problem of writing a pong game:

- user starts the game with a button click
- user input moves a paddle up and down with the mouse
- a ball moves linearly across the screen
	- spawn the ball at start of game
	- increase the ball speed after each return
- border of play field constrains the ball:
	- bounces on far wall
	- bounces on side walls
	- ball gets past player and he loses
- scoring mechanism:
	- keeps track of successful player returns (the score)
	- display the score
	- persistent high score tracking
- restart mechanism for the next game
