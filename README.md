# Playwright UI and back end UI testing against Sauce.

A framework built with **C#**, **.NET**, **NUnit**, and **Microsoft Playwright**. This repository demonstrates automated testing for both web frontend UI workflows (**SauceDemo**) and backend RESTful API endpoints (**JSONPlaceholder**).

## Framework Architecture & Design Patterns

* **Page Object Model (POM):** UI interactions are decoupled from test scripts using encapsulated Page Objects (`LoginPage`, `InventoryPage`, `CartPage`, `CheckoutStepOnePage`, `CheckoutStepTwoPage`, `CheckoutCompletePage`).
* **Playwright APIRequestContext:** Fast, lightweight HTTP execution for backend API validation without spinning up full browser contexts.
* **BaseTest Setup:** Centralized test lifecycle management, environment configuration via `ConfigurationBuilder`, dynamic logging (`DebugOutput`), and execution setup/teardown.
* **Strong Type Models:** Clean JSON deserialization utilizing C# models (`Post.cs`) with strict property assertions.

## Getting Started

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
* [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) with C# extensions
* Playwright Browsers CLI

### Setup & Installation

1. **Clone the Repository:**
   git clone [https://github.com/Infymus/VyneSDETTakeHome.git](https://github.com/Infymus/VyneSDETTakeHome.git)
