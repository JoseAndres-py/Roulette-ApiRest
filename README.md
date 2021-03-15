# Roulette-ApiRest
Roulette Api Rest .NET Core


New roulette creation endpoint that returns the roulette id of the new roulette created.

Roulette opening endpoint (the input is a roulette id) that allows subsequent betting requests, it should simply return a status that confirms that the operation was successful or denied.

Betting endpoint on a number (valid numbers to bet are from 0 to 36) or color (black or red) of the roulette wheel a given amount of money (maximum $10,000) on an open roulette wheel. note: this enpoint receives besides the bet parameters, a user id in the HEADERS assuming that the service making the request has already performed an authentication and validation that the client has the necessary credit to place the bet.

Endpoint of closing bets given a roulette id, this endpoint must return the result of the bets placed from opening to closing. The winning number should be selected automatically by the application when closing the roulette and for numerical bets it should return 5 times the money bet if they hit the winning number, for color bets it should return 1.8 times the money bet, all others will lose the money bet. note: to select the winning color it should be taken into account that even numbers are red and odd numbers are black.

Endpoint of list of created roulettes with their states (open or closed)


## Endpoints and Methods
<p align="center">
 
| Method| Endpoint|
| ------------ | ------------ |
| POST |api/roulette/create |
| POST |api/roulette/open |
| POST |api/roulette/open |
| POST |api/roulette/bet |
| POST |api/roulette/close |
| GET |api/roulette/list-roulettes|

</p>

For the invocation of the api services it would be done in the following way:

  *Creation of roulette[PUT]: https://localhost:44302/api/roulette/create?access_key={access_key}

  *Roulette activation [POST]: https://localhost:44302/api/roulette/open?access_key={access_key}f&id_roulette={id_roulette]

  *Betting[PUT]: https://localhost:44302/api/roulette/bet?access_key={access_key}f&id_roulette={id_roulette]&number={number_bet}&color={color_bet}&money_bet={money_bet}

  *Close roulette [POST]: https://localhost:44302/api/roulette/close?access_key={access_key}f&id_roulette={id_roulette]

  *Roulette list[GET]: https://localhost:44302/api/roulette/list-roulettes

The test ApiKeys are as follows:

AccessKey Crupier: 
  *Crupier Id 1 - 93d36591-b06b-47c8-99c0-105aa735025f
  *Crupier Id 2 - 8d9fddd4-cb48-48d7-aefb-b5e2da815325
AccessKey Gambler 1:  
  *Gambler Id 1 - 60d71bfa-63f9-4b07-85e9-e9b22d828efe
  *Gambler Id 2 - d3c62385-2b4e-449c-866a-6ade7358daa8
