# ReleaseRetentionAPI

Contains the solution to run the API for getting retained releases 

Please clone this repo and follow instructions below to see converter in action

**ReleaseRetentionAPI Solutions - Includes a Web API and a Unit Test Project**
 - Open **ReleaseRetentionAPI.sln** in Visual Studio
 - Build and Run the solution
 - Leave the solution running 
 - API can be tested using your preferred tool.
 
 - If using Postman:
   - use this sample URL - https://localhost:7147/api/ReleaseRetention/1 with the below sample data
   - with the below sample data
   - {
  "project": {
    "id": "Project-1",
    "name": "Random Quotes"
  },
  "environments": [
    {
      "id": "Environment-1",
      "name": "Production"
    }
  ],
  "releases": [
    {
      "id": "Release-1",
      "projectId": "Project-1",
      "version": "1.0.0",
      "created": "2000-01-01T09:00:00"
    }
  ],
  "deployments": [
    {
      "id": "Deployment-1",
      "releaseId": "Release-1",
      "environmentId": "Environment-1",
      "deployedAt": "2000-01-01T08:00:00"
    }
  ]
}

 - **Assumptions**:
   - I have assumed that there will only ever be 1 project as part of the input 
   - Inout will always be in the above format: 1 project -> a list of environments -> a list of releases -> a list of deployments
