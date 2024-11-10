# ISSUE NEST
A ticketing system designed to manage client-reported issues and provide administrative responses. The repository is structured into two parts: a React JS front-end and a .NET API back-end. To support the application, a SQL Server database is required.
## Frontend Information
### Prerequisites
Before running this project, ensure you have the following installed:
- [Node JS](https://nodejs.org/en)
### Instalation
1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/username/repo.git
   ```
2. Navigate to the project directory:
   ```bash
   cd frontend-issue-nest
   ```
3. Install Dependencies
   ```bash
   npm install
   ```
4. Rename .env.example to .env and change the api base url value to route that api application runs
   
5. Run Application on Development
   ```bash
   npm run dev
   ```
## Backend Information
### Prerequisites
Before running this project, ensure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or another IDE that supports .NET 6
- (Optional) [Postman](https://www.postman.com/) for testing APIs

### Installation

#### Preparation
1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/username/repo.git
   ```
2. Navigate to the project directory:
   ```bash
   cd backend-issue-nest
   ```
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
#### Database Migration
Run query on migration and seeder to SQL Server DB Server
```bash
issue-nest\backend-issue-nest\SQL\Migration.sql
```

#### Running the Program
To run the application, use the following command:
```bash
dotnet run
```

#### Postman Import
(Optional) There is a Postman file that can be imported to the local computer.
```bash
issue-nest\backend-issue-nest\postman_collection.json
```




