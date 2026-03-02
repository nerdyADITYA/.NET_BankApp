# BankApp (.NET Windows Forms Application)

A desktop-based Bank Management System built using **C#**, **.NET Framework (Windows Forms)**, and **SQL Server (LocalDB)**. The application features user authentication with distinct roles (Admin and Client), account management, user management, and transaction handling.

## Features

### Authentication & Roles
*   **Login System:** Secure login requiring username, password, and role selection.
*   **Role-Based Access Control:** 
    *   **Admin:** Full access to manage accounts and system users.
    *   **Client:** Access restricted to their own account dashboard and transactions.

### Admin Features
*   **Account Management (`Accounts.cs`):**
    *   View all bank accounts in a DataGridView.
    *   Add new accounts (Account Number, Name, Phone, Address, Balance, Secret Code).
    *   Edit existing account details.
    *   Delete accounts.
*   **User Management (`Users.cs`):**
    *   View all system users (including admins and clients).
    *   Add new users (Name, Phone, Gender, Password, Role).
    *   Edit existing user credentials and details.
    *   Delete users.

### Client Features
*   **Customer Dashboard (`Dashboard.cs`):**
    *   **Balance Overview:** Displays the current available balance.
    *   **Income & Expense Tracking:** Calculates total money sent (Expense) and received (Income).
    *   **Money Transfer:** Allows transferring money to other registered bank accounts securely.
        *   Checks for sufficient balance before transferring.
        *   Automatically updates sender and receiver balances.
    *   **Transaction History:** Displays a list of all incoming and outgoing transactions related to the logged-in client.

## Technologies Used
*   **Language:** C#
*   **UI Framework:** Windows Forms (WinForms)
*   **Database:** Microsoft SQL Server (LocalDB)
*   **Data Access:** ADO.NET (`SqlConnection`, `SqlCommand`, `SqlDataAdapter`, `DataTable`)

## Database Schema (Overview)
The application relies on three primary tables:
1.  **Users:** Stores system login credentials (`UId`, `UName`, `UPassword`, `URole`, `UPhone`, `UGender`). Roles are typically `admin` or `client`.
2.  **AccountTbl:** Stores customer bank account information (`AcNumber`, `AcName`, `Balance`, `AcPhone`, `AcAddress`, `SecretCode`, `Date`).
3.  **TransactionTbl:** Records all transfer activities (`SenderAcNo`, `SenderName`, `RecieverAcNo`, `RecieverName`, `Amount`, `BalanceAfter`, `TransactionType`, `Date`, `Message`).

## Database Connection Setup
The database connection is managed centrally in `Functions.cs`.
If you clone this project, ensure that the connection string in `Functions.cs` points to your active LocalDB instance:
```csharp
ConStr = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Adika\OneDrive\Documents\BankAppDb.mdf;Integrated Security=True;Connect Timeout=30";
```
*Note: Update the `AttachDbFilename` path to match the location of your `.mdf` database file.*

## How to Run
1.  Open the `BankApp.csproj` file in **Visual Studio**.
2.  Ensure you have the SQL Server database setup (or attached) using the schema structures mentioned above.
3.  Modify the connection string in `Functions.cs` to map to your database.
4.  Build and Run (`F5`) the project.
5.  Login using your Admin or Client credentials.
