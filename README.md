Food Delivery API with .NET 8.0
Overview

This is a microservice-based food delivery application built using .NET 8.0. The project aims to provide services like user management, restaurant management, order processing, and courier services in a scalable and modular architecture.

    ⚠️ Note: This project is currently under development.

Features

    User Service: Handles user registration, authentication, and management.
    Restaurant Service: Manages restaurant information, including menus and locations.
    Order Service: Processes and tracks customer orders.
    Courier Service: Manages couriers for food delivery and assigns them to orders.

Technologies Used

    .NET 8.0
    ASP.NET Core
    Entity Framework Core
    PostgreSQL (Npgsql)
    Docker & Docker Compose
    RabbitMQ for inter-service communication
    JWT Authentication
    Identity Framework for user authentication and authorization

Microservices Architecture

The project is divided into several microservices, each responsible for a specific functionality:

    UserService
        User registration, login, and profile management.

    RestaurantService
        Restaurant creation and management.

    OrderService
        Order placement and status tracking.

    CourierService
        Assigning couriers to orders and tracking deliveries.
