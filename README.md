Unity3D Pong Solitaire Tutorial

by Kurt Dekker - http://www.plbm.com/ - http://www.twitter.com/kurtdekker

This repository is a tutorial for Pong solitaire (one paddle, one ball, three walls).

Unity 5.2.2f1 - kicked off November 29, 2015

Let's do it in one scene with one script.

Let's break down the problem of writing a pong game:

- user starts the game with a button click
- user input moves a paddle up and down
- a ball moves linearly across the screen
	- spawn the ball at start
	- increase the ball speed after each return
- border of play field constrains the ball:
	- bounces on far wall
	- bounces on side walls
	- ball gets past player and he loses
- scoring mechanism:
	- keeps track of successful player returns (the score)
	- displays the score
- restart mechanism for the next game
