## Installation

Before getting started, ensure that you have .NET SDK 8 installed. You can download and install the SDK from [here](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

To verify if you have the SDK 8 installed, run the following command in your terminal:

```
dotnet --version
```

If the command does not return version 8.x of the SDK, follow the installation instructions provided in the link.

Also make sure that you have a cloudinary account beforehand.

## Quick Start

1. Clone this repository to your local machine:

```
git clone https://github.com/HiroshiGamma/taller01.git
```

2. Restore the project dependencies:

```
dotnet restore
```

3. Ensure the database is operational:

```
$ dotnet ef database update
```
4. Run the application:

```
$ dotnet run
```
## Note

You will need to create a .env file and fill the next information in it for the api to work:
DATABASE_URL = Data 'NameOfYourDataBase.db' 
CloudinaryName = 'YourCloudinaryName'
ApiKey = 'YourApiKey'
ApiSecret = 'YourApiSecret'

You can create your cloudinary account in [here] (https://cloudinary.com/users/login) if you don't have one already.

## Usage
You can test the API using the Postman collection file included in this repository:
AuthController.postman_collection
Account.postman_collection
ProductController.postman_collection# backend-catedra03
# backend-catedra03
