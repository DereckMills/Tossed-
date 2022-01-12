# Tossed!
Programming Test for Tentworks Interactive: Two salad chefs operate a kitchen serving hungry customers. May the best chef win!

Objective:

  Face off against your fellow chef in the salad kitchen. Serve customers a variety of (6) different ingredients. However, make sure you get the order correct as your customers are not the most patient of guests. Get orders out faster than your opponent to earn points, throwing food away will cost points, and a customer that walks away unsated will cost points for both players. Really fast delivery of orders will spawn that player a Power Up! Collect them to earn extra time, points, or some extra speed for a few seconds.

Interactable Items:

Ingredient Stockpile: (6) different ingredients can be picked up from around the kitchen. The player can carry 2 different items at the same time

Chopping Board: The player can add items to the chopping board. Once an item has been added, it must be chopped before another can be added. Once an item has been placed, interact with the board again to chop it.

Plate: If there are items on the chopping board, put them on a plate to serve a customer. If the player is holding an item, it will be placed on the stack of plates.

Trash Can: The player can throw away items their are holding or a plate full of ingredients, this will cost the player points.

Customer: If the player has a plate, they can give it to a customer. If the order is correct, that player will recieve points and the customer will leave. Up to (7) customers can be spawned at a time. If a customer recieves an incorrect order, they will become Angry!

Power Ups! If a customer gets their order quickly, a powerup will spawn on the map for the player who completed the order. The power ups are Time, Points, and Speed.


Controls:

        Player 1                           Player 2
          Up: W                            Up: Up Arrow
        Down: S                          Down: Down Arrow
        Left: A                          Left: Left Arrow
       Right: D                         Right: Right Arrow
    Interact: Space                  Interact: Num Pad 0
        Swap: Left Shift                 Swap: Right Ctrl
        

In Progress/Furute Tasks

--Debugging: Investigate inconisistancy with the movement of the players

--Improvements: Make sure the powerups spawn in an open space

--Features: Add a Main Menu

Required Tasks (Complete):

--Top Down Camera
    I decided against the panning camera to make sure all information about the game space is available at all times.
    
--Couch Co-Op
    All controls for both players are submitted through a standard keyboard with a keypad
    Everything relevent to Player 1 is Blue, and Player 2 is in Red
    
--Different Veggies
    The vegtables are represented by a distinct shape and each of the (6) variations have a unique color.
    
--Item Placing
    A player is able to pick up a total of 2 items. When they are next to an interactable object, such as the cutting board or ingredient stockpile, various actions occur with the held items

--Customer Interactions
    Score increases with correct orders and they leave after a set time (which is longer for more complex orders)
    
--Angry Customers
    When a customer recieves an incorrect order, they become angry and they leave quicker. If they leave after their set time, the chef they are angry with loses twice as many points. A customer can be angry with both chefs.

--PowerUps 
    When a customer is given the correct order within 70% of their set time, that player has a PowerUp spawned for them.
    
--Game Over
    The game is over when both players run out of time. A game over screen is display with the winner and their points. The players can start a new game from this screen
    
--High Scores
    The top 10 scores are persistantly saved and updated as the game is played.
