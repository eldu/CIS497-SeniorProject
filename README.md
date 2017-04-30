# CIS497-SeniorProject
# Final Project: Cloth Simulation



## Design Documentation



### Introduction
Cloth simulations are used in animation to produce believable clothes on characters, flags, banners, and other cloth-like objects. 

Cloth simulations can quickly scale and are bottlenecked by satisfying the constraints on particles. Though, the concept is simple, the execution of a cloth simulation is difficult. In addition, collisions between the cloth and other objects have to be accounted for. Forces upon the cloth such as wind and gravity have to be simulated as well. Another problem is dealing with self-intersections or intersections with other cloths. 

### Goal
To create a physically-based cloth simulation that behaves like a real cloth in terms of movement, collision, look, and feel. 

### Inspiration
2016 AICP Sponsor Reel - Dir Cut
https://vimeo.com/169599296

### Specification
- Mass Spring Model
	- Particle System
	- Contraints
		- Structural
		- Shear
		- Bend: Reduces pinching made by the structural and shear constraints
- Interactivity
	- Before the Simulation
		- Add in more cloths by creating an empty object and attaching the “MyParticleSystem.cs” script.
		- Adjust the cloth height and width
		- Adjust how many particles are along the width and height of the cloth
	- While the simulation runs
		- Spring Constant
		- Wind Direction

### Demo
[Presentation](https://vimeo.com/215362321)

Final Demos (Also has statistics on them)
[Single Cloth](https://vimeo.com/215362077)
[Single Cloth with Sphere](https://vimeo.com/215362073)
[Muliple Cloths](https://vimeo.com/215362072)

Previous Videos
[Debug Particle Visualization](https://vimeo.com/215362096)
[Beta: Sphere Collision](https://vimeo.com/215362092)
[Beta: Multiple Sphere Collision](https://vimeo.com/215362090)
[Long Video Messing Around with Cloth](https://vimeo.com/215362083)

### Techniques
- Mass Spring Model

### References
https://cis700-procedural-graphics.github.io/files/dynamics_2_28_17.pdf
https://web.archive.org/web/20070610223835/http://www.teknikus.dk/tj/gdc2001.htm

