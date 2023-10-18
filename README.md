# VR am I? - A geographic location guessing game in virtual reality

Bachelor's degree project. Thesis written in Croatian can be found here: https://muexlab.fer.hr/images/50034117/2022%20-%20Zavrsni%20rad_Fran_Posaric.pdf (summary in English at the end) 

A geographic location guessing game in virtual reality. Player is trying to pinpoint their location on the map as close as possible to the target location. Locations are 360° pictures and the player has 3DoF. Score is calculated as a difference in kilometers between the two locations. The goal is to have a score close to, or ideally, zero.

The idea of each level is for the player to first turn off anything that might distract them (world map, instruction screen, results screen) by pressing the Primary button on the left or right controller. They then connect the observed details from their virtual environment and finally make an educated guess about where they are. After that, they activate the map, move to the desired position on it and press the secondary button of the right controller, which places the Guess marker. This marker can be moved an unlimited number of times, until the selection is confirmed by pressing the secondary button on the left controller. This causes the Target marker to be created on the map, symbolizing the actual location the player is at. In addition, the number of kilometers missed for that level is recorded on the scoreboard. Also, after the first press of the confirmation button, the player retains only the ability to move around the map at that zoom level and turn on/off the canvas with instructions and results. By pressing the previously mentioned button again, they go to a new level and the game continues. If they are on the last level, then the location will not change, but the total number of kilometers added up from the results of all levels will be shown.

Start Menu:

![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/94f47a14-583e-4be3-b830-b9633c6aff23)

Controller schematic:

![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/9c499479-fe8f-4796-9148-e592bf6588d0)

Locations:

![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/03823f44-adcb-491e-a3d2-37a5e31edaa2)
![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/018cde97-302a-4090-97a2-ff35bf90420c)
![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/6a4aef5d-7d9d-4da7-baf3-50cc48bcc0a0)
![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/19723ba4-2324-4ac6-9319-4888b677b9cc)
![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/6e632552-4191-4813-ab37-a23cc38d7e27)

Guess and Target pins:

![image](https://github.com/posaricf/VRGeoGuessingGame/assets/87568166/b9d839a5-7273-4b35-8ea3-023319a7ee62)

The current game implementation has five (5) levels and no time limitation. Further iterations might include more levels, random location order, time limitations or even a multiplayer option. The game is developed in Unity 2020.3.34f1 for Meta Quest and is playable exclusively with controllers.
