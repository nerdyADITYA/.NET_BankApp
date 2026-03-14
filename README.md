Below is a **comprehensive `README.md`** based on all the `.cs` and `.Designer.cs` files in your project. I carefully analyzed the structure, UI forms, database access layer, and logic across the project including:

*  (Program entry point)
*  (Login system)
*  (Dashboard logic)
*  (Accounts management)
*  (Users management)
*  (Database functions)

This README explains **architecture, forms, database interactions, UI design, and application workflow**.

---

# BankApp – Windows Forms Banking Management System

## Overview

BankApp is a desktop banking management system built using **C# Windows Forms (.NET)** with a **SQL Server LocalDB database**. The application simulates a basic banking environment where administrators can manage users and accounts, and clients can log in to perform financial transactions such as transferring money and viewing their transaction history.

The system uses a **role-based login system** that differentiates between administrative users and client users. Administrators can manage system entities such as user accounts and bank accounts, while clients can access their personal banking dashboard.

The project also includes a clean graphical interface built using **Guna UI2 components**, which modernizes the standard Windows Forms appearance.

---

# Application Architecture

The project follows a **layered architecture**, separating UI logic from database operations.

The main layers are:

1. **Presentation Layer (Windows Forms)**
2. **Business Logic Layer**
3. **Data Access Layer**
4. **Database Layer**

---

# Project Structure

```
BankApp
│
├── Program.cs
├── Functions.cs
│
├── Login.cs
├── Login.Designer.cs
│
├── Dashboard.cs
├── Dashboard.Designer.cs
│
├── Accounts.cs
├── Accounts.Designer.cs
│
├── Users.cs
└── Users.Designer.cs
```

Each form contains two files:

* **.cs file** → logic and event handling
* **.Designer.cs file** → UI component definitions

---

# Technologies Used

## Programming Language

* C#

## Framework

* .NET Windows Forms

## UI Library

* Guna.UI2.WinForms

## Database

* SQL Server LocalDB

## Data Access

* ADO.NET

---

# Application Entry Point

The application begins execution in **Program.cs**.

```
Application.Run(new Login());
```

When the application launches, the **Login form** is displayed first.
This ensures that users must authenticate before accessing any features.

Source reference: 

---

# Database Layer

The application connects to a **SQL Server LocalDB database**.

Connection string:

```
Data Source=(localdb)\MSSQLLocalDB;
AttachDbFilename=C:\Users\Adika\OneDrive\Documents\BankAppDb.mdf;
Integrated Security=True;
Connect Timeout=30
```

Database operations are handled through the **Functions class**, which acts as a lightweight data access layer.

Source reference: 

---

# Functions.cs – Database Helper Class

The `Functions` class provides reusable methods for interacting with the database.

### Responsibilities

* Open database connections
* Execute SQL queries
* Retrieve data tables
* Execute insert/update/delete operations

### Methods

### 1. GetData()

Executes a SELECT query and returns results as a DataTable.

Example usage:

```
DataTable dt = Con.GetData(query);
```

This method uses a **SqlDataAdapter** to fill the DataTable.

---

### 2. SetData()

Executes INSERT, UPDATE, or DELETE operations.

```
Cmd.ExecuteNonQuery();
```

Returns the number of affected rows.

---

# Login System

The Login form provides authentication functionality for both administrators and clients.

Source reference: 

---

## Login Interface Components

The login form includes:

* Username textbox
* Password textbox
* Role dropdown
* Login button

Users must select their role before logging in.

Available roles:

```
Admin
Client
```

---

# Login Authentication Process

The login button executes a SQL query:

```
SELECT UId, UName, UPassword, URole
FROM Users
WHERE UName = '{username}' AND UPassword = '{password}'
```

If no record is found:

```
Wrong Username or Password
```

If the role does not match:

```
Role does not match your account
```

---

# Admin Login Flow

If the user logs in with role **Admin**, the system redirects to the **Accounts management form**.

```
Accounts obj = new Accounts();
obj.Show();
this.Hide();
```

Administrators gain access to:

* User management
* Account management

---

# Client Login Flow

If the user logs in as **Client**, the system loads account information.

The following fields are retrieved:

```
AcNumber
AcName
Balance
```

These values are stored in static variables:

```
Login.AcNumber
Login.AcName
Login.Balance
```

Then the **Dashboard form** opens.

---

# Dashboard Module

The dashboard is the main interface for clients.

Source reference: 

---

# Dashboard Features

The dashboard provides:

* Account balance display
* Transaction history
* Income summary
* Expense summary
* Money transfer functionality

---

# Account Summary Display

The dashboard displays the user's balance.

```
AmountLbl.Text = "₹" + Balance;
```

The account number is partially masked for security.

Example:

```
**** **** **** 1234
```

---

# Transaction History

Transaction data is retrieved from `TransactionTbl`.

SQL query:

```
SELECT
CASE
 WHEN SenderAcNo = '{0}' THEN 'Sent'
 WHEN RecieverAcNo = '{0}' THEN 'Received'
END AS [Transaction Type],
Amount,
TrDate
FROM TransactionTbl
```

Transactions are displayed inside a **DataGridView table**.

---

# Income and Expense Tracking

Two SQL queries calculate totals:

### Total Sent

```
SELECT SUM(Amount)
FROM TransactionTbl
WHERE SenderAcNo = '{account}'
```

### Total Received

```
SELECT SUM(Amount)
FROM TransactionTbl
WHERE RecieverAcNo = '{account}'
```

These values are displayed as:

```
Income
Expenses
```

---

# Money Transfer System

Clients can transfer money to another account.

Required inputs:

* Destination account
* Amount
* Message

Validation rules include:

* Amount must be greater than zero
* Amount must not exceed current balance

---

# Transfer Process

Steps:

1. Insert transaction record
2. Deduct balance from sender
3. Add balance to receiver

Example query:

```
INSERT INTO TransactionTbl
VALUES (...)
```

Balances are updated immediately after the transaction.

---

# Accounts Management (Admin)

Administrators manage bank accounts through the **Accounts form**.

Source reference: 

---

# Account Management Features

Admins can:

* Create accounts
* Edit accounts
* Delete accounts
* View account list

---

# Account Fields

Each account includes:

```
Account Number
Account Name
Balance
Phone
Address
Secret Code
```

---

# Add Account

SQL Query:

```
INSERT INTO AccountTbl
VALUES(...)
```

The account is immediately added to the DataGridView.

---

# Edit Account

Selected records can be updated.

```
UPDATE AccountTbl
SET Balance = ...
WHERE AcNumber = ...
```

---

# Delete Account

Accounts can be removed using:

```
DELETE FROM AccountTbl
WHERE AcNumber = ...
```

---

# User Management (Admin)

Administrators manage system users through the **Users form**.

Source reference: 

---

# User Management Features

Admins can:

* Create users
* Edit users
* Delete users
* View user list

---

# User Fields

Each user record contains:

```
User ID
Username
Phone Number
Gender
Password
Role
```

---

# Add User

```
INSERT INTO Users
VALUES(...)
```

---

# Edit User

```
UPDATE Users
SET ...
WHERE UId = ...
```

---

# Delete User

```
DELETE FROM Users
WHERE UId = ...
```

---

# UI Design

The application uses **Guna.UI2 components** for modern styling.

Features include:

* Rounded panels
* Styled buttons
* Modern DataGridViews
* Animated UI components

The interface includes a sidebar navigation panel with options like:

```
Dashboard
Transactions
Accounts
Users
Logout
```

---

# Data Display Components

The system uses **DataGridView controls** to display:

* Accounts
* Users
* Transactions

These tables support:

* Row selection
* Real-time refresh
* Styled headers

---

# Navigation System

Forms navigate using:

```
new Form().Show();
this.Hide();
```

This allows the application to move between modules without closing the program.

---

# Security Considerations

The system implements basic authentication and role validation.

However, some improvements could include:

* Parameterized SQL queries
* Password hashing
* Input validation
* Transaction rollback support

---

# Potential Future Improvements

Possible enhancements include:

* Secure authentication
* Bank statement export
* Transaction search filters
* Account creation by clients
* Mobile banking interface
* Graphical financial analytics
* API integration
* Multi-account support

---

# Conclusion

BankApp demonstrates a complete **banking management desktop application** built with Windows Forms and SQL Server. The project includes user authentication, role-based access, account management, and financial transaction handling.

The use of modular forms, reusable database utilities, and modern UI components makes the project well-structured and easy to extend.

This project is a strong example of applying **C#, Windows Forms, SQL Server, and ADO.NET** to build a real-world financial management system.

---
