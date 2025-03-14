# Chemical Inventory Management System

Welcome to the **TuDa.CIMS** project!
This system is specifically designed for **TU Darmstadt** to help efficiently manage
chemical stocks, record purchases, organize working groups, and generate PDF invoices.
It is a project developed by IT students as part of their **Bachelor Praktikum**.

## Table of Contents

1. [Overview](#overview)
2. [Getting Started](#getting-started)
3. [License](#license)
4. [Contact](#contact)

---

## Overview

**TuDa.CIMS** was created to address the unique requirements of chemical inventory management at **TU Darmstadt**.

### Main Features

- **Purchase Interface**
  - Simplified purchase process for individual working groups
- **Asset Database**
  - Easily add, edit, or remove assets in the database
- **Working Group Management**
  - Overview of all working groups with detailed information

### Technology Stack

- **.NET**: Core framework
  - **ASP.NET**: Powers the `TuDa.CIMS.Api`
  - **Blazor**: Supports the `TuDa.CIMS.Web` front-end
- **PostgreSQL**: Main database solution

---

## Getting Started

### Using the Provided Docker Container

> Requires Docker to be installed and the daemon to be running.

1. **Copy the `docker-compose.yml` file to your local machine**:

2. **Run the following command in the directory where the `docker-compose.yml` file is located**:

    ```sh
    docker compose up
    ```

3. **The `API`, `Web`, and `DB` containers should now be running**

> [!TIP] Using a specific Version
> If you want to use a specific version of the application,
> you can specify the version in the `docker-compose.yml` file.
> Change `:latest` to the desired version in the `image` field.

### Build Docker image locally

> **Requires docker to be installed and the daemon to be running**

1. **Clone the repository**:

    ```sh
    git clone https://github.com/ProjectCIMS/TuDa.CIMS.git
    ```

2. **Build and run with Docker Compose**:

    ```sh
    docker compose -f ./docker-compose.dev.yml up --build
    ```

3. **The `API`, `Web`, and `DB` containers should now be running**

### Local Development with Aspire AppHost

> **Requires .NET v9 preinstalled**

1. **Clone the repository**:

    ```sh
    git clone https://github.com/ProjectCIMS/TuDa.CIMS.git
    ```

2. **Navigate to the AppHost project**:

    ```sh
    cd ./infrastructure/TuDa.CIMS.AppHost
    ```

3. **Run the AppHost**:

    ```sh
    dotnet run
    ```

    Follow the console instructions.

---

## License

This project is licensed under the [MIT License](LICENSE.md), allowing free use, modification, and distribution under its terms.

**Disclaimer**: This is not an official project of **TU Darmstadt** nor is it associated or endorsed by them.
While it was originally developed by IT students for the **BachelorPraktikum**, it will continue to be used beyond the scope of the program.

The software is not officially published by **TU Darmstadt**.

---

## Contact

For questions, suggestions, or assistance, please open an issue on GitHub.
