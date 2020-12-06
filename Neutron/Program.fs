(*
 Charles Quigley
 Assigment 3: Neutron
 COSC 3353
 Version 1.0
 Due Date: 4 October 2020
 --------------------------------------------------------

 This assignment is meant to simulate the game "Neutron".
 This is a 2 player game in which the goal is to move the Neutron (denoted as "N" within the game)
 into the first row of a player's starting side. Whoever's starting side the Netutron is on wins the game.

 Two Players
 -------------
 Player W
 Player B


 Player W starts on the top of the board, and his/her soldiers are designated "W1" "W2" "W3" "W4" "W5".
 Player B starts on the bottom of the board, and his/her soldiers are designated "B1" "B2" "B3" "B4" "B5".

 During the first turn, Player W can only move on of his/her soldier pieces.
 Every subsequent turn, each player moves the Neutron first, then one of his/her soldier pieces.

 For any piece moved in the game, that piece must keep moving until it collides with another piece of the edge 
 of the board.

 So, if W1 was at the top of the board, and B1 was at the bottom, and W1 was moved downwards, W1 must continue
 to move downards until it hits B1. After colliding with B1, W1 would be placed in the available space right above B1.

 Player's COMMANDS:  (note that %s stands for either "W" or "B" and %d stands for 1,2,3,4, or 5
 -> N up/down/left/right/right up/right down/left up/left down
 -> %s%d up/donw/left/right/right up/right down/left up/left down

 Examples of Player commands:
 ---------------------------------
 N UP                    <- moves the Neutron upwards
 W5 right                <- moves the soldier W5 to the right
 B4 left up              <- moves the soldier B4 diagonally such that it moves left and upwards


 Player's are allowed to move a piece in a direction that keeps the piece in the same spot.
 For example, if W1 was on the right edge of the board, and the player entered "W1 right",
 W1 would not move, but that turn would count.

 If, however, the Neutron piece is completely trapped, such that it can not move at least 1 square in any direction,
 then whoever trapped the Neutron wins.

 For example: suppose N was surrounded by B1 on the top, B2 on the top right, B3 on the right, W1 on the bottom right
              W2 on the bottom, W3 on the bottom left, and W4 on the left.
              This means N is surrounded by soldier pieces on all sides except the top left.
              Now suppose W5 was moved so that it was adjacent to N on the top left. 
              This would mean N is completely trapped in all directions, and since Player W completed this entrapment,
              Player W wins.


*)

open System


let mutable wOrB = ""


///This function draws the 5 x 5 board for the pieces to move on.
///dummyParameter is used to prevent eager evaluation. It does nothing else for the function.
///So, just pass 0 to this function.
let drawMap dummyParameter = 

    Console.SetCursorPosition(1, 2)
    printfn "---------------------------------------"
    Console.SetCursorPosition(1, 6)
    printfn "---------------------------------------"
    Console.SetCursorPosition(1, 10)
    printfn "---------------------------------------"
    Console.SetCursorPosition(1, 14)
    printfn "---------------------------------------"
    Console.SetCursorPosition(1, 18)
    printfn "---------------------------------------"
    Console.SetCursorPosition(1, 22)
    printfn "---------------------------------------"
    
    for i in 2 .. 22 do
        Console.SetCursorPosition(0,i)
        printfn "|"
    for i in 2 .. 22 do
        Console.SetCursorPosition(8, i)
        printfn "|"
    for i in 2 .. 22 do
        Console.SetCursorPosition(16, i)
        printfn "|"
    for i in 2 .. 22 do
        Console.SetCursorPosition(24, i)
        printfn "|"
    for i in 2 .. 22 do
        Console.SetCursorPosition(32, i)
        printfn "|"
    for i in 2 .. 22 do
        Console.SetCursorPosition(40, i)
        printfn "|"

///This function simply clears the screen using System.Console.Clear(). 
///dummyParameter is used to prevent eager evaluation. It does nothing else for the function.
///So, just pass 0 to this function.
let clearScreen dummyParameter =
    System.Console.Clear()

///This function is used periodically in main() to determine if the Neutron is on the top row of the board
///or bottom row of the board. Thus, it takes Neutron's coordinates in a list list as its parameters. Since the Neutron
///being on the top or bottom row of the board is how we determine a winner, this function helps to determine
///who the winner is.
let winCondition (N: int list list) =
    let x = N.[0] //Since N is a list list (for example: [[4;20]], x helps parse through it.
    if x.[1] = 4 then //If the y-coord of the neutron is 4, then its on the top row of the board. End the game return 1.
        1
    elif x.[1] = 20 then //If the y-coord of the Neutron is 20, then its on the bottom row of the board. End game return 1.
        1
    else   //If the y-coord of the neutron is neither 4 nor 20, then the game is still going. Return 0.
        0
 
///This function works almost exactlt like winCondition(). The only difference being that 
///this function also clears the screen of the board and pieces before printing which player won.
let gameOver (N: int list list) =
    let x = N.[0]
    if x.[1] = 4 then
        clearScreen 0
        printfn"\n   PLAYER W WINS!!!!"
    elif x.[1] = 20 then
        clearScreen 0
        printfn "\n  PLAYER B WINS!!!"
        
    
///This function uses the coordinates of each of a player's pieces (which are all stored together in a list list)
///along with a separate list containing each piece's name in order to print where the pieces are on the board.
///So, parameter x is the list list of a player's soldier's coordinates, and the parameter name is a string list
///of each piece's names. 
let setPieces (x:int list list) (name: string list) =
    
    
    if name = ["N"] then
        let mutable (y:int list) = List.item (0:int) x
        Console.SetCursorPosition(List.item 0 y, List.item 1 y)
        printfn "%s" name.Head
    else
        for i in 0 .. 4 do 
            let mutable (y:int list) = List.item (i:int) x
            Console.SetCursorPosition(List.item 0 y, List.item 1 y)
            printfn "%s" <| List.item (i:int) name


///This function takes user input and parses through it to see if its a valid command.
///So, if a player inputs "B2 right up", then this function will determine that the command is valid.
///If a player inputs "B2 right right", then this function will return "ERROR" because that is not a valid
///command. The parameter playerCommand is the user input. The Parameter areWeMovingNeutron is a bool
///with true meaning that it is the player's turn to move the Neutron, and false meaning that it is not.
///areWeMovingNeutron helps determine whether or not neutron movement commands are allowed at the point in the 
///game that this function is called in.
let parsePlayerCommand (playerCommand:string) (areWeMovingNeutron: bool) = 
    let mutable playerMove = ""

    //If The first character a user inputs in 'N' or 'n' and it is not that player's turn to move the Neutron,
    //then return "ERROR"
    if areWeMovingNeutron = false && (playerCommand.[0] = 'N' || playerCommand.[0] =  'n' ) then
        Console.SetCursorPosition(17, 23)
        Console.SetCursorPosition(17,23)
        "ERROR"
    else //The first character of user input must be either upper/lower case 'W', 'N', or 'B'
        if playerCommand.[0] = 'W' || playerCommand.[0] = 'B' ||playerCommand.[0] = 'w' || playerCommand.[0] = 'b'
         || playerCommand.[0] = 'N' || playerCommand.[0] = 'n' then
            
            wOrB <- Char.ToString( Char.ToUpper ( playerCommand.[0])) 
       
            //If a player inputs something like "W2" an and nothing else, that is erroneous
            if playerCommand.Length <= 2 then
                "ERROR"
            //If we're moving the Neutron, we just put something like "N up"
            //If we're moving one of the soldier pieces, user input must have either 1,2,3,4, or 5 after the "W"
            //or "B". Otherwise, how would we know which soldier piece we were moving?
            elif  wOrB = "N" || playerCommand.[1] = '1' || playerCommand.[1] = '2' || playerCommand.[1] = '3' || playerCommand.[1] = '4' || playerCommand.[1] = '5' then
                if wOrB <> "N" then
                    playerMove <- String.Concat (playerMove, playerCommand.[1])
           
                //For a loop
                let mutable length = String.length playerCommand
                length <- length - 1

            
                //Ignore any and all white spaces between the soldier/Neutron name and the directional portion
                //of the command. So, something like "W2          DOWN" is valid.
                //While loop for F# found in: https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/loops-while-do-expression
                let mutable i = 2
                while playerCommand.[i] = ' ' && i < length do
                     i <- i + 1
                     printfn ""  //Probably not good practice, but loops in f# do not like ending in assignment statements
            
                //Will hold the directional portion of the user-input command.
                let mutable directionalCommand = ""
            
                for j in i .. length  do
                 
                    directionalCommand <- String.Concat( directionalCommand,  Char.ToUpper playerCommand.[j] ) 
                    printfn ""
                //The directional portion of the command must be one of these!
                //So, "W1 DOWN " with a space after the "N" does not work. Neither does "W1 LEFT  UP" with 2 spaces
                //in between "LEFT" and "UP".
                if directionalCommand = "UP" || directionalCommand = "DOWN" || directionalCommand =  "LEFT"
                 || directionalCommand = "RIGHT" || directionalCommand = "RIGHT UP" || directionalCommand = "RIGHT DOWN"
                 || directionalCommand = "LEFT UP" || directionalCommand = "LEFT DOWN" then
                    playerMove <- String.Concat(playerMove, directionalCommand)
                    playerMove //return playerMove
                else
                    Console.SetCursorPosition(17, 23)
                    Console.SetCursorPosition(17,23)
                    "ERROR"

                        
            else
                Console.SetCursorPosition(17, 23)
                Console.SetCursorPosition(17,23)
                "ERROR"
            

        else 
            Console.SetCursorPosition(17, 23)
            Console.SetCursorPosition(17,23)
            "ERROR"

///This function changes the coordinates of the Neutron piece. It takes the parsed-through user-input as a parameter.
///It also takes the list list neutron as a parameter (which holds the Neutron's coordinates).
///It also takes each soldier list list as parameters (to determine if the Neutron's coordinates while moving become
///the same as a soldier's coordinates, in which case the Neutron must stop).
let moveNeutron (instruction: string) (neutron: int list list) (playerOneCoords: int list list) (playerTwoCoords: int list list) =
      
      //Neutron
      let mutable N = neutron.[0]  

      //Either player W's soldiers' coordinates or Player B's soldiers' coordinates.
      let  P1 = playerOneCoords.[0] //PlayerOneCoords is a list list. So, P1 = the first list within the list list.
      let  P2 = playerOneCoords.[1] //Example: P2 = [4;12]
      let  P3 = playerOneCoords.[2]
      let  P4 = playerOneCoords.[3]
      let  P5 = playerOneCoords.[4]

       //The other player's coordiantes.
      let O1 = playerTwoCoords.[0]
      let O2 = playerTwoCoords.[1]
      let O3 = playerTwoCoords.[2]
      let O4 = playerTwoCoords.[3]
      let O5 = playerTwoCoords.[4]

      //Used to control the while loops that change the Neutron's coordinates.
      let mutable key = true

      //Used for the "UP" instruction while I was still figuring out how to do this.
      //Not really necessary, but if I got rid of it I would have to change "UP"'s code, which is too much 
      //extra work considering I still have to do this week's Tasks.
      let mutable hitSomeone = false
      
      //If the player's input was "N DOWN", then we would go in this loop, in which we keep 
      //increasing the y-axis by 4 until N's coords become the same as one of the soldier pieces coords
      //or until we overlap the bottom boundary of the map (which is set at y-coord = 20).
      if instruction = "DOWN" then
         
         while key do 
             if N = P1 || N = P2 || N = P3 || N = P4 || N = P5
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[1] > 20  then
                    key <- false
                    
                    
             else 
                    N <- [N.[0];N.[1] + 4]
                    key <- true
         //Once N has the same coordinates as one of the other peices or has gone beyond the map,
         //Go back up 1 square ( - 4 for the y-coordinate) in order for N to be placed on the last available square
         //of the map.
         N <-  [N.[0];N.[1] - 4]
         let newCoordinates = [[N.[0];N.[1]]]
         newCoordinates //newCoordinates
      elif instruction = "UP" then  //The rest of these pretty much work like "DOWN"'s if statement, except the additions/subtractions to the x/y coordinates change.
      
         while key do 
             if N = P1 || N = P2 || N = P3 || N = P4 || N = P5
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 then 
                      key <- false
                      hitSomeone <-true
             elif N.[1] < 0 then
                      key <- false
             else 
                N <- [N.[0];N.[1] - 4]
                key <- true
         if hitSomeone then
              N <- [N.[0]; N.[1]+4]
         elif N.[1] < 4 then 
              N <-  [N.[0];4]
         else
              N <-  [N.[0];N.[1] + 4]
         
         
         let newCoordinates = [[N.[0];N.[1]]]
         newCoordinates //newCoordinates
      elif instruction = "LEFT" then

          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] < 4 then
                      key <- false
              else 
                  N <- [N.[0]-8; N.[1]]
                  key <- true
          if N.[0] < 4 then
              N <-  [4 ;N.[1]]
          else 
              N <- [N.[0] + 8; N.[1]]
          let newCoordinates = [[N.[0];N.[1]]]
          newCoordinates //newCoordinates

      elif instruction = "RIGHT" then

          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] > 36  then
                      key <- false
              else 
                  N <- [N.[0]+8; N.[1]]
                  key <- true
          if N.[0] > 36 then
              N <-  [36 ;N.[1]]
          else 
              N <- [N.[0] - 8; N.[1]]
          let newCoordinates = [[N.[0];N.[1]]]
          newCoordinates //newCoordinates

      elif instruction = "RIGHT UP" then
          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] > 36 || N.[1] < 4  then
                      key <- false
              else 
                  N <- [N.[0]+8; N.[1]-4]
                  key <- true
          
          N <- [N.[0] - 8; N.[1]+4]
          let newCoordinates = [[N.[0];N.[1]]]


          newCoordinates //newCoordinates
      
      elif instruction = "RIGHT DOWN" then
          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] > 36 || N.[1] > 20  then
                      key <- false
              else 
                  N <- [N.[0]+8; N.[1]+4]
                  key <- true
          //if N.[0] > 36  then
            //N <- [N.[0]-8 ; N.[1]]
          //elif N.[1] > 20 then
            //  N <- [N.[0]; N.[1] - 4]
          //else 
          N <- [N.[0] - 8; N.[1]-4]
          let newCoordinates = [[N.[0];N.[1]]]
          newCoordinates //newCoordinates
      elif instruction = "LEFT UP" then
          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] < 4 || N.[1] < 4  then
                      key <- false
              else 
                  N <- [N.[0]-8; N.[1]-4]
                  key <- true
          
          N <- [N.[0] + 8; N.[1]+4]
          let newCoordinates = [[N.[0];N.[1]]]
          newCoordinates //newCoordinates

      elif instruction = "LEFT DOWN" then
          while key do
              
              if N = P1 || N = P2 || N = P3 || N = P4 || N = P5 
                  || N = O1 || N = O2 || N = O3 || N = O4 || N = O5 || N.[0] < 4 || N.[1] > 20  then
                      key <- false
              else 
                  N <- [N.[0]-8; N.[1]+4]
                  key <- true
          
          N <- [N.[0] + 8; N.[1]-4]
          let newCoordinates = [[N.[0];N.[1]]] 
          newCoordinates //newCoordinates

      else 
        //Place holders just incase something went really wrong...which I hope it shouldn't
         let newCoordinates = [[1;2]]
         newCoordinates

///One of the win conditions for this game is that a player becomes stalemated due to not being able to complete his/her turn.
///This means that the Neutron is completely trapped such that it could not move at least 1 square in any direction.
///This function is used in main() to check whether this scenario is the case. As such, the parameters are:
/// neutron (the coordinates of the Neutron), playerOneCoords (the coordinates of all a player's soldiers), and 
/// playerTwoCoords (the coordinates of all the other player's soldiers).
/// This function returns 1 if the Neutron is completely trapped (a player has been stalemated). 
/// This function returns 0 if the Neutron is not completelly trapped (no player has been stalemated).
let checkNeutronStaleMate (neutron: int list list) (playerOneCoords: int list list) (playerTwoCoords: int list list) =
    //Neutron
       let mutable N = neutron.[0]  //Holds the Neutron's coordinates. For example, N = [4;12]
       let mutable testN = neutron.[0] //Is used to test scenarios in which the Neutron moves.

       //Player's soldier coordinates
       let  P1 = playerOneCoords.[0] //These work the same way that they do in moveNeutron()
       let  P2 = playerOneCoords.[1]
       let  P3 = playerOneCoords.[2]
       let  P4 = playerOneCoords.[3]
       let  P5 = playerOneCoords.[4]

        //Other player's soldier coordinates
       let O1 = playerTwoCoords.[0]
       let O2 = playerTwoCoords.[1]
       let O3 = playerTwoCoords.[2]
       let O4 = playerTwoCoords.[3]
       let O5 = playerTwoCoords.[4]


       //holds the number of directions that N cannot move at least 1 square in. 
       //So, if this holds 8, that means N is completely trapped.
       let mutable numberOfDirectionsTrappedIn = 0
    
       //Can we move at least 1 square Down?
       testN <- [N.[0]; N.[1] + 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[1] > 20  then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1 //If we can't, then +1
        
       //Can we move at least 1 square Up?
       testN <- [N.[0]; N.[1] - 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[1] < 4 then 
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1 //If we can't, then +1
                      
       //Can we move at least 1 square Left?     
       testN <- [N.[0] - 8; N.[1]]        
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[0] < 4 then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1
       //Can we move at least 1 square Right?
       testN <- [N.[0] + 8; N.[1]]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[0] > 36  then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1
       //Can we move at least 1 square diagnoally such that we move Right Up?
       testN <- [N.[0] + 8; N.[1] - 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                     || testN = O1 || testN = O2 || testN = O3 || testN= O4 || testN = O5 || testN.[0] > 36 || testN.[1] < 4  then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1
       //Can we move at least 1 square diagonally such that we move Right Down?
       testN <- [N.[0] + 8; N.[1] + 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                   || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[0] > 36 || testN.[1] > 20  then
                       numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1
       
       //Can we move at least 1 square diagonally such that we move Left Up?
       testN <- [N.[0] - 8; N.[1] - 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[0] < 4 || testN.[1] < 4  then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1 

       //Can we move at least 1 square diagonally such that we move Left Down?
       testN <- [N.[0] - 8; N.[1] + 4]
       if testN = P1 || testN = P2 || testN = P3 || testN = P4 || testN = P5 
                     || testN = O1 || testN = O2 || testN = O3 || testN = O4 || testN = O5 || testN.[0] < 4 || testN.[1] > 20  then
                        numberOfDirectionsTrappedIn <- numberOfDirectionsTrappedIn + 1

       //If we cannot move at least 1 square in any of the 8 directions, then the neutron is completely trapped
       //and we've reached a stalemate situation.
       if numberOfDirectionsTrappedIn = 8 then
            1
       else 
            0

///This function works like moveNeutron() except we're changing one of a player's soldier's coordinates.
///Parameters: instruction (the movement command, such as "W1 UP"), pieceCoordinatesToMove (The player's list list of soldier coordinates, in which one of the soldiers' position will change),
/// otherPlayerCoords (The opponent's list list of soldier coordinates), and neutron (the list list that has the Neutron's coordinates).
let movePiece (instruction: string) (pieceCoordinatesToMove: int list list) (otherPlayerCoords: int list list) (neutron: int list list) = 
    //Current player's pieces
    let mutable P1 = pieceCoordinatesToMove.[0] //Works the same as in moveNeutron()
    let mutable P2 = pieceCoordinatesToMove.[1]
    let mutable P3 = pieceCoordinatesToMove.[2]
    let mutable P4 = pieceCoordinatesToMove.[3]
    let mutable P5 = pieceCoordinatesToMove.[4]

    //Opponent's pieces
    let O1 = otherPlayerCoords.[0]
    let O2 = otherPlayerCoords.[1]
    let O3 = otherPlayerCoords.[2]
    let O4 = otherPlayerCoords.[3]
    let O5 = otherPlayerCoords.[4]

    //Neutron
    let N = neutron.[0]

    //Controls the while loops that change a soldier's coordinates.
    let mutable key = true

    //A relic of when I was still figuring out how to do this.Is still in use for "UP" commands.
    let mutable hitSomeone = false
    
    //If it's either Player W's or Player B's turn, then this if-statement moves either W1 or B1 down.
    if instruction = "1DOWN" then
       
       while key do //Basically collision detection for P1 (which is either W1 or B1).
           if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[1] > 20 || P1 = N then
                  key <- false
                  
                  
           else 
                  P1 <- [P1.[0];P1.[1] + 4] //If we haven't collided with another piece or the edge of the map, keep adding 4 to the y-coord
                  key <- true
                        
       P1 <-  [P1.[0];P1.[1] - 4]  //Once we've collided with something, subtract 4 from y-coord to get either W1 or B1 to the last available square.
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "1UP" then    //The rest of these work like "1DOWN" if statement, but for there respective directions.
    
       while key do 
           if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1 = N then 
                    key <- false
                    hitSomeone <-true
           elif P1.[1] < 0 then
                    key <- false
           else 
              P1 <- [P1.[0];P1.[1] - 4]
              key <- true
       if hitSomeone then
            P1 <- [P1.[0]; P1.[1]+4]
       elif P1.[1] < 4 then 
            P1 <-  [P1.[0];4]
       else
            P1 <-  [P1.[0];P1.[1] + 4]
       
       
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "1LEFT" then

        while key do
            
            if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] < 4 || P1 = N then
                    key <- false
            else 
                P1 <- [P1.[0]-8; P1.[1]]
                key <- true
        if P1.[0] < 4 then
            P1 <-  [4 ;P1.[1]]
        else 
            P1 <- [P1.[0] + 8; P1.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "1RIGHT" then

        while key do
            
            if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] > 36 || P1 = N then
                    key <- false
            else 
                P1 <- [P1.[0]+8; P1.[1]]
                key <- true
        if P1.[0] > 36 then
            P1 <-  [36 ;P1.[1]]
        else 
            P1 <- [P1.[0] - 8; P1.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "1RIGHT UP" then
        while key do
            
            if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 || P1 = N 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] > 36 || P1.[1] < 4  then
                    key <- false
            else 
                P1 <- [P1.[0]+8; P1.[1]-4]
                key <- true
        
        P1 <- [P1.[0] - 8; P1.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]


        newCoordinates //newCoordinates
    
    elif instruction = "1RIGHT DOWN" then
        while key do
            
            if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 || P1 = N 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] > 36 || P1.[1] > 20  then
                    key <- false
            else 
                P1 <- [P1.[0]+8; P1.[1]+4]
                key <- true
        //if N.[0] > 36  then
          //N <- [N.[0]-8 ; N.[1]]
        //elif N.[1] > 20 then
          //  N <- [N.[0]; N.[1] - 4]
        //else 
        P1 <- [P1.[0] - 8; P1.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "1LEFT UP" then
        while key do
            
            if  P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 || P1 = N 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] < 4 || P1.[1] < 4  then
                    key <- false
            else 
                P1 <- [P1.[0]-8; P1.[1]-4]
                key <- true
        
        P1 <- [P1.[0] + 8; P1.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "1LEFT DOWN" then
        while key do
            
            if P1 = P2 || P1 = P3 || P1 = P4 || P1 = P5 || P1 = N 
                || P1 = O1 || P1 = O2 || P1 = O3 || P1 = O4 || P1 = O5 || P1.[0] < 4 || P1.[1] > 20  then
                    key <- false
            else 
                P1 <- [P1.[0]-8; P1.[1]+4]
                key <- true
        
        P1 <- [P1.[0] + 8; P1.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "2DOWN" then   //If we're moving either W2 or B2 down.
        
       while key do 
           if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[1] > 20 ||  P2 = N then
                  key <- false
           else 
                  P2 <- [P2.[0];P2.[1] + 4]
                  key <- true
                        
       P2 <-  [P2.[0];P2.[1] - 4]
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "2UP" then //The rest of these work like the "2DOWN" elif instruction, but for their respective direction.
    
       while key do 
           if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2 = N then 
                    key <- false
                    hitSomeone <-true
           elif P2.[1] < 0 then
                    key <- false
           else 
              P2 <- [P2.[0];P2.[1] - 4]
              key <- true
       if hitSomeone then
            P2 <- [P2.[0]; P2.[1]+4]
       elif P2.[1] < 4 then 
            P2 <-  [P2.[0];4]
       else
            P2 <-  [P2.[0];P2.[1] + 4]
       
       
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "2LEFT" then

        while key do
            
            if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] < 4 || P2 = N then
                    key <- false
            else 
                P2 <- [P2.[0]-8; P2.[1]]
                key <- true
        if P2.[0] < 4 then
            P2 <-  [4 ;P2.[1]]
        else 
            P2 <- [P2.[0] + 8; P2.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "2RIGHT" then

        while key do
            
            if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] > 36 || P2 = N then
                    key <- false
            else 
                P2 <- [P2.[0]+8; P2.[1]]
                key <- true
        if P2.[0] > 36 then
            P2 <-  [36 ;P2.[1]]
        else 
            P2 <- [P2.[0] - 8; P2.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates


    elif instruction = "2RIGHT UP" then
        while key do
            
            if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 || P2 = N 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] > 36 || P2.[1] < 4  then
                    key <- false
            else 
                P2 <- [P2.[0]+8; P2.[1]-4]
                key <- true
        
        P2 <- [P2.[0] - 8; P2.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]


        newCoordinates //newCoordinates
    
    elif instruction = "2RIGHT DOWN" then
        while key do
            
            if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 || P2 = N 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] > 36 || P2.[1] > 20  then
                    key <- false
            else 
                P2 <- [P2.[0]+8; P2.[1]+4]
                key <- true
        //if N.[0] > 36  then
          //N <- [N.[0]-8 ; N.[1]]
        //elif N.[1] > 20 then
          //  N <- [N.[0]; N.[1] - 4]
        //else 
        P2 <- [P2.[0] - 8; P2.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "2LEFT UP" then
        while key do
            
            if  P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 || P2 = N 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] < 4 || P2.[1] < 4  then
                    key <- false
            else 
                P2 <- [P2.[0]-8; P2.[1]-4]
                key <- true
        
        P2 <- [P2.[0] + 8; P2.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "2LEFT DOWN" then
        while key do
            
            if P2 = P1 || P2 = P3 || P2 = P4 || P2 = P5 || P2 = N 
                || P2 = O1 || P2 = O2 || P2 = O3 || P2 = O4 || P2 = O5 || P2.[0] < 4 || P2.[1] > 20  then
                    key <- false
            else 
                P2 <- [P2.[0]-8; P2.[1]+4]
                key <- true
        
        P2 <- [P2.[0] + 8; P2.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "3DOWN" then //If we're moving W3 or B3 down.
        
       while key do 
           if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[1] > 20 || P3 = N then
                  key <- false
           else 
                  P3 <- [P3.[0];P3.[1] + 4]
                  key <- true
                        
       P3 <-  [P3.[0];P3.[1] - 4]
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "3UP" then //The rest of these work like "3DOWN", but for their respective direction.
    
       while key do 
           if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3 = N then 
                    key <- false
                    hitSomeone <-true
           elif P3.[1] < 0 then
                    key <- false
           else 
              P3 <- [P3.[0];P3.[1] - 4]
              key <- true
       if hitSomeone then
            P3 <- [P3.[0]; P3.[1]+4]
       elif P3.[1] < 4 then 
            P3 <-  [P3.[0];4]
       else
            P3 <-  [P3.[0];P3.[1] + 4]
       
       
       let newCoordinates = [P1;P2;P3;P4;P5]
       newCoordinates //newCoordinates
    elif instruction = "3LEFT" then

        while key do
            
            if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] < 4 || P3 = N then
                    key <- false
            else 
                P3 <- [P3.[0]-8; P3.[1]]
                key <- true
        if P3.[0] < 4 then
            P3 <-  [4 ;P3.[1]]
        else 
            P3 <- [P3.[0] + 8; P3.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "3RIGHT" then

        while key do
            
            if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] > 36 || P3 = N then
                    key <- false
            else 
                P3 <- [P3.[0]+8; P3.[1]]
                key <- true
        if P3.[0] > 36 then
            P3 <-  [36 ;P3.[1]]
        else 
            P3 <- [P3.[0] - 8; P3.[1]]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "3RIGHT UP" then
        while key do
            
            if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 || P3 = N 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] > 36 || P3.[1] < 4  then
                    key <- false
            else 
                P3 <- [P3.[0]+8; P3.[1]-4]
                key <- true
        
        P3 <- [P3.[0] - 8; P3.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]


        newCoordinates //newCoordinates
    
    elif instruction = "3RIGHT DOWN" then
        while key do
            
            if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 || P3 = N 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] > 36 || P3.[1] > 20  then
                    key <- false
            else 
                P3 <- [P3.[0]+8; P3.[1]+4]
                key <- true
        //if N.[0] > 36  then
          //N <- [N.[0]-8 ; N.[1]]
        //elif N.[1] > 20 then
          //  N <- [N.[0]; N.[1] - 4]
        //else 
        P3 <- [P3.[0] - 8; P3.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "3LEFT UP" then
        while key do
            
            if  P3 = P1 || P3 = P2 || P3 = P4 || P1 = P5 || P3 = N 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] < 4 || P3.[1] < 4  then
                    key <- false
            else 
                P3 <- [P3.[0]-8; P3.[1]-4]
                key <- true
        
        P3 <- [P3.[0] + 8; P3.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "3LEFT DOWN" then
        while key do
            
            if P3 = P1 || P3 = P2 || P3 = P4 || P3 = P5 || P3 = N 
                || P3 = O1 || P3 = O2 || P3 = O3 || P3 = O4 || P3 = O5 || P3.[0] < 4 || P3.[1] > 20  then
                    key <- false
            else 
                P3 <- [P3.[0]-8; P3.[1]+4]
                key <- true
        
        P3 <- [P3.[0] + 8; P3.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "4DOWN" then //If we're moving either W4 or B4 down.
            
           while key do 
               if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[1] > 20 || P4 = N then
                      key <- false
               else 
                      P4 <- [P4.[0];P4.[1] + 4]
                      key <- true
                            
           P4 <-  [P4.[0];P4.[1] - 4]
           let newCoordinates = [P1;P2;P3;P4;P5]
           newCoordinates //newCoordinates
    elif instruction = "4UP" then //The rest of these work like "4DOWN", but for their respective direction.
        
           while key do 
               if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4 = N then 
                        key <- false
                        hitSomeone <-true
               elif P4.[1] < 0 then
                        key <- false
               else 
                  P4 <- [P4.[0];P4.[1] - 4]
                  key <- true
           if hitSomeone then
                P4 <- [P4.[0]; P4.[1]+4]
           elif P4.[1] < 4 then 
                P4 <-  [P4.[0];4]
           else
                P4 <-  [P4.[0];P4.[1] + 4]
           
           
           let newCoordinates = [P1;P2;P3;P4;P5]
           newCoordinates //newCoordinates
    elif instruction = "4LEFT" then

            while key do
                
                if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] < 4 || P4 = N then
                        key <- false
                else 
                    P4 <- [P4.[0]-8; P4.[1]]
                    key <- true
            if P4.[0] < 4 then
                P4 <-  [4 ;P4.[1]]
            else 
                P4 <- [P4.[0] + 8; P4.[1]]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates

    elif instruction = "4RIGHT" then

            while key do
                
                if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] > 36 || P4 = N then
                        key <- false
                else 
                    P4 <- [P4.[0]+8; P4.[1]]
                    key <- true
            if P4.[0] > 36 then
                P4 <-  [36 ;P4.[1]]
            else 
                P4 <- [P4.[0] - 8; P4.[1]]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates

    elif instruction = "4RIGHT UP" then
            while key do
                
                if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 || P4 = N 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] > 36 || P4.[1] < 4  then
                        key <- false
                else 
                    P4 <- [P4.[0]+8; P4.[1]-4]
                    key <- true
            
            P4 <- [P4.[0] - 8; P4.[1]+4]
            let newCoordinates = [P1;P2;P3;P4;P5]


            newCoordinates //newCoordinates
        
    elif instruction = "4RIGHT DOWN" then
            while key do
                
                if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 || P4 = N 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] > 36 || P4.[1] > 20  then
                        key <- false
                else 
                    P4 <- [P4.[0]+8; P4.[1]+4]
                    key <- true
            //if N.[0] > 36  then
              //N <- [N.[0]-8 ; N.[1]]
            //elif N.[1] > 20 then
              //  N <- [N.[0]; N.[1] - 4]
            //else 
            P4 <- [P4.[0] - 8; P4.[1]-4]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates
    elif instruction = "4LEFT UP" then
            while key do
                
                if  P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 || P4 = N 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] < 4 || P4.[1] < 4  then
                        key <- false
                else 
                    P4 <- [P4.[0]-8; P4.[1]-4]
                    key <- true
            
            P4 <- [P4.[0] + 8; P4.[1]+4]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates

    elif instruction = "4LEFT DOWN" then
            while key do
                
                if P4 = P1 || P4 = P2 || P4 = P3 || P4 = P5 || P4 = N 
                    || P4 = O1 || P4 = O2 || P4 = O3 || P4 = O4 || P4 = O5 || P4.[0] < 4 || P4.[1] > 20  then
                        key <- false
                else 
                    P4 <- [P4.[0]-8; P4.[1]+4]
                    key <- true
            
            P4 <- [P4.[0] + 8; P4.[1]-4]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates
         
    elif instruction = "5DOWN" then //If we're moving either W5 or B5 down.
             
            while key do 
                if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 
                     || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5  || P5.[1] > 20|| P5 = N then
                       key <- false
                else 
                       P5 <- [P5.[0];P5.[1] + 4]
                       key <- true
                             
            P5 <-  [P5.[0];P5.[1] - 4]
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates
    elif instruction = "5UP" then //The rest of these work like "5DOWN" but for their respective direction.
         
            while key do 
                if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 
                     || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5 = N then 
                         key <- false
                         hitSomeone <-true
                elif P5.[1] < 0 then
                         key <- false
                else 
                   P5 <- [P5.[0];P5.[1] - 4]
                   key <- true
            if hitSomeone then
                 P5 <- [P5.[0]; P5.[1]+4]
            elif P5.[1] < 4 then 
                 P5 <-  [P5.[0];4]
            else
                 P5 <-  [P5.[0];P5.[1] + 4]
            
            
            let newCoordinates = [P1;P2;P3;P4;P5]
            newCoordinates //newCoordinates
    elif instruction = "5LEFT" then

             while key do
                 
                 if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 
                     || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] < 4 || P5 = N then
                         key <- false
                 else 
                     P5 <- [P5.[0]-8; P5.[1]]
                     key <- true
             if P5.[0] < 4 then
                 P5 <-  [4 ;P5.[1]]
             else 
                 P5 <- [P5.[0] + 8; P5.[1]]
             let newCoordinates = [P1;P2;P3;P4;P5]
             newCoordinates //newCoordinates

    elif instruction = "5RIGHT" then

             while key do
                 
                 if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 
                     || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] > 36 || P5 = N then
                         key <- false
                 else 
                     P5 <- [P5.[0]+8; P5.[1]]
                     key <- true
             if P5.[0] > 36 then
                 P5 <-  [36 ;P5.[1]]
             else 
                 P5 <- [P5.[0] - 8; P5.[1]]
             let newCoordinates = [P1;P2;P3;P4;P5]
             newCoordinates //newCoordinates

    elif instruction = "5RIGHT UP" then
        while key do
            
            if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 || P5 = N 
                || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] > 36 || P5.[1] < 4  then
                    key <- false
            else 
                P5 <- [P5.[0]+8; P5.[1]-4]
                key <- true
        
        P5 <- [P5.[0] - 8; P5.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]


        newCoordinates //newCoordinates
    
    elif instruction = "5RIGHT DOWN" then
        while key do
            
            if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 || P5 = N 
                || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] > 36 || P5.[1] > 20  then
                    key <- false
            else 
                P5 <- [P5.[0]+8; P5.[1]+4]
                key <- true
        //if N.[0] > 36  then
          //N <- [N.[0]-8 ; N.[1]]
        //elif N.[1] > 20 then
          //  N <- [N.[0]; N.[1] - 4]
        //else 
        P5 <- [P5.[0] - 8; P5.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates
    elif instruction = "5LEFT UP" then
        while key do
            
            if  P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 || P5 = N 
                || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] < 4 || P5.[1] < 4  then
                    key <- false
            else 
                P5 <- [P5.[0]-8; P5.[1]-4]
                key <- true
        
        P5 <- [P5.[0] + 8; P5.[1]+4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

    elif instruction = "5LEFT DOWN" then
        while key do
            
            if P5 = P1 || P5 = P2 || P5 = P3 || P5 = P4 || P5 = N 
                || P5 = O1 || P5 = O2 || P5 = O3 || P5 = O4 || P5 = O5 || P5.[0] < 4 || P5.[1] > 20  then
                    key <- false
            else 
                P5 <- [P5.[0]-8; P5.[1]+4]
                key <- true
        
        P5 <- [P5.[0] + 8; P5.[1]-4]
        let newCoordinates = [P1;P2;P3;P4;P5]
        newCoordinates //newCoordinates

   
    else 
            //Place holders just incase something goes wrong...which I hope it shouldn't
       [[0;0];[0;0]]
        
    

            
           

///MAIN
[<EntryPoint>]
let main argv =
   
    ///The key for our game loop.
    let mutable gameLoop = true

    //The soldier names, the positions of each soldier name within their respective list corresponds to the position
    //of that same soldier's coordinates within the soliderCoords' list list. For example, "W1" is in index 0
    //and [4;4], which corresponds to W1, is also in index 0.
    let soldierNamesPlayer1 = ["W1"; "W2"; "W3"; "W4"; "W5"]
    let soldierNamesPlayer2 = ["B1"; "B2"; "B3"; "B4"; "B5"]
  
    //List list of each player's soldier coordinates. So, [4;4] is W1, [12;4] is W2...[4;20] is B1...etc.
    let mutable soldierCoordsPlayer1 = [[4; 4]; [12; 4]; [20; 4]; [28; 4]; [36; 4]]
    let mutable soldierCoordsPlayer2 = [[4; 20]; [12; 20]; [20; 20]; [28; 20]; [36; 20]]

    //Same idea as above, but for Neutron.
    let neutronName = ["N"];
    let mutable Neutron = [[20; 12]]

    //To get the user input
    let mutable playerCommand = ""

    //Used to help correctly parse through user input.
    let mutable areWeMovingNeutron = false

    //once the user input is parsed, we have our movement instruction
    let mutable moveInstruction = ""


    //Draw each player's soldiers on the board in their correct positions
    setPieces soldierCoordsPlayer1 soldierNamesPlayer1 
    setPieces soldierCoordsPlayer2 soldierNamesPlayer2

    //Draw the Neutron on the board in its correct position
    setPieces Neutron neutronName

    //Draw the board to the screen.
    drawMap 0


    //MOVE A SOLDIER FOR PLAYER W. THE GAME STARTS WITH PLAYER W JUST MOVING A SOLDIER
    areWeMovingNeutron <- false
    let mutable correctCommand = false

    Console.SetCursorPosition(0, 23)
    printfn "Player W, Please Move A Soldier: "
    Console.SetCursorPosition(33, 23)
    playerCommand <- Console.ReadLine()

    //If the parsed user-input is not an actual movement instruction (i.e erroneous), go in this while loop.
    while correctCommand = false do
        moveInstruction <- parsePlayerCommand playerCommand areWeMovingNeutron
     
        if moveInstruction = "ERROR" then
            Console.SetCursorPosition(33, 23)
            printfn "Sorry, That Command Is Not Valid. Please Try Again."
            System.Threading.Thread.Sleep(2000)
            Console.SetCursorPosition(33, 23)
            printfn "                                                                         "
            Console.SetCursorPosition(0, 0)
            Console.SetCursorPosition(0, 23)
            printfn "Player W, Please Move A Soldier: "
            Console.SetCursorPosition(33, 23)
            playerCommand <- Console.ReadLine()

            
        else   //If Player W tries to move Player B's soldier, that's erroneous. Go in here
            correctCommand <- true
            if wOrB = "B" then
                correctCommand <- false
                Console.SetCursorPosition(33, 23)
                printfn "Sorry, It's W's Turn. Please Try Again."
                System.Threading.Thread.Sleep(2000)
                Console.SetCursorPosition(33, 23)
                printfn "                                                                         "
                Console.SetCursorPosition(0, 0)
                Console.SetCursorPosition(0, 23)
                printfn "Player W, Please Move A Soldier: "
                Console.SetCursorPosition(33, 23)
                playerCommand <- Console.ReadLine()

    //Now that we have correct user-input, move the soldier that Player W wants to move.
    soldierCoordsPlayer1 <- movePiece moveInstruction soldierCoordsPlayer1 soldierCoordsPlayer2  Neutron

    //Clear the screen
    clearScreen 0 
      
    //Set up the pieces and board again, reflecting the change in one of the soldier's positions.
    setPieces soldierCoordsPlayer1 soldierNamesPlayer1
    setPieces soldierCoordsPlayer2 soldierNamesPlayer2
    setPieces Neutron neutronName

    drawMap 0
  

    

    //NOW EACH PLAYER MOVES THE NEURTON FIRST AND THEN ONE OF THEIR SOLDIERS, STARTING WITH PLAYER B.
    while gameLoop do


        if gameLoop then 
            //Determine whether or not the Neutron has been completely trapped.
            if checkNeutronStaleMate Neutron soldierCoordsPlayer1 soldierCoordsPlayer2 = 1 then
                gameLoop <- false //If so then a player wins and the game is over.
                Neutron <- [[4;4]] //Used in order to help determine the winner of the game.  
                                  //If Player B cannot move the Neutron because its completely trapped,
                                  //then Player W wins. So, set Neutron's y-coord  to 4, which is the y-coord
                                  //for Player W's starting row.

            //If Neutron has not been completely trapped.
            if gameLoop then
                areWeMovingNeutron <- true  //Player B inputs command to move the neutron. Also error-checking.
                correctCommand <- false
                Console.SetCursorPosition(0, 23)
                printfn "Player B Now Moves The Neutron: "
                Console.SetCursorPosition(33, 23)
                playerCommand <- Console.ReadLine()

                while correctCommand = false do
                    moveInstruction <- parsePlayerCommand playerCommand areWeMovingNeutron
         
                    if moveInstruction = "ERROR" then
                        Console.SetCursorPosition(33, 23)
                        printfn "Sorry, That Command Is Not Valid. Please Try Again."
                        System.Threading.Thread.Sleep(2000)
                        Console.SetCursorPosition(33, 23)
                        printfn "                                                                           "
                        Console.SetCursorPosition(0, 0)
                        Console.SetCursorPosition(0, 23)
                        printfn "Player B Now Moves The Neutron: "
                        Console.SetCursorPosition(33, 23)

                        playerCommand <- Console.ReadLine()

                
                    else 
                        correctCommand <- true
                        if wOrB <> "N" then
                            correctCommand <- false
                            Console.SetCursorPosition(33, 23)
                            printfn "Sorry, It's B's Turn To Move The Neutron. Please Try Again."
                            System.Threading.Thread.Sleep(2000)
                            Console.SetCursorPosition(17, 23)
                            printfn "                                                                           "
                            Console.SetCursorPosition(0, 0)
                            Console.SetCursorPosition(0, 23)
                            printfn "Player B Now Moves The Neutron: "
                            Console.SetCursorPosition(33, 23)
                            playerCommand <- Console.ReadLine()    
                Neutron <- moveNeutron moveInstruction Neutron soldierCoordsPlayer1 soldierCoordsPlayer2 //Neutron's coordinates change



                //Reset the board and pieces, reflecting the change in the Neutron's coordinates.
                clearScreen 0 
                

                setPieces soldierCoordsPlayer1 soldierNamesPlayer1
                setPieces soldierCoordsPlayer2 soldierNamesPlayer2
                setPieces Neutron neutronName

                drawMap 0
        //See if the Neutron is at y-coord = 4 or y -coord = 20. 4 would mean 
        //that Player W wins (because y-coord = 4 is the starting row for Player W).
        //Likewise, 20 would mean that Player B wins (because y-coord = 20 i the starting row for Player B).
        //If Neutron's y-coord != 4 or 20, then the show must go on!
        if winCondition Neutron = 1 then
            gameLoop <- false


        //If no win condition has been met, then it is Player B's turn to move one of his/her soldiers.
        if gameLoop then
            areWeMovingNeutron <- false
            correctCommand <- false

            Console.SetCursorPosition(0, 23)
            printfn "Player B, Please Move A Soldier: "
            Console.SetCursorPosition(33, 23)
            playerCommand <- Console.ReadLine()

            while correctCommand = false do
                moveInstruction <- parsePlayerCommand playerCommand areWeMovingNeutron
         
                if moveInstruction = "ERROR" then
                    Console.SetCursorPosition(33, 23)
                    printfn "Sorry, That Command Is Not Valid. Please Try Again."
                    System.Threading.Thread.Sleep(2000)
                    Console.SetCursorPosition(33, 23)
                    printfn "                                                                           "
                    Console.SetCursorPosition(0, 0)
                    Console.SetCursorPosition(0, 23)
                    printfn "Player B, Please Move A Soldier: "
                    Console.SetCursorPosition(33, 23)
                    playerCommand <- Console.ReadLine()

                
                else 
                    correctCommand <- true
                    if wOrB = "W" then
                        correctCommand <- false
                        Console.SetCursorPosition(33, 23)
                        printfn "Sorry, It's B's Turn. Please Try Again."
                        System.Threading.Thread.Sleep(2000)
                        Console.SetCursorPosition(33, 23)
                        printfn "                                                                         "
                        Console.SetCursorPosition(0, 0)
                        Console.SetCursorPosition(0, 23)
                        printfn "Player W, Please Move A Soldier: "
                        Console.SetCursorPosition(33, 23)
                        playerCommand <- Console.ReadLine()
            //Change the coordinates of one of Player B's soldiers.
            soldierCoordsPlayer2 <- movePiece moveInstruction soldierCoordsPlayer2 soldierCoordsPlayer1  Neutron
   
            

            //Reset the board and pieces, reflecting the change in coordinates for one of Player B's soldiers.
            clearScreen 0 
            

            setPieces soldierCoordsPlayer1 soldierNamesPlayer1
            setPieces soldierCoordsPlayer2 soldierNamesPlayer2
            setPieces Neutron neutronName

            drawMap 0

            //Now we're entring Player W's turn, so check to see if Player W cannt move the Neutron by at least 1 square in
            //any direction.
            if checkNeutronStaleMate Neutron soldierCoordsPlayer1 soldierCoordsPlayer2 = 1 then
                gameLoop <- false   //If the Neutron has been completely trapped, game over and Player B wins.
                Neutron <- [[4;20]] //y-coord = 20 for Neutron means that the Neutron goes to the starting row for Player B.
                                   //This just helps determine the winner of the game.
            //Everything here is pretty much the same as the code for Player B's turn, but now for Player W. 
            if gameLoop then

                areWeMovingNeutron <- true //Player W Neutron Movement stuff
                correctCommand <- false

                Console.SetCursorPosition(0, 23)
                printfn "Player W Now Moves The Neutron: "
                Console.SetCursorPosition(33, 23)
                playerCommand <- Console.ReadLine()

                while correctCommand = false do
                    moveInstruction <- parsePlayerCommand playerCommand areWeMovingNeutron
         
                    if moveInstruction = "ERROR" then
                        Console.SetCursorPosition(33, 23)
                        printfn "Sorry, That Command Is Not Valid. Please Try Again."
                        System.Threading.Thread.Sleep(2000)
                        Console.SetCursorPosition(33, 23)
                        printfn "                                                                              "
                        Console.SetCursorPosition(0, 0)
                        Console.SetCursorPosition(0, 23)
                        printfn "Player W Now Moves The Neutron: "
                        Console.SetCursorPosition(33, 23)

                        playerCommand <- Console.ReadLine()

                
                    else 
                        correctCommand <- true
                        if wOrB <> "N" then
                            correctCommand <- false
                            Console.SetCursorPosition(33, 23)
                            printfn "Sorry, It's W's Turn To Move The Neutron. Please Try Again."
                            System.Threading.Thread.Sleep(2000)
                            Console.SetCursorPosition(33, 23)
                            printfn "                                                                           "
                            Console.SetCursorPosition(0, 0)
                            Console.SetCursorPosition(0, 23)
                            printfn "Player W Now Moves The Neutron: "
                            Console.SetCursorPosition(33, 23)
                            playerCommand <- Console.ReadLine()    

                Neutron <- moveNeutron moveInstruction Neutron soldierCoordsPlayer1 soldierCoordsPlayer2


            clearScreen 0 
            

            setPieces soldierCoordsPlayer1 soldierNamesPlayer1
            setPieces soldierCoordsPlayer2 soldierNamesPlayer2
            setPieces Neutron neutronName

            drawMap 0
        
        if winCondition Neutron = 1 then
            gameLoop <- false
        
        if gameLoop then
            areWeMovingNeutron <- false //Player W Soldier moving stuff
            let mutable correctCommand = false

            Console.SetCursorPosition(0, 23)
            printfn "Player W, Please Move A Soldier: "
            Console.SetCursorPosition(33, 23)
            playerCommand <- Console.ReadLine()

            while correctCommand = false do
                moveInstruction <- parsePlayerCommand playerCommand areWeMovingNeutron
         
                if moveInstruction = "ERROR" then
                    Console.SetCursorPosition(33, 23)
                    printfn "Sorry, That Command Is Not Valid. Please Try Again."
                    System.Threading.Thread.Sleep(2000)
                    Console.SetCursorPosition(33, 23)
                    printfn "                                                                         "
                    Console.SetCursorPosition(0, 0)
                    Console.SetCursorPosition(0, 23)
                    printfn "Player W, Please Move A Soldier: "
                    Console.SetCursorPosition(33, 23)
                    playerCommand <- Console.ReadLine()

                
                else 
                    correctCommand <- true
                    if wOrB = "B" then
                        correctCommand <- false
                        Console.SetCursorPosition(33, 23)
                        printfn "Sorry, It's W's Turn. Please Try Again."
                        System.Threading.Thread.Sleep(2000)
                        Console.SetCursorPosition(33, 23)
                        printfn "                                                                         "
                        Console.SetCursorPosition(0, 0)
                        Console.SetCursorPosition(0, 23)
                        printfn "Player W, Please Move A Soldier: "
                        Console.SetCursorPosition(33, 23)
                        playerCommand <- Console.ReadLine()


            soldierCoordsPlayer1 <- movePiece moveInstruction soldierCoordsPlayer1 soldierCoordsPlayer2  Neutron

            clearScreen 0 
              

            setPieces soldierCoordsPlayer1 soldierNamesPlayer1
            setPieces soldierCoordsPlayer2 soldierNamesPlayer2
            setPieces Neutron neutronName

            drawMap 0

    //Once the game is over and we have exited from the game loop, gameOver() determines the winner by
    //using the y-coordinate of the Neutron. Player W's starting row is at y = 4. Player B's starting row is at y = 20.
    //So, if Neutron's y-coord = 4, then Player W wins. If Neutron's y-coord = 20, then Player B wins.
    gameOver Neutron    


    
    Console.SetCursorPosition(0, 6) //Found Here: https://docs.microsoft.com/en-us/dotnet/api/system.console.setcursorposition?view=netcore-3.1
    0 // return an integer exit code
