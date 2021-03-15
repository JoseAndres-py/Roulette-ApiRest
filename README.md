# Roulette-ApiRest
Roulette Api Rest .NET Core


New roulette creation endpoint that returns the roulette id of the new roulette created.

Roulette opening endpoint (the input is a roulette id) that allows subsequent betting requests, it should simply return a status that confirms that the operation was successful or denied.

Betting endpoint on a number (valid numbers to bet are from 0 to 36) or color (black or red) of the roulette wheel a given amount of money (maximum $10,000) on an open roulette wheel. note: this enpoint receives besides the bet parameters, a user id in the HEADERS assuming that the service making the request has already performed an authentication and validation that the client has the necessary credit to place the bet.

Endpoint of closing bets given a roulette id, this endpoint must return the result of the bets placed from opening to closing. The winning number should be selected automatically by the application when closing the roulette and for numerical bets it should return 5 times the money bet if they hit the winning number, for color bets it should return 1.8 times the money bet, all others will lose the money bet. note: to select the winning color it should be taken into account that even numbers are red and odd numbers are black.

Endpoint of list of created roulettes with their states (open or closed)


## Endpoints and Methods

| Method| Endpoint|
| ------------ | ------------ |
| POST |api/roulette/create |
| POST |api/roulette/open |
| POST |api/roulette/open |
| POST |api/roulette/bet |
| POST |api/roulette/close |
| GET |api/roulette/list-roulettes|
