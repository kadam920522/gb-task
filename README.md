# Customer Manager API

## Setup
1. Check out the repository or download it.
2. You can find powershell scripts to build the project and setup docker containers in the `GlobalBlue.CustomerManager/build` folder.
    - BuildImage.ps1 - To build and create docker image from the project.
    - SetupContainers.ps1 - Uses the docker-compose command to create and start the containers
    - TeardownContainers.ps1 - Delete containers and the corresponding resources.
3. Start docker. 
4. Execute the `BuildImage.ps1` powershell script.
5. Execute the `SetupContainers.ps1` powershell script.
6. Docker containers should be up and running.

## Testing the API
### Swagger
Once the containers are up and running navigate to [http://localhost:5010/swagger](http://localhost:5010/swagger) to open the Swagger UI for the API. 
Here you can see the descriptions of the Web API endpoints and can try them out.

### Postman Requests
I also added [Postman](https://www.postman.com/) request collection with valid sample input data to the repository for testing purpose which can be imported.

### Integration Tests
The solution contains integration tests which use Testcontainer for Postgresql. In order to run them successfully, docker should be up and running.