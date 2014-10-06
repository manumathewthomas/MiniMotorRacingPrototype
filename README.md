MiniMotorRacingPrototype
========================
A Mini Motor Car Racing Prototype developed in Unity Engine 4. The prototype consists of:

1) Animations and Menu System

2) Physics

3) AI

4) Camera Transformations

5) Shaders


1) Animations and Menu System.

Animations are used to enhance the experience of 3d interactive menu. All animations are based on the animation curve for position, rotation and scale of various game objects and camera objects.

2) Physics

To give a realistic feel for the racing cars, following advanced vehicle dynamics concepts are implemented.

A Rigid - Body System, which controls the mass of the vehicle, drag and angular drag.

A Suspension System, which controls the range of suspension, damper and suspension springs for front and rear wheels.

A Wheel - Collider System, which controls the wheel friction curve for sideways friction, forward friction and friction brake.

A Steering System, which first adjust the angular-drag with respect to the relative-velocity of the vehicle, then evaluate the speed of the turn and sets the steer-angle corresponding to the user-input or AI-input.

A Gear System, which controls the automatic transmission (5 gears) based on the current RPM and throttle value.

A Torque-System, where RPM is calculated with wheel RPM, gear-ratio and differential ratio. Maximum Engine Torque is calculated from the engine-torque curve. Final torque -- calculated from engine-torque, gear-ratio, differential-ratio and transmission-efficiency -- is set as rear-wheel torque.

A Resistance-System, which creates a resistance force with relative drag, rolling resistance, air-density, side-area-multiplier and front-area-multiplier.


3) AI

AI mainly has three parts, One is the AI car control, which follows the same physics as the player car with AI component controlling it's acceleration, hand-brakes and turnings. Second component helps the AI cars to avoid collisions with other cars and obstacles. Final component is the waypoint search  which draws the path for AI car to follow.

4) Camera Transformation 

Multiple cameras are being used with the primary camera set to follow-mode. 

5) Shaders

Various pre-built shaders are used for car's reflections.




